using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityWebApi.DAL.Utilities
{
    public static class DatabaseUtilities
    {
        public static bool RoleExists(IEnumerable<string> roles, string role) =>
            roles.Any(x => string.Equals(x, role, StringComparison.CurrentCultureIgnoreCase));
    }
}