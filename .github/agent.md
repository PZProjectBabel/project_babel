# .github

Structure: GitHub Actions workflows.

Function: run the translation pipeline and publish its updates.

Notes: `workflows/update-translations.yml` maps repository secrets to the pipeline environment; `TRANSLATION_BOT_TOKEN` creates PRs and enables native auto-merge after required checks pass. Do not create `config/secrets.json` in CI.