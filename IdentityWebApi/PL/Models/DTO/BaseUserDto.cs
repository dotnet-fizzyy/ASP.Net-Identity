using IdentityWebApi.PL.Validation;

using System;

namespace IdentityWebApi.PL.Models.DTO;

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
