# Review PR

## Objective

Review a change for defects, regressions and maintainability risks.

## When To Use

Use before merging or when asked for a code review.

## Reusable Prompt

```txt
Review this PR as a senior engineer. Focus on bugs, regressions, contract
changes, missing tests, security risks and maintainability issues. Lead with
findings ordered by severity and reference exact files/lines. Do not rewrite
the PR unless asked.
```

## Checklist

- Inspect diff and touched files.
- Check contracts and validation.
- Check tests for changed behavior.
- Check security and data handling.
- Check docs for accuracy.

## Expected Output

Findings first, then questions, then a brief summary.

## Acceptance Criteria

- Findings are actionable.
- Severity is clear.
- No speculative issues are presented as facts.
