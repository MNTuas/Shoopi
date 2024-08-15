using DAO.Data;
using DAO.ViewModels;
using Repository.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
	public interface IUserRepository
	{
		Task<Result<User>> Login(LoginVM model);
		Task<Result<User>> SignUp(RegisterVM model);


	}
}
