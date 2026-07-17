# RepoDataLoader

Structure: service project referencing `Common`.

Function: load `data/` and `translation_ref/` caches; parse translation status pairs; load entry metadata/embeddings; diff fresh entries against cache.

Rules:
- translation status: `processed|unprocessed` + `verified|unverified`.
- entry metadata: per-mod files under `entry_metadata/`.
- diff keeps historical keys, marks missing updated-mod keys inactive.
- source hash uses base text only; missing base text hashes as key-only state and does not follow generated target translations.
- embeddings: zstd-compressed binary `.bin`; decompress to `runTempDir/embeddings_decompressed/` via `BinaryEmbeddingSerializer.ReadCompressed`; memory fp32, disk fp16.
