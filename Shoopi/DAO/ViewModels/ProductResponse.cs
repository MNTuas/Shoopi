using DAO.Data;


namespace DAO.ViewModels
{
	public class ProductResponse
	{
			public List<Product> Products { get; set; }
			public int TotalPages { get; set; }
			public int PageIndex { get; set; }

	}
}
