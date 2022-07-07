using System.ComponentModel.DataAnnotations;

namespace AtHomeProject.Data.Entities
{
    public class UsersClaims
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}
