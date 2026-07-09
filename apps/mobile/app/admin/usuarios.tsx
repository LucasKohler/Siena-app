import { useState } from "react";
import {
  Alert,
  FlatList,
  StyleSheet,
  Text,
  View,
} from "react-native";
import { ApiError } from "../../src/api/client";
import * as endpoints from "../../src/api/endpoints";
import type { UserSummaryDto } from "../../src/api/types";
import { useApi } from "../../src/api/useApi";
import { useAuth } from "../../src/auth/AuthContext";
import { Button } from "../../src/components/Button";
import { Chip } from "../../src/components/Chip";
import { FormField } from "../../src/components/FormField";
import { ScreenState } from "../../src/components/ScreenState";
import {
  PLAYER_POSITIONS,
  USER_ROLES,
} from "../../src/constants/domain";
import { colors, spacing, typography } from "../../src/theme";

function formatApiError(e: unknown, fallback: string) {
  if (e instanceof ApiError) {
    return e.message;
  }
  if (e instanceof Error) {
    return e.message;
  }
  return fallback;
}

export default function AdminUsuariosScreen() {
  const { token } = useAuth();
  const { data, loading, error, refetch } = useApi(
    () => {
      if (!token) throw new Error("Não autenticado");
      return endpoints.listAdminUsers(token, true);
    },
    [token]
  );

  const [editingId, setEditingId] = useState<string | null>(null);
  const [userId, setUserId] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("+5511");
  const [displayName, setDisplayName] = useState("");
  const [role, setRole] = useState<string>(USER_ROLES[0]);
  const [position, setPosition] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);

  const resetForm = () => {
    setEditingId(null);
    setUserId("");
    setPhoneNumber("+5511");
    setDisplayName("");
    setRole(USER_ROLES[0]);
    setPosition(null);
    setFormError(null);
  };

  const startEdit = (user: UserSummaryDto) => {
    setEditingId(user.id);
    setUserId(user.id);
    setPhoneNumber(user.phoneNumber);
    setDisplayName(user.displayName);
    setRole(user.role);
    setPosition(user.position);
    setFormError(null);
  };

  const submit = async () => {
    if (!token) return;
    if (!displayName.trim() || !userId.trim()) {
      setFormError("Id e nome são obrigatórios.");
      return;
    }
    if (!editingId && !phoneNumber.trim()) {
      setFormError("Telefone é obrigatório para novo usuário.");
      return;
    }

    setSaving(true);
    setFormError(null);
    try {
      const positionValue =
        role === "Atleta" ? position : null;

      if (editingId) {
        await endpoints.updateAdminUser(token, editingId, {
          displayName: displayName.trim(),
          role,
          position: positionValue,
        });
        Alert.alert("Sucesso", "Usuário atualizado.");
      } else {
        await endpoints.createAdminUser(token, {
          id: userId.trim(),
          phoneNumber: phoneNumber.trim(),
          displayName: displayName.trim(),
          role,
          position: positionValue,
        });
        Alert.alert("Sucesso", "Usuário criado.");
      }
      resetForm();
      refetch();
    } catch (e) {
      setFormError(formatApiError(e, "Erro ao salvar usuário"));
    } finally {
      setSaving(false);
    }
  };

  const confirmToggleActive = (user: UserSummaryDto) => {
    const action = user.isActive ? "desativar" : "ativar";
    Alert.alert(
      user.isActive ? "Desativar usuário" : "Ativar usuário",
      user.isActive
        ? `${user.displayName} não poderá mais fazer login. Continuar?`
        : `Reativar ${user.displayName}?`,
      [
        { text: "Cancelar", style: "cancel" },
        {
          text: action.charAt(0).toUpperCase() + action.slice(1),
          style: user.isActive ? "destructive" : "default",
          onPress: () => void toggleActive(user.id, !user.isActive),
        },
      ]
    );
  };

  const toggleActive = async (id: string, isActive: boolean) => {
    if (!token) return;
    try {
      await endpoints.setAdminUserActive(token, id, isActive);
      refetch();
    } catch (e) {
      Alert.alert("Erro", formatApiError(e, "Erro ao alterar status"));
    }
  };

  const formSection = (
    <View style={styles.formBlock}>
      <Text style={styles.section}>
        {editingId ? "Editar usuário" : "Novo usuário"}
      </Text>
      <FormField
        label="Id"
        value={userId}
        onChangeText={setUserId}
        placeholder="user-atleta-01"
        editable={!editingId}
      />
      {!editingId && (
        <FormField
          label="Telefone (E.164)"
          value={phoneNumber}
          onChangeText={setPhoneNumber}
          placeholder="+5511999990005"
        />
      )}
      <FormField label="Nome" value={displayName} onChangeText={setDisplayName} />
      <Text style={styles.label}>Papel</Text>
      <View style={styles.chips}>
        {USER_ROLES.map((r) => (
          <Chip
            key={r}
            label={r}
            selected={role === r}
            onPress={() => {
              setRole(r);
              if (r !== "Atleta") {
                setPosition(null);
              }
            }}
          />
        ))}
      </View>
      {role === "Atleta" && (
        <>
          <Text style={styles.label}>Posição</Text>
          <View style={styles.chips}>
            {PLAYER_POSITIONS.map((p) => (
              <Chip
                key={p}
                label={p}
                selected={position === p}
                onPress={() => setPosition(p)}
              />
            ))}
          </View>
        </>
      )}
      {formError && <Text style={styles.error}>{formError}</Text>}
      <Button
        label={editingId ? "Salvar alterações" : "Criar usuário"}
        onPress={submit}
        loading={saving}
      />
      {editingId && (
        <Button label="Cancelar edição" variant="ghost" onPress={resetForm} />
      )}
    </View>
  );

  return (
    <ScreenState loading={loading} error={error}>
      <FlatList
        data={data ?? []}
        keyExtractor={(item) => item.id}
        ListHeaderComponent={formSection}
        contentContainerStyle={styles.list}
        renderItem={({ item }) => (
          <View style={styles.row}>
            <Text style={styles.name}>{item.displayName}</Text>
            <Text style={styles.meta}>
              {item.role}
              {item.position ? ` • ${item.position}` : ""}
              {!item.isActive ? " • inativo" : ""}
            </Text>
            <Text style={styles.phone}>{item.phoneNumber}</Text>
            <View style={styles.rowActions}>
              <Button label="Editar" variant="ghost" onPress={() => startEdit(item)} />
              <Button
                label={item.isActive ? "Desativar" : "Ativar"}
                variant="ghost"
                onPress={() => confirmToggleActive(item)}
              />
            </View>
          </View>
        )}
      />
    </ScreenState>
  );
}

const styles = StyleSheet.create({
  list: {
    padding: spacing.containerMarginMobile,
    paddingBottom: 40,
    backgroundColor: colors.surface,
  },
  formBlock: { gap: spacing.stackMd, marginBottom: spacing.stackLg },
  section: {
    ...typography.headlineMd,
    color: colors.onSurface,
    fontFamily: "Inter_600SemiBold",
  },
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
    gap: spacing.stackSm,
  },
  name: {
    ...typography.bodyMd,
    fontWeight: "600",
    fontFamily: "Inter_600SemiBold",
  },
  meta: {
    ...typography.labelSm,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_400Regular",
  },
  phone: {
    ...typography.labelSm,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_400Regular",
  },
  rowActions: { flexDirection: "row", gap: 8 },
});
