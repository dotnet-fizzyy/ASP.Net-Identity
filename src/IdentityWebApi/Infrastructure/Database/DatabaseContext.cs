using IdentityWebApi.Core.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityWebApi.Infrastructure.Database;

/// <summary>
/// Database EF context.
/// </summary>
public sealed class DatabaseContext : IdentityDbContext<
    AppUser,
    AppRole,
    Guid,
    IdentityUserClaim<Guid>,
    AppUserRole,
    IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>,
    IdentityUserToken<Guid>>
{
    /// <summary>
    /// Gets or sets <see cref="EmailTemplate"/> table.
    /// </summary>
    public DbSet<EmailTemplate> EmailTemplates { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseContext"/> class.
    /// </summary>
    /// <param name="options"><see cref="DbContextOptions{T}"/>.</param>
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
        this.Database.Migrate();
    }

    /// <summary>
    /// Searches for entity by id.
    /// </summary>
    /// <typeparam name="T">Inheritor of <see cref="IBaseEntity"/>.</typeparam>
    /// <param name="id">Entity id.</param>
    /// <returns>Entity by search criteria.</returns>
    public async Task<T> SearchById<T>(Guid id)
        where T : class, IBaseEntity =>
            await this.Set<T>().SingleOrDefaultAsync(entity => entity.Id == id && !entity.IsDeleted);

    /// <summary>
    /// Searches for entity by id with including properties.
    /// </summary>
    /// <typeparam name="T">Inheritor of <see cref="IBaseEntity"/>.</typeparam>
    /// <param name="id">Entity id.</param>
    /// <param name="includeTracking">Flag indicating whether entities tracking should be enabled.</param>
    /// <param name="includes">Collection of related entities to include.</param>
    /// <returns>Entity by search criteria.</returns>
    public async Task<T> SearchById<T>(Guid id, bool includeTracking, params Expression<Func<T, object>>[] includes)
        where T : class, IBaseEntity
    {
        var query = includeTracking
            ? this.Set<T>().Where(entity => entity.Id == id && !entity.IsDeleted)
            : this.Set<T>().Where(entity => entity.Id == id && !entity.IsDeleted).AsNoTracking();

        if (includes.Any())
        {
            query = includes
                .Aggregate(
                    query,
                    (current, includeProperty) => current.Include(includeProperty));
        }

        return await query.SingleOrDefaultAsync();
    }

    /// <summary>
    /// Performs entity soft remove.
    /// </summary>
    /// <typeparam name="T">Inheritor of <see cref="IBaseEntity"/>.</typeparam>
    /// <param name="entity">Entity.</param>
    public void SoftRemove<T>(T entity)
        where T : class, IBaseEntity
    {
        entity.IsDeleted = true;
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}
