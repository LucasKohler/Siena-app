import { useEffect, useState } from "react";
import * as endpoints from "../../api/endpoints";
import { TRAINING_EVENT_TYPE } from "../../constants/domain";

export function usePendingAttendanceCount(token: string | null): number | null {
  const [count, setCount] = useState<number | null>(null);

  useEffect(() => {
    if (!token) {
      setCount(null);
      return;
    }
    let cancelled = false;

    (async () => {
      try {
        const events = await endpoints.listAdminEvents(token);
        const trainings = events.filter((e) => e.type === TRAINING_EVENT_TYPE);
        let total = 0;
        for (const training of trainings) {
          const pending = await endpoints.listPendingAttendances(
            token,
            training.id
          );
          total += pending.length;
        }
        if (!cancelled) setCount(total);
      } catch {
        if (!cancelled) setCount(null);
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [token]);

  return count;
}
