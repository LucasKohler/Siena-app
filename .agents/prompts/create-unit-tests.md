# Create Unit Tests

## Objective

Add focused unit tests for meaningful behavior.

## When To Use

Use when application/domain logic or utility behavior needs protection.

## Reusable Prompt

```txt
Add unit tests for the approved behavior. Test observable decisions, not private
implementation details. Do not add placeholder tests. Keep test names clear and
run the relevant test command.
```

## Checklist

- Identify behavior under test.
- Avoid brittle implementation assertions.
- Cover success and relevant failure paths.
- Keep fixtures small.
- Run tests.

## Expected Output

Focused tests and a summary of behavior covered.

## Acceptance Criteria

- Tests fail for a meaningful regression.
- Tests are deterministic.
- Validation results are reported.
