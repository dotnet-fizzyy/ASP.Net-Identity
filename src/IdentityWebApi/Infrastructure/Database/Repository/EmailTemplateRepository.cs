using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Interfaces.Infrastructure;

namespace IdentityWebApi.Infrastructure.Database.Repository;

/// <inheritdoc cref="IEmailTemplateRepository" />
public class EmailTemplateRepository : BaseRepository<EmailTemplate>, IEmailTemplateRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailTemplateRepository"/> class.
    /// </summary>
    /// <param name="databaseContext">Database EF context.</param>
    public EmailTemplateRepository(DatabaseContext databaseContext)
        : base(databaseContext)
    {

    }
}
