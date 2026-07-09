import * as endpoints from "../src/api/endpoints";

const mockFetch = jest.fn();

beforeEach(() => {
  mockFetch.mockReset();
  global.fetch = mockFetch as typeof fetch;
});

describe("api endpoints", () => {
  it("login posts phone number without token", async () => {
    mockFetch.mockResolvedValue({
      ok: true,
      status: 200,
      headers: { get: () => "application/json" },
      json: async () => ({
        token: "jwt",
        expiresAt: "2026-01-01T00:00:00Z",
        displayName: "Test",
        role: "Atleta",
      }),
    });

    await endpoints.login("+5511999990001");

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5000/api/auth/login",
      expect.objectContaining({
        method: "POST",
        headers: expect.objectContaining({
          "Content-Type": "application/json",
        }),
        body: JSON.stringify({ phoneNumber: "+5511999990001" }),
      })
    );
  });

  it("getEvents uses public GET", async () => {
    mockFetch.mockResolvedValue({
      ok: true,
      status: 200,
      headers: { get: () => "application/json" },
      json: async () => [],
    });

    await endpoints.getEvents();

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5000/api/events",
      expect.objectContaining({ method: "GET" })
    );
  });

  it("updateAdminEvent sends PUT with bearer token", async () => {
    mockFetch.mockResolvedValue({
      ok: true,
      status: 204,
      headers: { get: () => null },
    });

    await endpoints.updateAdminEvent("token-abc", "event-1", {
      title: "Treino",
      type: "Treino Físico",
      category: "Feminino",
      startsAt: "2026-09-15T08:00:00-03:00",
      location: "CT",
      opponent: null,
      description: null,
    });

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5000/api/admin/events/event-1",
      expect.objectContaining({
        method: "PUT",
        headers: expect.objectContaining({
          Authorization: "Bearer token-abc",
        }),
      })
    );
  });

  it("deleteAdminEvent sends DELETE", async () => {
    mockFetch.mockResolvedValue({
      ok: true,
      status: 204,
      headers: { get: () => null },
    });

    await endpoints.deleteAdminEvent("token-abc", "event-1");

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5000/api/admin/events/event-1",
      expect.objectContaining({ method: "DELETE" })
    );
  });

  it("createAdminUser posts user payload", async () => {
    mockFetch.mockResolvedValue({
      ok: true,
      status: 201,
      headers: { get: () => null },
    });

    await endpoints.createAdminUser("token-abc", {
      id: "user-new",
      phoneNumber: "+5511999990099",
      displayName: "Novo Atleta",
      role: "Atleta",
      position: "Levantadora",
    });

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5000/api/admin/users",
      expect.objectContaining({
        method: "POST",
        body: JSON.stringify({
          id: "user-new",
          phoneNumber: "+5511999990099",
          displayName: "Novo Atleta",
          role: "Atleta",
          position: "Levantadora",
        }),
      })
    );
  });

  it("createAdminEvent posts event payload", async () => {
    mockFetch.mockResolvedValue({
      ok: true,
      status: 201,
      headers: { get: () => null },
    });

    await endpoints.createAdminEvent("token-abc", {
      id: "treino-extra",
      title: "Treino Extra",
      type: "Treino Físico",
      category: "Masculino",
      startsAt: "2026-10-01T08:00:00-03:00",
      location: "Ginásio",
      opponent: null,
      description: null,
    });

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5000/api/admin/events",
      expect.objectContaining({
        method: "POST",
        headers: expect.objectContaining({
          Authorization: "Bearer token-abc",
        }),
      })
    );
  });

  it("updateAdminUser sends PUT with bearer token", async () => {
    mockFetch.mockResolvedValue({
      ok: true,
      status: 204,
      headers: { get: () => null },
    });

    await endpoints.updateAdminUser("token-abc", "user-1", {
      displayName: "Nome",
      role: "Atleta",
      position: "Central",
    });

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5000/api/admin/users/user-1",
      expect.objectContaining({
        method: "PUT",
        body: JSON.stringify({
          displayName: "Nome",
          role: "Atleta",
          position: "Central",
        }),
      })
    );
  });

  it("setAdminUserActive patches active flag", async () => {
    mockFetch.mockResolvedValue({
      ok: true,
      status: 204,
      headers: { get: () => null },
    });

    await endpoints.setAdminUserActive("token-abc", "user-1", false);

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5000/api/admin/users/user-1/active",
      expect.objectContaining({
        method: "PATCH",
        body: JSON.stringify({ isActive: false }),
      })
    );
  });
});
