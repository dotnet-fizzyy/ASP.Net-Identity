using System;
using IdentityWebApi.PL.Validation;

namespace IdentityWebApi.PL.Models.DTO
{
    public class UserDto
    {
        [DefaultValue]
        public Guid Id { get; set; }
        
        [DefaultValue]
        public string UserName { get; set; }
        
        [DefaultValue]
        public string Password { get; set; }
        
        [DefaultValue]
        public string UserRole { get; set; }
        
        [DefaultValue]
        public string ConcurrencyStamp { get; set; }
        
        [DefaultValue]
        public string Email { get; set; }
        
        [DefaultValue]
        public string PhoneNumber { get; set; }
    }
}