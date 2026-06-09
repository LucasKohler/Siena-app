# Bugfix

## Objective

Fix a confirmed defect with minimal blast radius.

## When To Use

Use when behavior is broken, a test fails, or a reproducible issue is reported.

## Reusable Prompt

```txt
Investigate and fix this bug. Reproduce or reason from existing tests/logs
first. Make the smallest change that fixes the root cause. Add a regression
test when practical. Do not refactor unrelated code. Run relevant validation.
```

## Checklist

- Identify expected vs actual behavior.
- Locate the smallest affected area.
- Add or update a regression test when useful.
- Avoid unrelated cleanup.
- Validate the fix.

## Expected Output

Bug explanation, focused patch and validation results.

## Acceptance Criteria

- Bug is fixed at the root cause.
- No unrelated behavior changes.
- Regression risk is covered or documented.
