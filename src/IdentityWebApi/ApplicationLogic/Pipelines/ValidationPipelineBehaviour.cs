using FluentValidation;

using IdentityWebApi.Core.Exceptions;

using MediatR;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Pipelines;

/// <summary>
/// Base validation behaviour for commands/queries.
/// </summary>
/// <typeparam name="TRequest">Incoming command/query request.</typeparam>
/// <typeparam name="TResponse">Out coming command/query response.</typeparam>
public class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationPipelineBehaviour{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="validators">Collection of <see cref="IValidator{T}"/>.</param>
    public ValidationPipelineBehaviour(
        IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (this.validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                this.validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                throw new ModelValidationException(
                    typeof(TRequest).Name,
                    failures.Select(x => x.ErrorMessage));
            }
        }

        return await next();
    }
}
