# .github

Structure: GitHub Actions workflows.

Function: run the translation pipeline and publish its updates.

Notes: `workflows/update-translations.yml` uses injected environment secrets; do not create `config/secrets.json` in CI.