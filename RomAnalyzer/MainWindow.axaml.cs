using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using RomAnalyzer.ViewModels;

namespace RomAnalyzer;

public partial class MainWindow : Window
{
    private MainViewModel? ViewModel => DataContext as MainViewModel;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        var files = e.Data.GetFiles();
        if (files == null) return;

        foreach (var file in files)
        {
            var path = file.Path.LocalPath;
            if (File.Exists(path))
            {
                ViewModel?.LoadRom(path);
                break;
            }
        }
    }
}