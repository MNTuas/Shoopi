
using AutoMapper;
using DAO.AutoMapper;
using DAO.Data;
using DAO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
	public class UserDAO
	{
		private readonly ShoopiContext _context;
		private readonly IMapper _mapper;

		public UserDAO(IMapper mapper)
		{
			_context = new ShoopiContext();
			_mapper = mapper;
		}

		public async Task SignUp(RegisterVM model)
		{
			try
			{
				var user = _mapper.Map<User>(model);
				user.RandomKey = MyUtil.GenerateRamdomKey();
				user.Password = model.Password.ToMd5Hash(user.RandomKey);
				user.RoleId = 1;
				user.Status = true;
				
				_context.Add(user);
				_context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw ex;
			}


		}
	}
}