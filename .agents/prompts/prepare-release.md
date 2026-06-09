# Prepare Release

## Objective

Prepare a release candidate with validation and risk notes.

## When To Use

Use before tagging, deploying or presenting the project publicly.

## Reusable Prompt

```txt
Prepare this repository for release. Review docs, environment variables,
frontend build, backend build/tests, Docker config, security notes and known
risks. Do not deploy unless explicitly asked. Return a release checklist with
commands run and unresolved items.
```

## Checklist

- Check git status.
- Run backend validation.
- Run frontend validation.
- Validate Docker if available.
- Review docs and environment variables.
- List known risks.

## Expected Output

Release readiness summary and checklist.

## Acceptance Criteria

- Commands and results are explicit.
- Unvalidated areas are listed.
- No deployment happens without approval.
