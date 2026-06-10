# AGENTS.md — Siena Voleibol

Fonte única de verdade para agentes de IA e para a equipe. Leia este arquivo antes de qualquer mudança no repositório.

---

## 1. Contexto do projeto

**Siena Voleibol** é o hub digital interno do clube **A.E. Siena** — plataforma de gestão e desempenho para o time (~**40 usuários**, tráfego leve, uso interno).

| Fato | Detalhe |
|------|---------|
| Público | Atletas, comissão técnica, administração do clube |
| Canal principal | App **React Native** (TypeScript) |
| Backend | **.NET LTS** / ASP.NET Core — desenvolvido **primeiro** |
| Admin | App mobile admin + **painel admin web** simples (mesma API) |
| Visual | [DESIGN.md](docs/product/DESIGN.md) (export Stitch) |
| IA | **Claude Opus** (planejamento) + **Cursor AUTO** (execução) — ver [AI-CONFIG.md](docs/ai/AI-CONFIG.md) |

### Funcionalidades (observadas no Stitch)

- Login por **número de telefone** + termos/privacidade
- Abas: **Financeiro**, **Calendário**, **Destaques**, **Vídeos**
- **Calendário:** eventos (liga, treino, amistoso) com categoria (Masculino, Feminino)
- **Presença no treino:** Eu vou / Não vou + lista de confirmados (nome, posição)
- **Vídeos:** canal oficial
- **Destaques** e **Financeiro:** placeholder no export — a definir
- **Admin:** gestão de conteúdo (mobile + web)

Detalhes de domínio: [DOMAIN.md](docs/architecture/DOMAIN.md). Visão de produto: [PRODUCT.md](docs/product/PRODUCT.md).

### Fora de escopo (não aplicar a este projeto)

Conteúdo de outro contexto (ex.: rascunho Grok) que **não** se aplica:

- Migração MongoDB → SQL Server ou processamento de alto volume
- CQRS, Saga, event-driven, Outbox, microserviços
- Kubernetes, mutation/perf testing em escala enterprise, Blazor
- Papéis multi-agente específicos de outras ferramentas (Grok/Leba/Tai/Corvo)

---

## 2. Papel e comportamento do agente

Atue como **engenheiro sênior** com foco em soluções **simples e sustentáveis** para um app interno de baixo tráfego.

**Obrigatório:**

- Raciocínio claro (passo a passo quando a tarefa for não trivial)
- **Diagramas Mermaid** quando ajudarem (fluxos, camadas) — sem excesso
- Análise de trade-offs **proporcional** ao tamanho do problema (~40 usuários)
- Código e docs **production-ready**, sem overengineering
- Menor mudança segura; não inventar regras de negócio nem campos não documentados

**Evitar:**

- Burocracia de “enterprise” (camadas/abstrações sem necessidade real)
- Suposições não verificadas sobre Financeiro, Destaques ou auth OTP
- Afirmar que build/test passou sem executar o comando

---

## 3. Referências obrigatórias

Antes de implementar ou alterar comportamento:

| Prioridade | Documento |
|------------|-----------|
| 1 | `AGENTS.md` (este arquivo) |
| 2 | [ARCHITECTURE.md](docs/architecture/ARCHITECTURE.md), [DOMAIN.md](docs/architecture/DOMAIN.md), [PRODUCT.md](docs/product/PRODUCT.md), [OVERENGINEERING.md](docs/architecture/OVERENGINEERING.md) |
| 3 | [DESIGN.md](docs/product/DESIGN.md) |
| 4 | [CODING_STANDARDS.md](docs/process/CODING_STANDARDS.md), [TESTING.md](docs/process/TESTING.md), [SECURITY.md](SECURITY.md) |
| 5 | [AI-CONFIG.md](docs/ai/AI-CONFIG.md), `.agents/` (roles, prompts, skills) |
| 6 | `docs/architecture/adrs/` |
| 7 | Export Stitch (`stitch_siena_voleibol_digital_hub/`) — referência visual apenas |

Índice completo da documentação: [docs/ai/README.md](docs/ai/README.md).

---

## 4. Stack e arquitetura (resumo)

```txt
apps/api/       → Siena.Api, Application, Domain, Infrastructure
apps/mobile/    → React Native + TypeScript (Expo)
apps/admin-web/ → painel admin simples (futuro, mesma API)
```

- **Clean Architecture pragmática** no backend (ver [ARCHITECTURE.md](docs/architecture/ARCHITECTURE.md))
- Backend é **fonte de verdade**; mobile não duplica catálogos
- Dados: **PostgreSQL** + EF Core ([ADR-0003](docs/architecture/adrs/ADR-0003-persistencia-postgresql.md)); Docker Compose (API + banco)
- Proporcionalidade: [OVERENGINEERING.md](docs/architecture/OVERENGINEERING.md)

Versões: .NET LTS; Node LTS para React Native. Sem preview/canary sem aprovação.

---

## 5. Princípios arquiteturais

1. **Simplicidade primeiro** — adequado a ~40 usuários internos.
2. **Domínio no centro** — regras em Domain/Application; HTTP e persistência nas bordas.
3. **DTOs explícitos** na API; Domain sem ASP.NET.
4. **Monólito modular** — sem microserviços.
5. **Testes que importam** — contratos de endpoint, validação, fluxos críticos (ver [TESTING.md](docs/process/TESTING.md)).
6. **ADRs** para decisões relevantes (auth OTP, banco, libs novas).
7. **LGPD** — telefone e dados de atletas; ver [SECURITY.md](SECURITY.md).

---

## 6. Escopo e gates humanos

Classifique cada tarefa: **Documentation** | **Backend** | **Mobile** | **AdminWeb** | **Tests** | **Docker** | **Security**.

Mais de duas áreas → sugerir PRs separados.

**Pare e peça humano antes de:**

- Breaking change de API ou deep links
- Persistência de PII ou migration destrutiva
- Provedor de SMS/OTP (ADR-0002)
- Introduzir banco, CQRS, filas ou padrões distribuídos
- Secrets, URLs de produção, deploy

---

## 7. Fluxo de execução de tarefa

1. Ler referências e `git status`
2. Mapear arquivos afetados (explore read-only se necessário)
3. **Opus:** plano + riscos + validação (features não triviais)
4. Aprovação humana para write-heavy
5. **AUTO:** implementar incrementalmente
6. Validar (`dotnet build/test`, `npm run typecheck && npm test` no mobile quando aplicável)
7. Atualizar docs no mesmo PR se comportamento mudou
8. Reportar o que **não** foi validado

Para tarefas multi-agente ou write-heavy, use definições em `.agents/agents/*.toml`, prompts em `.agents/prompts/` e workflows em `.agents/skills/` (ver [AI-CONFIG.md](docs/ai/AI-CONFIG.md)).

---

## 8. Proibições

- Inventar endpoints, tabelas, status de partida ou dados financeiros
- Dados falsos de atletas, presenças ou eventos em produção
- Microserviços, CQRS/Saga/event-bus sem ADR aprovado
- Secrets em código, Docker ou markdown
- Ignorar [DESIGN.md](docs/product/DESIGN.md) em mudanças de UI

---

## 9. Commits e PRs

Conventional Commits: `feat:`, `fix:`, `refactor:`, `docs:`, `test:`, `chore:`

PR deve incluir: o quê/por quê, uso Opus/AUTO, comandos executados, riscos, mudanças de contrato.

Detalhes: [CONTRIBUTING.md](CONTRIBUTING.md).

---

## 10. Manutenção

Atualize este arquivo quando mudarem arquitetura, stack, processo de IA ou funcionalidades confirmadas no Stitch.
