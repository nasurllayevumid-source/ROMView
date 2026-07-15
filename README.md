# ROM Analyzer

**ROM Analyzer** is a lightweight, cross-platform desktop tool for analyzing and identifying ROM files from gaming consoles. Built with C# and Avalonia UI.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Avalonia](https://img.shields.io/badge/Avalonia-11.1.0-purple?logo=avalonia)
![License](https://img.shields.io/badge/license-MIT-green)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-lightgrey)

## Table of Contents

- [Description](#description)
- [Features](#features)
- [System Requirements](#system-requirements)
- [Installation & Build](#installation--build)
- [Usage](#usage)
- [Supported Formats](#supported-formats)
- [Project Structure](#project-structure)
- [API & Extensibility](#api--extensibility)
- [Screenshots](#screenshots)
- [Roadmap](#roadmap)
- [License](#license)
- [Contact](#contact)

## Description

ROM Analyzer is a fast, intuitive tool for analyzing ROM files without the need for HEX editors or complex command-line utilities. It automatically detects ROM type, extracts header information, calculates checksums, and displays all relevant data in a clean, modern interface.

Built from scratch in C# with Avalonia UI, it delivers native performance and a consistent look across Windows, Linux, and macOS.

## Features

### Core Features

- **Automatic ROM Type Detection** — detects NES (iNES), Game Boy, ELF (PSP/PS1), Sega Mega Drive, MS-DOS EXE, and more
- **Header Metadata Extraction** — game title, mapper, bank count, entry point
- **Checksum Calculation** — CRC32 and SHA1 for file integrity verification
- **Drag & Drop Support** — drag files directly into the application window
- **Dark Theme UI** — modern, readable interface designed for extended use

### Technical Highlights

- Fully managed C# 12 codebase
- Cross-platform GUI with Avalonia UI 11
- Asynchronous file loading
- Graceful error handling with user-friendly messages
- Easily extensible to support new ROM formats

## System Requirements

| Component | Minimum Requirements |
|-----------|----------------------|
| **Operating System** | Windows 10/11, Linux (any distro), macOS 10.15+ |
| **.NET Runtime** | .NET 8.0 or higher |
| **Processor** | x64 / ARM64, 1.5 GHz+ |
| **RAM** | 128 MB |
| **Disk Space** | ~20 MB (ROM size is not limited) |
| **Additional** | Drag & Drop support (optional) |

## Installation & Build

### Prerequisites

Install [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or higher.

Build from Source
bash

dotnet restore
dotnet build -c Release

Run
bash

dotnet run --project RomAnalyzer.csproj

Publish (Standalone Executable)
Windows (x64)
bash

dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish

Linux (x64)
bash

dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o ./publish

macOS (x64)
bash

dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -o ./publish

Executable will be in the publish/ folder.
Usage
Launch Application

Run RomAnalyzer.exe (Windows) or RomAnalyzer (Linux/macOS).
Load a ROM

Method 1: Menu

    Click File → Open ROM

    Select a ROM file from the dialog

Method 2: Drag & Drop

    Simply drag and drop a ROM file into the application window

Method 3: Command Line
bash

RomAnalyzer.exe "C:\path\to\game.gb"

Displayed Information

After loading, the application shows:
Field	Description
File Name	Name of the file
Type	ROM type (NES, Game Boy, ELF, etc.)
Size	File size in kilobytes
Header Info	Header metadata (title, mapper, etc.)
CRC32	CRC32 checksum
SHA1	SHA1 hash
Supported Formats
Format	Extensions	Detection	Extracted Info
NES (iNES)	.nes	Magic bytes 4E 45 53 1A	Mapper, PRG/CHR banks
Game Boy	.gb, .gbc	Header at 0x134	Game title
ELF	.elf, .prx	Magic bytes 7F 45 4C 46	Entry point
Sega Mega Drive	.bin, .gen	4D 42 at 0x100	—
MS-DOS EXE	.exe	Magic bytes 4D 5A	—
Raw Binary	.bin, .rom	—	First 16 bytes in HEX
Planned Support

    SNES (SFC)

    Sega Genesis

    PlayStation 1 (EXE)

    PSP (PRX)

    GBA

Project Structure
text

RomAnalyzer/
├── RomAnalyzer.csproj              # Project file
├── Program.cs                      # Entry point
├── App.axaml                       # Avalonia application config
├── App.axaml.cs                    # Application code
├── MainWindow.axaml                # Main window XAML
├── MainWindow.axaml.cs             # Main window logic
├── ViewModels/
│   ├── ViewModelBase.cs            # Base ViewModel (ReactiveUI)
│   └── MainViewModel.cs            # Main ViewModel
├── Models/
│   └── RomInfo.cs                  # ROM data model
├── Utils/
│   └── RomParser.cs                # ROM parser
└── README.md                       # This documentation

API & Extensibility
Adding a New ROM Format

To add support for a new format, edit DetectRomType and ParseHeader in RomParser.cs.
Example: Adding GBA Support
csharp

private static void DetectRomType(RomInfo info, byte[] data)
{
    // ... existing code ...

    if (data.Length >= 0xC0 && data[0xC0] == 0x24 && data[0xC1] == 0xFF && data[0xC2] == 0xAE && data[0xC3] == 0x51)
        info.RomType = "Game Boy Advance (GBA)";
}

Extending RomInfo Model

Add new properties to RomInfo:
csharp

public class RomInfo
{
    // ... existing properties ...
    public string Publisher { get; set; } = string.Empty;
    public int Revision { get; set; }
}
