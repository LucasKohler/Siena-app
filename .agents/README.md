# AI Agent Workspace — Siena Voleibol

This folder keeps model-agnostic AI workflow assets for the repository (adapted from Portfolio).

- `agents/`: reusable agent role definitions, permissions and instructions.
- `prompts/`: reusable task prompts for planning, implementation and review.
- `skills/`: structured workflows for repeatable engineering activities.
- `config.toml`: shared limits and settings for local multi-agent workflows.

These files are project guidance, not tool lock-in. Any coding assistant should
read the root `AGENTS.md`, inspect the repository state and use only the assets
that fit the current task. See `AI-CONFIG.md` for Opus/AUTO mapping and validation commands.
