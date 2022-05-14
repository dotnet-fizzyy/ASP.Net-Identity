using System.IO;

namespace IdentityWebApi.Core.Utilities;

/// <summary>
/// Template reader.
/// </summary>
public static class TemplateReader
{
    /// <summary>
    /// Search for existing default templates inside of application.
    /// </summary>
    /// <param name="templateName">Template name required for search (no extension needed).</param>
    /// <returns>Stringified content of template.</returns>
    public static string ReadTemplateFromFolder(string templateName)
    {
        var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Handlebars", $"{templateName}.hbs");
        var template = new StreamReader(pathToFile).ReadToEnd();

        return template;
    }
}
