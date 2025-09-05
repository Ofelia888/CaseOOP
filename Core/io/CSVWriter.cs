using System.Reflection;

namespace Core.io;

public class CSVWriter<T>(string filePath, char separator = ',') : IDatabaseWriter<T>
    where T : class, new()
{
    public readonly string FilePath = filePath;

    public void AddEntry(T entry, DatabaseWriteOptions? options = null)
    {
        AddEntries([entry], options);
    }

    public void AddEntries(IEnumerable<T> entries, DatabaseWriteOptions? options = null)
    {
        options ??= new DatabaseWriteOptions();
        var fields = typeof(T).GetFields();
        var offset = options.Append ? GetLastIndex() : 0;
        var contents = GetValues(entries, options, offset, fields);
        if (options.Append && File.Exists(FilePath)) File.AppendAllLines(FilePath, contents.Skip(1));
        else File.WriteAllLines(FilePath, contents);
    }

    public int Remove(Predicate<T> predicate)
    {
        if (!File.Exists(FilePath)) return 0;
        var lines = File.ReadAllLines(FilePath);
        var modified = new List<string> { lines[0] };
        var fields = (from field in typeof(T).GetFields()
            join column in lines[0].Split(separator) on field.Name equals column
            select field).ToArray();
        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(separator);
            var entry = new T();
            for (var j = 0; j < values.Length; j++)
            {
                var fieldValue = CSVHelper.ParseType(fields[j].FieldType, values[j]);
                fields[j].SetValue(entry, fieldValue);
            }
            if (!predicate.Invoke(entry)) modified.Add(lines[i]);
        }
        var changed = lines.Length - modified.Count;
        if (changed > 0) File.WriteAllLines(FilePath, modified);
        return changed;
    }

    public int Remove(T entry)
    {
        return Remove(obj => obj.Equals(entry));
    }

    private int GetLastIndex()
    {
        if (!File.Exists(FilePath)) return 0;
        var last = File.ReadAllLines(FilePath).Last();
        var separatorIndex = last.IndexOf(separator);
        var idString = separatorIndex == -1 ? last : last[..separatorIndex];
        return int.TryParse(idString, out var lastIndex) ? lastIndex : 0;
    }
    
    private string[] GetValues(IEnumerable<T> content, DatabaseWriteOptions options, int offset, params FieldInfo[] fields)
    {
        var elements = content as T[] ?? content.ToArray();
        var contents = new string[elements.Length + 1];
        contents[0] = string.Join(separator, fields.Select(field => field.Name));
        if (options.GenerateId) contents[0] = "Id" + separator + contents[0];
        for (var i = 0; i < elements.Length; i++)
        {
            var element = elements[i];
            contents[i + 1] = string.Join(separator, fields.Select(field => field.GetValue(element)));
            if (options.GenerateId) contents[i + 1] = $"{offset + i + 1}" + separator + contents[i + 1];
        }
        return contents;
    }
}
