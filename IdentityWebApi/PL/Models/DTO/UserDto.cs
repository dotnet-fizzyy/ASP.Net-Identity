using System;

namespace IdentityWebApi.PL.Models.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        
        public string UserName { get; set; }
        
        public string Password { get; set; }
        
        public string UserRole { get; set; }
        
        public string ConcurrencyStamp { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
    }
}