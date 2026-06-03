import { MaterialIcons } from "@expo/vector-icons";
import { router } from "expo-router";
import { useState } from "react";
import {
  KeyboardAvoidingView,
  Platform,
  ScrollView,
  StyleSheet,
  Text,
  TextInput,
  View,
} from "react-native";
import { ApiError } from "../src/api/client";
import { useAuth } from "../src/auth/AuthContext";
import { Button } from "../src/components/Button";
import { formatPhoneMask, normalizePhone } from "../src/utils/phone";
import { colors, radius, spacing, typography } from "../src/theme";

export default function LoginScreen() {
  const { login } = useAuth();
  const [phone, setPhone] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  const onContinue = async () => {
    const normalized = normalizePhone(phone);
    if (normalized.length < 12) {
      setError("Informe um telefone válido com DDD.");
      return;
    }
    setError(null);
    setSubmitting(true);
    try {
      await login(normalized);
      router.replace("/(tabs)/calendario");
    } catch (e) {
      if (e instanceof ApiError && e.status === 401) {
        setError("Telefone não autorizado. Verifique com a administração.");
      } else {
        setError(e instanceof Error ? e.message : "Não foi possível entrar.");
      }
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <KeyboardAvoidingView
      style={styles.root}
      behavior={Platform.OS === "ios" ? "padding" : undefined}
    >
      <ScrollView
        contentContainerStyle={styles.scroll}
        keyboardShouldPersistTaps="handled"
      >
        <View style={styles.hero}>
          <View style={styles.logoWrap}>
            <MaterialIcons name="sports-volleyball" size={72} color={colors.primary} />
          </View>
          <Text style={styles.title}>Bem-vindo ao Siena Voleibol</Text>
          <Text style={styles.subtitle}>
            Insira seu número de telefone para acessar a plataforma de gestão e
            desempenho.
          </Text>
        </View>

        <View style={styles.form}>
          <Text style={styles.label}>Telefone Celular</Text>
          <View style={styles.inputWrap}>
            <MaterialIcons
              name="call"
              size={20}
              color={colors.onSurfaceVariant}
              style={styles.inputIcon}
            />
            <TextInput
              style={styles.input}
              placeholder="+55 (00) 00000-0000"
              placeholderTextColor={colors.outline}
              keyboardType="phone-pad"
              value={phone}
              onChangeText={(t) => setPhone(formatPhoneMask(t))}
              autoComplete="tel"
            />
          </View>
          {error && <Text style={styles.error}>{error}</Text>}
          <Button
            label="Continuar"
            onPress={onContinue}
            loading={submitting}
            icon={
              <MaterialIcons name="arrow-forward" size={20} color={colors.onPrimary} />
            }
          />
        </View>

        <Text style={styles.legal}>
          Ao continuar, você concorda com nossos Termos de Serviço e Política de
          Privacidade.
        </Text>
      </ScrollView>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  root: { flex: 1, backgroundColor: colors.surface },
  scroll: {
    flexGrow: 1,
    justifyContent: "center",
    paddingHorizontal: spacing.containerMarginMobile,
    paddingVertical: spacing.sectionGap,
  },
  hero: { alignItems: "center", marginBottom: spacing.sectionGap },
  logoWrap: {
    width: 150,
    height: 150,
    borderRadius: 75,
    backgroundColor: colors.surfaceContainerLowest,
    borderWidth: 1,
    borderColor: colors.surfaceContainerHigh,
    alignItems: "center",
    justifyContent: "center",
    marginBottom: spacing.stackLg,
    shadowColor: "#000",
    shadowOffset: { width: 0, height: 8 },
    shadowOpacity: 0.06,
    shadowRadius: 24,
    elevation: 4,
  },
  title: {
    ...typography.headlineLgMobile,
    color: colors.onSurface,
    textAlign: "center",
    fontFamily: "Inter_600SemiBold",
  },
  subtitle: {
    ...typography.bodyMd,
    color: colors.onSurfaceVariant,
    textAlign: "center",
    marginTop: spacing.stackSm,
    maxWidth: 280,
    fontFamily: "Inter_400Regular",
  },
  form: { gap: spacing.stackLg },
  label: {
    ...typography.labelMd,
    color: colors.onSurfaceVariant,
    marginLeft: 4,
    fontFamily: "Inter_500Medium",
  },
  inputWrap: { position: "relative" },
  inputIcon: { position: "absolute", left: 16, top: 14, zIndex: 1 },
  input: {
    ...typography.bodyMd,
    backgroundColor: colors.surfaceContainerLowest,
    borderWidth: 1,
    borderColor: colors.outlineVariant,
    borderRadius: radius.lg,
    paddingVertical: 12,
    paddingLeft: 48,
    paddingRight: 16,
    color: colors.onSurface,
    fontFamily: "Inter_400Regular",
  },
  error: {
    ...typography.labelSm,
    color: colors.error,
    fontFamily: "Inter_400Regular",
  },
  legal: {
    ...typography.labelSm,
    color: colors.outline,
    textAlign: "center",
    marginTop: spacing.sectionGap,
    maxWidth: 250,
    alignSelf: "center",
    fontFamily: "Inter_400Regular",
  },
});
