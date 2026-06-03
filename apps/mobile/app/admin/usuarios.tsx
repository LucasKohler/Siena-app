import { FlatList, StyleSheet, Text, View } from "react-native";
import * as endpoints from "../../src/api/endpoints";
import { useApi } from "../../src/api/useApi";
import { useAuth } from "../../src/auth/AuthContext";
import { ScreenState } from "../../src/components/ScreenState";
import { colors, spacing, typography } from "../../src/theme";

export default function AdminUsuariosScreen() {
  const { token } = useAuth();
  const { data, loading, error } = useApi(
    () => {
      if (!token) throw new Error("Não autenticado");
      return endpoints.listAdminUsers(token, true);
    },
    [token]
  );

  return (
    <ScreenState loading={loading} error={error}>
      <FlatList
        data={data ?? []}
        keyExtractor={(item) => item.id}
        contentContainerStyle={styles.list}
        renderItem={({ item }) => (
          <View style={styles.row}>
            <Text style={styles.name}>{item.displayName}</Text>
            <Text style={styles.meta}>
              {item.role} • {item.phoneNumber}
              {!item.isActive ? " • inativo" : ""}
            </Text>
          </View>
        )}
      />
    </ScreenState>
  );
}

const styles = StyleSheet.create({
  list: { padding: spacing.containerMarginMobile, backgroundColor: colors.surface },
  row: {
    padding: spacing.stackMd,
    borderBottomWidth: 1,
    borderBottomColor: colors.surfaceVariant,
  },
  name: { ...typography.bodyMd, fontWeight: "600", fontFamily: "Inter_600SemiBold" },
  meta: { ...typography.labelSm, color: colors.onSurfaceVariant, fontFamily: "Inter_400Regular" },
});
