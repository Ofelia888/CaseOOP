namespace PluckList.src.io;

public class CSVWriter : FileWriter
{
    public CSVWriter(string filePath) : base(filePath)
    {
    }

    public override void WriteAll<T>(IEnumerable<T> content)
    {
        var fields = typeof(T).GetFields();
        var elements = content as T[] ?? content.ToArray();
        var contents = new string[elements.Length + 1];
        contents[0] = string.Join(",", fields.Select(field => field.Name));
        for (var i = 0; i < elements.Length; i++)
        {
            var element = elements[i];
            contents[i + 1] = string.Join(",", fields.Select(field => field.GetValue(element)));
        }
        File.WriteAllLines(FilePath, contents);
    }
}
