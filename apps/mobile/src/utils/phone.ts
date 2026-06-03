/** Matches backend PhoneNumberNormalizer */
export function normalizePhone(input: string): string {
  return input
    .trim()
    .split("")
    .filter((c) => /\d/.test(c) || c === "+")
    .join("");
}

export function formatPhoneMask(value: string): string {
  const digits = value.replace(/\D/g, "");
  const match = digits.match(/(\d{0,2})(\d{0,2})(\d{0,5})(\d{0,4})/);
  if (!match) return "";
  const [, cc, ddd, part1, part2] = match;
  if (!cc) return "";
  if (!ddd) return `+${cc}`;
  if (!part1) return `+${cc} (${ddd}`;
  if (!part2) return `+${cc} (${ddd}) ${part1}`;
  return `+${cc} (${ddd}) ${part1}-${part2}`;
}
