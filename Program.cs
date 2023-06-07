using Md5Hasher.Utils;
using System.Security.Cryptography;
using System.Text.Json;

namespace Md5Hasher
{
    class Program
    {
        static void Main()
        {
            string previousPath = LoadPreviousPath();

            Console.WriteLine("Path : (1 to keep the old path)");
            string input = Console.ReadLine();

            string directoryPath;
            if (input == "1")
            {
                directoryPath = previousPath;
            }
            else
            {
                directoryPath = input;
                SavePath(input);
            }

            try
            {
                string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
                var fileHashes = new List<FileHash>();

                foreach (string filePath in files)
                {
                    string relativePath = filePath[(directoryPath.Length + 1)..];
                    string fileHash = CalculateMD5(filePath);
                    fileHashes.Add(new FileHash { Path = relativePath, Hash = fileHash });
                }

                string json = JsonSerializer.Serialize(fileHashes, new JsonSerializerOptions { WriteIndented = true });
                string jsonFilePath = "fileHashes.json";
                File.WriteAllText(jsonFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occurred : {ex.Message}");
            }
        }

        static string LoadPreviousPath()
        {
            string pathFile = "path.json";
            if (File.Exists(pathFile))
            {
                string json = File.ReadAllText(pathFile);
                return JsonSerializer.Deserialize<PathData>(json)?.Path;
            }

            return string.Empty;
        }

        static void SavePath(string path)
        {
            string pathFile = "path.json";
            string json = JsonSerializer.Serialize(new PathData { Path = path });
            File.WriteAllText(pathFile, json);
        }

        static string CalculateMD5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}