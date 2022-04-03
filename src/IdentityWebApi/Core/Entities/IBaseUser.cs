using System;

namespace IdentityWebApi.Core.Entities;

public interface IBaseUser
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public bool IsDeleted { get; set; }
}
