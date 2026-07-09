# Siena Voleibol

Hub digital interno da **A.E. Siena** — gestão e desempenho do time (~**40 usuários**, tráfego leve).

Este README é o guia **canônico** para clonar, configurar, subir e validar o projeto. Se algo falhar, use a seção [Problemas comuns](#problemas-comuns) antes de inventar outro fluxo.

---

## O que existe hoje

| Camada | Tecnologia | Onde | Status |
|--------|------------|------|--------|
| API | .NET 10, ASP.NET Core, EF Core, PostgreSQL | [`apps/api/`](apps/api/) | Implementada |
| Mobile | Expo 52 + Expo Router + TypeScript | [`apps/mobile/`](apps/mobile/) | Implementada (Fase 3) |
| Admin web | — | `apps/admin-web/` | **Ainda não existe** (Fase 4) |
| Auth | Telefone na allowlist + JWT (sem OTP) | [ADR-0002](docs/architecture/adrs/ADR-0002-autenticacao-telefone.md) | v1 interna |

**Abas do app (v1):** Financeiro (placeholder), Calendário, Vídeos. **Destaques** está adiado ([ADR-0004](docs/architecture/adrs/ADR-0004-mobile-expo-router.md)).

---

## Pré-requisitos

Instale **antes** de qualquer comando:

| Ferramenta | Versão / nota | Para quê |
|------------|---------------|----------|
| **Git** | Qualquer recente | Clonar o repo |
| **Docker** + Compose | Docker Desktop (Windows/macOS) ou Engine + Compose no Linux/WSL | API + Postgres (caminho recomendado) |
| **.NET SDK** | **10.0.201** (ver [`global.json`](global.json); `rollForward: latestFeature` aceita patch mais novo da série 10) | Build/test da API no host; opcional se só usar Docker |
| **Node.js** | **LTS** (CI usa Node 22) | App mobile |
| **npm** | Vem com o Node | Dependências do mobile |

### Opcionais

| Ferramenta | Para quê |
|------------|----------|
| **Expo Go** (celular) ou emulador Android/iOS | Rodar o app |
| **Postman** | Exercitar a API sem o mobile |
| **WSL 2** (Windows) | Se `docker` não estiver no PATH do PowerShell |

### Conferir versões

```bash
git --version
docker --version
docker compose version
dotnet --version          # esperado: 10.x
node --version            # LTS (ex.: 22.x)
npm --version
```

Se `dotnet --version` for menor que 10, instale o SDK alinhado a [`global.json`](global.json).

---

## Clone

```bash
git clone https://github.com/LucasKohler/Siena-app.git
cd Siena-app
```

Estrutura relevante:

```txt
/
  apps/api/                 # Backend .NET (Siena.slnx)
  apps/mobile/              # Expo
  docs/                     # Arquitetura, produto, ADRs, processo
  postman/                  # Collection + environment
  scripts/                  # Helpers (ex.: setup-database.ps1)
  docker-compose.yml        # Dev: Postgres + API
  docker-compose.override.yml  # Overlay local (hot reload; aplicado automaticamente)
  docker-compose.prod.yml   # Overlay produção (sem hot reload / sem seed DEV)
  .env.example              # Variáveis do Compose
  .github/workflows/ci.yml  # CI (api + mobile + compose config)
```

---

## Caminho recomendado: API + banco com Docker

Este é o fluxo padrão. A API sobe com **migrations EF** e **seed DEV** quando `ASPNETCORE_ENVIRONMENT=Development`.

### 1. Criar o `.env` na raiz

```bash
# Linux / macOS / Git Bash / WSL
cp .env.example .env

# PowerShell (Windows)
Copy-Item .env.example .env
```

O Docker Compose carrega automaticamente o arquivo **`.env`** na raiz (não o `.env.example`). O `.env` está no [`.gitignore`](.gitignore) — não commite secrets.

Em DEV você **pode** manter os defaults do example. Em produção, **troque** `Jwt__SigningKey` e `POSTGRES_PASSWORD`.

| Variável | Default | Significado |
|----------|---------|-------------|
| `POSTGRES_PASSWORD` | `siena_dev` | Senha do usuário Postgres `siena` |
| `POSTGRES_PORT` | `5433` | Porta no **host** (container escuta 5432) — evita conflito com Postgres nativo na 5432 |
| `API_PORT` | `5000` | Porta da API no host |
| `ASPNETCORE_ENVIRONMENT` | `Development` | Em Development: migrations + seed |
| `API_DOCKER_TARGET` | `development` | Target do Dockerfile (hot reload) |
| `Jwt__*` | ver `.env.example` | Issuer, audience, signing key (≥ 32 chars), TTL minutos |
| `SIENA__CORS__ALLOWEDORIGINS__*` | localhost:3000 e :8081 | Origens CORS (admin web futuro / Metro) |
| `ALLOWED_HOSTS` | (só prod overlay) | Hosts aceitos pela API em Production |

### 2. Subir a stack

Na **raiz** do repositório:

```bash
docker compose up --build
```

Isso mescla automaticamente [`docker-compose.yml`](docker-compose.yml) + [`docker-compose.override.yml`](docker-compose.override.yml) (Development, hot reload, CORS locais).

Primeira vez demora (restore NuGet + build da imagem). Deixe o terminal aberto; logs da API e do Postgres aparecem aí.

**Windows sem `docker` no PATH** — rode via WSL (ajuste o caminho):

```powershell
wsl -e bash -lc "cd /mnt/c/Users/lucas/Documents/Projects/Siena && docker compose up --build"
```

### 3. Confirmar que a API está no ar

Em outro terminal:

```bash
curl http://localhost:5000/api/health
```

PowerShell (se não tiver `curl`/alias):

```powershell
Invoke-RestMethod http://localhost:5000/api/health
```

Resposta esperada: HTTP **200** com algo como `{ "status": "ok", "service": "siena-api" }`.

Também:

| URL | O que é |
|-----|---------|
| http://localhost:5000/ | Root `{ service, status }` |
| http://localhost:5000/api/health | Liveness |
| http://localhost:5000/scalar/v1 | Scalar (OpenAPI UI) — **só Development/Testing** |
| http://localhost:5000/openapi/v1.json | Documento OpenAPI — **só Development/Testing** |

### 4. Postgres (acesso externo / ferramentas)

| Item | Valor |
|------|-------|
| Host | `localhost` |
| Porta no host | **5433** (default) |
| Database | `siena` |
| User | `siena` |
| Password | `siena_dev` (ou o valor de `POSTGRES_PASSWORD`) |
| Volume | `siena_pg_data` (persiste entre `down`/`up`) |

Dentro da rede Docker, a API conecta em `Host=postgres;Port=5432;...`.

### 5. Parar / limpar

```bash
# Para containers; mantém dados do volume
docker compose down

# Apaga também o volume do Postgres (perde seed e dados locais)
docker compose down -v
```

**Importante:** o seed só roda se a tabela `users` estiver **vazia**. Se você mudou o seed no código e o volume já tem dados antigos, use `docker compose down -v` e suba de novo, ou limpe o banco manualmente.

---

## Seed DEV (usuários e dados de exemplo)

Criados automaticamente na primeira subida em **Development** (API Docker ou `dotnet run` com env Development).

### Usuários (login por telefone)

| Telefone | Papel | Id interno |
|----------|-------|------------|
| `+5511999990001` | Administrador | `user-admin-dev` |
| `+5511999990002` | Comissão | `user-coach-dev` |
| `+5511999990003` | Atleta | `user-athlete-dev-1` |
| `+5511999990004` | Atleta | `user-athlete-dev-2` |

Auth v1: o número precisa existir na allowlist e o usuário estar **ativo**. Não há OTP/SMS.

### Evento de treino usado nos fluxos de presença

| Campo | Valor |
|-------|-------|
| Id | `treino-fisico-2026-09-15` |
| Tipo | Treino Físico |
| Início | 2026-09-15 (UTC a partir do offset -03:00 no seed) |

Há também eventos de Liga Nacional e Amistoso, e vídeos de exemplo.

---

## App mobile (Expo)

Com a **API no ar** em `http://localhost:5000` (ou o host/IP que o dispositivo alcançar):

### 1. Configurar

```bash
cd apps/mobile
cp .env.example .env          # PowerShell: Copy-Item .env.example .env
npm install
```

Edite `apps/mobile/.env`:

```env
EXPO_PUBLIC_API_URL=http://localhost:5000
```

**Sem barra no final.** Nome da variável: `EXPO_PUBLIC_API_URL` (não use `EXPO_PUBLIC_API_BASE_URL`).

| Onde o app roda | `EXPO_PUBLIC_API_URL` |
|-----------------|------------------------|
| iOS Simulator | `http://localhost:5000` |
| Android Emulator | `http://10.0.2.2:5000` |
| Celular físico (Expo Go) | `http://<IP-LAN-do-PC>:5000` |

No celular físico, PC e telefone precisam estar na **mesma rede**. Descubra o IP do PC (`ipconfig` no Windows, `ip a` / `ifconfig` no Linux/macOS) e libere a porta **5000** no firewall se necessário.

### 2. Iniciar

```bash
cd apps/mobile
npm start
```

Depois: abra no Expo Go (QR code), pressione `a` (Android) ou `i` (iOS simulator).

### 3. Login no app

Use um dos telefones da tabela de seed (ex.: `+5511999990001` para Admin).

- **Atleta:** calendário, vídeos, presença no treino.
- **Administrador / Comissão (Staff):** menu Admin — eventos, usuários, aprovação de presenças.

### Scripts mobile

| Comando | Função |
|---------|--------|
| `npm start` | Metro / Expo |
| `npm run typecheck` | TypeScript (`tsc --noEmit`) |
| `npm test` | Jest |
| `npm run lint` | ESLint |
| `npm run smoke:api` | Smoke HTTP contra a API (precisa API em `:5000` ou URL passada) |

```bash
npm run smoke:api
# ou
node scripts/smoke-api.mjs http://localhost:5000
```

Detalhes extras do app: [`apps/mobile/README.md`](apps/mobile/README.md).

---

## Alternativa: API no host (sem container da API)

Útil para debug no Visual Studio / Rider / `dotnet watch`.

### Opção A — Só Postgres no Docker

```bash
# Na raiz
docker compose up postgres -d
```

Ajuste a connection string da API para a porta do **host** (default **5433**):

- Em [`apps/api/src/Siena.Api/appsettings.Development.json`](apps/api/src/Siena.Api/appsettings.Development.json) o default é `Port=5432`.
- Com Compose padrão, use `Port=5433`, por exemplo via ambiente:

```bash
cd apps/api/src/Siena.Api
# Linux/macOS/WSL
export ConnectionStrings__Default="Host=localhost;Port=5433;Database=siena;Username=siena;Password=siena_dev"
export ASPNETCORE_ENVIRONMENT=Development
dotnet run
```

```powershell
# PowerShell
cd apps/api/src/Siena.Api
$env:ConnectionStrings__Default = "Host=localhost;Port=5433;Database=siena;Username=siena;Password=siena_dev"
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run
```

A API escuta em **`http://localhost:5000`** por padrão ([`launchSettings.json`](apps/api/src/Siena.Api/Properties/launchSettings.json)). Se mudar a porta, atualize `EXPO_PUBLIC_API_URL` no mobile.

### Opção B — Postgres nativo na 5432 (sem Docker)

Se você já tem PostgreSQL instalado no host na porta **5432**:

1. Crie role/database `siena` (senha alinhada a `appsettings.Development.json`, default `siena_dev`).
2. No Windows, o helper [`scripts/setup-database.ps1`](scripts/setup-database.ps1) cria o banco via `psql` local e aplica migrations EF — **não** usa o Compose; exige Postgres nativo + senha do superuser `postgres`.
3. Depois: `cd apps/api/src/Siena.Api` e `dotnet run` com `ASPNETCORE_ENVIRONMENT=Development`.

O default de [`appsettings.Development.json`](apps/api/src/Siena.Api/appsettings.Development.json) aponta para `Host=localhost;Port=5432;...`.

---

## Mapa da API (DEV)

Base: `http://localhost:5000`

| Método | Rota | Auth | Notas |
|--------|------|------|-------|
| GET | `/` | Público | Status do serviço |
| GET | `/api/health` | Público | Health |
| POST | `/api/auth/login` | Público | Body: `{ "phoneNumber": "+55..." }` — **rate limit** 20 req/min/IP |
| GET | `/api/auth/me` | Bearer | Usuário do JWT |
| GET | `/api/events` | Público | Calendário |
| GET | `/api/events/{id}` | Público | Detalhe |
| GET | `/api/videos` | Público | Lista de vídeos |
| GET | `/api/trainings/next` | Bearer | Próximo treino + presença |
| POST | `/api/trainings/{eventId}/attendance` | Bearer **Atleta** | `{ "status": "Eu vou" \| "Não vou" }` → fica **Pendente** |
| GET/POST/PUT/DELETE | `/api/admin/events...` | Bearer **Staff** | CRUD eventos |
| GET/POST/PUT/PATCH | `/api/admin/users...` | Bearer **Staff** | CRUD allowlist + ativar/desativar |
| GET/POST | `/api/admin/trainings/.../attendances...` | Bearer **Staff** | Pendentes + approve/reject |

**Staff** = papéis `Administrador` ou `Comissão` (policy JWT `Staff`).

Exemplo de login:

```bash
curl -s -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d "{\"phoneNumber\":\"+5511999990001\"}"
```

Use o `token` retornado:

```bash
curl -s http://localhost:5000/api/auth/me \
  -H "Authorization: Bearer <token>"
```

### Postman

1. Importar [`postman/Siena-API.postman_collection.json`](postman/Siena-API.postman_collection.json)
2. Importar [`postman/Siena-Local.postman_environment.json`](postman/Siena-Local.postman_environment.json)
3. Ativar environment **Siena Local (Docker)**
4. Fluxo: Health → Login → demais pastas

Guia: [`postman/README.md`](postman/README.md).

---

## Build e testes (local)

Espelham o CI ([`.github/workflows/ci.yml`](.github/workflows/ci.yml)).

### Backend

```bash
dotnet build apps/api/Siena.slnx
dotnet test  apps/api/Siena.slnx
```

Testes de API usam **SQLite in-memory** (não precisam do Docker).

### Mobile

```bash
cd apps/mobile
npm ci          # ou npm install na primeira vez
npm run typecheck
npm test
```

### Docker Compose (validação de arquivo)

```bash
docker compose config
```

---

## Perfil “produção” em Compose (cuidado)

```bash
docker compose -f docker-compose.yml -f docker-compose.prod.yml up --build
```

Diferenças importantes:

- `ASPNETCORE_ENVIRONMENT=Production`
- Imagem **runtime** (sem bind mount / sem hot reload)
- **Sem** seed automático de DEV (migrations/seed só rodam em Development no código atual)
- `AllowedHosts` via env `ALLOWED_HOSTS` (default `api.siena.local;localhost`)
- OpenAPI/Scalar **não** são mapeados
- Você **deve** definir `Jwt__SigningKey` e senha fortes no `.env` — nunca use os defaults de DEV em ambiente real

Este overlay **não** substitui um checklist completo de deploy (HTTPS, secrets manager, backup, etc.).

---

## Checklist “está funcionando?”

1. `docker compose up --build` sobe sem erro de healthcheck.
2. `curl http://localhost:5000/api/health` → 200.
3. Login Admin DEV retorna `token`.
4. `GET /api/events` retorna lista (inclui treino seed).
5. Mobile: `npm start` + login com `+5511999990003` (atleta) ou Admin.
6. `dotnet test apps/api/Siena.slnx` e `cd apps/mobile && npm test` passam.

---

## Problemas comuns

| Sintoma | Causa provável | O que fazer |
|---------|----------------|-------------|
| `docker: command not found` (Windows) | Docker só no WSL / Desktop sem PATH | Use o comando `wsl -e bash -lc "..."` ou abra terminal WSL integrado |
| Porta 5000 ou 5433 em uso | Outro processo | Altere `API_PORT` / `POSTGRES_PORT` no `.env` e reinicie o Compose |
| API sobe mas mobile não conecta no celular | `localhost` no device aponta para o próprio telefone | Use IP LAN do PC em `EXPO_PUBLIC_API_URL` |
| Android emulator não alcança API | `localhost` errado no emulador | Use `http://10.0.2.2:5000` |
| Login 401 | Número fora da allowlist / usuário inativo / seed não rodou | Confira telefone da tabela; `docker compose down -v && docker compose up --build` |
| Login 429 | Rate limit (20/min/IP) | Espere ~1 minuto; não dispare scripts agressivos no `/login` |
| Seed “não atualizou” | Volume já tinha `users` | `docker compose down -v` (apaga dados) e suba de novo |
| `dotnet` não acha SDK 10 | SDK antigo | Instale .NET 10 conforme `global.json` |
| Scalar/OpenAPI 404 | Ambiente Production | Só existem em Development/Testing |
| CORS no browser | Origem não listada | Adicione origem em `SIENA__CORS__ALLOWEDORIGINS__*` no `.env` |
| Teste mobile falha sem `node_modules` | Dependências não instaladas | `cd apps/mobile && npm install` |
| Connection refused na 5432 com `dotnet run` | Compose publica Postgres em **5433** | Use `Port=5433` na connection string (Opção A) |
| `setup-database.ps1` falha | Script espera Postgres **nativo** + `psql` | Prefira `docker compose up`; ou instale Postgres local |
| `npm ci` falha | Lock desatualizado / Node errado | Use Node LTS; ou `npm install` na primeira vez |

---

## Segurança (DEV vs produção)

- Chaves JWT e senhas em [`.env.example`](.env.example) são **só para desenvolvimento**.
- Não commite `.env` com secrets reais.
- Telefone e dados de atletas são PII — ver [`SECURITY.md`](SECURITY.md).
- Rate limiting está ativo em `POST /api/auth/login`.
- Em Production: configure `AllowedHosts`, JWT forte, e não exponha OpenAPI.

---

## Documentação do projeto

| Documento | Conteúdo |
|-----------|----------|
| [AGENTS.md](AGENTS.md) | Regras para agentes e time |
| [docs/architecture/ARCHITECTURE.md](docs/architecture/ARCHITECTURE.md) | Camadas e decisões |
| [docs/architecture/DOMAIN.md](docs/architecture/DOMAIN.md) | Domínio e contratos |
| [docs/architecture/OVERENGINEERING.md](docs/architecture/OVERENGINEERING.md) | Proporcionalidade (~40 usuários) |
| [docs/product/PRODUCT.md](docs/product/PRODUCT.md) | Produto |
| [docs/product/DESIGN.md](docs/product/DESIGN.md) | Visual (Stitch) |
| [docs/process/TESTING.md](docs/process/TESTING.md) | Estratégia de testes |
| [CONTRIBUTING.md](CONTRIBUTING.md) | Branches, commits, validação |
| [SECURITY.md](SECURITY.md) | Segurança e LGPD |
| [docs/ai/AI-CONFIG.md](docs/ai/AI-CONFIG.md) | Workflow IA |
| [docs/history/MIGRATION-PLAN.md](docs/history/MIGRATION-PLAN.md) | Histórico de fases |
| ADRs | [`docs/architecture/adrs/`](docs/architecture/adrs/) |

---

## Próximas fatias (não bloqueiam o run local)

- Specs de **Financeiro** e **Destaques**
- Painel **admin-web** (Fase 4)
- OTP/SMS ([ADR-0002](docs/architecture/adrs/ADR-0002-autenticacao-telefone.md))

---

## Resumo em 60 segundos

```bash
# 1) Raiz
cp .env.example .env
docker compose up --build

# 2) Outro terminal — mobile
cd apps/mobile
cp .env.example .env
# Ajuste EXPO_PUBLIC_API_URL se for emulador Android ou celular físico
npm install
npm start

# 3) Login no app: +5511999990001 (Admin) ou +5511999990003 (Atleta)
```

Dúvida restante após este guia? Abra issue/PR descrevendo SO, comando exato e log do erro.
