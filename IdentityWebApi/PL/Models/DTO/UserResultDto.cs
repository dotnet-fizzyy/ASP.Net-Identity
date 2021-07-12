using System.Collections.Generic;

namespace IdentityWebApi.PL.Models.DTO
{
    public class UserResultDto : UserDto
    {
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}