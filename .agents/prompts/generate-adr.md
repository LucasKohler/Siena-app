# Generate ADR

## Objective

Document an architectural decision before implementation.

## When To Use

Use for folder migrations, database introduction, runtime upgrades, major
dependencies, CI/CD provider choice or contract strategy changes.

## Reusable Prompt

```txt
Generate an ADR for this decision. Include context, options considered,
decision, consequences, risks, validation plan and rollback considerations. Do
not implement the decision yet.
```

## Checklist

- State the problem.
- List realistic options.
- Explain the selected option.
- Document tradeoffs.
- Define validation and rollback.

## Expected Output

An ADR draft ready for human review.

## Acceptance Criteria

- Decision is explicit.
- Alternatives are considered fairly.
- Implementation is not performed in the ADR task.
