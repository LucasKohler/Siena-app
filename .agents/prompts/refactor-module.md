# Refactor Module

## Objective

Improve structure without changing behavior.

## When To Use

Use when a module is coupled, hard to test or inconsistent with local patterns.

## Reusable Prompt

```txt
Refactor this module without changing public behavior. Identify protection
tests first. Move one concern at a time. Preserve contracts, routes and data
shape. Run build and tests after the change. Stop if behavior becomes ambiguous.
```

## Checklist

- Confirm no behavior change is intended.
- Identify protection tests.
- Move one responsibility at a time.
- Preserve public contracts.
- Run validation.

## Expected Output

Focused refactor summary with before/after responsibilities and validation.

## Acceptance Criteria

- Public behavior is unchanged.
- Tests/build pass or limitations are reported.
- The module is simpler to understand.
