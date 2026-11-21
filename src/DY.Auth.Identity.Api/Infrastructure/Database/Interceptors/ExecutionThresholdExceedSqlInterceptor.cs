using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Infrastructure.Database.Interceptors;

/// <summary>
/// EF SQL interceptor identifying long time execution queries exceeding threshold.
/// </summary>
/// <param name="logger">The instance of <see cref="ILogger"/>.</param>
/// <param name="sqlQueryExecutionThresholdInMilliseconds">Long time execution SQL query threshold.</param>
/// <exception cref="ArgumentNullException">Will be thrown if no constructor arguments are provided.</exception>
public class ExecutionThresholdExceedSqlInterceptor(ILogger<ExecutionThresholdExceedSqlInterceptor> logger, int sqlQueryExecutionThresholdInMilliseconds) : DbCommandInterceptor
{
    private readonly ILogger<ExecutionThresholdExceedSqlInterceptor> logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public override DbDataReader ReaderExecuted(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result)
    {
        this.TrackThresholdLimitExceededQueries(command, eventData);

        return base.ReaderExecuted(command, eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = default)
    {
        this.TrackThresholdLimitExceededQueries(command, eventData);

        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    private void TrackThresholdLimitExceededQueries(DbCommand command, CommandExecutedEventData eventData)
    {
        if (eventData.Duration.Milliseconds < sqlQueryExecutionThresholdInMilliseconds)
        {
            return;
        }

        this.logger.LogWarning(
            "Identified slow SQL query ({0}ms). Command source: {2}. Is async operation: {3}. SQL query: {1}",
            eventData.Duration.Milliseconds,
            eventData.CommandSource,
            eventData.IsAsync,
            command.CommandText);
    }
}
