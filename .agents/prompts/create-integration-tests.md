# Create Integration Tests

## Objective

Validate behavior across application boundaries.

## When To Use

Use for API endpoints, persistence behavior, serialization, routing or service
composition.

## Reusable Prompt

```txt
Add integration tests for the approved behavior. Use existing test projects and
fixtures where possible. Validate public behavior and contracts. Do not require
external services unless the task explicitly provides them. Run relevant tests.
```

## Checklist

- Identify the boundary under test.
- Reuse existing test setup.
- Validate status codes and response shapes.
- Cover error cases.
- Keep tests deterministic.

## Expected Output

Integration tests with validation results and any environment assumptions.

## Acceptance Criteria

- Tests cover real integration behavior.
- External dependencies are controlled or documented.
- Public contracts are protected.
