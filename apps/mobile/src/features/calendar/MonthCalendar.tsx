import { MaterialIcons } from "@expo/vector-icons";
import { Pressable, StyleSheet, Text, View } from "react-native";
import {
  daysInMonth,
  firstWeekdayOfMonth,
  monthYearLabel,
} from "../../utils/date";
import { colors, radius, spacing, typography } from "../../theme";

const WEEKDAYS = ["D", "S", "T", "Q", "Q", "S", "S"];

type Props = {
  year: number;
  month: number;
  selectedDay: number;
  today: { year: number; month: number; day: number };
  daysWithEvents: Set<number>;
  onSelectDay: (day: number) => void;
  onPrevMonth: () => void;
  onNextMonth: () => void;
};

export function MonthCalendar({
  year,
  month,
  selectedDay,
  today,
  daysWithEvents,
  onSelectDay,
  onPrevMonth,
  onNextMonth,
}: Props) {
  const totalDays = daysInMonth(year, month);
  const leading = firstWeekdayOfMonth(year, month);
  const cells: (number | null)[] = [];
  for (let i = 0; i < leading; i++) cells.push(null);
  for (let d = 1; d <= totalDays; d++) cells.push(d);
  while (cells.length % 7 !== 0) cells.push(null);

  const isToday = (day: number) =>
    day === today.day && month === today.month && year === today.year;

  return (
    <View style={styles.wrap}>
      <View style={styles.header}>
        <Text style={styles.monthTitle}>{monthYearLabel(year, month)}</Text>
        <View style={styles.nav}>
          <Pressable onPress={onPrevMonth} style={styles.navBtn}>
            <MaterialIcons name="chevron-left" size={20} color={colors.onSurface} />
          </Pressable>
          <Pressable onPress={onNextMonth} style={styles.navBtn}>
            <MaterialIcons name="chevron-right" size={20} color={colors.onSurface} />
          </Pressable>
        </View>
      </View>
      <View style={styles.weekRow}>
        {WEEKDAYS.map((w, i) => (
          <Text key={`${w}-${i}`} style={styles.weekday}>
            {w}
          </Text>
        ))}
      </View>
      <View style={styles.grid}>
        {cells.map((day, index) => {
          if (day === null) {
            return <View key={`e-${index}`} style={styles.cell} />;
          }
          const selected = day === selectedDay;
          const todayMark = isToday(day);
          const hasEvent = daysWithEvents.has(day);
          return (
            <Pressable
              key={day}
              onPress={() => onSelectDay(day)}
              style={styles.cell}
            >
              <View
                style={[
                  styles.dayCircle,
                  selected && styles.daySelected,
                  todayMark && !selected && styles.dayToday,
                ]}
              >
                <Text
                  style={[
                    styles.dayText,
                    selected && styles.dayTextSelected,
                    todayMark && !selected && styles.dayTextToday,
                  ]}
                >
                  {day}
                </Text>
              </View>
              {hasEvent && <View style={styles.dot} />}
            </Pressable>
          );
        })}
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  wrap: { gap: spacing.stackMd },
  header: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  monthTitle: {
    ...typography.headlineMd,
    color: colors.onSurface,
    textTransform: "capitalize",
  },
  nav: { flexDirection: "row", gap: 8 },
  navBtn: {
    width: 32,
    height: 32,
    borderRadius: radius.full,
    borderWidth: 1,
    borderColor: colors.outlineVariant,
    alignItems: "center",
    justifyContent: "center",
  },
  weekRow: {
    flexDirection: "row",
    justifyContent: "space-around",
  },
  weekday: {
    ...typography.bodyMd,
    color: colors.onSurfaceVariant,
    width: 40,
    textAlign: "center",
  },
  grid: { flexDirection: "row", flexWrap: "wrap" },
  cell: {
    width: `${100 / 7}%`,
    height: 40,
    alignItems: "center",
    justifyContent: "center",
  },
  dayCircle: {
    width: 32,
    height: 32,
    borderRadius: radius.full,
    alignItems: "center",
    justifyContent: "center",
  },
  daySelected: { backgroundColor: colors.primary },
  dayToday: { borderWidth: 2, borderColor: colors.primary },
  dayText: { ...typography.bodyMd, color: colors.onSurface },
  dayTextSelected: { color: colors.onPrimary, fontWeight: "700" },
  dayTextToday: { color: colors.primary, fontWeight: "700" },
  dot: {
    position: "absolute",
    bottom: 2,
    width: 6,
    height: 6,
    borderRadius: 3,
    backgroundColor: colors.primary,
  },
});
