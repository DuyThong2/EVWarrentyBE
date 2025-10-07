using Scriban;

namespace BuildingBlocks.Email.Templates;

public class TemplateRenderer
{
    private readonly string _layoutPath;

    public TemplateRenderer(string layoutPath = "Templates/Layout.html")
    {
        _layoutPath = layoutPath;
    }

    public string Render(string subject, string content)
    {
        var layout = File.ReadAllText(_layoutPath);

        var template = Template.Parse(layout);

        var result = template.Render(new
        {
            subject,
            content
        });

        return result;
    }
}
