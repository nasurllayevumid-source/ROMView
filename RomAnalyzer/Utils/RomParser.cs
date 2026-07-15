using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using RomAnalyzer.Models;

namespace RomAnalyzer.Utils;

public static class RomParser
{
    public static RomInfo Parse(string path)
    {
        var info = new RomInfo();
        var bytes = File.ReadAllBytes(path);

        info.FileName = Path.GetFileName(path);
        info.FilePath = path;
        info.FileSize = bytes.Length;
        info.RawData = bytes;
        info.Crc32 = ComputeCrc32(bytes);
        info.Sha1 = ComputeSha1(bytes);

        DetectRomType(info, bytes);
        ParseHeader(info, bytes);

        return info;
    }

    private static void DetectRomType(RomInfo info, byte[] data)
    {
        if (data.Length < 4) return;

        if (data[0] == 0x4E && data[1] == 0x45 && data[2] == 0x53 && data[3] == 0x1A)
            info.RomType = "NES (iNES)";
        else if (data[0] == 0x54 && data[1] == 0x45 && data[2] == 0x53 && data[3] == 0x54)
            info.RomType = "Game Boy (TITLE)";
        else if (data[0] == 0x7F && data[1] == 0x45 && data[2] == 0x4C && data[3] == 0x46)
            info.RomType = "ELF (PSP/PS1)";
        else if (data[0] == 0x4D && data[1] == 0x5A)
            info.RomType = "MS-DOS EXE";
        else if (data.Length > 0x100 && data[0x100] == 0x4D && data[0x101] == 0x42)
            info.RomType = "Sega Mega Drive";
        else
            info.RomType = "Unknown / Raw Binary";
    }

    private static void ParseHeader(RomInfo info, byte[] data)
    {
        if (info.RomType == "Game Boy (TITLE)")
        {
            if (data.Length >= 0x143)
            {
                string title = Encoding.ASCII.GetString(data, 0x134, 16).TrimEnd('\0');
                info.HeaderInfo = $"Title: {title}";
            }
        }
        else if (info.RomType == "NES (iNES)")
        {
            if (data.Length >= 16)
            {
                int mapper = (data[6] >> 4) | (data[7] & 0xF0);
                int prgBanks = data[4];
                int chrBanks = data[5];
                info.HeaderInfo = $"Mapper: {mapper}, PRG: {prgBanks} x 16KB, CHR: {chrBanks} x 8KB";
            }
        }
        else if (info.RomType == "ELF (PSP/PS1)")
        {
            if (data.Length >= 52)
            {
                uint entry = BitConverter.ToUInt32(data, 24);
                info.HeaderInfo = $"Entry Point: 0x{entry:X8}";
            }
        }
        else
        {
            if (data.Length >= 16)
            {
                string hex = BitConverter.ToString(data, 0, 16).Replace("-", " ");
                info.HeaderInfo = $"First 16 bytes: {hex}";
            }
        }
    }

    private static string ComputeCrc32(byte[] data)
    {
        uint crc = 0xFFFFFFFF;
        for (int i = 0; i < data.Length; i++)
        {
            byte b = data[i];
            crc ^= b;
            for (int j = 0; j < 8; j++)
            {
                if ((crc & 1) != 0)
                    crc = (crc >> 1) ^ 0xEDB88320;
                else
                    crc >>= 1;
            }
        }
        return (crc ^ 0xFFFFFFFF).ToString("X8");
    }

    private static string ComputeSha1(byte[] data)
    {
        using var sha1 = SHA1.Create();
        byte[] hash = sha1.ComputeHash(data);
        return BitConverter.ToString(hash).Replace("-", "");
    }
}