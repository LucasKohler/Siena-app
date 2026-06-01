# DESIGN.md — Siena Voleibol

Fonte: export Google Stitch em `stitch_siena_voleibol_digital_hub.zip` (`siena_voleibol_identity/DESIGN.md`). Referência visual; não copiar HTML do export como arquitetura final.

---

---
name: Siena Voleibol Identity
colors:
  surface: '#fbf9f8'
  surface-dim: '#dbdad9'
  surface-bright: '#fbf9f8'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f5f3f3'
  surface-container: '#efeded'
  surface-container-high: '#e9e8e7'
  surface-container-highest: '#e4e2e2'
  on-surface: '#1b1c1c'
  on-surface-variant: '#5e3f3b'
  inverse-surface: '#303031'
  inverse-on-surface: '#f2f0f0'
  outline: '#936e69'
  outline-variant: '#e9bcb6'
  surface-tint: '#c0000c'
  primary: '#b5000b'
  on-primary: '#ffffff'
  primary-container: '#e30613'
  on-primary-container: '#fff5f3'
  inverse-primary: '#ffb4aa'
  secondary: '#5f5e5e'
  on-secondary: '#ffffff'
  secondary-container: '#e2dfde'
  on-secondary-container: '#636262'
  tertiary: '#575959'
  on-tertiary: '#ffffff'
  tertiary-container: '#707171'
  on-tertiary-container: '#f6f7f7'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#ffdad5'
  primary-fixed-dim: '#ffb4aa'
  on-primary-fixed: '#410001'
  on-primary-fixed-variant: '#930007'
  secondary-fixed: '#e5e2e1'
  secondary-fixed-dim: '#c8c6c5'
  on-secondary-fixed: '#1c1b1b'
  on-secondary-fixed-variant: '#474746'
  tertiary-fixed: '#e2e2e2'
  tertiary-fixed-dim: '#c6c6c7'
  on-tertiary-fixed: '#1a1c1c'
  on-tertiary-fixed-variant: '#454747'
  background: '#fbf9f8'
  on-background: '#1b1c1c'
  surface-variant: '#e4e2e2'
typography:
  headline-xl:
    fontFamily: Inter
    fontSize: 40px
    fontWeight: '600'
    lineHeight: 48px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Inter
    fontSize: 32px
    fontWeight: '600'
    lineHeight: 40px
    letterSpacing: -0.01em
  headline-lg-mobile:
    fontFamily: Inter
    fontSize: 28px
    fontWeight: '600'
    lineHeight: 36px
    letterSpacing: -0.01em
  headline-md:
    fontFamily: Inter
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
  body-lg:
    fontFamily: Inter
    fontSize: 18px
    fontWeight: '400'
    lineHeight: 28px
  body-md:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  label-md:
    fontFamily: Inter
    fontSize: 14px
    fontWeight: '500'
    lineHeight: 20px
    letterSpacing: 0.01em
  label-sm:
    fontFamily: Inter
    fontSize: 12px
    fontWeight: '500'
    lineHeight: 16px
    letterSpacing: 0.02em
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  unit: 4px
  container-margin-mobile: 20px
  container-margin-desktop: 40px
  gutter: 16px
  stack-sm: 8px
  stack-md: 16px
  stack-lg: 24px
  section-gap: 48px
---

## Brand & Style

The brand personality for the design system is **Elite, Competitive, and Disciplined**. Inspired by the howling wolf of the A.E. Siena crest, the UI evokes a sense of leadership and community. It balances the aggression of competitive sports with the refined precision of professional club management.

The visual style follows a **Corporate Modern** aesthetic with **Minimalist** influences. This approach prioritizes performance data and player information through high-contrast typography and intentional whitespace. The "Institutional Red" is used as a high-energy "pounce" color against a clinical, light-gray environment, ensuring that call-to-actions feel urgent and authoritative.

## Colors

The palette is anchored by **Institutional Red**, used strategically for primary actions, branding elements, and critical highlights. 

- **Primary:** #E30613 — Used for buttons, active states, and brand-defining accents.
- **Secondary/Text Primary:** #1A1A1A — Used for headings, icons, and dark-themed components to provide a grounded, professional weight.
- **Background:** #F8F8F8 — A clean, sophisticated neutral that reduces eye strain compared to pure white.
- **Secondary Text:** #666666 — Used for metadata, captions, and deactivated states.
- **Status Colors:** Success and Warning hues are calibrated to remain legible against the light gray background while maintaining enough saturation to be distinct from the primary red.

## Typography

This design system utilizes **Inter** across all levels to maintain a systematic, utilitarian, and modern feel. 

- **Weight Strategy:** Use **SemiBold (600)** for all headlines to project strength. **Medium (500)** is reserved for labels and interactive components (buttons, chips). **Regular (400)** is used for body copy to ensure maximum readability in long-form content or player bios.
- **Scalability:** For mobile, the `headline-lg` transitions to `headline-lg-mobile` to prevent excessive line-breaking. 
- **Character:** Letter spacing is slightly tightened on larger headings to create a more "compact" and "impactful" look, reminiscent of editorial sports journalism.

## Layout & Spacing

The layout philosophy uses a **Fluid Grid** model with a base unit of **4px**. 

- **Mobile:** A 4-column grid with 20px side margins and 16px gutters.
- **Desktop:** A 12-column grid centered in a 1280px max-width container with 40px margins.
- **Rhythm:** Generous whitespace is a requirement. Elements are grouped using a "Stack" system where related items (e.g., a player's name and their jersey number) use `stack-sm`, while unrelated sections use `section-gap`. 

Layouts should prioritize "scanning" behavior; stats and match scores are given wide margins to stand out as distinct units of information.

## Elevation & Depth

This design system employs **Tonal Layers** combined with **Low-contrast Outlines** to create depth. Because the primary color is a vivid red, heavy shadows are avoided to prevent the UI from feeling "muddy."

1.  **Level 0 (Base):** Light Gray (#F8F8F8) surface.
2.  **Level 1 (Cards/Containers):** Pure White (#FFFFFF) surfaces with a subtle 1px border (#EAEAEA). 
3.  **Active State:** Soft, ambient shadows (0px 4px 12px rgba(0,0,0,0.05)) are used only for floating elements like Modals or Bottom Sheets to signify they are above the main content plane.

## Shapes

The shape language is consistently **Rounded (Level 2)**. 

- **Components:** Standard buttons, input fields, and small cards use a **0.5rem (8px)** radius.
- **Large Containers:** Content cards, match panels, and player profiles use **1rem (16px)** to create a soft, premium feel that balances the "sharpness" of the brand's wolf logo and typography.
- **Interactive Elements:** Checkboxes use a smaller 4px radius, while search bars can transition to a pill-shape for better visual distinction in the header.

## Components

### Buttons
- **Primary:** Solid Institutional Red with White text. High-contrast, SemiBold typography.
- **Secondary:** Transparent background with a 2px Black (#1A1A1A) outline. 
- **Tertiary:** Ghost style; text-only using Red or Black depending on context.

### Cards
- Always use a White background. 
- Use the 16px rounded corner.
- Padding should be a minimum of 24px (stack-lg) to maintain the "premium" feel.

### Input Fields
- Border-based (1px #EAEAEA) with a 12px internal padding. 
- Active state uses a 2px Institutional Red border.
- Labels are always positioned above the field using `label-md`.

### Lists & Chips
- **Lists:** Use a simple divider line (1px #F1F1F1). Icons should be Linear (2px stroke) in Black.
- **Chips:** Used for "Match Status" or "Player Position." Rounded (Pill) with a light gray background and `label-sm` text.

### Icons
- All icons must be **Linear** with a consistent stroke weight of 1.5pt to 2pt. Avoid filled icons unless they represent an active "selected" state in the navigation bar.
