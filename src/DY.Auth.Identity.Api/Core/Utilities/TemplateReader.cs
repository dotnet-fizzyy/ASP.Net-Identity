using System.IO;

namespace DY.Auth.Identity.Api.Core.Utilities;

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
        var projectDirectory = Directory.GetCurrentDirectory();
        var templateRootFolder = "wwwroot/Handlebars";
        var fullTemplateName = $"{templateName}.hbs";

        var pathToFile = Path.Combine(
            projectDirectory,
            templateRootFolder,
            fullTemplateName);

        using var streamReader = new StreamReader(pathToFile);

        var template = streamReader.ReadToEnd();

        return template;
    }
}
