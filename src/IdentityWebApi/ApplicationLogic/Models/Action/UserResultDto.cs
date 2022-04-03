using System.Collections.Generic;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

public class UserResultDto : BaseUserDto
{
    public IEnumerable<string> Roles { get; set; } = new List<string>();
}