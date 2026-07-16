# .github

Structure: GitHub Actions workflows.

Function: run the translation pipeline and publish its updates.

Notes: `workflows/update-translations.yml` maps repository secrets to the pipeline environment; `TRANSLATION_BOT_TOKEN` creates PRs and `TRANSLATION_APPROVER_TOKEN` approves only the matching repository, branch, author, and commit. Do not create `config/secrets.json` in CI.