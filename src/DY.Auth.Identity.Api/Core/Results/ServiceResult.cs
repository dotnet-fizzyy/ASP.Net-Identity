using DY.Auth.Identity.Api.Core.Enums;

namespace DY.Auth.Identity.Api.Core.Results;

/// <summary>
/// Optional pattern implementation without arguments.
/// </summary>
public class ServiceResult
{
    /// <summary>
    /// Gets or sets operation result.
    /// </summary>
    public ServiceResultType Result { get; set; }

    /// <summary>
    /// Gets or sets operation error message.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets a value indicating whether operation result is not successful.
    /// </summary>
    public bool IsResultFailed => this.Result != ServiceResultType.Success;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResult"/> class.
    /// </summary>
    /// <param name="result">Operation status indicator.</param>
    public ServiceResult(ServiceResultType result)
    {
        this.Result = result;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResult"/> class.
    /// </summary>
    /// <param name="result">Operation status indicator.</param>
    /// <param name="errorMessage">Error description message.</param>
    public ServiceResult(ServiceResultType result, string errorMessage)
        : this(result)
    {
        this.ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResult"/> class.
    /// </summary>
    protected ServiceResult()
    {
    }

    /// <summary>
    /// Generates error result for current instance.
    /// </summary>
    /// <typeparam name="TReturnType">Type for conversion.</typeparam>
    /// <returns><see cref="ServiceResult"/>.</returns>
    public ServiceResult<TReturnType> GenerateErrorResult<TReturnType>() =>
        new (this.Result, this.ErrorMessage);
}

/// <summary>
/// Optional pattern implementation with arguments.
/// </summary>
/// <typeparam name="T">Represents operation result data.</typeparam>
public class ServiceResult<T> : ServiceResult
{
    /// <summary>
    /// Gets or sets operation result data.
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResult{T}"/> class.
    /// </summary>
    public ServiceResult()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResult{T}"/> class.
    /// </summary>
    /// <param name="result">Operation status indicator.</param>
    public ServiceResult(ServiceResultType result)
    {
        this.Result = result;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResult{T}"/> class.
    /// </summary>
    /// <param name="data">Result of performed operation.</param>
    public ServiceResult(T data)
    {
        this.Result = ServiceResultType.Success;
        this.Data = data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResult{T}"/> class.
    /// </summary>
    /// <param name="result">Operation status indicator.</param>
    /// <param name="errorMessage">Error description message.</param>
    public ServiceResult(ServiceResultType result, string errorMessage)
        : this(result)
    {
        this.ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResult{T}"/> class.
    /// </summary>
    /// <param name="result">Operation status indicator.</param>
    /// <param name="data">Result of performed operation.</param>
    public ServiceResult(ServiceResultType result, T data)
        : this(result)
    {
        this.Data = data;
    }
}
