# Postman — Siena API

## Importar

1. Abra o Postman → **Import**
2. Arraste ou selecione:
   - `Siena-API.postman_collection.json`
   - `Siena-Local.postman_environment.json` (opcional)
3. Ative o environment **Siena Local (Docker)**

## Pré-requisito

Stack no ar:

```bash
docker compose up --build
```

Base URL padrão: `http://localhost:5000`

## Fluxo sugerido

1. **System → Health** — confirma API
2. **Auth → Login (Admin DEV)** — grava `token` na collection
3. **Trainings** — use **Login (Atleta DEV 1)** antes de marcar presença
4. **Admin** — use login Admin ou Comissão

## Variáveis

| Variável | Uso |
|----------|-----|
| `baseUrl` | Host da API |
| `token` | JWT (preenchido após login) |
| `eventId` | Treino/evento (seed: `treino-fisico-2026-06-15`) |
| `userId` | Atleta para aprovação (seed: `user-athlete-dev-1`) |

OpenAPI alternativo: `http://localhost:5000/openapi/v1.json` (Scalar: `/scalar`).
