using System;

namespace IdentityWebApi.Core.Entities;

public class EmailTemplate : IBaseUser
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Layout { get; set; }

    public DateTime CreationDate { get; set; }

    public bool IsDeleted { get; set; }
}
