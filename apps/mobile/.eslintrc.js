module.exports = {
  extends: "expo",
  ignorePatterns: ["/node_modules/", "/.expo/"],
  overrides: [
    {
      files: ["jest.setup.js", "**/__tests__/**"],
      env: { jest: true },
    },
  ],
};
