using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAO.ViewModels
{
	public class RegisterVM
	{
		[Required]
		[MaxLength(20,ErrorMessage = "FullName must be < 20 keys")]
		public string? FullName { get; set; }
		
		[Required]
		[MaxLength(20, ErrorMessage = "Password must be < 20 keys")]
		public string? Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
		public string? Gender { get; set; }
		[Required]
		public DateOnly? Birthday { get; set; }
		[EmailAddress]
		[Required]
		public string? Email { get; set; }

		public bool Status { get; set; }

		public int? RoleId { get; set; }

		public string? RandomKey { get; set; }
	}
}
