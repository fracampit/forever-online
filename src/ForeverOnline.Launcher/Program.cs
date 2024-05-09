using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using ForeverOnline.Launcher.Models;

Console.WriteLine("Enter the token for accessing GitHub:");
var token = Console.ReadLine();
while (token?.Length != 40)
{
    Console.WriteLine("Invalid token. Enter the token for accessing GitHub:");
    token = Console.ReadLine();
}

var client = new HttpClient();
client.DefaultRequestHeaders.Add("User-Agent", "request");
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

Console.WriteLine("Enter the path for downloading the latest release and installing it:");
var appPath = Console.ReadLine();
while (!Directory.Exists(appPath))
{
    Console.WriteLine("Invalid path. Enter the path for downloading the latest release and installing it:");
    appPath = Console.ReadLine();
}

var latestRelease = await GetLatestRelease();
var currentVersion = GetCurrentVersion();

if (latestRelease.tag_name != currentVersion)
{
    Console.WriteLine("New version available. Updating...");
    await DownloadLatestRelease(latestRelease.assets_url);
}
else
{
    Console.WriteLine("No new version available. Running app...");
}

RunApp();
return;

async Task<Release> GetLatestRelease()
{
    var response = await client.GetAsync($"https://api.github.com/repos/fracampit/forever-online/releases/latest");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var release = JsonSerializer.Deserialize<Release>(content);
    return release ?? throw new WebException($"Failed to get latest release: {response.ReasonPhrase}");
}

static string GetCurrentVersion()
{
    // Implement your logic to get the current version of your app
    return "v1.0.0";
}

async Task DownloadLatestRelease(string assetsUrl)
{
    var response = await client.GetAsync(assetsUrl);
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var assets = JsonSerializer.Deserialize<Asset[]>(content);
    if (assets == null) throw new WebException($"Failed to get assets: {response.ReasonPhrase}");
    var asset = assets.Single(a => a.name.EndsWith(".zip"));

    CleanUpFolder(appPath);
    await DownloadFile(asset.browser_download_url, Path.Combine(appPath, asset.name));
    ExtractZipFile(Path.Combine(appPath, asset.name), appPath);
}

async Task DownloadFile(string url, string outputPath)
{
    Console.WriteLine($"Downloading {url}...");
    var response = await client.GetAsync(url);
    response.EnsureSuccessStatusCode();
    await using var outputFileStream = File.Create(outputPath);
    await response.Content.CopyToAsync(outputFileStream);
    Console.WriteLine($"Downloaded {url} to {outputPath}");
}

void ExtractZipFile(string zipFilePath, string outputDirectory)
{
    Console.WriteLine($"Extracting {zipFilePath} to {outputDirectory}...");
    ZipFile.ExtractToDirectory(zipFilePath, outputDirectory);
    Console.WriteLine($"Extracted {zipFilePath} to {outputDirectory}");
}

// method to clean up the folder
void CleanUpFolder(string folderPath)
{
    if (Directory.Exists(folderPath))
    {
        Console.WriteLine($"Cleaning up {folderPath}...");
        var files = Directory.GetFiles(folderPath);
        
        foreach (var file in files)
        {
            Console.WriteLine($"Deleting {file}...");
            File.Delete(file);
            Console.WriteLine($"Deleted {file}");
        }
        
        var directories = Directory.GetDirectories(folderPath);
        
        foreach (var directory in directories)
        {
            Console.WriteLine($"Deleting {directory}...");
            Directory.Delete(directory, true);
            Console.WriteLine($"Deleted {directory}");
        }
    }
    else
    {
        Console.WriteLine($"Directory {folderPath} does not exist.");
    }
}

void RunApp()
{
    var filePath = Path.Combine(appPath, @"publish\ForeverOnline\app");
    
    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = Path.Combine(filePath, "ForeverOnline.exe"),
            Arguments = $"\"{filePath}\"",
            UseShellExecute = true
        }
    };
    Console.WriteLine($"Starting {process.StartInfo.FileName}...");
    process.Start();
}