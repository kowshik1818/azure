# File Browser Demo

This ASP.NET Core app displays a text file in the browser using two path sources:

- `/read/hardcoded` uses `Data/hardcoded.txt`, declared in `Program.cs`.
- `/read/config` uses `FilePath` from `appsettings.json`.

## Run

```powershell
dotnet run
```

Open the HTTPS or HTTP URL printed by ASP.NET Core. The home page links to both examples.
