using System.Text.RegularExpressions;

namespace PluckList;

public partial class HTMLTemplate
{
    private readonly string _contents;
    private readonly IEnumerable<string> _variables;
    
    private HTMLTemplate(string contents)
    {
        _contents = contents;
        var matches = VariablePattern().Matches(contents);
        _variables = matches.Select(match => match.Groups[1].Value);
    }

    public IEnumerable<string> GetVariables()
    {
        return _variables;
    }

    public string GetContents()
    {
        return _contents;
    }
    
    public string GetContents(Dictionary<string, string> variables)
    {
        var commonVariables = _variables.Where(variables.ContainsKey);
        return commonVariables.Aggregate(_contents,
            (contents, variable) => contents.Replace($"[{variable}]", variables[variable]));
    }

    public void Write(string filePath, Core.Models.PluckList pluckList)
    {
        var printItem = pluckList.GetPrintItem();
        if (printItem == null) throw new ArgumentException("No print item found");

        var vars = new Dictionary<string, string>
        {
            { "Name", pluckList.Name! },
            { "Adresse", pluckList.Address! },
            {
                "Plukliste",
                string.Join($"<br>{Environment.NewLine}",
                    pluckList.Lines.Select(item => $"{item.Title} (x{item.Amount})"))
            },
        };
        File.WriteAllText(filePath, GetContents(vars));
    }

    public static HTMLTemplate? Load(string path)
    {
        if (!File.Exists(path)) return null;
        var contents = File.ReadAllText(path);
        var template = new HTMLTemplate(contents);
        return template;
    }

    [GeneratedRegex(@"\[(.*?)\]")]
    private static partial Regex VariablePattern();
}
