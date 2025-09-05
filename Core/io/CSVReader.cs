namespace Core.io;

public class CSVReader<T>(string filePath, char separator = ',') : IDatabaseReader<T>
    where T : class, new()
{
    public readonly string FilePath = filePath;

    public List<T> ReadEntries(Predicate<T>? predicate = null)
    {
        var constructor = typeof(T).GetConstructor(Type.EmptyTypes);
        if (constructor == null) throw new ArgumentException($"Type does not have a default constructor: {typeof(T).FullName}");
        if (!File.Exists(FilePath)) return [];
        var lines = File.ReadAllLines(FilePath);
        var fields = (from field in typeof(T).GetFields()
            join column in lines[0].Split(separator) on field.Name equals column
            select field).ToArray();
        if (fields.Length == 0) return [];
        var equal = fields.Select(info => info.Name).Order().SequenceEqual(lines[0].Split(separator).Order());
        if (!equal) throw new InvalidCastException($"Cannot cast fields {string.Join(separator + " ", lines[0].Split(separator))} to {typeof(T).FullName}");
        var list = new List<T>();
        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(separator);
            var obj = constructor.Invoke([]);
            for (var j = 0; j < values.Length; j++)
            {
                var fieldValue = CSVHelper.ParseType(fields[j].FieldType, values[j]);
                fields[j].SetValue(obj, fieldValue);
            }
            var typedObject = (obj as T)!;
            if (predicate == null || predicate.Invoke(typedObject)) list.Add(typedObject);
        }
        return list;
    }

    public T? ReadEntry(Predicate<T> predicate)
    {
        return ReadEntries(predicate).FirstOrDefault();
    }
}
