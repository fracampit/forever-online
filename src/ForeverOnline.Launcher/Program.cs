using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text.Json;
using ForeverOnline.Launcher.Models;

var client = new HttpClient();
client.DefaultRequestHeaders.Add("User-Agent", "request");

var location = Assembly.GetExecutingAssembly().Location;
var currentPath = Path.GetDirectoryName(location);
if (currentPath == null) throw new DirectoryNotFoundException($"Failed to get current path. (location: {location})");
var appPath = Path.Combine(currentPath, "ForeverOnline");

var latestRelease = await GetLatestRelease();
string currentVersion;
try
{
    currentVersion = GetCurrentVersion();
}
catch (Exception e)
{
    Console.WriteLine(e);
    currentVersion = "0.0.0";
}

if (!IsLatestVersion(latestRelease.tag_name, currentVersion))
{
    await DownloadLatestRelease(latestRelease.assets_url);
}

RunApp();
return;

bool IsLatestVersion(string latest, string current)
{
    if (latest.StartsWith('v'))
    {
        latest = latest[1..];
    }

    if (current.EndsWith(".0"))
    {
        current = current[..^2];
    }

    if (latest != current)
    {
        Console.WriteLine("Current version: " + current);
        Console.WriteLine("Latest version: " + latest);
        Console.WriteLine("New version available. Updating...");
        return false;
    }

    Console.WriteLine("No new version available. Running app...");
    return true;
}

async Task<Release> GetLatestRelease()
{
    var response = await client.GetAsync($"https://api.github.com/repos/fracampit/forever-online/releases/latest");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var release = JsonSerializer.Deserialize<Release>(content);
    return release ?? throw new WebException($"Failed to get latest release: {response.ReasonPhrase}");
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
    // delete the zip file after extraction
    Console.WriteLine($"Deleting {zipFilePath}...");
    File.Delete(zipFilePath);
    Console.WriteLine($"Deleted {zipFilePath}");
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
        Console.WriteLine($"Directory {folderPath} does not exist. Creating...");
        Directory.CreateDirectory(folderPath);
        Console.WriteLine($"Created {folderPath}");
    }
}

void RunApp()
{
    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = Path.Combine(appPath, "ForeverOnline.exe"),
            Arguments = $"\"{appPath}\"",
            UseShellExecute = true
        }
    };
    Console.WriteLine($"Starting {process.StartInfo.FileName}...");
    process.Start();
}

string GetCurrentVersion()
{
    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = Path.Combine(appPath, "ForeverOnline.exe"),
            Arguments = $"\"{appPath}\"",
            UseShellExecute = true
        }
    };
    
    // get file version from process
    var fileVersionInfo = FileVersionInfo.GetVersionInfo(process.StartInfo.FileName);
    return fileVersionInfo.FileVersion ?? throw new FileLoadException("Failed to get current version.");
}