using DAO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.ViewModels
{
    public class OrderResponse
    {
        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
    }

    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string FullName { get; set; }
        public string MethodPayment { get; set; }
        public string PhoneNumber { get; set; }

        public string OrderStatus { get; set; }

        public List<OrderDetailViewModel> OrderDetails { get; set; } = new List<OrderDetailViewModel>();
    }

    public class OrderDetailViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
