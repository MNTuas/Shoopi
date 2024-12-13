using DAO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.ViewModels.Response
{
    public class UserResponse
    {
        public List<User> Users { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
    }
}
