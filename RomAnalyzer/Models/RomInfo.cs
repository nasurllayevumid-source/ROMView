using System;

namespace RomAnalyzer.Models;

public class RomInfo
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string FileSizeFormatted => $"{FileSize / 1024.0:F2} KB";
    public string Crc32 { get; set; } = string.Empty;
    public string Sha1 { get; set; } = string.Empty;
    public string RomType { get; set; } = "Unknown";
    public string HeaderInfo { get; set; } = "No header detected";
    public byte[] RawData { get; set; } = Array.Empty<byte>();
    public bool IsValid => FileSize > 0;
}