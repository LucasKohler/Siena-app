# Siena Voleibol — Mobile

App React Native (Expo managed) + Expo Router, fiel ao export Google Stitch.

## Pré-requisitos

- Node.js LTS
- API rodando (Docker Compose ou `dotnet run` em `apps/api`)
- Expo Go ou emulador Android/iOS

## Configuração

```bash
cd apps/mobile
cp .env.example .env
npm install
```

Ajuste `EXPO_PUBLIC_API_URL`:

| Ambiente | URL típica |
|----------|------------|
| iOS Simulator | `http://localhost:5000` |
| Android Emulator | `http://10.0.2.2:5000` |
| Dispositivo físico | `http://<IP-da-máquina>:5000` |

## Executar

```bash
npm start
```

## Login DEV (seed PostgreSQL)

| Telefone | Papel |
|----------|-------|
| +5511999990001 | Administrador |
| +5511999990002 | Comissão |
| +5511999990003 | Atleta |
| +5511999990004 | Atleta |

## Scripts

- `npm run typecheck` — TypeScript
- `npm run lint` — ESLint
- `npm test` — Jest (smoke)
- `npm run smoke:api` — HTTP smoke contra a API (requer API em `:5000`)

### API local

```bash
# WSL (recomendado)
docker compose up --build

# Ou dotnet run com Postgres em localhost:5432/5433
```

| Plataforma | `EXPO_PUBLIC_API_URL` |
|------------|------------------------|
| iOS Simulator | `http://localhost:5000` |
| Android Emulator | `http://10.0.2.2:5000` |
| Expo Go (celular) | `http://<IP-do-PC>:5000` |

## Telas

- Login (allowlist, sem OTP)
- Abas: Financeiro (placeholder), Calendário, Vídeos
- Presença no treino (stack a partir do Calendário)
- Admin mobile (Staff: Administrador / Comissão)
