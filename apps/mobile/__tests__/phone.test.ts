import { formatPhoneMask, normalizePhone } from "../src/utils/phone";

describe("phone utils", () => {
  it("normalizes to digits and plus", () => {
    expect(normalizePhone("+55 (11) 99999-0001")).toBe("+5511999990001");
  });

  it("formats mask progressively", () => {
    expect(formatPhoneMask("5511999990001")).toContain("+55");
  });
});
