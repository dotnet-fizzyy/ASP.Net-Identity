using DY.Auth.Identity.Api.Infrastructure.Database;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using System;

namespace DY.Auth.Identity.Api.UnitTests.Infrastructure;

public class SqliteConfiguration : IDisposable
{
    protected readonly DatabaseContext DatabaseContext;

    private const string InMemoryConnectionString = "DataSource=:memory:";

    private readonly SqliteConnection connection;

    private bool isDisposed;

    protected SqliteConfiguration()
    {
        this.connection = new SqliteConnection(InMemoryConnectionString);
        this.connection.Open();

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite(this.connection)
            .ConfigureWarnings(builder => builder.Ignore(RelationalEventId.PendingModelChangesWarning))
            .Options;

        this.DatabaseContext = new DatabaseContext(options);

        this.DatabaseContext.Database.EnsureDeleted();
        this.DatabaseContext.Database.EnsureCreated();
    }

    ~SqliteConfiguration()
    {
        this.Dispose(disposing: false);
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (disposing)
        {
            this.connection?.Dispose();
        }

        this.isDisposed = true;
    }
}
