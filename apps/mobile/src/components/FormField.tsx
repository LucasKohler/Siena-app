import { StyleSheet, Text, TextInput, View } from "react-native";
import { colors, radius, typography } from "../theme";

type Props = {
  label: string;
  value: string;
  onChangeText: (v: string) => void;
  placeholder?: string;
  multiline?: boolean;
  editable?: boolean;
};

export function FormField({
  label,
  value,
  onChangeText,
  placeholder,
  multiline,
  editable = true,
}: Props) {
  return (
    <View style={styles.wrap}>
      <Text style={styles.label}>{label}</Text>
      <TextInput
        style={[styles.input, multiline && styles.multiline, !editable && styles.readOnly]}
        value={value}
        onChangeText={onChangeText}
        placeholder={placeholder}
        placeholderTextColor={colors.outline}
        multiline={multiline}
        editable={editable}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  wrap: { gap: 4 },
  label: {
    ...typography.labelMd,
    color: colors.onSurfaceVariant,
    fontFamily: "Inter_500Medium",
  },
  input: {
    ...typography.bodyMd,
    backgroundColor: colors.surfaceContainerLowest,
    borderWidth: 1,
    borderColor: colors.outlineVariant,
    borderRadius: radius.lg,
    paddingHorizontal: 12,
    paddingVertical: 10,
    color: colors.onSurface,
    fontFamily: "Inter_400Regular",
  },
  multiline: { minHeight: 80, textAlignVertical: "top" },
  readOnly: { opacity: 0.7 },
});
