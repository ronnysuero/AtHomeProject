using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Data.Entities
{
    [ExcludeFromCodeCoverage]
    public class Users
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public ICollection<UsersClaims> Claims { get; set; } = new HashSet<UsersClaims>();
    }
}
