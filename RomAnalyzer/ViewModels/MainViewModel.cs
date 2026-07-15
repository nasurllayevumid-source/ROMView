using System;
using System.Collections.ObjectModel;
using System.IO;
using ReactiveUI;
using RomAnalyzer.Models;
using RomAnalyzer.Utils;

namespace RomAnalyzer.ViewModels;

public class MainViewModel : ViewModelBase
{
    private RomInfo? _currentRom;
    private string _status = "Drop a ROM file or click 'Open'";

    public RomInfo? CurrentRom
    {
        get => _currentRom;
        set => this.RaiseAndSetIfChanged(ref _currentRom, value);
    }

    public string Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    public void LoadRom(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                Status = "File not found!";
                return;
            }

            CurrentRom = RomParser.Parse(path);
            Status = $"Loaded: {CurrentRom.FileName} ({CurrentRom.FileSizeFormatted})";
        }
        catch (Exception ex)
        {
            Status = $"Error: {ex.Message}";
        }
    }
}