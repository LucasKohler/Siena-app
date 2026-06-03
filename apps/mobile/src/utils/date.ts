const ptBr = "pt-BR";

export function formatEventTime(iso: string): string {
  return new Date(iso).toLocaleTimeString(ptBr, {
    hour: "2-digit",
    minute: "2-digit",
  });
}

export function formatEventDateBlock(iso: string): { day: string; month: string } {
  const d = new Date(iso);
  return {
    day: d.getDate().toString(),
    month: d
      .toLocaleDateString(ptBr, { month: "short" })
      .replace(".", "")
      .toUpperCase(),
  };
}

export function formatTrainingWhen(iso: string): string {
  const d = new Date(iso);
  const now = new Date();
  const sameDay =
    d.getDate() === now.getDate() &&
    d.getMonth() === now.getMonth() &&
    d.getFullYear() === now.getFullYear();
  const time = formatEventTime(iso);
  if (sameDay) return `Hoje ${time}`;
  return d.toLocaleDateString(ptBr, {
    weekday: "short",
    day: "numeric",
    month: "short",
    hour: "2-digit",
    minute: "2-digit",
  });
}

export function formatDuration(seconds: number): string {
  const m = Math.floor(seconds / 60);
  const s = seconds % 60;
  return `${m}:${s.toString().padStart(2, "0")}`;
}

export function formatPublishedAgo(iso: string): string {
  const diffMs = Date.now() - new Date(iso).getTime();
  const days = Math.floor(diffMs / (1000 * 60 * 60 * 24));
  if (days < 1) return "Publicado hoje";
  if (days === 1) return "Publicado há 1 dia";
  if (days < 7) return `Publicado há ${days} dias`;
  const weeks = Math.floor(days / 7);
  if (weeks === 1) return "Publicado há 1 semana";
  return `Publicado há ${weeks} semanas`;
}

export function formatViews(views: number): string {
  if (views >= 1000) {
    return `${(views / 1000).toFixed(1).replace(".", ",")}k visualizações`;
  }
  return `${views} visualizações`;
}

export function monthYearLabel(year: number, month: number): string {
  const d = new Date(year, month, 1);
  return d.toLocaleDateString(ptBr, { month: "long", year: "numeric" });
}

export function startOfMonth(year: number, month: number): Date {
  return new Date(year, month, 1);
}

export function daysInMonth(year: number, month: number): number {
  return new Date(year, month + 1, 0).getDate();
}

/** Sunday = 0 */
export function firstWeekdayOfMonth(year: number, month: number): number {
  return new Date(year, month, 1).getDay();
}
