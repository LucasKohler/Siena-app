import { useMemo, useState } from "react";
import {
  ActivityIndicator,
  Pressable,
  ScrollView,
  StyleSheet,
  Text,
  View,
} from "react-native";
import { ApiError } from "../../src/api/client";
import * as endpoints from "../../src/api/endpoints";
import { useApi } from "../../src/api/useApi";
import { useAuth } from "../../src/auth/AuthContext";
import { ScreenState } from "../../src/components/ScreenState";
import { TRAINING_EVENT_TYPE } from "../../src/constants/domain";
import { colors, radius, spacing, typography } from "../../src/theme";

export default function AdminPresencasScreen() {
  const { token } = useAuth();
  const [selectedEventId, setSelectedEventId] = useState<string | null>(null);
  const [actionError, setActionError] = useState<string | null>(null);
  const [acting, setActing] = useState<string | null>(null);

  const { data: events, loading: eventsLoading, error: eventsError } = useApi(
    () => {
      if (!token) throw new Error("Não autenticado");
      return endpoints.listAdminEvents(token);
    },
    [token]
  );

  const trainings = useMemo(
    () => events?.filter((e) => e.type === TRAINING_EVENT_TYPE) ?? [],
    [events]
  );

  const {
    data: pending,
    loading: pendingLoading,
    error: pendingError,
    refetch: refetchPending,
  } = useApi(
    () => {
      if (!token || !selectedEventId) return Promise.resolve([]);
      return endpoints.listPendingAttendances(token, selectedEventId);
    },
    [token, selectedEventId]
  );

  const approve = async (userId: string, action: "approve" | "reject") => {
    if (!token || !selectedEventId) return;
    setActing(userId);
    setActionError(null);
    try {
      await endpoints.setAttendanceApproval(
        token,
        selectedEventId,
        userId,
        action
      );
      refetchPending();
    } catch (e) {
      setActionError(
        e instanceof ApiError
          ? e.message
          : e instanceof Error
            ? e.message
            : "Erro na aprovação"
      );
    } finally {
      setActing(null);
    }
  };

  return (
    <ScrollView style={styles.root} contentContainerStyle={styles.content}>
      <Text style={styles.section}>Treinos</Text>
      <ScreenState loading={eventsLoading} error={eventsError}>
        {trainings.length === 0 ? (
          <Text style={styles.hint}>Nenhum treino físico cadastrado.</Text>
        ) : (
          trainings.map((t) => (
            <Pressable
              key={t.id}
              style={[
                styles.trainingRow,
                selectedEventId === t.id && styles.trainingSelected,
              ]}
              onPress={() => setSelectedEventId(t.id)}
            >
              <Text style={styles.trainingTitle}>{t.title}</Text>
              <Text style={styles.trainingMeta}>{t.location}</Text>
            </Pressable>
          ))
        )}
      </ScreenState>

      {selectedEventId && (
        <>
          <Text style={styles.section}>Pendentes</Text>
          {actionError && <Text style={styles.error}>{actionError}</Text>}
          <ScreenState
            loading={pendingLoading}
            error={pendingError}
            empty={!pending?.length}
            emptyMessage="Nenhuma presença pendente neste treino."
          >
            {pending?.map((p) => (
              <View key={p.userId} style={styles.pendingRow}>
                <View style={styles.pendingInfo}>
                  <Text style={styles.name}>{p.displayName}</Text>
                  <Text style={styles.meta}>
                    {p.response}
                    {p.position ? ` • ${p.position}` : ""}
                  </Text>
                </View>
                <View style={styles.actions}>
                  <Pressable
                    style={styles.approveBtn}
                    onPress={() => approve(p.userId, "approve")}
                    disabled={acting === p.userId}
                  >
                    {acting === p.userId ? (
                      <ActivityIndicator color="#fff" size="small" />
                    ) : (
                      <Text style={styles.approveText}>Aprovar</Text>
                    )}
                  </Pressable>
                  <Pressable
                    style={styles.rejectBtn}
                    onPress={() => approve(p.userId, "reject")}
                    disabled={acting === p.userId}
                  >
                    <Text style={styles.rejectText}>Rejeitar</Text>
                  </Pressable>
                </View>
              </View>
            ))}
          </ScreenState>
        </>
      )}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  root: { flex: 1, backgroundColor: colors.surface },
  content: {
    padding: spacing.containerMarginMobile,
    paddingBottom: 40,
    gap: spacing.stackMd,
  },
  section: {
    ...typography.headlineMd,
    color: colors.onSurface,
    fontFamily: "Inter_600SemiBold",
  },
  hint: {
    ...typography.bodyMd,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_400Regular",
  },
  trainingRow: {
    padding: spacing.stackMd,
    borderRadius: radius.lg,
    borderWidth: 1,
    borderColor: colors.outlineVariant,
    marginBottom: 8,
  },
  trainingSelected: {
    borderColor: colors.primary,
    backgroundColor: colors.surfaceContainerLow,
  },
  trainingTitle: {
    ...typography.bodyMd,
    fontWeight: "600",
    fontFamily: "Inter_600SemiBold",
  },
  trainingMeta: {
    ...typography.labelSm,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_400Regular",
  },
  error: { ...typography.labelSm, color: colors.error },
  pendingRow: {
    flexDirection: "row",
    alignItems: "center",
    gap: 8,
    paddingVertical: spacing.stackMd,
    borderBottomWidth: 1,
    borderBottomColor: colors.surfaceVariant,
  },
  pendingInfo: { flex: 1 },
  name: {
    ...typography.bodyMd,
    fontWeight: "600",
    fontFamily: "Inter_600SemiBold",
  },
  meta: {
    ...typography.labelSm,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_400Regular",
  },
  actions: { flexDirection: "row", gap: 6 },
  approveBtn: {
    backgroundColor: colors.confirmGreen,
    paddingHorizontal: 10,
    paddingVertical: 6,
    borderRadius: radius.md,
    minWidth: 72,
    alignItems: "center",
  },
  approveText: {
    ...typography.labelSm,
    color: colors.onPrimary,
    fontWeight: "600",
  },
  rejectBtn: {
    borderWidth: 1,
    borderColor: colors.black,
    paddingHorizontal: 10,
    paddingVertical: 6,
    borderRadius: radius.md,
  },
  rejectText: { ...typography.labelSm, fontWeight: "600" },
});
