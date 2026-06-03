import { StyleSheet, Text, View } from "react-native";
import { colors, spacing, typography } from "../../src/theme";

export default function AdminRelatoriosStub() {
  return (
    <View style={styles.box}>
      <Text style={styles.title}>Relatórios & Financeiro</Text>
      <Text style={styles.body}>
        Módulo financeiro ainda sem spec de produto — placeholder alinhado ao Stitch.
      </Text>
    </View>
  );
}

const styles = StyleSheet.create({
  box: { flex: 1, padding: spacing.containerMarginMobile, backgroundColor: colors.surface },
  title: { ...typography.headlineMd, fontFamily: "Inter_600SemiBold" },
  body: { ...typography.bodyMd, marginTop: 8, fontFamily: "Inter_400Regular" },
});
