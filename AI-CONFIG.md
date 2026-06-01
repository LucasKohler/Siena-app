# AI Configuration — Siena Voleibol

Workflow de engenharia assistida por IA para o hub interno A.E. Siena. Migrado do padrão **Portfolio** (`.agents/` + `AGENTS.md`); executado com **Claude Opus** + **Cursor AUTO**.

---

## 1. Inventory — Portfolio AI setup (reference)

The Portfolio repo used model-agnostic assets (not Codex/Copilot config files):

| Asset | Role |
|-------|------|
| `AGENTS.md` | Operational rules |
| `AI_WORKFLOW.md` | Single vs multi-agent flows |
| `AI_PROMPTS.md` | 16 phased implementation prompts |
| `.agents/agents/*.toml` | Role definitions (explorer, architect, workers, reviewers) |
| `.agents/prompts/*.md` | Reusable task prompts |
| `.agents/skills/*/SKILL.md` | Structured workflows |

Siena maps these to **AGENTS.md** + **AI-CONFIG.md** + **`.cursor/rules/`** + Opus/AUTO (no Grok-specific roles).

---

## 2. Siena workflow — Claude Opus + Cursor AUTO

| Role | Tool | When |
|------|------|------|
| Planning | **Claude Opus** | ADRs, feature plans, architecture review, trade-offs |
| Execution | **Cursor AUTO** | Incremental implementation, tests, Docker |
| Review | Opus or checklists | PR, security (LGPD/OTP) |

### Standard flow

1. Understand task + `git status`
2. Read [AGENTS.md](AGENTS.md) and area docs
3. Inspect relevant code
4. **Opus:** plan + acceptance criteria + files (non-trivial work)
5. Human approval for write-heavy scope
6. **AUTO:** small steps
7. Run validation commands
8. Report unvalidated areas

### Scope tags

`Documentation` | `Backend` | `Mobile` | `AdminWeb` | `Tests` | `Docker` | `Security`

More than two → split PRs.

---

## 3. Agent interaction style (from Grok draft — adapted, proportional)

For a **simple internal app (~40 users)**, apply rigor **without enterprise ceremony**:

### Do

- Step-by-step reasoning when the task is non-trivial
- **Mermaid** diagrams for architecture or flows when they add clarity
- Short trade-off notes when choosing between 2–3 options
- Production-ready code: validated, tested where it matters, aligned with [CODING_STANDARDS.md](CODING_STANDARDS.md)
- Honest placeholders for Financeiro/Destaques until specced

### Do not

- Mandate CQRS/Saga/migration playbooks (wrong project)
- Invent OTP provider, financial rules, or highlight content
- Claim `dotnet test` / `npm test` passed without running
- Multi-agent personas from other tools (Grok/Leba/Tai/Corvo)

### Task execution template (condensed from enterprise AGENTS draft)

For implementation requests:

1. **Context** — read AGENTS, ARCHITECTURE, DOMAIN, DESIGN as needed
2. **Impact** — list files, bounded areas, LGPD/auth touchpoints
3. **Proposal** — approach + optional Mermaid + trade-offs (Opus / plan mode)
4. **Implementation** — AUTO, smallest safe diff
5. **Validation** — commands + results
6. **Delivery** — what changed, risks, docs/ADR updates

---

## 4. Portfolio → Cursor mapping

| Portfolio | Siena |
|-----------|-------|
| `.agents/agents/explorer.toml` | Cursor `explore` or "map before edit" |
| `.agents/agents/architect.toml` | Opus + ARCHITECTURE.md |
| `backend-worker` | AUTO scoped to `apps/api/` |
| `frontend-worker` | AUTO scoped to `apps/mobile/` |
| `qa-reviewer` | TESTING.md checklist |
| `security-reviewer` | SECURITY.md + ADR-0002 |
| `AI_PROMPTS.md` | Phased prompts when backend/mobile work starts |

---

## 5. Cursor rules (`.cursor/rules/`)

| File | Scope |
|------|-------|
| `siena-core.mdc` | Always — context, simplicity, gates |
| `ai-workflow.mdc` | Always — Opus/AUTO |
| `agent-interaction.mdc` | Always — CoT/Mermaid/trade-offs proportional |
| `backend-dotnet.mdc` | `apps/api/**` |
| `mobile-app.mdc` | `apps/mobile/**` |
| `documentation.mdc` | `**/*.md` |

---

## 6. Multi-perspective review (lightweight)

For larger changes:

1. Read-only explore
2. Opus: boundaries + ADR need
3. Human approval
4. AUTO: backend OR mobile (not conflicting writes)
5. Security pass if auth/PII
6. Document validation

```txt
Review Siena against AGENTS.md and ARCHITECTURE.md in read-only mode.
Return findings by severity with file paths. State what was not validated.
```

---

## 7. Validation commands

**Backend:**

```bash
dotnet build apps/api/Siena.slnx
dotnet test apps/api/Siena.slnx
```

**Mobile (when exists):**

```bash
cd apps/mobile && npm run lint && npm test
```

**Docker:**

```bash
docker compose config && docker compose build
```

**Docs only:**

```bash
git diff --check && git status --short
```

---

## 8. Commits and PRs

See [CONTRIBUTING.md](CONTRIBUTING.md). PRs must note Opus/AUTO usage and command results.

---

## 9. Decision hierarchy

```txt
1. AGENTS.md
2. ARCHITECTURE.md / DOMAIN.md / PRODUCT.md
3. DESIGN.md
4. CODING_STANDARDS.md / TESTING.md / SECURITY.md
5. AI-CONFIG.md (this file)
6. docs/architecture/adrs/
7. Stitch export (visual only)
```

---

## 10. Source references

- Portfolio: `C:\Users\lucas\Documents\Projects\Portfolio\AGENTS.md`, `AI_WORKFLOW.md`, `.agents/`
- Siena Stitch: `stitch_siena_voleibol_digital_hub.zip`
