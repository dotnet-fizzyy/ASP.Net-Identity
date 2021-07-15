using System.Collections.Generic;

namespace IdentityWebApi.PL.Models.DTO
{
    public class UserResultDto : BaseUserDto
    {
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}