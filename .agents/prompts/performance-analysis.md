# Performance Analysis

## Objective

Analyze performance risks without inventing benchmarks.

## When To Use

Use when a route, endpoint, query, rendering path or Docker setup may be slow.

## Reusable Prompt

```txt
Analyze performance for the approved area. Use real code paths and available
measurements. Do not invent metrics. Identify likely bottlenecks, propose small
improvements and list validation methods. Implement only if explicitly approved.
```

## Checklist

- Identify the hot path.
- Separate measured facts from hypotheses.
- Check data loading and caching behavior.
- Check rendering and serialization costs.
- Recommend validation steps.

## Expected Output

Performance findings, hypotheses, proposed changes and measurement plan.

## Acceptance Criteria

- No fake metrics.
- Recommendations are scoped.
- Measurement approach is clear.
