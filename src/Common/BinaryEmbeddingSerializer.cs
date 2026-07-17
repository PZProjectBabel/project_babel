using System.Buffers.Binary;
using ZstdSharp;

namespace Common;

/// <summary>
/// Binary embedding serialization: fp16 vectors + zstd compression.
/// File format (little-endian), per record:
///   int32  keyLen (UTF-8 byte count)
///   byte[] compositeKey = "{translationKey}|{sourceKind}|{targetLang}"
///   byte[32] hash (raw, full SHA256)
///   Half[384] embedding vector (768 bytes, fp16 LE)
/// On-disk: raw bytes compressed with zstd (using ZstdStream).
/// </summary>
public static class BinaryEmbeddingSerializer
{
    /// <summary>Dimensionality of the embedding vectors (bge-small-en-v1.5).</summary>
    public const int EMBEDDING_DIM = 384;
    /// <summary>Raw byte count of a SHA-256 hash.</summary>
    public const int HASH_RAW_BYTES = 32;
    /// <summary>Byte count of one fp16 embedding vector (DIM * sizeof(Half)).</summary>
    public const int FP16_VEC_BYTES = EMBEDDING_DIM * 2; // 768

    /// <summary>Deserialized embedding record from the binary file format.</summary>
    public readonly record struct Record(
        /// <summary>Translation key for this embedding.</summary>
        string TranslationKey,
        /// <summary>Kind of embedding source ("normal_base_text", "ref_target_text", etc.).</summary>
        string SourceKind,
        /// <summary>Target language ISO code for ref-target embeddings.</summary>
        string TargetLang,
        /// <summary>Raw SHA-256 hash bytes.</summary>
        byte[] Hash,
        /// <summary>Float32 embedding vector.</summary>
        float[] Vector
    );

    // ── Composite key ──

    /// <summary>Builds a composite key from translation key, source kind, and target language.</summary>
    public static string BuildCompositeKey(string translationKey, string sourceKind, string targetLang)
        => $"{translationKey}|{sourceKind}|{targetLang ?? ""}";

    /// <summary>Parses a composite key back into its three components.</summary>
    public static (string translationKey, string sourceKind, string targetLang) ParseCompositeKey(string composite)
    {
        var parts = composite.Split('|', 3);
        return (
            parts.Length > 0 ? parts[0] : "",
            parts.Length > 1 ? parts[1] : "",
            parts.Length > 2 ? parts[2] : ""
        );
    }

    // ── Raw serialize / deserialize ──

    /// <summary>Serializes a list of records into the raw binary format.</summary>
    public static byte[] Serialize(IReadOnlyList<Record> records)
    {
        if (records.Count == 0) return [];
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms, System.Text.Encoding.UTF8, leaveOpen: true);
        foreach (var rec in records)
        {
            var keyBytes = System.Text.Encoding.UTF8.GetBytes(
                BuildCompositeKey(rec.TranslationKey, rec.SourceKind, rec.TargetLang));
            bw.Write((int)keyBytes.Length);
            bw.Write(keyBytes);
            bw.Write(rec.Hash);
            WriteFp16Vector(bw, rec.Vector);
        }
        bw.Flush();
        return ms.ToArray();
    }

    /// <summary>Deserializes raw binary data back into records.</summary>
    public static List<Record> Deserialize(byte[] data)
    {
        var records = new List<Record>();
        if (data.Length == 0) return records;
        var span = data.AsSpan();
        int offset = 0;
        while (offset + 4 <= span.Length)
        {
            int keyLen = BinaryPrimitives.ReadInt32LittleEndian(span[offset..]);
            offset += 4;
            if (keyLen < 0 || offset + keyLen + HASH_RAW_BYTES + FP16_VEC_BYTES > span.Length)
                break;
            var keyStr = System.Text.Encoding.UTF8.GetString(span.Slice(offset, keyLen));
            offset += keyLen;
            var hash = span.Slice(offset, HASH_RAW_BYTES).ToArray();
            offset += HASH_RAW_BYTES;
            var vec = ReadFp16Vector(span.Slice(offset, FP16_VEC_BYTES));
            offset += FP16_VEC_BYTES;
            var (tk, sk, tl) = ParseCompositeKey(keyStr);
            records.Add(new Record(tk, sk, tl, hash, vec));
        }
        return records;
    }

    // ── Zstd (stream-based, no need to know uncompressed size) ──

    /// <summary>Compresses raw binary data with Zstandard.</summary>
    public static byte[] Compress(byte[] data)
    {
        if (data.Length == 0) return [];
        using var outMs = new MemoryStream();
        using (var zs = new ZstdStream(outMs, ZstdStreamMode.Compress))
        {
            zs.Write(data, 0, data.Length);
            zs.Flush();
        }
        return outMs.ToArray();
    }

    /// <summary>Decompresses Zstandard-compressed data.</summary>
    public static byte[] Decompress(byte[] compressed)
    {
        if (compressed.Length == 0) return [];
        using var inMs = new MemoryStream(compressed);
        using var zs = new ZstdStream(inMs, ZstdStreamMode.Decompress);
        using var outMs = new MemoryStream();
        zs.CopyTo(outMs);
        return outMs.ToArray();
    }

    // ── File-level read / write ──

    /// <summary>Serializes, compresses, and writes records to a .bin file.</summary>
    public static void WriteCompressed(string filePath, IReadOnlyList<Record> records)
    {
        var raw = Serialize(records);
        var compressed = Compress(raw);
        File.WriteAllBytes(filePath, compressed);
    }

    /// <summary>Reads a compressed .bin file, decompresses it, and returns parsed records.</summary>
    public static List<Record> ReadCompressed(string compressedPath, string tempDir)
    {
        if (!File.Exists(compressedPath)) return [];
        var compressed = File.ReadAllBytes(compressedPath);
        var raw = Decompress(compressed);

        var fileName = Path.GetFileName(compressedPath);
        var tempPath = Path.Combine(tempDir, fileName);
        Directory.CreateDirectory(tempDir);
        File.WriteAllBytes(tempPath, raw);

        return Deserialize(raw);
    }

    // ── fp16 helpers ──

    /// <summary>Writes a float32 vector as fp16 little-endian to the binary writer.</summary>
    private static void WriteFp16Vector(BinaryWriter bw, float[] vec)
    {
        Span<byte> buf = stackalloc byte[FP16_VEC_BYTES];
        for (int i = 0; i < EMBEDDING_DIM; i++)
        {
            var h = (Half)(i < vec.Length ? vec[i] : 0f);
            BinaryPrimitives.WriteHalfLittleEndian(buf.Slice(i * 2, 2), h);
        }
        bw.Write(buf);
    }

    /// <summary>Reads an fp16 little-endian byte span and converts to float32.</summary>
    private static float[] ReadFp16Vector(ReadOnlySpan<byte> data)
    {
        var vec = new float[EMBEDDING_DIM];
        for (int i = 0; i < EMBEDDING_DIM; i++)
            vec[i] = (float)BinaryPrimitives.ReadHalfLittleEndian(data.Slice(i * 2, 2));
        return vec;
    }
}
