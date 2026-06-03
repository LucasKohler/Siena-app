import { MaterialIcons } from "@expo/vector-icons";
import { router } from "expo-router";
import { Pressable, ScrollView, StyleSheet, Text, View } from "react-native";
import * as endpoints from "../../src/api/endpoints";
import { useApi } from "../../src/api/useApi";
import { useAuth } from "../../src/auth/AuthContext";
import { usePendingAttendanceCount } from "../../src/features/admin/usePendingAttendanceCount";
import { colors, radius, spacing, typography } from "../../src/theme";

type Action = {
  title: string;
  icon: keyof typeof MaterialIcons.glyphMap;
  route: "/admin/eventos" | "/admin/usuarios" | "/admin/presencas" | "/admin/relatorios";
  primary?: boolean;
  badge?: boolean;
};

const ACTIONS: Action[] = [
  { title: "Criar Eventos", icon: "add", route: "/admin/eventos", primary: true },
  { title: "Gerenciar Usuários", icon: "manage-accounts", route: "/admin/usuarios" },
  {
    title: "Aprovar Presenças",
    icon: "fact-check",
    route: "/admin/presencas",
    badge: true,
  },
  {
    title: "Relatórios & Financeiro",
    icon: "bar-chart",
    route: "/admin/relatorios",
  },
];

export default function AdminHomeScreen() {
  const { token } = useAuth();
  const { data: users } = useApi(
    () => {
      if (!token) throw new Error("Não autenticado");
      return endpoints.listAdminUsers(token);
    },
    [token]
  );

  const activeAthletes =
    users?.filter((u) => u.role === "Atleta" && u.isActive).length ?? 0;
  const pendingCount = usePendingAttendanceCount(token);

  return (
    <ScrollView style={styles.root} contentContainerStyle={styles.content}>
      <View style={styles.header}>
        <MaterialIcons name="notifications" size={24} color={colors.onSurface} />
      </View>
      <Text style={styles.restricted}>ACESSO RESTRITO</Text>
      <View style={styles.stats}>
        <StatCard
          icon="groups"
          value={String(activeAthletes)}
          label="Atletas Ativos"
        />
        <StatCard
          icon="pending-actions"
          value={pendingCount === null ? "…" : String(pendingCount)}
          label="Presenças Pendentes"
          highlight={pendingCount !== null && pendingCount > 0}
        />
      </View>
      <Text style={styles.section}>Ações Rápidas</Text>
      <View style={styles.grid}>
        {ACTIONS.map((action) => (
          <Pressable
            key={action.route}
            style={[styles.actionCard, action.primary && styles.actionPrimary]}
            onPress={() => router.push(action.route)}
          >
            {action.badge && pendingCount != null && pendingCount > 0 && (
              <View style={styles.badge} />
            )}
            <MaterialIcons
              name={action.icon}
              size={28}
              color={action.primary ? colors.onPrimary : colors.onSurface}
            />
            <Text
              style={[
                styles.actionLabel,
                action.primary && styles.actionLabelPrimary,
              ]}
            >
              {action.title}
            </Text>
          </Pressable>
        ))}
      </View>
    </ScrollView>
  );
}

function StatCard({
  icon,
  value,
  label,
  highlight,
  note,
}: {
  icon: keyof typeof MaterialIcons.glyphMap;
  value: string;
  label: string;
  highlight?: boolean;
  note?: string;
}) {
  return (
    <View style={styles.statCard}>
      <View style={styles.statDecor} />
      <MaterialIcons name={icon} size={24} color={colors.primary} />
      <Text style={styles.statValue}>{value}</Text>
      <Text style={[styles.statLabel, highlight && styles.statLabelHighlight]}>
        {label}
      </Text>
      {note && <Text style={styles.statNote}>{note}</Text>}
    </View>
  );
}

const styles = StyleSheet.create({
  root: { flex: 1, backgroundColor: colors.surface },
  content: {
    padding: spacing.containerMarginMobile,
    paddingBottom: 40,
  },
  header: { alignItems: "flex-end", marginBottom: spacing.stackMd },
  restricted: {
    ...typography.labelMd,
    color: colors.primary,
    textAlign: "center",
    letterSpacing: 2,
    marginBottom: spacing.stackLg,
    fontFamily: "Inter_600SemiBold",
  },
  stats: { flexDirection: "row", gap: spacing.gutter, marginBottom: spacing.stackLg },
  statCard: {
    flex: 1,
    backgroundColor: colors.surfaceContainerLowest,
    borderRadius: radius.lg,
    borderWidth: 1,
    borderColor: colors.outlineVariant,
    padding: spacing.stackMd,
    overflow: "hidden",
  },
  statDecor: {
    position: "absolute",
    top: -16,
    right: -16,
    width: 56,
    height: 56,
    borderRadius: 28,
    backgroundColor: `${colors.outlineVariant}55`,
  },
  statValue: {
    ...typography.headlineMd,
    color: colors.onSurface,
    marginTop: 8,
    fontFamily: "Inter_600SemiBold",
  },
  statLabel: {
    ...typography.labelSm,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_400Regular",
  },
  statLabelHighlight: { color: colors.primary },
  statNote: {
    ...typography.labelSm,
    fontSize: 10,
    color: colors.outline,
    marginTop: 4,
    fontFamily: "Inter_400Regular",
  },
  section: {
    ...typography.headlineMd,
    color: colors.onSurface,
    marginBottom: spacing.stackMd,
    fontFamily: "Inter_600SemiBold",
  },
  grid: {
    flexDirection: "row",
    flexWrap: "wrap",
    gap: spacing.gutter,
  },
  actionCard: {
    width: "47%",
    aspectRatio: 1,
    backgroundColor: colors.surfaceContainerLowest,
    borderRadius: radius.lg,
    borderWidth: 1,
    borderColor: colors.outlineVariant,
    padding: spacing.stackMd,
    justifyContent: "space-between",
  },
  actionPrimary: {
    backgroundColor: colors.primary,
    borderColor: colors.primary,
  },
  actionLabel: {
    ...typography.labelMd,
    color: colors.onSurface,
    fontWeight: "600",
    fontFamily: "Inter_600SemiBold",
  },
  actionLabelPrimary: { color: colors.onPrimary },
  badge: {
    position: "absolute",
    top: 12,
    right: 12,
    width: 10,
    height: 10,
    borderRadius: 5,
    backgroundColor: colors.primaryContainer,
  },
});
