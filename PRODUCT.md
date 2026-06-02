# PRODUCT.md — Siena Voleibol

Base knowledge do produto. Fonte visual: export Google Stitch (`stitch_siena_voleibol_digital_hub.zip`).

---

## Product Summary

**Siena Voleibol** é o hub digital interno da **A.E. Siena** para gestão e desempenho do time de voleibol.

Não é app público de massa nem produto comercial. É ferramenta do **time** (~40 usuários, tráfego tranquilo).

---

## Primary Goal

Centralizar no celular o que o time precisa no dia a dia:

- Ver **calendário** de jogos e treinos
- Confirmar **presença** nos treinos
- Acompanhar **vídeos** do canal do clube
- Acessar áreas **financeira** e **destaques** (conteúdo a definir)
- Permitir **administração** de conteúdo (admin)

---

## Users

| Persona | Uso principal |
|---------|----------------|
| Atleta | Calendário, presença, vídeos |
| Comissão técnica | Calendário, presença, destaques (futuro) |
| Administração | Admin mobile / painel web |

Login observado no Stitch: **número de telefone** (detalhes de OTP em ADR-0002).

---

## Core Experience (Stitch)

### Onboarding / Login

- Boas-vindas ao Siena Voleibol
- Campo de telefone celular
- Continuar → Termos de Serviço e Política de Privacidade

### Navegação principal (bottom tabs)

1. **Financeiro** — placeholder no export (*a definir*)
2. **Calendário** — implementado no protótipo
3. **Destaques** — placeholder no export (*a definir*)
4. **Vídeos** — lista do canal oficial

### Calendário

- Filtro por categoria: Masculino, Feminino, Sub-20
- Grade mensal + lista "Próximos Eventos"
- Tipos observados: Liga Nacional, Treino Físico, Amistoso
- Cada evento: data, horário, local (ex.: Ginásio Principal, Centro de Treinamento)

### Presença no treino

- Próximo treino (data/hora/local)
- Ações: **Eu vou** / **Não vou**
- Lista **Confirmados** com nome e posição (ex.: Levantadora, Ponteiro, Central, Líbero)

### Vídeos

- Canal oficial com itens: título, duração, data de publicação, visualizações, ação Assistir

### Admin

- **Backend (Fase 2f):** API `/api/admin` — Staff cadastra eventos e usuários (allowlist), aprova presenças em treino (fluxo dois passos)
- **Admin mobile** — tela presente no export (screenshot); UI **Fase 3**
- **Painel admin web** — placeholder no export; **Fase 4**

---

## Non-Goals (v1)

- Rede social aberta, comentários públicos
- Apostas ou gamificação agressiva
- Escala para milhares de usuários simultâneos
- Arquitetura enterprise (CQRS, Saga, migração massiva de dados)

---

## Design & Brand

Ver [DESIGN.md](DESIGN.md): vermelho institucional **#E30613**, Inter, estética corporate modern, lobo do brasão A.E. Siena.

---

## Content Governance

- Eventos, presenças e listas de atletas devem refletir dados **reais** aprovados pelo clube
- Agentes de IA não devem inventar jogos, placares ou nomes em seeds de produção
- Placeholders de UI (Financeiro, Destaques) não implicam regra de negócio até spec humana

---

## Open Questions

1. Escopo exato de **Financeiro** e **Destaques**
2. Provedor e fluxo de **OTP/SMS** (ADR-0002)
3. Papéis no admin (quem edita calendário vs vídeos)
4. Política de privacidade e termos (texto legal humano)
5. Categorias além de Masculino / Feminino / Sub-20

---

## Relationship to Portfolio (reference repo)

| Portfolio | Siena |
|-----------|-------|
| Site pessoal / case studies | Hub interno do clube |
| Next.js web | React Native mobile |
| Projetos API | Calendário, presença, vídeos |

Disciplina de engenharia (monorepo, camadas, testes, IA) é reutilizada; produto é novo.
