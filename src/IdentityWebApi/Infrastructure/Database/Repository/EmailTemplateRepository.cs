using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Interfaces.Infrastructure;

using System;

namespace IdentityWebApi.Infrastructure.Database.Repository;

/// <inheritdoc cref="IEmailTemplateRepository" />
[Obsolete("Remove after CQRS pattern full implementation")]
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
