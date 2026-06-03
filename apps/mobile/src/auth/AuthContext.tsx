import React, {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
} from "react";
import { ApiError } from "../api/client";
import * as endpoints from "../api/endpoints";
import {
  clearSession,
  getStoredUser,
  getToken,
  saveSession,
  type StoredUser,
} from "./storage";

type AuthContextValue = {
  user: StoredUser | null;
  token: string | null;
  loading: boolean;
  login: (phoneNumber: string) => Promise<void>;
  logout: () => Promise<void>;
  refreshUser: () => Promise<void>;
};

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<StoredUser | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  const logout = useCallback(async () => {
    await clearSession();
    setUser(null);
    setToken(null);
  }, []);

  const refreshUser = useCallback(async () => {
    const currentToken = await getToken();
    if (!currentToken) {
      await logout();
      return;
    }
    try {
      const me = await endpoints.getCurrentUser(currentToken);
      const stored = await getStoredUser();
      const next: StoredUser = {
        id: me.id,
        displayName: me.displayName,
        role: me.role,
        expiresAt: stored?.expiresAt ?? new Date().toISOString(),
      };
      await saveSession(currentToken, next);
      setUser(next);
      setToken(currentToken);
    } catch (err) {
      if (err instanceof ApiError && err.status === 401) {
        await logout();
      }
    }
  }, [logout]);

  useEffect(() => {
    (async () => {
      try {
        const [storedToken, storedUser] = await Promise.all([
          getToken(),
          getStoredUser(),
        ]);
        if (storedToken && storedUser) {
          setToken(storedToken);
          setUser(storedUser);
          await refreshUser();
        }
      } finally {
        setLoading(false);
      }
    })();
  }, [refreshUser]);

  const login = useCallback(
    async (phoneNumber: string) => {
      const result = await endpoints.login(phoneNumber);
      const stored: StoredUser = {
        id: "",
        displayName: result.displayName,
        role: result.role,
        expiresAt: result.expiresAt,
      };
      await saveSession(result.token, stored);
      setToken(result.token);
      const me = await endpoints.getCurrentUser(result.token);
      const withId: StoredUser = { ...stored, id: me.id, role: me.role };
      await saveSession(result.token, withId);
      setUser(withId);
    },
    []
  );

  const value = useMemo(
    () => ({ user, token, loading, login, logout, refreshUser }),
    [user, token, loading, login, logout, refreshUser]
  );

  return (
    <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error("useAuth must be used within AuthProvider");
  }
  return ctx;
}
