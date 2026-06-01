# Security — Siena Voleibol

Postura de segurança para app interno com dados pessoais (telefone, nomes de atletas, presença).

---

## Threat Model (proportionate)

- Poucos usuários (~40), uso interno
- Risco principal: vazamento de PII, auth fraca, secrets no repositório
- Não é alvo típico de ataque em massa — ainda assim aplicar boas práticas básicas

---

## Authentication

- Login por **telefone** — v1 **allowlist interna** + **JWT Bearer** ([ADR-0002](docs/architecture/adrs/ADR-0002-autenticacao-telefone.md) Accepted)
- Allowlist v1: posse do número cadastrado é o único fator — adequado para ~40 usuários internos; **OTP/SMS é follow-up**
- Não usar `users.json` DEV nem chave `Jwt:SigningKey` de Development em produção
- JWT: configurar `Jwt__Issuer`, `Jwt__Audience`, `Jwt__SigningKey` (mín. 32 caracteres) via ambiente; expiração em `Jwt__AccessTokenMinutes`
- Refresh token e revogação centralizada: **não** implementados na v1
- Logs: não registrar telefone completo nem token em texto claro

---

## LGPD / Personal Data

Dados sensíveis prováveis:

- Número de telefone
- Nome de atletas e posição
- Presença em treinos (comportamento)
- Possíveis **menores** em categorias de base (Sub-20)

Rules:

- Coletar só o necessário
- Não persistir PII sem decisão documentada e base legal (responsável humano)
- Política de privacidade e termos: conteúdo legal humano, não gerado por IA como verdade jurídica
- Logs sem telefone completo em texto claro

---

## Secrets

- Nunca commitar `.env`, chaves SMS, connection strings com senha
- `.env.example` só com placeholders seguros
- Secrets via variáveis de ambiente / vault em produção
- Não colar secrets em prompts de IA ou ADRs

---

## API

- Validar DTOs de entrada
- Respostas de erro claras, sem stack trace
- CORS restrito a origens conhecidas (mobile dev, admin web)
- Rate limiting antes de exposição pública (se algum dia houver)

---

## Mobile

- Não logar tokens em release builds
- Armazenamento seguro de sessão quando auth existir
- Links externos (vídeos) com comportamento seguro

---

## Dependencies

- Adicionar pacotes com justificativa
- Manter lockfiles (`package-lock.json`, `Directory.Packages.props` se usado)

---

## PR Security Checklist

- [ ] Sem secrets no diff
- [ ] Inputs validados
- [ ] Mudança em auth/PII referencia ADR ou spec
- [ ] CORS intencional
- [ ] Sem dados fictícios de atletas em seeds de produção
