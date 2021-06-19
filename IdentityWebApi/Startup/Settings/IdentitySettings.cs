using System.Collections.Generic;

namespace IdentityWebApi.Startup.Settings
{
    public class IdentitySettings
    {
        public ICollection<string> Roles { get; set; } = new List<string>();
        
        public IdentitySettingsPassword Password { get; set; }
        
        public DefaultAdminSettings DefaultAdmin { get; set; }
        
        public EmailSettings Email { get; set; }
    }
}