using System.Collections.Generic;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication
{
    public class Identity
    {
        public Identity(string name, IEnumerable<string> roles, bool needsPasswordChange = false)
        {
            Name = name;
            Roles = roles;
            NeedsPasswordChange = needsPasswordChange;
        }

        public string Name { get; }
        public IEnumerable<string> Roles { get; }
        public bool NeedsPasswordChange { get; }
    }
}