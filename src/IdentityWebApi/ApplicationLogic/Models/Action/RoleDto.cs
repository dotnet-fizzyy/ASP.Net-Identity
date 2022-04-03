using System;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

public class RoleDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string ConcurrencyStamp { get; set; }

    public DateTime CreationDate { get; set; }
}