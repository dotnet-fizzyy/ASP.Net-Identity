---
name: git-commit
description: Stage and commit the current changes using the exact commit message provided when manually invoking /git-commit.
argument-hint: <commit message>
disable-model-invocation: true
user-invocable: true
---

# Git commit

Run this workflow only when the user explicitly invokes `/git-commit`. Do not invoke it automatically for ordinary requests to commit, stage, or inspect changes.

Treat all text after `/git-commit` as the requested commit message. Do not invent, rewrite, or infer a different message. If the message is missing or blank, stop immediately without inspecting or staging anything, and reply exactly: `Empty commit message is not allowed`.

1. Inspect the repository root and `git status --short --untracked-files=all`. Review both staged and unstaged diffs, and inspect the contents of every untracked file that may be included.
2. Check that every modified, added, deleted, renamed, and already-staged file is relevant to the supplied commit message. If any change is unrelated, ambiguous, or cannot be safely included, stop and ask the user rather than silently omitting or mixing changes.
3. Stage all relevant changes that are not already staged, using explicit paths identified during review. Do not stage ignored files, secrets, or changes that appeared after inspection; if the worktree changed, inspect those changes first.
4. Review the complete staged diff to confirm it contains only the analyzed changes and matches the supplied message. Run `git diff --cached --check`. Resolve any reported whitespace issues before continuing.
5. Commit with the exact supplied message and add this co-author trailer exactly once:

   `Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>`

   Safely quote the message as a single argument; never evaluate it as shell code. Do not amend an existing commit or push.
6. Verify the resulting commit and inspect `git status --short`. Report the commit hash and message, and explicitly report any remaining changes or errors.
