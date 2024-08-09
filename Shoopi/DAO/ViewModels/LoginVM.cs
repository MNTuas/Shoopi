using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.ViewModels
{
    public class LoginVM
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; }
        
        [Required]
        [MaxLength(20, ErrorMessage = "Password must be < 20 keys")]
        public string? Password { get; set; }
    }
}
