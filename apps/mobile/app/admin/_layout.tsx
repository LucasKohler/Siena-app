import { Redirect, Stack } from "expo-router";
import { useAuth } from "../../src/auth/AuthContext";
import { isStaffRole } from "../../src/auth/roles";

export default function AdminLayout() {
  const { user, loading } = useAuth();

  if (!loading && !isStaffRole(user?.role)) {
    return <Redirect href="/(tabs)/calendario" />;
  }

  return (
    <Stack screenOptions={{ headerShown: true, title: "Admin" }}>
      <Stack.Screen name="index" options={{ title: "Acesso restrito" }} />
      <Stack.Screen name="eventos" options={{ title: "Criar eventos" }} />
      <Stack.Screen name="usuarios" options={{ title: "Usuários" }} />
      <Stack.Screen name="presencas" options={{ title: "Aprovar presenças" }} />
      <Stack.Screen name="relatorios" options={{ title: "Relatórios" }} />
    </Stack>
  );
}
