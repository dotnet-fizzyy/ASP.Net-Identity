using DY.Auth.Identity.Api.Infrastructure.Database;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using System;

namespace DY.Auth.Identity.Api.UnitTests.Infrastructure;

public class SqliteConfiguration : IDisposable
{
    private const string InMemoryConnectionString = "DataSource=:memory:";

    protected readonly DatabaseContext DatabaseContext;

    private readonly SqliteConnection connection;

    protected SqliteConfiguration()
    {
        this.connection = new SqliteConnection(InMemoryConnectionString);
        this.connection.Open();

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite(this.connection)
            .Options;

        this.DatabaseContext = new DatabaseContext(options);

        this.DatabaseContext.Database.EnsureDeleted();
        this.DatabaseContext.Database.EnsureCreated();
    }

    ~SqliteConfiguration()
    {
        this.connection?.Close();
    }

    public void Dispose()
    {
        this.connection?.Close();
        GC.SuppressFinalize(this);
    }
}
