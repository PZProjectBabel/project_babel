# test

Structure: xUnit project referencing the pipeline entry/modules.

Function: regression tests for shared types, config, collectors, download parsing, extraction, content checking, cross-language review cache reuse, debug-subset queue filtering, persisted modinfo merge, state cache read/write, key-only missing-source handling, embeddings, batching, RAG, adaptive LLM task-pool behavior, and result writing.

Inputs: in-memory fixtures, temp folders, stub `HttpMessageHandler` instances. No real network or steamcmd calls.

Command: `dotnet test src/TranslationPipeline.slnx`

Notes: Prefer focused tests around dictionary mutation and emitted ISO-named temp files. Keep API-dependent modules injectable and stubbed.
