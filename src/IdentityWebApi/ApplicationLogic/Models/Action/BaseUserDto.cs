using IdentityWebApi.ApplicationLogic.Validation;

using System;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

public abstract class BaseUserDto
{
    [DefaultValue] 
    public Guid Id { get; set; }

    [DefaultValue] 
    public string UserName { get; set; }

    [DefaultValue]
    public string ConcurrencyStamp { get; set; }

    [DefaultValue]
    public string Email { get; set; }

    [DefaultValue]
    public string PhoneNumber { get; set; }
}
