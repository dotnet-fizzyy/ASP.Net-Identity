using System.Collections.Generic;

namespace IdentityWebApi.Presentation.Models.DTO;

public class UserResultDto : BaseUserDto
{
    public IEnumerable<string> Roles { get; set; } = new List<string>();
}