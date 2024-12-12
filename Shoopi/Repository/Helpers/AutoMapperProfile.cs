using AutoMapper;
using DAO.Data;
using DAO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<RegisterVM, User>();

            CreateMap<CheckoutVM, OrderDetail>();

            // Ánh xạ từ Order sang OrderViewModel
            CreateMap<Order, OrderViewModel>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
                .ForMember(dest => dest.OrderStatus,  opt => opt.MapFrom(src => src.OrderStatus.Name));

            // Ánh xạ từ OrderDetail sang OrderDetailViewModel
            CreateMap<OrderDetail, OrderDetailViewModel>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Quantity * src.Product.Price));
        }
	}
}
