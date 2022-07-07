using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Data.Entities
{
    [ExcludeFromCodeCoverage]
    public class UsersClaims
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}
