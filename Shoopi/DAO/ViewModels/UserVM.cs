using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.ViewModels
{
    public class UserVM
    {
        public int UserId { get; set; }

        public string? FullName { get; set; }

        public string? Gender { get; set; }

        public DateOnly? Birthday { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Picture { get; set; }

        public bool Status { get; set; }

        public int? RoleId { get; set; }

      
    }
}
