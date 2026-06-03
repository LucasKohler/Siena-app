import type { ExpoConfig } from "expo/config";

const config: ExpoConfig = {
  name: "Siena Voleibol",
  slug: "siena-voleibol",
  version: "1.0.0",
  orientation: "portrait",
  scheme: "siena",
  userInterfaceStyle: "light",
  newArchEnabled: true,
  splash: {
    backgroundColor: "#fbf9f8",
  },
  ios: {
    supportsTablet: false,
    bundleIdentifier: "com.aesiena.voleibol",
  },
  android: {
    adaptiveIcon: {
      backgroundColor: "#fbf9f8",
    },
    package: "com.aesiena.voleibol",
  },
  plugins: ["expo-router", "expo-secure-store"],
  experiments: {
    typedRoutes: true,
  },
  extra: {
    apiUrl:
      process.env.EXPO_PUBLIC_API_URL ?? "http://localhost:5000",
  },
};

export default config;
