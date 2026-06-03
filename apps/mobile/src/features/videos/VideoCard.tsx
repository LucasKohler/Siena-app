import { MaterialIcons } from "@expo/vector-icons";
import * as Linking from "expo-linking";
import { Pressable, StyleSheet, Text, View } from "react-native";
import type { VideoSummaryDto } from "../../api/types";
import {
  formatDuration,
  formatPublishedAgo,
  formatViews,
} from "../../utils/date";
import { colors, radius, spacing, typography } from "../../theme";

type Props = { video: VideoSummaryDto };

export function VideoCard({ video }: Props) {
  const openVideo = () => Linking.openURL(video.url);

  return (
    <View style={styles.card}>
      <Pressable onPress={openVideo} style={styles.thumb}>
        <View style={styles.thumbPlaceholder}>
          <View style={styles.play}>
            <MaterialIcons name="play-arrow" size={32} color={colors.onPrimary} />
          </View>
        </View>
        <View style={styles.duration}>
          <Text style={styles.durationText}>
            {formatDuration(video.durationSeconds)}
          </Text>
        </View>
      </Pressable>
      <View style={styles.body}>
        <Text style={styles.title} numberOfLines={2}>
          {video.title}
        </Text>
        <View style={styles.footer}>
          <Text style={styles.meta}>
            {formatPublishedAgo(video.publishedAt)} • {formatViews(video.views)}
          </Text>
          <Pressable onPress={openVideo} style={styles.watchBtn}>
            <Text style={styles.watchText}>Assistir</Text>
          </Pressable>
        </View>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  card: {
    backgroundColor: colors.surfaceContainerLowest,
    borderRadius: radius.lg,
    borderWidth: 1,
    borderColor: colors.surfaceVariant,
    overflow: "hidden",
  },
  thumb: { aspectRatio: 16 / 9, backgroundColor: colors.surfaceVariant },
  thumbPlaceholder: {
    flex: 1,
    backgroundColor: colors.surfaceContainer,
    alignItems: "center",
    justifyContent: "center",
  },
  play: {
    width: 56,
    height: 56,
    borderRadius: 28,
    backgroundColor: `${colors.primaryContainer}E6`,
    alignItems: "center",
    justifyContent: "center",
  },
  duration: {
    position: "absolute",
    bottom: 8,
    right: 8,
    backgroundColor: "rgba(0,0,0,0.8)",
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: radius.sm,
  },
  durationText: { ...typography.labelSm, color: "#fff" },
  body: { padding: spacing.stackMd, gap: spacing.stackSm },
  title: {
    ...typography.bodyLg,
    fontWeight: "600",
    color: colors.onSurface,
  },
  footer: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
    gap: 8,
  },
  meta: {
    ...typography.labelSm,
    color: colors.onSurfaceVariant,
    flex: 1,
  },
  watchBtn: {
    borderWidth: 1,
    borderColor: colors.primary,
    borderRadius: radius.full,
    paddingHorizontal: 16,
    paddingVertical: 6,
  },
  watchText: {
    ...typography.labelMd,
    color: colors.primary,
    fontWeight: "600",
  },
});
