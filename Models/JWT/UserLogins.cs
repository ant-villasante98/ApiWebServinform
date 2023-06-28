using System.ComponentModel.DataAnnotations;

namespace Servirform.Models.JWT;
public class UserLogins
{
    [Required]
    public string UserEmail { get; set; }
    [Required]
    public string Password { get; set; }
}
