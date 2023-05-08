using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityWebApi.Core.Exceptions;

/// <summary>
/// Model validation exception.
/// </summary>
public class ModelValidationException : Exception
{
    /// <summary>
    /// Gets model name.
    /// </summary>
    public string ModelName { get; }

    /// <summary>
    /// Gets model fields errors.
    /// </summary>
    public List<string> Errors { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelValidationException"/> class.
    /// </summary>
    /// <param name="modelName">Name of model errors occured.</param>
    /// <param name="errors">Collection of model fields errors.</param>
    public ModelValidationException(string modelName, IEnumerable<string> errors)
    {
        this.ModelName = modelName;
        this.Errors = errors.ToList();
    }
}
