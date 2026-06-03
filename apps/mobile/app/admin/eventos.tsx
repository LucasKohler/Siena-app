import { useState } from "react";
import {
  Alert,
  ScrollView,
  StyleSheet,
  Text,
  View,
} from "react-native";
import { ApiError } from "../../src/api/client";
import * as endpoints from "../../src/api/endpoints";
import { useApi } from "../../src/api/useApi";
import { useAuth } from "../../src/auth/AuthContext";
import { Button } from "../../src/components/Button";
import { Chip } from "../../src/components/Chip";
import { FormField } from "../../src/components/FormField";
import { ScreenState } from "../../src/components/ScreenState";
import {
  EVENT_CATEGORIES,
  EVENT_TYPES,
} from "../../src/constants/domain";
import { colors, spacing, typography } from "../../src/theme";

export default function AdminEventosScreen() {
  const { token } = useAuth();
  const { data: events, loading, error, refetch } = useApi(
    () => {
      if (!token) throw new Error("Não autenticado");
      return endpoints.listAdminEvents(token);
    },
    [token]
  );

  const [id, setId] = useState("");
  const [title, setTitle] = useState("");
  const [type, setType] = useState<string>(EVENT_TYPES[1]);
  const [category, setCategory] = useState<string>(EVENT_CATEGORIES[0]);
  const [startsAt, setStartsAt] = useState("2026-06-20T19:00:00-03:00");
  const [location, setLocation] = useState("Ginásio Principal");
  const [opponent, setOpponent] = useState("");
  const [description, setDescription] = useState("");
  const [saving, setSaving] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);

  const submit = async () => {
    if (!token) return;
    if (!id.trim() || !title.trim()) {
      setFormError("Id e título são obrigatórios.");
      return;
    }
    setSaving(true);
    setFormError(null);
    try {
      await endpoints.createAdminEvent(token, {
        id: id.trim(),
        title: title.trim(),
        type,
        category,
        startsAt,
        location: location.trim(),
        opponent: opponent.trim() || null,
        description: description.trim() || null,
      });
      Alert.alert("Sucesso", "Evento criado.");
      setId("");
      setTitle("");
      refetch();
    } catch (e) {
      setFormError(
        e instanceof ApiError
          ? e.message
          : e instanceof Error
            ? e.message
            : "Erro ao criar evento"
      );
    } finally {
      setSaving(false);
    }
  };

  return (
    <ScrollView style={styles.root} contentContainerStyle={styles.content}>
      <Text style={styles.section}>Novo evento</Text>
      <FormField label="Id (slug único)" value={id} onChangeText={setId} placeholder="treino-2026-06-20" />
      <FormField label="Título" value={title} onChangeText={setTitle} />
      <Text style={styles.label}>Tipo</Text>
      <View style={styles.chips}>
        {EVENT_TYPES.map((t) => (
          <Chip key={t} label={t} selected={type === t} onPress={() => setType(t)} />
        ))}
      </View>
      <Text style={styles.label}>Categoria</Text>
      <View style={styles.chips}>
        {EVENT_CATEGORIES.map((c) => (
          <Chip key={c} label={c} selected={category === c} onPress={() => setCategory(c)} />
        ))}
      </View>
      <FormField
        label="Início (ISO 8601)"
        value={startsAt}
        onChangeText={setStartsAt}
        placeholder="2026-06-20T19:00:00-03:00"
      />
      <FormField label="Local" value={location} onChangeText={setLocation} />
      <FormField label="Adversário (opcional)" value={opponent} onChangeText={setOpponent} />
      <FormField
        label="Descrição (opcional)"
        value={description}
        onChangeText={setDescription}
        multiline
      />
      {formError && <Text style={styles.error}>{formError}</Text>}
      <Button label="Criar evento" onPress={submit} loading={saving} />

      <Text style={[styles.section, styles.listTitle]}>Eventos cadastrados</Text>
      <ScreenState loading={loading} error={error}>
        {events?.map((ev) => (
          <View key={ev.id} style={styles.row}>
            <Text style={styles.rowTitle}>{ev.title}</Text>
            <Text style={styles.rowMeta}>
              {ev.type} • {ev.category} • {ev.id}
            </Text>
          </View>
        ))}
      </ScreenState>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  root: { flex: 1, backgroundColor: colors.surface },
  content: {
    padding: spacing.containerMarginMobile,
    paddingBottom: 40,
    gap: spacing.stackMd,
  },
  section: {
    ...typography.headlineMd,
    color: colors.onSurface,
    fontFamily: "Inter_600SemiBold",
  },
  listTitle: { marginTop: spacing.stackLg },
  label: {
    ...typography.labelMd,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_500Medium",
  },
  chips: { flexDirection: "row", flexWrap: "wrap", gap: 8 },
  error: { ...typography.labelSm, color: colors.error },
  row: {
    padding: spacing.stackMd,
    borderBottomWidth: 1,
    borderBottomColor: colors.surfaceVariant,
  },
  rowTitle: {
    ...typography.bodyMd,
    fontWeight: "600",
    fontFamily: "Inter_600SemiBold",
  },
  rowMeta: {
    ...typography.labelSm,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_400Regular",
  },
});
