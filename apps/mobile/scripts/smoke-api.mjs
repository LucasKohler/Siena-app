/**
 * Smoke HTTP contra API local (sem Expo).
 * Uso: node scripts/smoke-api.mjs [baseUrl]
 */
const base = (process.argv[2] ?? process.env.EXPO_PUBLIC_API_URL ?? "http://localhost:5000").replace(/\/$/, "");

async function req(path, options = {}) {
  const res = await fetch(`${base}${path}`, {
    headers: { Accept: "application/json", ...options.headers },
    ...options,
  });
  const text = await res.text();
  let body = null;
  try {
    body = text ? JSON.parse(text) : null;
  } catch {
    body = text;
  }
  return { status: res.status, body };
}

async function main() {
  console.log(`Smoke API: ${base}\n`);

  const health = await req("/api/health");
  console.log("GET /api/health", health.status, health.status === 200 ? "OK" : health.body);

  const login = await req("/api/auth/login", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ phoneNumber: "+5511999990001" }),
  });
  console.log("POST /api/auth/login", login.status);
  if (login.status !== 200) {
    console.error(login.body);
    process.exit(1);
  }

  const token = login.body.token;
  const events = await req("/api/events");
  console.log("GET /api/events", events.status, Array.isArray(events.body) ? `${events.body.length} items` : "");

  const videos = await req("/api/videos");
  console.log("GET /api/videos", videos.status, Array.isArray(videos.body) ? `${videos.body.length} items` : "");

  const training = await req("/api/trainings/next", {
    headers: { Authorization: `Bearer ${token}` },
  });
  console.log("GET /api/trainings/next", training.status);

  const adminUsers = await req("/api/admin/users?includeInactive=false", {
    headers: { Authorization: `Bearer ${token}` },
  });
  console.log("GET /api/admin/users", adminUsers.status, Array.isArray(adminUsers.body) ? `${adminUsers.body.length} users` : adminUsers.body?.title ?? "");

  const adminEvents = await req("/api/admin/events", {
    headers: { Authorization: `Bearer ${token}` },
  });
  console.log("GET /api/admin/events", adminEvents.status);

  console.log("\nSmoke concluído.");
}

main().catch((e) => {
  console.error(e.message);
  process.exit(1);
});
