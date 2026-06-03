import { MaterialIcons } from "@expo/vector-icons";
import * as Linking from "expo-linking";
import { router } from "expo-router";
import { ScrollView, StyleSheet, Text, View } from "react-native";
import * as endpoints from "../../src/api/endpoints";
import { useApi } from "../../src/api/useApi";
import { useAuth } from "../../src/auth/AuthContext";
import { isStaffRole } from "../../src/auth/roles";
import { ScreenState } from "../../src/components/ScreenState";
import { TopBar } from "../../src/components/TopBar";
import { VideoCard } from "../../src/features/videos/VideoCard";
import { colors, spacing, typography } from "../../src/theme";

const CHANNEL_URL = "https://www.youtube.com";

export default function VideosScreen() {
  const { user } = useAuth();
  const { data: videos, loading, error } = useApi(() => endpoints.getVideos(), []);

  return (
    <View style={styles.root}>
      <TopBar
        onAdminPress={
          isStaffRole(user?.role) ? () => router.push("/admin") : undefined
        }
      />
      <ScrollView contentContainerStyle={styles.content}>
        <View style={styles.headerRow}>
          <Text style={styles.sectionTitle}>Canal Oficial</Text>
          <Text
            style={styles.subscribe}
            onPress={() => Linking.openURL(CHANNEL_URL)}
          >
            Inscrever-se{" "}
            <MaterialIcons name="open-in-new" size={14} color={colors.primary} />
          </Text>
        </View>
        <ScreenState
          loading={loading}
          error={error}
          empty={!videos?.length}
          emptyMessage="Nenhum vídeo publicado."
        >
          <View style={styles.list}>
            {videos?.map((v) => (
              <VideoCard key={v.id} video={v} />
            ))}
          </View>
        </ScreenState>
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  root: { flex: 1, backgroundColor: colors.surfaceContainerHigh },
  content: {
    padding: spacing.containerMarginMobile,
    paddingBottom: 100,
    gap: spacing.stackLg,
  },
  headerRow: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "flex-end",
  },
  sectionTitle: {
    ...typography.headlineLgMobile,
    color: colors.onSurface,
    fontFamily: "Inter_600SemiBold",
  },
  subscribe: {
    ...typography.labelMd,
    color: colors.primary,
    fontFamily: "Inter_500Medium",
  },
  list: { gap: spacing.stackLg },
});
