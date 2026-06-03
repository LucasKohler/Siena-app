import { MaterialIcons } from "@expo/vector-icons";
import { router, Stack } from "expo-router";
import { useState } from "react";
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
import { TopBar } from "../../src/components/TopBar";
import { formatTrainingWhen } from "../../src/utils/date";
import { colors, radius, spacing, typography } from "../../src/theme";

const STATUS_GOING = "Eu vou";
const STATUS_NOT_GOING = "Não vou";

export default function PresencaScreen() {
  const { token } = useAuth();
  const [submitting, setSubmitting] = useState(false);
  const [actionError, setActionError] = useState<string | null>(null);

  const { data: training, loading, error, refetch } = useApi(
    () => {
      if (!token) throw new Error("Não autenticado");
      return endpoints.getNextTraining(token);
    },
    [token]
  );

  const setAttendance = async (status: string) => {
    if (!token || !training) return;
    setSubmitting(true);
    setActionError(null);
    try {
      await endpoints.setTrainingAttendance(training.eventId, status, token);
      refetch();
    } catch (e) {
      if (e instanceof ApiError && e.status === 409) {
        setActionError("O treino já começou. Não é possível alterar a presença.");
      } else if (e instanceof ApiError && e.status === 403) {
        setActionError("Apenas atletas podem confirmar presença.");
      } else {
        setActionError(e instanceof Error ? e.message : "Erro ao salvar.");
      }
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <>
      <Stack.Screen options={{ headerShown: false }} />
      <View style={styles.root}>
        <TopBar title="Siena Voleibol" />
        <ScrollView contentContainerStyle={styles.content}>
          <ScreenState
            loading={loading}
            error={
              error?.includes("404") || error?.toLowerCase().includes("not found")
                ? "Nenhum treino físico agendado."
                : error
            }
            empty={!training}
            emptyMessage="Nenhum treino físico agendado."
          >
            {training && (
              <>
                <View style={styles.card}>
                  <View style={styles.cardHeader}>
                    <View>
                      <Text style={styles.cardTitle}>Próximo Treino</Text>
                      <View style={styles.whenRow}>
                        <MaterialIcons
                          name="event"
                          size={18}
                          color={colors.primary}
                        />
                        <Text style={styles.when}>
                          {formatTrainingWhen(training.startsAt)}
                        </Text>
                      </View>
                    </View>
                    <View style={styles.locationChip}>
                      <Text style={styles.locationText}>{training.location}</Text>
                    </View>
                  </View>
                  {actionError && (
                    <Text style={styles.actionError}>{actionError}</Text>
                  )}
                  <View style={styles.actions}>
                    <Pressable
                      style={[styles.btnGoing, submitting && styles.disabled]}
                      onPress={() => setAttendance(STATUS_GOING)}
                      disabled={submitting}
                    >
                      {submitting ? (
                        <ActivityIndicator color="#fff" />
                      ) : (
                        <>
                          <MaterialIcons name="check-circle" size={22} color="#fff" />
                          <Text style={styles.btnGoingText}>Eu vou</Text>
                        </>
                      )}
                    </Pressable>
                    <Pressable
                      style={[styles.btnSkip, submitting && styles.disabled]}
                      onPress={() => setAttendance(STATUS_NOT_GOING)}
                      disabled={submitting}
                    >
                      <MaterialIcons name="cancel" size={22} color={colors.onSurface} />
                      <Text style={styles.btnSkipText}>Não vou</Text>
                    </Pressable>
                  </View>
                  {training.myStatus && (
                    <Text style={styles.myStatus}>
                      Sua resposta: {training.myStatus}
                      {training.myApprovalStatus
                        ? ` (${training.myApprovalStatus})`
                        : ""}
                    </Text>
                  )}
                </View>

                <Text style={styles.listTitle}>
                  Confirmados ({training.confirmed.length})
                </Text>
                <View style={styles.listCard}>
                  {training.confirmed.map((a, i) => (
                    <View
                      key={`${a.displayName}-${i}`}
                      style={[
                        styles.row,
                        i < training.confirmed.length - 1 && styles.rowBorder,
                      ]}
                    >
                      <View style={styles.avatar}>
                        <Text style={styles.avatarText}>
                          {a.displayName.slice(0, 2).toUpperCase()}
                        </Text>
                      </View>
                      <View style={styles.rowBody}>
                        <Text style={styles.name}>{a.displayName}</Text>
                        {a.position && (
                          <View style={styles.posChip}>
                            <Text style={styles.posText}>{a.position}</Text>
                          </View>
                        )}
                      </View>
                      <MaterialIcons
                        name="check"
                        size={22}
                        color={colors.confirmGreen}
                      />
                    </View>
                  ))}
                </View>
              </>
            )}
          </ScreenState>
          <Pressable onPress={() => router.back()} style={styles.back}>
            <Text style={styles.backText}>Voltar ao calendário</Text>
          </Pressable>
        </ScrollView>
      </View>
    </>
  );
}

const styles = StyleSheet.create({
  root: { flex: 1, backgroundColor: colors.surface },
  content: {
    padding: spacing.containerMarginMobile,
    paddingBottom: 40,
    gap: spacing.stackLg,
  },
  card: {
    backgroundColor: colors.surfaceContainerLowest,
    borderRadius: radius.lg,
    borderWidth: 1,
    borderColor: colors.surfaceVariant,
    padding: spacing.stackLg,
  },
  cardHeader: {
    flexDirection: "row",
    justifyContent: "space-between",
    marginBottom: spacing.stackMd,
  },
  cardTitle: {
    ...typography.headlineMd,
    color: colors.onSurface,
    fontFamily: "Inter_600SemiBold",
  },
  whenRow: { flexDirection: "row", alignItems: "center", gap: 6, marginTop: 4 },
  when: {
    ...typography.bodyMd,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_400Regular",
  },
  locationChip: {
    backgroundColor: colors.surfaceContainer,
    paddingHorizontal: 12,
    paddingVertical: 4,
    borderRadius: radius.full,
  },
  locationText: {
    ...typography.labelSm,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_500Medium",
  },
  actionError: {
    ...typography.labelSm,
    color: colors.error,
    marginBottom: 8,
    fontFamily: "Inter_400Regular",
  },
  actions: { flexDirection: "row", gap: spacing.gutter, marginTop: spacing.stackLg },
  btnGoing: {
    flex: 1,
    backgroundColor: colors.confirmGreen,
    borderWidth: 1,
    borderColor: colors.confirmGreenDark,
    borderRadius: radius.md,
    paddingVertical: 12,
    alignItems: "center",
    gap: 4,
  },
  btnGoingText: {
    ...typography.labelMd,
    color: colors.onPrimary,
    fontWeight: "600",
    fontFamily: "Inter_600SemiBold",
  },
  btnSkip: {
    flex: 1,
    borderWidth: 2,
    borderColor: colors.black,
    borderRadius: radius.md,
    paddingVertical: 12,
    alignItems: "center",
    gap: 4,
  },
  btnSkipText: {
    ...typography.labelMd,
    color: colors.onSurface,
    fontWeight: "600",
    fontFamily: "Inter_600SemiBold",
  },
  disabled: { opacity: 0.6 },
  myStatus: {
    ...typography.labelSm,
    color: colors.onSurfaceVariant,
    marginTop: spacing.stackMd,
    textAlign: "center",
    fontFamily: "Inter_400Regular",
  },
  listTitle: {
    ...typography.headlineMd,
    color: colors.onSurface,
    fontFamily: "Inter_600SemiBold",
  },
  listCard: {
    backgroundColor: colors.surfaceContainerLowest,
    borderRadius: radius.lg,
    borderWidth: 1,
    borderColor: colors.surfaceVariant,
    overflow: "hidden",
  },
  row: {
    flexDirection: "row",
    alignItems: "center",
    padding: spacing.stackMd,
    gap: spacing.stackMd,
  },
  rowBorder: {
    borderBottomWidth: 1,
    borderBottomColor: colors.surfaceVariant,
  },
  avatar: {
    width: 48,
    height: 48,
    borderRadius: 24,
    backgroundColor: colors.surfaceContainer,
    alignItems: "center",
    justifyContent: "center",
  },
  avatarText: {
    ...typography.labelMd,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_600SemiBold",
  },
  rowBody: { flex: 1 },
  name: {
    ...typography.bodyMd,
    fontWeight: "600",
    color: colors.onSurface,
    fontFamily: "Inter_600SemiBold",
  },
  posChip: {
    alignSelf: "flex-start",
    marginTop: 4,
    backgroundColor: colors.surfaceContainerLow,
    paddingHorizontal: 8,
    paddingVertical: 2,
    borderRadius: radius.full,
  },
  posText: {
    ...typography.labelSm,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_400Regular",
  },
  back: { alignItems: "center", padding: spacing.stackMd },
  backText: {
    ...typography.labelMd,
    color: colors.primary,
    fontFamily: "Inter_500Medium",
  },
});
