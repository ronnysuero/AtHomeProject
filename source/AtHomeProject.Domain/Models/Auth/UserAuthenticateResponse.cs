using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Auth
{
    [ExcludeFromCodeCoverage]
    public class UserAuthenticateResponse
    {
        public string UserName { get; set; }

        public string Token { get; set; }

        public UserAuthenticateResponse(string userName, string token)
        {
            UserName = userName;
            Token = token;
        }
    }
}
