using AutoMapper;
using DAO.Data;
using DAO.ViewModels;

namespace Shoopi.Helper
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile() 
		{
			CreateMap<RegisterVM, Users>();
		}
	}
}
