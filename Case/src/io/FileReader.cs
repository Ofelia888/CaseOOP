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
        return File.ReadAllLines(FilePath).ToList();
    }
}
