import {
  EVENT_CATEGORIES,
  EVENT_TYPES,
  PLAYER_POSITIONS,
  TRAINING_EVENT_TYPE,
  USER_ROLES,
} from "../src/constants/domain";

describe("domain constants", () => {
  it("exposes event types aligned with backend DomainLabels", () => {
    expect(EVENT_TYPES).toEqual([
      "Liga Nacional",
      "Treino Físico",
      "Amistoso",
    ]);
    expect(TRAINING_EVENT_TYPE).toBe("Treino Físico");
  });

  it("exposes categories and roles used in admin forms", () => {
    expect(EVENT_CATEGORIES).toEqual(["Masculino", "Feminino"]);
    expect(USER_ROLES).toEqual(["Atleta", "Comissão", "Administrador"]);
  });

  it("lists player positions for athlete users", () => {
    expect(PLAYER_POSITIONS).toContain("Levantadora");
    expect(PLAYER_POSITIONS).toHaveLength(4);
  });
});
