using DAO.Data;
using DAO.ViewModels;
using DAO.ViewModels.Request;
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
		Task<List<User>> GetAllUser();
		Task<User?> getUserByEmailAsync(string email);
		Task<User?> getUserByIdlAsync(int id);
		Task<User?> getUserByLogin(int userId);
		Task<User?> UpdateUser(int userId, UserUpdateRequest userUpdateRequest);


    }
}
