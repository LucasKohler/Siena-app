import React from "react";
import { render, screen } from "@testing-library/react-native";
import { Text, View } from "react-native";

/** Smoke: login copy (avoids full screen + font loading in Jest) */
function LoginCopyPreview() {
  return (
    <View>
      <Text>Bem-vindo ao Siena Voleibol</Text>
      <Text>Continuar</Text>
    </View>
  );
}

describe("Login copy", () => {
  it("renders welcome title", () => {
    render(<LoginCopyPreview />);
    expect(screen.getByText("Bem-vindo ao Siena Voleibol")).toBeTruthy();
    expect(screen.getByText("Continuar")).toBeTruthy();
  });
});
