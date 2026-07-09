# Siena Voleibol — Mobile

App React Native (**Expo managed** + **Expo Router**), alinhado ao export Google Stitch e a [ADR-0004](../../docs/architecture/adrs/ADR-0004-mobile-expo-router.md).

> Guia completo do monorepo (Docker, seed, API, troubleshooting): **[README.md na raiz](../../README.md)**. Este arquivo foca só no app em `apps/mobile/`.

---

## Pré-requisitos

- **Node.js LTS** (CI usa Node 22)
- **API no ar** — normalmente `docker compose up --build` na raiz (porta **5000**)
- **Expo Go** (celular) ou emulador Android/iOS

---

## Configuração

```bash
cd apps/mobile
cp .env.example .env          # PowerShell: Copy-Item .env.example .env
npm install
```

Arquivo `.env` (variável **obrigatória**):

```env
EXPO_PUBLIC_API_URL=http://localhost:5000
```

- Sem barra no final.
- Nome correto: `EXPO_PUBLIC_API_URL` (não `EXPO_PUBLIC_API_BASE_URL`).

| Onde o app roda | Valor de `EXPO_PUBLIC_API_URL` |
|-----------------|--------------------------------|
| iOS Simulator | `http://localhost:5000` |
| Android Emulator | `http://10.0.2.2:5000` |
| Celular físico (Expo Go) | `http://<IP-LAN-do-PC>:5000` |

No celular: mesma Wi‑Fi do PC; firewall liberando a porta da API.

---

## Executar

```bash
npm start
```

Escaneie o QR no Expo Go, ou pressione `a` / `i` no terminal.

---

## Login DEV (seed PostgreSQL)

A API em **Development** popula estes usuários na primeira subida (tabela `users` vazia):

| Telefone | Papel |
|----------|-------|
| `+5511999990001` | Administrador |
| `+5511999990002` | Comissão |
| `+5511999990003` | Atleta |
| `+5511999990004` | Atleta |

Auth v1 = allowlist + JWT (**sem OTP**). Staff (Admin/Comissão) acessa as telas em `app/admin/`.

Treino seed para presença: id `treino-fisico-2026-09-15`.

---

## Telas

| Área | Conteúdo |
|------|----------|
| Login | Telefone + termos (texto estático) |
| Abas | Financeiro (placeholder), Calendário, Vídeos |
| Presença | Stack a partir do Calendário (`treino/presenca`) |
| Admin | Eventos (CRUD), usuários (CRUD + ativar/desativar), aprovação de presenças |

**Destaques:** adiado (ADR-0004).

---

## Scripts

| Comando | Função |
|---------|--------|
| `npm start` | Expo / Metro |
| `npm run typecheck` | TypeScript |
| `npm run lint` | ESLint |
| `npm test` | Jest |
| `npm run smoke:api` | Smoke HTTP (API precisa estar acessível) |

```bash
npm run smoke:api
node scripts/smoke-api.mjs http://10.0.2.2:5000   # exemplo Android emulator
```

---

## Problemas rápidos

| Problema | Solução |
|----------|---------|
| Network request failed | API fora do ar, URL errada, ou celular sem IP LAN |
| 401 no login | Telefone fora do seed / usuário inativo / seed não rodou |
| Android não acha API | Use `http://10.0.2.2:5000` |
| Mudou `.env` e nada mudou | Reinicie o Metro com cache limpo: `npx expo start -c` |

Mais casos: [README raiz — Problemas comuns](../../README.md#problemas-comuns).
