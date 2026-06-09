# Codebase Analysis

## Objective

Understand the current repository state before planning or editing.

## When To Use

Use at the start of a new task, before refactors, or when joining an unfamiliar
area of the codebase.

## Reusable Prompt

```txt
Analyze this repository before making changes. Read AGENTS.md and the relevant
documentation. Inspect the current folder structure, manifests, tests and
runtime configuration. Do not edit files. Return the current structure, key
architecture decisions, risks, validation commands and recommended next step.
```

## Checklist

- Read `AGENTS.md`.
- Inspect `README.md` and relevant docs.
- Run `git status --short --branch`.
- List backend, frontend, Docker and test entry points.
- Identify missing context and assumptions.

## Expected Output

A concise diagnostic report with structure, risks, gaps and recommended scope.

## Acceptance Criteria

- No files changed.
- Findings reference real files.
- Recommendations do not invent unapproved work.
