using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Auth
{
    [ExcludeFromCodeCoverage]
    public record AuthResponse(string Token)
    {
        public string Token { get; set; } = Token;
    }
}
