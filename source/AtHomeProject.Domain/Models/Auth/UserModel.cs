using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Auth
{
    [ExcludeFromCodeCoverage]
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
