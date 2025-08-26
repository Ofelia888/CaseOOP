namespace PluckList.src.io;

public class FileWriter : IContentWriter
{
    protected readonly string FilePath;
    
    public FileWriter(string filePath)
    {
        FilePath = filePath;
    }
    
    public virtual void Write<T>(T content)
    {
        File.WriteAllText(FilePath, content?.ToString() ?? string.Empty);
    }

    public virtual void Write<T>(IEnumerable<T> content)
    {
        File.WriteAllLines(FilePath, content.Select(x => x?.ToString() ?? string.Empty));
    }
}
