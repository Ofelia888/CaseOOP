namespace PluckList.src.io;

public class FileReader : IContentReader
{
    protected readonly string FilePath;

    public FileReader(string filePath)
    {
        FilePath = filePath;
    }

    public virtual List<string> ReadList()
    {
        if (Directory.Exists(FilePath)) return Directory.EnumerateFiles(FilePath).ToList();
        return File.Exists(FilePath) ? File.ReadAllLines(FilePath).ToList() : new List<string>();
    }
}
