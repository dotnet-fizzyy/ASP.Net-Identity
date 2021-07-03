using System;
using IdentityWebApi.PL.Validation;

namespace IdentityWebApi.PL.Models.Action
{
    public class UserRoleActionModel
    {
        [DefaultValue]
        public Guid RoleId { get; set; }
        
        [DefaultValue]
        public Guid UserId { get; set; }
    }
}