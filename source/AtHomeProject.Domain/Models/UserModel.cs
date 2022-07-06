using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class UserModel
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }
    }
}
