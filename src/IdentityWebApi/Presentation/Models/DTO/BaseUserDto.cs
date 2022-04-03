using IdentityWebApi.Presentation.Validation;

using System;

namespace IdentityWebApi.Presentation.Models.DTO;

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
