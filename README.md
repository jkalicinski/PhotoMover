# PhotoMover ??

Automatyczne sortowanie zdjêæ i filmów wed³ug daty do uporz¹dkowanej struktury katalogów.

## ?? Opis

PhotoMover to aplikacja konsolowa w C# (.NET 10), która automatycznie organizuje Twoje zdjêcia i filmy wed³ug daty. Program skanuje katalog, w którym siê znajduje, wykrywa daty z nazw plików lub metadanych EXIF i przenosi pliki do struktury katalogów `Zdjecia/rok/miesi¹c/dzieñ`.

## ? Funkcjonalnoœci

- **?? Wykrywanie dat z nazw plików** - obs³uguje wiele formatów:
  - `YYYY-MM-DD`, `YYYY_MM_DD`, `YYYY.MM.DD`
  - `YYYYMMDD`
  - `DD-MM-YYYY`, `DD_MM_YYYY`, `DD.MM.YYYY`
  - `DDMMYYYY`
  - `DD-MM-YY`, `MM-DD-YY` (z automatycznym okreœleniem wieku)

- **?? Odczyt metadanych EXIF** - dla plików graficznych bez daty w nazwie sprawdza:
  - DateTimeOriginal
  - DateTimeDigitized
  - DateTime

- **?? Obs³uga wielu formatów**
  - **Zdjêcia**: `.jpg`, `.jpeg`, `.png`, `.gif`, `.bmp`, `.tiff`, `.tif`, `.webp`, `.heic`, `.heif`, `.raw`, `.cr2`, `.nef`, `.arw`, `.dng`
  - **Filmy**: `.mp4`, `.avi`, `.mov`, `.wmv`, `.mkv`, `.flv`, `.webm`, `.m4v`, `.mpg`, `.mpeg`, `.3gp`, `.mts`, `.m2ts`

- **?? Automatyczna organizacja** - tworzy strukturê:
  ```
  Zdjecia/
  ??? 2024/
  ?   ??? 01/
  ?   ?   ??? 15/
  ?   ?   ?   ??? zdjecie1.jpg
  ?   ?   ?   ??? zdjecie2.jpg
  ?   ?   ??? 31/
  ?   ??? 12/
  ??? 2023/
  ```

- **?? Obs³uga duplikatów** - jeœli plik o takiej nazwie ju¿ istnieje, dodaje `_1`, `_2`, itd.

- **?? Folder Error** - pliki bez rozpoznanej daty trafiaj¹ do folderu `Error` w katalogu roboczym

## ?? U¿ycie

1. Skopiuj plik wykonywalny `PhotoMover.exe` do katalogu z plikami do posortowania
2. Uruchom aplikacjê
3. Program automatycznie:
   - Przeskanuje wszystkie pliki w bie¿¹cym katalogu (bez podfolderów)
   - Wykryje daty z nazw plików lub metadanych EXIF
   - Przeniesie pliki do struktury `../Zdjecia/rok/miesi¹c/dzieñ/`
   - Pliki bez daty przeniesie do folderu `Error`

### Przyk³ad

```
Katalog przed:
C:\Dropbox\Nowe\
??? PhotoMover.exe
??? 2024-01-15_wakacje.jpg
??? IMG_20231225.jpg
??? film_bez_daty.mp4

Katalog po:
C:\Dropbox\
??? Nowe\
?   ??? PhotoMover.exe
?   ??? Error\
?       ??? film_bez_daty.mp4
??? Zdjecia\
    ??? 2024\
    ?   ??? 01\
    ?       ??? 15\
    ?           ??? 2024-01-15_wakacje.jpg
    ??? 2023\
        ??? 12\
            ??? 25\
                ??? IMG_20231225.jpg
```

## ??? Wymagania

- .NET 10 Runtime

## ?? Zale¿noœci

- [MetadataExtractor](https://github.com/drewnoakes/metadata-extractor-dotnet) - do odczytu metadanych EXIF

## ?? Budowanie

```bash
git clone https://github.com/jkalicinski/PhotoMover.git
cd PhotoMover
dotnet build
dotnet run
```

## ?? Statystyki

Po zakoñczeniu pracy aplikacja wyœwietla podsumowanie:
- Liczba przetworzonych plików
- Liczba plików bez daty (w folderze Error)
- Liczba pominiêtych plików (inne ni¿ zdjêcia/filmy)

## ?? Licencja

MIT License

## ?? Autor

Jaros³aw Kaliciñski

## ?? Wspó³praca

Pull requesty s¹ mile widziane! W przypadku wiêkszych zmian, proszê najpierw otworzyæ issue, aby przedyskutowaæ proponowane zmiany.
