using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Auth
{
    [ExcludeFromCodeCoverage]
    public record UserAuthResponse(string UserName, string Token)
    {
        public string UserName { get; set; } = UserName;
        public string Token { get; set; } = Token;
    }
}
