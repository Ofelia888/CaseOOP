using System.Reflection;

namespace Core.io;

public class CSVWriter : FileWriter
{
    public CSVWriter(string filePath) : base(filePath)
    {
    }
    
    private static string[] GetValues<T>(IEnumerable<T> content, bool id, int offset, params FieldInfo[] fields)
    {
        var elements = content as T[] ?? content.ToArray();
        var contents = new string[elements.Length + 1];
        contents[0] = string.Join(",", fields.Select(field => field.Name));
        if (id) contents[0] = "Id," + contents[0];
        for (var i = 0; i < elements.Length; i++)
        {
            var element = elements[i];
            contents[i + 1] = string.Join(",", fields.Select(field => field.GetValue(element)));
            if (id) contents[i + 1] = $"{offset + i + 1}," + contents[i + 1];
        }
        return contents;
    }

    private int GetLastIndex()
    {
        if (!File.Exists(FilePath)) return 0;
        var last = File.ReadAllLines(FilePath).Last();
        var separator = last.IndexOf(',');
        var idString = separator == -1 ? last : last[..separator];
        return int.TryParse(idString, out var lastIndex) ? lastIndex : 0;
    }

    public void Write<T>(T content, bool append, bool id, params string[] fields)
    {
        var selectedFields = typeof(T).GetFields().Where(field => fields.Contains(field.Name)).ToArray();
        var offset = append ? GetLastIndex() : 0;
        var contents = GetValues(new[] { content }, id, offset, selectedFields);
        if (append) File.AppendAllLines(FilePath, File.Exists(FilePath) ? contents.Skip(1) : contents);
        else File.WriteAllLines(FilePath, contents);
    }

    public void Write<T>(T content, params string[] fields)
    {
        Write(content, false, false, fields);
    }

    public override void Write<T>(T content, bool append)
    {
        Write(content, append, false, typeof(T).GetFields().Select(field => field.Name).ToArray());
    }

    public void WriteAll<T>(IEnumerable<T> content, bool append, bool id, params string[] fields)
    {
        var selectedFields = typeof(T).GetFields().Where(field => fields.Contains(field.Name)).ToArray();
        var offset = append ? GetLastIndex() : 0;
        var contents = GetValues(content, id, offset, selectedFields);
        if (append) File.AppendAllLines(FilePath, File.Exists(FilePath) ? contents.Skip(1) : contents);
        else File.WriteAllLines(FilePath, contents);
    }
    
    public void WriteAll<T>(IEnumerable<T> contents, bool append, params string[] fields)
    {
        WriteAll(contents, append, false, fields);
    }
    
    public void WriteAll<T>(IEnumerable<T> content, params string[] fields)
    {
        WriteAll(content, false, fields);
    }

    public override void WriteAll<T>(IEnumerable<T> content, bool append)
    {
        WriteAll(content, append, typeof(T).GetFields().Select(field => field.Name).ToArray());
    }

    public int Remove(Predicate<KeyValuePair<string, string>> predicate)
    {
        if (!File.Exists(FilePath)) return 0;
        var lines = File.ReadAllLines(FilePath);
        var modified = new List<string> { lines[0] };
        var columns = lines[0].Split(',');
        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (!columns.Where((t, j) => predicate.Invoke(new KeyValuePair<string, string>(t, values[j]))).Any())
            {
                modified.Add(lines[i]);
            }
        }
        var changed = lines.Length - modified.Count;
        if (changed > 0) File.WriteAllLines(FilePath, modified);
        return changed;
    }
}
