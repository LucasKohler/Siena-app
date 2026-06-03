import { MaterialIcons } from "@expo/vector-icons";
import { Pressable, StyleSheet, Text, View } from "react-native";
import type { EventSummaryDto } from "../../api/types";
import { formatEventDateBlock, formatEventTime } from "../../utils/date";
import { colors, radius, spacing, typography } from "../../theme";

type Props = {
  event: EventSummaryDto;
  onPress?: () => void;
};

export function EventCard({ event, onPress }: Props) {
  const { day, month } = formatEventDateBlock(event.startsAt);
  return (
    <Pressable onPress={onPress} style={styles.card}>
      <View style={styles.dateBlock}>
        <Text style={styles.day}>{day}</Text>
        <Text style={styles.month}>{month}</Text>
      </View>
      <View style={styles.body}>
        <View style={styles.chip}>
          <Text style={styles.chipText}>{event.type}</Text>
        </View>
        <Text style={styles.title} numberOfLines={2}>
          {event.title}
        </Text>
        <View style={styles.meta}>
          <MaterialIcons name="schedule" size={16} color={colors.onSurfaceVariant} />
          <Text style={styles.metaText}>{formatEventTime(event.startsAt)}</Text>
          <MaterialIcons name="location-on" size={16} color={colors.onSurfaceVariant} />
          <Text style={styles.metaText} numberOfLines={1}>
            {event.location}
          </Text>
        </View>
      </View>
    </Pressable>
  );
}

const styles = StyleSheet.create({
  card: {
    flexDirection: "row",
    gap: spacing.gutter,
    backgroundColor: colors.surfaceContainerLowest,
    borderRadius: radius.lg,
    borderWidth: 1,
    borderColor: colors.outlineVariant,
    padding: 12,
  },
  dateBlock: {
    width: 56,
    height: 56,
    borderRadius: radius.lg,
    backgroundColor: colors.surfaceContainerLow,
    borderWidth: 1,
    borderColor: colors.outlineVariant,
    alignItems: "center",
    justifyContent: "center",
  },
  day: {
    ...typography.headlineMd,
    fontSize: 22,
    color: colors.onSurface,
    lineHeight: 26,
  },
  month: {
    ...typography.labelSm,
    color: colors.onSurfaceVariant,
    textTransform: "uppercase",
  },
  body: { flex: 1, gap: 4 },
  chip: {
    alignSelf: "flex-start",
    backgroundColor: colors.surfaceVariant,
    paddingHorizontal: 8,
    paddingVertical: 2,
    borderRadius: radius.full,
  },
  chipText: { ...typography.labelSm, color: colors.onSurfaceVariant },
  title: {
    ...typography.bodyLg,
    fontWeight: "700",
    color: colors.onSurface,
  },
  meta: {
    flexDirection: "row",
    alignItems: "center",
    flexWrap: "wrap",
    gap: 4,
  },
  metaText: {
    ...typography.labelMd,
    color: colors.onSurfaceVariant,
    marginRight: 8,
  },
});
