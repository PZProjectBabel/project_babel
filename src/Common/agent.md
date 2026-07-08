# Common

Structure: dependency-free shared project.

Function: shared DTOs, encoding/json helpers, workflow annotations, warnings, Steam text cleanup, embedding binary IO.

Key files:
- `CommonTypes.cs`: config/result/mod/entry/batch DTOs and translation state.
- `Utf8NoBom.cs`: UTF-8 no-BOM file and JSON helpers.
- `GitHubActions.cs`: escaped workflow annotations.
- `WarningFileWriter.cs`: warning JSON files under `warningsTempDir`.
- `DescriptionCleaner.cs`: Steam description cleanup.
- `BinaryEmbeddingSerializer.cs`: embedding binary read/write.

Notes: keep dependency-free. `translationValues` keys use ISO. `processed` != `verified`; binary format details stay in code comments, not here.
