# Create Frontend Component

## Objective

Create a reusable mobile UI component aligned with the Siena Voleibol design direction.

## When To Use

Use when the mobile app needs a reusable component or screen section (e.g. presença no treino, card de evento).

## Reusable Prompt

```txt
Create the approved mobile component in the existing React Native + Expo structure under apps/mobile/.
Follow DESIGN.md. Place screens/routes in app/ (Expo Router) and shared components in src/components/ or src/.
Use TypeScript (.tsx). Do not introduce global state or new UI libraries without justification.
Validate with `cd apps/mobile && npm run typecheck && npm test` when possible.
```

## Checklist

- Place the component in the correct folder (app/ for routes, src/ for shared UI).
- Reuse existing UI primitives and styling patterns.
- Keep props typed.
- Avoid duplicating backend-owned data.
- Validate accessibility basics for touch targets and labels.

## Expected Output

Component implementation, usage notes and validation results.

## Acceptance Criteria

- Component is reusable and cohesive.
- It follows the Siena Voleibol visual direction from DESIGN.md.
- No unnecessary client state or effects are introduced.
