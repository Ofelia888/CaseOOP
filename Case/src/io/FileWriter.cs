namespace PluckList.src.io;

public class FileWriter : IContentWriter
{
    protected readonly string FilePath;
    
    public FileWriter(string filePath)
    {
        FilePath = filePath;
    }
    
    public virtual void Write(string content)
    {
        File.WriteAllText(FilePath, content);
    }
}
