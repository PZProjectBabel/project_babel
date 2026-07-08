# project_babel

Structure: .NET 10 Project Zomboid multi-mod translation pipeline. Main code in `src/`; runtime data flows through `temp/`, `data/`, `translation_ref/`, and `config/` folders. `project_babel_ref/` (if present) is reference-only; do not modify it.

Function: collect Workshop mod IDs, fetch Steam metadata, download mods with steamcmd, extract translation entries, review content safety, build source embeddings/batches, prepare per-target RAG/prompt plans, run adaptive LLM task pool, write per-target output.

Navigation:
- `copilot.md`: workspace rules. Load `.github/copilot-instructions.md` before coding.
- `src/agent.md`: module map and pipeline order.
- `src/Program.cs`: orchestration entry point.
- `src/Common/`: shared DTOs/helpers.
- `src/test/`: xUnit tests.
- `docs/`: design notes, progress reports, contributing guide.

Commands:
- `dotnet test src/TranslationPipeline.slnx`
- `dotnet run --project src/TranslationPipeline.csproj`

Notes: Keep edits surgical. Runtime warnings/errors use `Common.GitHubActions`. Update nearby `agent.md` when module behavior changes.
