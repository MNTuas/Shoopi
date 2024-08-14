using DAO.Data;
using DAO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
	public interface IUser
	{
		Task SignUp(RegisterVM model);
		Task<User> Login(LoginVM model);

    }
}
