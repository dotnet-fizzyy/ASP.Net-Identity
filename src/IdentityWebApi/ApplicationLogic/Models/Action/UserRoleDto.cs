using IdentityWebApi.ApplicationLogic.Validation;

using System;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

public class UserRoleDto
{
    [DefaultValue]
    public Guid RoleId { get; set; }

    [DefaultValue]
    public Guid UserId { get; set; }
}
