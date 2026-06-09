# Create Endpoint

## Objective

Add or modify an API endpoint through the existing backend layers.

## When To Use

Use when a new HTTP capability is approved.

## Reusable Prompt

```txt
Create the approved endpoint using the current ASP.NET Core structure. Keep
business logic out of the endpoint file. Use explicit DTOs, validation and
application services. Do not invent data storage or external integrations. Add
endpoint tests and update API documentation when behavior changes.
```

## Checklist

- Confirm route, method and response contract.
- Add DTOs explicitly.
- Keep orchestration in Application.
- Keep external details in Infrastructure.
- Add endpoint tests.

## Expected Output

Endpoint implementation, relevant tests and contract documentation notes.

## Acceptance Criteria

- Endpoint returns documented status codes.
- Validation behavior is tested.
- No unapproved storage or integration is added.
