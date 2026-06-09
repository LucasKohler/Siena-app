# Create Migration

## Objective

Prepare a database migration only after the data model decision is approved.

## When To Use

Use when EF Core/SQL Server has already been introduced and a schema change is
approved.

## Reusable Prompt

```txt
Create the approved migration. First verify the current data strategy, ADR and
tests. Describe whether the migration is additive or destructive. Do not run or
commit destructive changes without explicit human approval. Update database docs
and run relevant build/test commands.
```

## Checklist

- Confirm EF Core exists in the project.
- Confirm migration scope and data impact.
- Identify destructive operations.
- Add/update tests around affected behavior.
- Update database documentation.

## Expected Output

Migration files, risk notes, rollback considerations and validation results.

## Acceptance Criteria

- No destructive migration is hidden.
- Schema change matches approved model.
- Tests and docs reflect the change.
