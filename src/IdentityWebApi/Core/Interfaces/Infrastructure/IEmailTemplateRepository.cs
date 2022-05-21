using IdentityWebApi.Core.Entities;

using System;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

/// <summary>
/// Repository with functionality for EmailTemplate entity.
/// </summary>
[Obsolete("Remove after CQRS pattern full implementation")]
public interface IEmailTemplateRepository : IBaseRepository<EmailTemplate>
{

}
