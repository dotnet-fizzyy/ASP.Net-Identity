using System;
using System.Collections.Generic;
using System.Linq;

namespace DY.Auth.Identity.Api.Core.Exceptions;

/// <summary>
/// Model validation exception.
/// </summary>
/// <param name="modelName">Name of model errors occured.</param>
/// <param name="errors">Collection of model fields errors.</param>
public class ModelValidationException(string modelName, IEnumerable<string> errors) : Exception
{
    /// <summary>
    /// Gets model name.
    /// </summary>
    public string ModelName { get; } = modelName;

    /// <summary>
    /// Gets model fields errors.
    /// </summary>
    public List<string> Errors { get; } = errors.ToList();
}
