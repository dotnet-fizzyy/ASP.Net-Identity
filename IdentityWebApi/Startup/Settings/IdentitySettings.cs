using System.Collections.Generic;

namespace IdentityWebApi.Startup.Settings
{
    public class IdentitySettings
    {
        public ICollection<string> Roles { get; set; } = new List<string>();
    }
}