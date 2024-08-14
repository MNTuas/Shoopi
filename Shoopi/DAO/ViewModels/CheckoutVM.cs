using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.ViewModels
{
	public class CheckoutVM
	{
		public bool IsSameInfo { get; set; }
		public string? FullName { get; set; }

		public string? Address { get; set; }

		public string? PhoneNumber { get; set; }

		public DateOnly? DeliveryDate { get; set; } 

		public string? MethodPayment { get; set; }

		public string? Delivery { get; set; }

		public string? Note { get; set; }
		public int? OrderStatusId { get; set; }
	}
}
