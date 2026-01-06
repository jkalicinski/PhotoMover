# PhotoMover 📸

Automatic photo and video organizer that sorts files by date into an organized directory structure.

## 📋 Description

PhotoMover is a C# console application (.NET 10) that automatically organizes your photos and videos by date. The program scans a directory, detects dates from file names or EXIF metadata, and moves files into a structured directory hierarchy `Photos/year/month/day`.

## ✨ Features

- **🔍 Date detection from file names** - supports multiple formats:
  - `YYYY-MM-DD`, `YYYY_MM_DD`, `YYYY.MM.DD`
  - `YYYYMMDD`
  - `DD-MM-YYYY`, `DD_MM_YYYY`, `DD.MM.YYYY`
  - `DDMMYYYY`
  - `DD-MM-YY`, `MM-DD-YY` (with automatic century detection)

- **📷 EXIF metadata reading** - for image files without a date in the filename, checks:
  - DateTimeOriginal
  - DateTimeDigitized
  - DateTime

- **🎬 Support for multiple formats**
  - **Images**: `.jpg`, `.jpeg`, `.png`, `.gif`, `.bmp`, `.tiff`, `.tif`, `.webp`, `.heic`, `.heif`, `.raw`, `.cr2`, `.nef`, `.arw`, `.dng`
  - **Videos**: `.mp4`, `.avi`, `.mov`, `.wmv`, `.mkv`, `.flv`, `.webm`, `.m4v`, `.mpg`, `.mpeg`, `.3gp`, `.mts`, `.m2ts`

- **📁 Automatic organization** - files are sorted into `year/month/day` directory structure

- **🔄 Duplicate handling** - if a file with the same name exists, appends `_1`, `_2`, etc.

- **⚠️ Error folder** - files without a recognized date are moved to the `Error` folder in the source directory

## 🚀 Usage

### Basic usage (default behavior)

1. Copy the executable `PhotoMover.exe` to the directory containing files to organize
2. Run the application
3. The program will automatically move files to the structure `year/month/day/` within the same directory

### Advanced usage (with parameters)

You can optionally specify source and/or destination directories as command-line arguments:

```bash
PhotoMover.exe [sourceDirectory] [destinationDirectory]
```

**Examples:**

- **Show version**:
  ```bash
  PhotoMover.exe --version
  ```
  or
  ```bash
  PhotoMover.exe -v
  ```

- **Default behavior** (scans and organizes in current directory):
  ```bash
  PhotoMover.exe
  ```
  Files will be organized in the same directory as the executable.

- **Custom source directory** (organizes files within the same directory):
  ```bash
  PhotoMover.exe "C:\Photos\Unsorted"
  ```
  Files will be organized within `C:\Photos\Unsorted\year\month\day\`

- **Custom source and destination directories**:
  ```bash
  PhotoMover.exe "C:\Photos\Unsorted" "D:\Organized\Photos"
  ```
  Files from `C:\Photos\Unsorted` will be organized into `D:\Organized\Photos\year\month\day\`

**Parameters:**
- `sourceDirectory` (optional) - Directory to scan for photos and videos. If not provided, uses the application's directory.
- `destinationDirectory` (optional) - Root directory where organized files will be moved. If not provided, uses the same directory as the source.

## 🛠️ Requirements

- .NET 10 Runtime

## 📦 Dependencies

- [MetadataExtractor](https://github.com/drewnoakes/metadata-extractor-dotnet) - for reading EXIF metadata

## 📝 Licencja

MIT License

## 👤 Autor

Jarosław Kaliciński
