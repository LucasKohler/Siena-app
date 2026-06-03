import { MaterialIcons } from "@expo/vector-icons";
import * as Clipboard from "expo-clipboard";
import { router } from "expo-router";
import { useState } from "react";
import {
  Pressable,
  ScrollView,
  StyleSheet,
  Text,
  View,
} from "react-native";
import { useAuth } from "../../src/auth/AuthContext";
import { isStaffRole } from "../../src/auth/roles";
import { Card } from "../../src/components/Card";
import { TopBar } from "../../src/components/TopBar";
import { colors, radius, spacing, typography } from "../../src/theme";

/** Placeholder — Financeiro a definir em PRODUCT.md */
const PLACEHOLDER_CNPJ = "12.345.678/0001-90";
const PLACEHOLDER_AMOUNT = "R$ 150,00";

export default function FinanceiroScreen() {
  const { user } = useAuth();
  const [copied, setCopied] = useState(false);

  const copyCnpj = async () => {
    await Clipboard.setStringAsync(PLACEHOLDER_CNPJ);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <View style={styles.root}>
      <TopBar
        showLogo
        onAdminPress={
          isStaffRole(user?.role) ? () => router.push("/admin") : undefined
        }
      />
      <ScrollView contentContainerStyle={styles.content}>
        <Text style={styles.placeholderNote}>
          Conteúdo financeiro em definição — layout de referência Stitch.
        </Text>
        <Card style={styles.cardTop}>
          <View style={styles.decor} />
          <View style={styles.checkCircle}>
            <MaterialIcons name="check" size={40} color={colors.onPrimary} />
          </View>
        </Card>
        <Card>
          <Text style={styles.amount}>{PLACEHOLDER_AMOUNT}</Text>
          <View style={styles.copyRow}>
            <View style={styles.copyBox}>
              <Text style={styles.cnpj}>{PLACEHOLDER_CNPJ}</Text>
            </View>
            <Pressable onPress={copyCnpj} style={styles.copyBtn}>
              <MaterialIcons
                name={copied ? "done" : "content-copy"}
                size={22}
                color={colors.primary}
              />
            </Pressable>
          </View>
        </Card>
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  root: { flex: 1, backgroundColor: colors.surfaceContainerHigh },
  content: {
    padding: spacing.containerMarginMobile,
    gap: spacing.stackLg,
    paddingBottom: 100,
  },
  placeholderNote: {
    ...typography.labelSm,
    color: colors.onSurfaceVariant,
    textAlign: "center",
    fontFamily: "Inter_400Regular",
  },
  cardTop: {
    alignItems: "center",
    minHeight: 160,
    overflow: "hidden",
  },
  decor: {
    position: "absolute",
    top: -20,
    right: -20,
    width: 80,
    height: 80,
    borderRadius: 40,
    backgroundColor: `${colors.outlineVariant}66`,
  },
  checkCircle: {
    width: 72,
    height: 72,
    borderRadius: 36,
    backgroundColor: colors.primary,
    alignItems: "center",
    justifyContent: "center",
    marginTop: spacing.stackLg,
  },
  amount: {
    ...typography.headlineMd,
    color: colors.onSurface,
    fontFamily: "Inter_600SemiBold",
    marginBottom: spacing.stackMd,
  },
  copyRow: { flexDirection: "row", alignItems: "center", gap: 8 },
  copyBox: {
    flex: 1,
    backgroundColor: colors.surfaceContainer,
    borderRadius: radius.md,
    padding: spacing.stackMd,
    borderWidth: 1,
    borderColor: colors.outlineVariant,
  },
  cnpj: {
    ...typography.bodyMd,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_400Regular",
  },
  copyBtn: { padding: 8 },
});
