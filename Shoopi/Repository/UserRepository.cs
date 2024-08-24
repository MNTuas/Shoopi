using AutoMapper;
using DAO;
using DAO.Data;
using DAO.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repository.Helpers;
using Repository.Helpers.Response;
using Repository.IRepository;

namespace Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly UserDAO _userDAO;
		private readonly IMapper _mapper;
        private readonly Shoopi1Context _context;

        public UserRepository(UserDAO userDAO, IMapper mapper, Shoopi1Context context)
		{
			_userDAO = userDAO;
			_mapper = mapper;
			_context = context;
		}

        public Task<List<User>> GetAllUser()
        {
            return _userDAO.GetAllUser();
        }

        public Task<User?> getUserByEmailAsync(string email)
        {
            return _userDAO.getUserByEmailAsync(email);
        }

        public Task<User?> getUserByIdlAsync(int id)
        {
            return _userDAO.getUserByIdlAsync(id);
        }

        public async Task<Result<User>> Login(LoginVM model)
		{
			var result = new Result<User>();
			try
			{
				var user = await _userDAO.getUserByEmailAsync(model.Email);
				if (user == null)
				{
					result.Success = false;
					result.ErrorMessage = "User not found";
					return result;
				}

				var hashedPassword = model.Password.ToMd5Hash(user.RandomKey); 
				if (user.Password != hashedPassword)
				{
					result.Success = false;
					result.ErrorMessage = "Email or Password incorrect!!!";
					return result;
				}
				model.FullName = user.FullName;
				model.UserId = user.UserId;
				model.RoleId = user.RoleId;	
				
				result.Success = true;
				result.Data = user;
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.ErrorMessage = "Something wrong!!";
				return result;
			}
			
			return result;
		}

		public async Task<Result<User>> SignUp(RegisterVM model)
		{
			var result = new Result<User>();
			try
			{
				var existingUser = await _userDAO.getUserByEmailAsync(model.Email);
				if (existingUser != null)
				{
					result.Success = false;
					result.ErrorMessage = "Email already exists.";
					return result;
				}

				var user = _mapper.Map<User>(model);
				if (model.Password != null)
				{                    
                    user.Password = model.Password.ToMd5Hash(user.RandomKey);
                }
                user.RandomKey = MyUtil.GenerateRamdomKey();
                user.RoleId = 2;
				user.Status = true;
				user.IsGoogleAccount = false;
				user.IsFacebookAccount = false;
				await _userDAO.AddUserAsync(user);
				result.Success = true;
				result.Data= user;
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.ErrorMessage = "Something is wrong!!";
				return result;
			}

			return result;
		}
	}

}
