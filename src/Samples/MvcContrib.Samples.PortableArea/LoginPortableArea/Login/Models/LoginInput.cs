using System.ComponentModel.DataAnnotations;

namespace LoginPortableArea.Login.Models
{
	public class LoginInput
	{
        [Required]
		public string Username { get; set; }
        [Required]
		public string Password { get; set; }
	}
}