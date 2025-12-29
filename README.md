# PhotoMover 📸

Automatyczne sortowanie zdjęć i filmów według daty do uporządkowanej struktury katalogów.

## 📋 Opis

PhotoMover to aplikacja konsolowa w C# (.NET 10), która automatycznie organizuje Twoje zdjęcia i filmy według daty. Program skanuje katalog, w którym się znajduje, wykrywa daty z nazw plików lub metadanych EXIF i przenosi pliki do struktury katalogów `Zdjecia/rok/miesiąc/dzień`.

## ✨ Funkcjonalności

- **🔍 Wykrywanie dat z nazw plików** - obsługuje wiele formatów:
  - `YYYY-MM-DD`, `YYYY_MM_DD`, `YYYY.MM.DD`
  - `YYYYMMDD`
  - `DD-MM-YYYY`, `DD_MM_YYYY`, `DD.MM.YYYY`
  - `DDMMYYYY`
  - `DD-MM-YY`, `MM-DD-YY` (z automatycznym określeniem wieku)

- **📷 Odczyt metadanych EXIF** - dla plików graficznych bez daty w nazwie sprawdza:
  - DateTimeOriginal
  - DateTimeDigitized
  - DateTime

- **🎬 Obsługa wielu formatów**
  - **Zdjęcia**: `.jpg`, `.jpeg`, `.png`, `.gif`, `.bmp`, `.tiff`, `.tif`, `.webp`, `.heic`, `.heif`, `.raw`, `.cr2`, `.nef`, `.arw`, `.dng`
  - **Filmy**: `.mp4`, `.avi`, `.mov`, `.wmv`, `.mkv`, `.flv`, `.webm`, `.m4v`, `.mpg`, `.mpeg`, `.3gp`, `.mts`, `.m2ts`

- **📁 Automatyczna organizacja**

- **🔄 Obsługa duplikatów** - jeśli plik o takiej nazwie już istnieje, dodaje `_1`, `_2`, itd.

- **⚠️ Folder Error** - pliki bez rozpoznanej daty trafiają do folderu `Error` w katalogu roboczym

## 🚀 Użycie

1. Skopiuj plik wykonywalny `PhotoMover.exe` do katalogu z plikami do posortowania
2. Uruchom aplikację
3. Program automatycznie przeniesie pliki do struktury `../Zdjecia/rok/miesiąc/dzień/`

## 🛠️ Wymagania

- .NET 10 Runtime

## 📦 Zależności

- [MetadataExtractor](https://github.com/drewnoakes/metadata-extractor-dotnet) - do odczytu metadanych EXIF

## 📝 Licencja

MIT License

## 👤 Autor

Jarosław Kaliciński
