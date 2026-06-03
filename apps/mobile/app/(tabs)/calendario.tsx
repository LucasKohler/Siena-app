import { router } from "expo-router";
import { useMemo, useState } from "react";
import { Pressable, ScrollView, StyleSheet, Text, View } from "react-native";
import * as endpoints from "../../src/api/endpoints";
import { useApi } from "../../src/api/useApi";
import { useAuth } from "../../src/auth/AuthContext";
import { isStaffRole } from "../../src/auth/roles";
import { Chip } from "../../src/components/Chip";
import { ScreenState } from "../../src/components/ScreenState";
import { TopBar } from "../../src/components/TopBar";
import { EventCard } from "../../src/features/calendar/EventCard";
import { MonthCalendar } from "../../src/features/calendar/MonthCalendar";
import { colors, spacing, typography } from "../../src/theme";

const CATEGORIES = ["Masculino", "Feminino"] as const;

export default function CalendarioScreen() {
  const { user } = useAuth();
  const now = new Date();
  const [year, setYear] = useState(now.getFullYear());
  const [month, setMonth] = useState(now.getMonth());
  const [selectedDay, setSelectedDay] = useState(now.getDate());
  const [category, setCategory] = useState<string>(CATEGORIES[0]);

  const { data: events, loading, error } = useApi(
    () => endpoints.getEvents(),
    []
  );

  const filtered = useMemo(() => {
    if (!events) return [];
    return events
      .filter((e) => e.category === category)
      .sort(
        (a, b) =>
          new Date(a.startsAt).getTime() - new Date(b.startsAt).getTime()
      );
  }, [events, category]);

  const daysWithEvents = useMemo(() => {
    const set = new Set<number>();
    for (const e of filtered) {
      const d = new Date(e.startsAt);
      if (d.getFullYear() === year && d.getMonth() === month) {
        set.add(d.getDate());
      }
    }
    return set;
  }, [filtered, year, month]);

  const upcoming = useMemo(() => {
    const start = new Date(year, month, selectedDay);
    start.setHours(0, 0, 0, 0);
    return filtered.filter((e) => new Date(e.startsAt) >= start).slice(0, 10);
  }, [filtered, year, month, selectedDay]);

  const prevMonth = () => {
    if (month === 0) {
      setMonth(11);
      setYear((y) => y - 1);
    } else setMonth((m) => m - 1);
  };

  const nextMonth = () => {
    if (month === 11) {
      setMonth(0);
      setYear((y) => y + 1);
    } else setMonth((m) => m + 1);
  };

  return (
    <View style={styles.root}>
      <TopBar
        onAdminPress={
          isStaffRole(user?.role) ? () => router.push("/admin") : undefined
        }
      />
      <ScrollView contentContainerStyle={styles.content}>
        <View style={styles.chips}>
          {CATEGORIES.map((c) => (
            <Chip
              key={c}
              label={c}
              selected={category === c}
              onPress={() => setCategory(c)}
            />
          ))}
        </View>

        <ScreenState loading={loading} error={error}>
          <View style={styles.calendarCard}>
            <MonthCalendar
              year={year}
              month={month}
              selectedDay={selectedDay}
              today={{
                year: now.getFullYear(),
                month: now.getMonth(),
                day: now.getDate(),
              }}
              daysWithEvents={daysWithEvents}
              onSelectDay={setSelectedDay}
              onPrevMonth={prevMonth}
              onNextMonth={nextMonth}
            />
          </View>

          <Pressable
            style={styles.trainingCta}
            onPress={() => router.push("/treino/presenca")}
          >
            <Text style={styles.trainingCtaText}>Presença no próximo treino</Text>
          </Pressable>

          <Text style={styles.sectionTitle}>Próximos Eventos</Text>
          <View style={styles.list}>
            {upcoming.length === 0 ? (
              <Text style={styles.emptyList}>Nenhum evento neste período.</Text>
            ) : (
              upcoming.map((ev) => (
                <EventCard key={ev.id} event={ev} />
              ))
            )}
          </View>
        </ScreenState>
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  root: { flex: 1, backgroundColor: colors.surface },
  content: {
    padding: spacing.containerMarginMobile,
    paddingBottom: 100,
    gap: spacing.stackLg,
  },
  chips: { flexDirection: "row", flexWrap: "wrap", gap: 8 },
  calendarCard: {
    backgroundColor: colors.surfaceContainerLowest,
    borderRadius: 16,
    borderWidth: 1,
    borderColor: colors.outlineVariant,
    padding: spacing.stackMd,
  },
  trainingCta: {
    backgroundColor: colors.primaryContainer,
    padding: spacing.stackMd,
    borderRadius: 12,
    alignItems: "center",
  },
  trainingCtaText: {
    ...typography.labelMd,
    color: colors.onPrimaryContainer,
    fontFamily: "Inter_600SemiBold",
  },
  sectionTitle: {
    ...typography.headlineMd,
    color: colors.onSurface,
    fontFamily: "Inter_600SemiBold",
  },
  list: { gap: spacing.stackMd },
  emptyList: {
    ...typography.bodyMd,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_400Regular",
  },
});
