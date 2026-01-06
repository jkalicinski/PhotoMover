using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

if (args.Length > 0 && (args[0] == "--version" || args[0] == "-v"))
{
    var version = Assembly.GetExecutingAssembly().GetName().Version;
    var informationalVersion = Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.
        InformationalVersion ?? version?.ToString() ?? "Unknown";
    Console.WriteLine($"PhotoMover v{informationalVersion}");
    return;
}

string sourceDirectory;
string destinationDirectory;

if (args.Length >= 1 && !string.IsNullOrWhiteSpace(args[0]))
{
    sourceDirectory = Path.GetFullPath(args[0]);
    if (!System.IO.Directory.Exists(sourceDirectory))
    {
        Console.WriteLine($"Source directory does not exist: {sourceDirectory}");
        return;
    }
}
else
{
    sourceDirectory = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
}

if (args.Length >= 2 && !string.IsNullOrWhiteSpace(args[1]))
{
    destinationDirectory = Path.GetFullPath(args[1]);
}
else
{
    destinationDirectory = sourceDirectory;
}

var executablePath = sourceDirectory + Path.DirectorySeparatorChar;
var errorDirectory = Path.Combine(executablePath, "Error");
var photosDirectory = destinationDirectory;

Console.WriteLine($"Source directory: {sourceDirectory}");
Console.WriteLine($"Destination directory: {photosDirectory}");
Console.WriteLine($"Error directory: {errorDirectory}");
Console.WriteLine();

var imageExtensions = new HashSet<string>
{
    ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif", ".webp", ".heic", ".heif", ".raw", ".cr2", ".nef", ".arw", ".dng"
};

var videoExtensions = new HashSet<string>
{
    ".mp4", ".avi", ".mov", ".wmv", ".mkv", ".flv", ".webm", ".m4v", ".mpg", ".mpeg", ".3gp", ".mts", ".m2ts"
};

var files = System.IO.Directory.GetFiles(executablePath);
var processedCount = 0;
var errorCount = 0;
var skippedCount = 0;

foreach (var file in files)
{
    var fileName = Path.GetFileName(file);
    var extension = Path.GetExtension(file).ToLowerInvariant();

    if (fileName == Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName))
    {
        continue;
    }

    if (!imageExtensions.Contains(extension) && !videoExtensions.Contains(extension))
    {
        skippedCount++;
        continue;
    }

    Console.WriteLine($"Processing: {fileName}");

    DateTime? fileDate = null;

    fileDate = TryExtractDateFromFileName(fileName);

    if (!fileDate.HasValue && imageExtensions.Contains(extension))
    {
        fileDate = TryExtractDateFromExif(file);
    }

    if (fileDate.HasValue)
    {
        var year = fileDate.Value.Year.ToString();
        var month = fileDate.Value.Month.ToString("D2");
        var day = fileDate.Value.Day.ToString("D2");

        var targetDirectory = Path.Combine(photosDirectory, year, month, day);
        System.IO.Directory.CreateDirectory(targetDirectory);

        var targetPath = GetUniqueFilePath(targetDirectory, fileName);

        try
        {
            File.Move(file, targetPath);
            Console.WriteLine($"  → Moved to: {Path.GetRelativePath(sourceDirectory, targetPath)}");
            processedCount++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ✗ Move error: {ex.Message}");
            errorCount++;
        }
    }
    else
    {
        System.IO.Directory.CreateDirectory(errorDirectory);
        var targetPath = GetUniqueFilePath(errorDirectory, fileName);

        try
        {
            File.Move(file, targetPath);
            Console.WriteLine($"  → Moved to Error (no date found)");
            errorCount++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ✗ Error moving to Error folder: {ex.Message}");
        }
    }
}

Console.WriteLine();
Console.WriteLine($"Completed. Processed: {processedCount}, Errors/No date: {errorCount}, Skipped (other files): {skippedCount}");

static DateTime? TryExtractDateFromFileName(string fileName)
{
    var patterns = new[]
    {
        @"(\d{4})[-_.](\d{2})[-_.](\d{2})",
        @"(\d{4})(\d{2})(\d{2})",
        @"(\d{2})[-_.](\d{2})[-_.](\d{4})",
        @"(\d{2})(\d{2})(\d{4})",
        @"(\d{2})[-_/](\d{2})[-_/](\d{2})",
        @"(\d{8})",
    };

    foreach (var pattern in patterns)
    {
        var match = Regex.Match(fileName, pattern);
        if (match.Success)
        {
            try
            {
                if (pattern.Contains("4") && match.Groups[1].Value.Length == 4)
                {
                    var year = int.Parse(match.Groups[1].Value);
                    var month = int.Parse(match.Groups[2].Value);
                    var day = int.Parse(match.Groups[3].Value);

                    if (year >= 1900 && year <= 2100 && month >= 1 && month <= 12 && day >= 1 && day <= 31)
                    {
                        return new DateTime(year, month, day);
                    }
                }
                else if (match.Groups[1].Value.Length == 8)
                {
                    var dateStr = match.Groups[1].Value;
                    if (DateTime.TryParseExact(dateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    {
                        return date;
                    }
                    if (DateTime.TryParseExact(dateStr, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        return date;
                    }
                }
                else if (match.Groups[3].Value.Length == 4)
                {
                    var day = int.Parse(match.Groups[1].Value);
                    var month = int.Parse(match.Groups[2].Value);
                    var year = int.Parse(match.Groups[3].Value);

                    if (year >= 1900 && year <= 2100 && month >= 1 && month <= 12 && day >= 1 && day <= 31)
                    {
                        return new DateTime(year, month, day);
                    }
                }
                else if (match.Groups[1].Value.Length == 2)
                {
                    var part1 = int.Parse(match.Groups[1].Value);
                    var part2 = int.Parse(match.Groups[2].Value);
                    var part3 = int.Parse(match.Groups[3].Value);

                    if (part3 >= 0 && part3 <= 99)
                    {
                        var year = part3 < 50 ? 2000 + part3 : 1900 + part3;

                        if (part1 >= 1 && part1 <= 12 && part2 >= 1 && part2 <= 31)
                        {
                            return new DateTime(year, part1, part2);
                        }
                        if (part2 >= 1 && part2 <= 12 && part1 >= 1 && part1 <= 31)
                        {
                            return new DateTime(year, part2, part1);
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }

    return null;
}

static DateTime? TryExtractDateFromExif(string filePath)
{
    try
    {
        var directories = ImageMetadataReader.ReadMetadata(filePath);

        var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
        if (subIfdDirectory != null)
        {
            if (subIfdDirectory.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out var dateTime))
            {
                return dateTime;
            }
            if (subIfdDirectory.TryGetDateTime(ExifDirectoryBase.TagDateTimeDigitized, out dateTime))
            {
                return dateTime;
            }
        }

        var exifDirectory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
        if (exifDirectory != null)
        {
            if (exifDirectory.TryGetDateTime(ExifDirectoryBase.TagDateTime, out var dateTime))
            {
                return dateTime;
            }
        }
    }
    catch
    {
    }

    return null;
}

static string GetUniqueFilePath(string directory, string fileName)
{
    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
    var extension = Path.GetExtension(fileName);
    var targetPath = Path.Combine(directory, fileName);
    var counter = 1;

    while (File.Exists(targetPath))
    {
        var newFileName = $"{fileNameWithoutExtension}_{counter}{extension}";
        targetPath = Path.Combine(directory, newFileName);
        counter++;
    }

    return targetPath;
}
