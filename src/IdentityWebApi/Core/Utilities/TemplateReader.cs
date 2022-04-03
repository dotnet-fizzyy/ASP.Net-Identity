using System.IO;

namespace IdentityWebApi.Core.Utilities;

public static class TemplateReader
{
    public static string ReadTemplateFromFolder(string templateName)
    {
        var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Handlebars", $"{templateName}.hbs");
        var template = new StreamReader(pathToFile).ReadToEnd();

        return template;
    }
}
