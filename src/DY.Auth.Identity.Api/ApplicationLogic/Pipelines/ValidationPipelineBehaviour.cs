using DY.Auth.Identity.Api.Core.Exceptions;

using FluentValidation;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Pipelines;

/// <summary>
/// Base validation behavior for commands/queries.
/// </summary>
/// <typeparam name="TRequest">Incoming command/query request.</typeparam>
/// <typeparam name="TResponse">Out coming command/query response.</typeparam>
/// <param name="validators">Collection of <see cref="IValidator{T}"/>.</param>
public class ValidationPipelineBehaviour<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators = validators ?? throw new ArgumentNullException(nameof(validators));

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (this.validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults =
                    await Task.WhenAll(
                        this.validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                throw new ModelValidationException(
                    typeof(TRequest).Name,
                    failures.Select(error => error.ErrorMessage));
            }
        }

        return await next();
    }
}
