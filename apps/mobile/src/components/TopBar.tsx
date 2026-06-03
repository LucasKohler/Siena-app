import { MaterialIcons } from "@expo/vector-icons";
import { router } from "expo-router";
import { Pressable, StyleSheet, Text, View } from "react-native";
import { isStaffRole } from "../auth/roles";
import { useAuth } from "../auth/AuthContext";
import { colors, spacing, typography } from "../theme";

type Props = {
  title?: string;
  showLogo?: boolean;
  onAdminPress?: () => void;
  rightAction?: React.ReactNode;
};

export function TopBar({
  title = "Siena Voleibol",
  showLogo = true,
  onAdminPress,
  rightAction,
}: Props) {
  const { user, logout } = useAuth();
  const staff = isStaffRole(user?.role);

  const handleLogout = async () => {
    await logout();
    router.replace("/login");
  };

  return (
    <View style={styles.wrap}>
      <View style={styles.row}>
        {showLogo && (
          <View style={styles.logoCircle}>
            <MaterialIcons name="sports-volleyball" size={22} color={colors.primary} />
          </View>
        )}
        <Text style={styles.title}>{title}</Text>
        <View style={styles.actions}>
          {rightAction}
          {staff && onAdminPress && (
            <Pressable onPress={onAdminPress} hitSlop={8} style={styles.iconBtn}>
              <MaterialIcons name="admin-panel-settings" size={24} color={colors.onSurface} />
            </Pressable>
          )}
          <Pressable onPress={handleLogout} hitSlop={8} style={styles.iconBtn}>
            <MaterialIcons name="logout" size={22} color={colors.onSurfaceVariant} />
          </Pressable>
        </View>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  wrap: {
    paddingHorizontal: spacing.containerMarginMobile,
    paddingVertical: spacing.stackMd,
    backgroundColor: colors.surface,
    borderBottomWidth: 1,
    borderBottomColor: colors.outlineVariant,
  },
  row: {
    flexDirection: "row",
    alignItems: "center",
    gap: spacing.stackMd,
  },
  logoCircle: {
    width: 40,
    height: 40,
    borderRadius: 20,
    backgroundColor: colors.surfaceContainerLow,
    alignItems: "center",
    justifyContent: "center",
  },
  title: {
    flex: 1,
    ...typography.headlineMd,
    fontSize: 20,
    color: colors.onSurface,
  },
  actions: { flexDirection: "row", alignItems: "center", gap: 8 },
  iconBtn: { padding: 4 },
});
