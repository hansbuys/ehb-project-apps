using System.Collections.Generic;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Authentication
{
    public class Duty
    {
        public static Duty User = new Duty("User", "Gebruiker");
        public static Duty Admin = new Duty("Admin", "Beheerder");

        public static IEnumerable<Duty> AllDuties => new[]
        {
            User,
            Admin
        };

        public string Name { get; }
        public string DisplayName { get; }

        private Duty(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }
    }
}
