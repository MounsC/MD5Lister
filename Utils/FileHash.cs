namespace Md5Hasher.Utils
{
    class FileHashes
    {
        public List<FileHash> Files { get; set; }
        public FileHashes()
        {
            Files = new List<FileHash>();
        }
    }

    class FileHash
    {
        public string Path { get; set; }
        public string Hash { get; set; }
    }
}
