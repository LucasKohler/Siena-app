# Security Review

## Objective

Review code and configuration for practical security issues.

## When To Use

Use before release, before adding integrations, or when endpoints/configuration
change.

## Reusable Prompt

```txt
Perform a security review for the approved scope. Check secrets, validation,
CORS, headers, dependency risk, data handling and external links. Do not add
security tools or dependencies unless approved. Report findings by severity with
file references and recommended fixes.
```

## Checklist

- Check secret handling.
- Check request validation.
- Check CORS and environment variables.
- Check external links and user input.
- Check dependency and Docker risks.

## Expected Output

Security findings, risk level, recommended remediation and validation notes.

## Acceptance Criteria

- Findings are tied to real code/configuration.
- No speculative alarmism.
- Fixes preserve intended behavior.
