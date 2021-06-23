using System;

namespace IdentityWebApi.PL.Models.Action
{
    public class UserRoleActionModel
    {
        public Guid RoleId { get; set; }
        
        public Guid UserId { get; set; }
    }
}