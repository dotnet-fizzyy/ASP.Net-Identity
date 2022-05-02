using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Interfaces.Infrastructure;

namespace IdentityWebApi.Infrastructure.Database.Repository;

public class EmailTemplateRepository : BaseRepository<EmailTemplate>, IEmailTemplateRepository
{
    public EmailTemplateRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
        
    }
}
