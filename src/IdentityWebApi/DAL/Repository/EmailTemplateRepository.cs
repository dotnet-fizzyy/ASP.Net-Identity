using IdentityWebApi.DAL.Entities;
using IdentityWebApi.DAL.Interfaces;

namespace IdentityWebApi.DAL.Repository;

public class EmailTemplateRepository : BaseRepository<EmailTemplate>, IEmailTemplateRepository
{
    public EmailTemplateRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
        
    }
}
