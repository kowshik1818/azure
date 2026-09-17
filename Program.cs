var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string hardcodedRelativePath = "Data/hardcoded.txt";

app.MapGet("/", () => Results.Content("""
	<!doctype html>
	<html lang="en">
	<head>
		<meta charset="utf-8">
		<title>File Reader Demo</title>
		<style>
			body {{ font-family: system-ui, sans-serif; max-width: 760px; margin: 48px auto; padding: 0 20px; }}
			a {{ display: inline-block; margin: 8px 8px 8px 0; }}
			pre {{ background: #f3f4f6; padding: 16px; white-space: pre-wrap; }}
		</style>
	</head>
	<body>
		<h1>Read a file</h1>
		<p>Choose how the path is supplied:</p>
		<a href="/read/hardcoded">Read using a hardcoded C# path</a>
		<a href="/read/config">Read using appsettings.json</a>
	</body>
	</html>
	""", "text/html"));

app.MapGet("/read/hardcoded", async (IWebHostEnvironment environment) =>
	await ReadFileResultAsync(Path.Combine(environment.ContentRootPath, hardcodedRelativePath)));

app.MapGet("/read/config", async (IConfiguration configuration, IWebHostEnvironment environment) =>
{
	var configuredRelativePath = configuration["FilePath"];
	if (string.IsNullOrWhiteSpace(configuredRelativePath))
	{
		return Results.Problem("FilePath is missing from appsettings.json.");
	}

	var fullPath = Path.Combine(environment.ContentRootPath, configuredRelativePath);
	return await ReadFileResultAsync(fullPath);
});

app.Run();

static async Task<IResult> ReadFileResultAsync(string path)
{
	if (!File.Exists(path))
	{
		return Results.NotFound($"File not found: {path}");
	}

	var content = await File.ReadAllTextAsync(path);
	var escapedContent = System.Net.WebUtility.HtmlEncode(content);
	return Results.Content($"""
		<!doctype html>
		<html lang="en">
		<head><meta charset="utf-8"><title>File contents</title></head>
		<body style="font-family: system-ui, sans-serif; max-width: 760px; margin: 48px auto; padding: 0 20px;">
			<p><a href="/">Back</a></p>
			<h1>File contents</h1>
			<pre>{escapedContent}</pre>
		</body>
		</html>
		""", "text/html");
}
