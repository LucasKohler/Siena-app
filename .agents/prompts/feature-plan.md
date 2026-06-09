# Feature Plan

## Objective

Plan a feature before implementation.

## When To Use

Use when the feature affects multiple files, public behavior, contracts or UI
flows.

## Reusable Prompt

```txt
Plan this feature incrementally. Read AGENTS.md and relevant docs first. Do not
edit files. Identify impacted backend, frontend, tests, Docker, docs and
contracts. Propose small implementation steps, validation commands, risks and
acceptance criteria.
```

## Checklist

- Define the user-visible behavior.
- Identify contract changes.
- Identify test coverage needed.
- Split into small steps.
- Flag dependencies and human decisions.

## Expected Output

A phase-by-phase plan with files likely affected and validation commands.

## Acceptance Criteria

- Plan is scoped.
- No invented business rules.
- Breaking changes are called out.
