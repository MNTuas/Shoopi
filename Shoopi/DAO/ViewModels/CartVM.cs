using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.ViewModels
{
	public class CartVM
	{
		public int ProductID { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Quantity {  get; set; }
		public string Picture { get; set; }	
	}
}
