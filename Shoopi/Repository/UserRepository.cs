using AutoMapper;
using DAO;
using DAO.Data;
using DAO.ViewModels;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class UserRepository : IUser
	{
		private readonly UserDAO _userDAO;

		public UserRepository(UserDAO userDAO)
		{
			_userDAO = userDAO;
		}

        public async Task<Users> Login(LoginVM model)
        {
            return await _userDAO.Login(model);
        }

        public async Task SignUp(RegisterVM model)
		{
			await _userDAO.SignUp(model); // Await the async call
		}
	}

}
