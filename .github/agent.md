# .github

Structure: GitHub Actions workflows.

Function: run the translation pipeline and publish its updates.

Notes: `workflows/update-translations.yml` maps repository secrets to the pipeline environment; do not create `config/secrets.json` in CI.