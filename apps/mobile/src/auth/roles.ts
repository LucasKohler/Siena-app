export const STAFF_ROLES = ["Administrador", "Comissão"] as const;

export function isStaffRole(role: string | null | undefined): boolean {
  return role != null && STAFF_ROLES.includes(role as (typeof STAFF_ROLES)[number]);
}
