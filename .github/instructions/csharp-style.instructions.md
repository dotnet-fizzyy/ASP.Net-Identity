---
description: "Use when creating or editing C# files. Apply this repository's .editorconfig C# syntax, naming, analyzer, and formatting rules."
applyTo: "**/*.cs"
---

# C# Style

Follow the repository-root `.editorconfig` for C# syntax, formatting, naming, and analyzer rules. Treat it as authoritative; do not introduce conflicting conventions.

- Use file-scoped namespaces.
- Prefix instance member and local function calls with `this.` where required by the configured analyzer.
- Use PascalCase for types and members. Use lowercase field names without prefixes or a leading underscore.
- Separate using-directive groups with blank lines, and remove unnecessary using directives. Do not reorder groups contrary to `.editorconfig`.
- Align arguments when splitting argument lists across lines, and include trailing commas in multiline initializers.
- Keep braces and blank lines consistent with the configured analyzer rules, and end files with a newline.
- Address enabled analyzer diagnostics, including disposal-related `CA2000`, unless ownership or another code constraint makes the diagnostic inapplicable.

When a style detail is not specified here, consult `.editorconfig` rather than guessing or broadening the rule.
