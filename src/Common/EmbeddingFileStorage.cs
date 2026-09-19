namespace Common;

/// <summary>
/// File naming and split-file storage for repository embedding caches.
/// </summary>
public static class EmbeddingFileStorage
{
    private const string PartMarker = ".part-";

    /// <summary>Returns the mod ID represented by a normal or split embedding filename.</summary>
    public static string GetModIdFromFilePath(string filePath)
        => GetModIdFromFileName(Path.GetFileName(filePath));

    /// <summary>
    /// Returns the mod ID represented by a filename such as
    /// <c>123.bin</c> or <c>123.part-0001.bin</c>.
    /// </summary>
    public static string GetModIdFromFileName(string fileName)
    {
        var stem = Path.GetFileNameWithoutExtension(fileName);
        var markerIndex = stem.LastIndexOf(PartMarker, StringComparison.Ordinal);
        if (markerIndex > 0
            && int.TryParse(
                stem[(markerIndex + PartMarker.Length)..],
                System.Globalization.NumberStyles.None,
                System.Globalization.CultureInfo.InvariantCulture,
                out var partNumber)
            && partNumber > 0)
        {
            return stem[..markerIndex];
        }

        return stem;
    }

    /// <summary>Returns whether a filename uses the split embedding naming scheme.</summary>
    public static bool IsPartFileName(string fileName)
        => !string.Equals(GetModIdFromFileName(fileName), Path.GetFileNameWithoutExtension(fileName), StringComparison.Ordinal);

    /// <summary>Returns the canonical filename for one output chunk.</summary>
    public static string GetFileName(string modId, int chunkNumber, int chunkCount)
    {
        if (chunkNumber < 1 || chunkNumber > chunkCount)
            throw new ArgumentOutOfRangeException(nameof(chunkNumber));
        if (chunkCount < 1)
            throw new ArgumentOutOfRangeException(nameof(chunkCount));

        return chunkCount == 1
            ? $"{modId}.bin"
            : $"{modId}{PartMarker}{chunkNumber:D4}.bin";
    }

    /// <summary>Finds all existing .bin files belonging to one mod.</summary>
    public static IReadOnlyList<string> FindFiles(string directory, string modId)
    {
        if (!Directory.Exists(directory))
            return [];

        return Directory.GetFiles(directory, "*.bin")
            .Where(path => string.Equals(GetModIdFromFilePath(path), modId, StringComparison.Ordinal))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();
    }

    /// <summary>
    /// Writes a mod's records as one or more canonical files, deleting stale
    /// unsplit/split siblings only after all new payloads have been prepared.
    /// </summary>
    public static IReadOnlyList<string> WriteSplit(
        string directory,
        string modId,
        IReadOnlyList<BinaryEmbeddingSerializer.Record> records,
        long maxFileBytes = BinaryEmbeddingSerializer.MAX_EMBEDDING_FILE_BYTES)
    {
        Directory.CreateDirectory(directory);

        var payloads = BinaryEmbeddingSerializer.SerializeCompressedChunks(records, maxFileBytes);
        if (payloads.Count == 0)
            payloads = [Array.Empty<byte>()];

        var outputPaths = new List<string>(payloads.Count);
        var tempPaths = new List<string>(payloads.Count);
        try
        {
            for (var i = 0; i < payloads.Count; i++)
            {
                var fileName = GetFileName(modId, i + 1, payloads.Count);
                var outputPath = Path.Combine(directory, fileName);
                var tempPath = outputPath + $".tmp-{Guid.NewGuid():N}";
                File.WriteAllBytes(tempPath, payloads[i]);
                outputPaths.Add(outputPath);
                tempPaths.Add(tempPath);
            }

            var outputSet = outputPaths.ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var oldPath in FindFiles(directory, modId))
            {
                if (!outputSet.Contains(oldPath))
                    File.Delete(oldPath);
            }

            for (var i = 0; i < outputPaths.Count; i++)
                MoveFileWithRetry(tempPaths[i], outputPaths[i]);

            return outputPaths;
        }
        finally
        {
            foreach (var tempPath in tempPaths)
            {
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
        }
    }

    private static void MoveFileWithRetry(string source, string destination)
    {
        for (var attempt = 0; ; attempt++)
        {
            try
            {
                File.Move(source, destination, overwrite: true);
                return;
            }
            catch (Exception ex) when ((ex is IOException || ex is UnauthorizedAccessException) && attempt < 3)
            {
                Thread.Sleep(100 * (attempt + 1));
            }
        }
    }
}
