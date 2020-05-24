namespace MolkZipping
{
    /// <summary>
    /// Used to get filename and filetype.
    /// </summary>
    public class FileInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public FileInfo(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
