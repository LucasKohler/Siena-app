# Implement Feature

## Objective

Implement an approved feature safely.

## When To Use

Use after a plan has been accepted and the scope is clear.

## Reusable Prompt

```txt
Implement only the approved feature scope. Follow AGENTS.md. Reuse existing
patterns. Add or update relevant tests when behavior changes. Do not add
unapproved dependencies, endpoints, tables or migrations. Run relevant
validation and report results.
```

## Checklist

- Confirm approved scope.
- Check `git status --short --branch`.
- Make the smallest code changes.
- Update docs if behavior changes.
- Run relevant validation.

## Expected Output

Implemented feature, tests or docs as needed, and a validation summary.

## Acceptance Criteria

- Scope is respected.
- Existing contracts are preserved unless approved.
- Validation results are reported honestly.
