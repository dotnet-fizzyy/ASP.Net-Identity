using IdentityWebApi.Presentation.Validation;

using System;

namespace IdentityWebApi.Presentation.Models.Action;

public class UserRoleActionModel
{
    [DefaultValue] 
    public Guid RoleId { get; set; }

    [DefaultValue] 
    public Guid UserId { get; set; }
}
