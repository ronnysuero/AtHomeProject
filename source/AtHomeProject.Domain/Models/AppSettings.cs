using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class AppSettings
    {
        public string Secret { get; set; }

        public UserModel DefaultCredentials { get; set; } = new();
    }
}
