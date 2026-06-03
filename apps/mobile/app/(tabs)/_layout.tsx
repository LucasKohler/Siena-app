import { MaterialIcons } from "@expo/vector-icons";
import { Redirect, Tabs } from "expo-router";
import { StyleSheet, View } from "react-native";
import { useAuth } from "../../src/auth/AuthContext";
import { colors, typography } from "../../src/theme";

export default function TabsLayout() {
  const { token, loading } = useAuth();

  if (!loading && !token) {
    return <Redirect href="/login" />;
  }

  return (
    <Tabs
      screenOptions={{
        headerShown: false,
        tabBarActiveTintColor: colors.onPrimaryContainer,
        tabBarInactiveTintColor: colors.onSurfaceVariant,
        tabBarStyle: {
          backgroundColor: colors.surface,
          borderTopColor: colors.outlineVariant,
          paddingTop: 8,
          paddingBottom: 8,
          height: 72,
        },
        tabBarLabelStyle: {
          ...typography.labelSm,
          fontFamily: "Inter_500Medium",
        },
      }}
    >
      <Tabs.Screen
        name="financeiro"
        options={{
          title: "Financeiro",
          tabBarIcon: ({ color, focused }) => (
            <TabIcon name="payments" color={color} focused={focused} />
          ),
        }}
      />
      <Tabs.Screen
        name="calendario"
        options={{
          title: "Calendário",
          tabBarIcon: ({ color, focused }) => (
            <TabIcon name="calendar-month" color={color} focused={focused} />
          ),
        }}
      />
      <Tabs.Screen
        name="videos"
        options={{
          title: "Vídeos",
          tabBarIcon: ({ color, focused }) => (
            <TabIcon name="videocam" color={color} focused={focused} />
          ),
        }}
      />
    </Tabs>
  );
}

function TabIcon({
  name,
  color,
  focused,
}: {
  name: keyof typeof MaterialIcons.glyphMap;
  color: string;
  focused: boolean;
}) {
  if (focused) {
    return (
      <View style={styles.activeWrap}>
        <MaterialIcons
          name={name}
          size={24}
          color={colors.onPrimaryContainer}
        />
      </View>
    );
  }
  return <MaterialIcons name={name} size={24} color={color} />;
}

const styles = StyleSheet.create({
  activeWrap: {
    backgroundColor: colors.primaryContainer,
    borderRadius: 12,
    paddingHorizontal: 12,
    paddingVertical: 6,
  },
});
