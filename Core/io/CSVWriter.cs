using System.Reflection;

namespace Core.io;

public class CSVWriter : FileWriter
{
    public CSVWriter(string filePath) : base(filePath)
    {
    }
    
    private static string[] GetValues<T>(IEnumerable<T> content, params FieldInfo[] fields)
    {
        var elements = content as T[] ?? content.ToArray();
        var contents = new string[elements.Length + 1];
        contents[0] = string.Join(",", fields.Select(field => field.Name));
        for (var i = 0; i < elements.Length; i++)
        {
            var element = elements[i];
            contents[i + 1] = string.Join(",", fields.Select(field => field.GetValue(element)));
        }
        return contents;
    }

    public void Write<T>(T content, bool append, params string[] fields)
    {
        var selectedFields = typeof(T).GetFields().Where(field => fields.Contains(field.Name)).ToArray();
        var contents = GetValues(new[] { content }, selectedFields);
        if (append) File.AppendAllLines(FilePath, File.Exists(FilePath) ? contents.Skip(1) : contents);
        else File.WriteAllLines(FilePath, contents);
    }

    public void Write<T>(T content, params string[] fields)
    {
        Write(content, false, fields);
    }

    public override void Write<T>(T content, bool append)
    {
        Write(content, append, typeof(T).GetFields().Select(field => field.Name).ToArray());
    }

    public void WriteAll<T>(IEnumerable<T> content, bool append, params string[] fields)
    {
        var selectedFields = typeof(T).GetFields().Where(field => fields.Contains(field.Name)).ToArray();
        var contents = GetValues(content, selectedFields);
        if (append) File.AppendAllLines(FilePath, File.Exists(FilePath) ? contents.Skip(1) : contents);
        else File.WriteAllLines(FilePath, contents);
    }
    
    public void WriteAll<T>(IEnumerable<T> content, params string[] fields)
    {
        WriteAll(content, false, fields);
    }

    public override void WriteAll<T>(IEnumerable<T> content, bool append)
    {
        WriteAll(content, append, typeof(T).GetFields().Select(field => field.Name).ToArray());
    }
}
