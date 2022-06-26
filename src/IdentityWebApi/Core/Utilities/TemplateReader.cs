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
        var assemblyLocation = typeof(TemplateReader).Assembly.Location;
        var projectDirectory = Path.GetDirectoryName(assemblyLocation)!;
        var templateRootFolder = "wwwroot/Handlebars";
        var fullTemplateName = $"{templateName}.hbs";

        var pathToFile = Path.Combine(
            projectDirectory,
            templateRootFolder,
            fullTemplateName
        );

        var template = new StreamReader(pathToFile).ReadToEnd();

        return template;
    }
}
