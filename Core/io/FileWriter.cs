namespace Core.io;

public class FileWriter : IContentWriter
{
    public readonly string FilePath;
    
    public FileWriter(string filePath)
    {
        FilePath = filePath;
    }
    
    public virtual void Write<T>(T content, bool append)
    {
        if (append)
        {
            File.AppendAllText(FilePath, content?.ToString() ?? string.Empty);
        }
        else
        {
            File.WriteAllText(FilePath, content?.ToString() ?? string.Empty);
        }
    }

    public void Write<T>(T content)
    {
        Write(content, false);
    }

    public virtual void WriteAll<T>(IEnumerable<T> content, bool append)
    {
        if (append)
        {
            File.AppendAllLines(FilePath, content.Select(x => x?.ToString() ?? string.Empty));
        }
        else
        {
            File.WriteAllLines(FilePath, content.Select(x => x?.ToString() ?? string.Empty));
        }
    }

    public void WriteAll<T>(IEnumerable<T> content)
    {
        WriteAll(content, false);
    }
}
