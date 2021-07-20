using System;

namespace IdentityWebApi.PL.Models.DTO
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string ConcurrencyStamp { get; set; }
        
        public DateTime CreationDate { get; set; }
    }
}