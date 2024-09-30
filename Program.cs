using MD5Lister.Utils;
using System.Security.Cryptography;
using System.Text.Json;

namespace MD5Lister
{
    class Program
    {
        static void Main()
        {
            string previousPath = LoadPreviousPath();

            Console.WriteLine("Enter directory path (press Enter to use the previous path):");
            string input = Console.ReadLine();

            string directoryPath = string.IsNullOrWhiteSpace(input) ? previousPath : input;

            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                Console.WriteLine("Invalid path. Please try again.");
                return;
            }

            SavePath(directoryPath);

            try
            {
                var fileHashes = new List<FileHash>();
                foreach (string filePath in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories))
                {
                    string relativePath = Path.GetRelativePath(directoryPath, filePath);
                    string fileHash = CalculateMD5(filePath);
                    fileHashes.Add(new FileHash { Path = relativePath, Hash = fileHash });
                }

                string json = JsonSerializer.Serialize(fileHashes, new JsonSerializerOptions { WriteIndented = true });
                string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fileHashes.json");
                File.WriteAllText(jsonFilePath, json);

                Console.WriteLine($"Hashes have been saved to {jsonFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static string LoadPreviousPath()
        {
            string pathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "path.json");
            if (File.Exists(pathFile))
            {
                string json = File.ReadAllText(pathFile);
                return JsonSerializer.Deserialize<PathData>(json)?.Path ?? string.Empty;
            }
            return string.Empty;
        }

        static void SavePath(string path)
        {
            string pathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "path.json");
            string json = JsonSerializer.Serialize(new PathData { Path = path });
            File.WriteAllText(pathFile, json);
        }

        static string CalculateMD5(string filePath)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            byte[] hashBytes = md5.ComputeHash(stream);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
    }
}