using DAO.Data;


namespace DAO.ViewModels.Response
{
    public class ProductResponse
    {
        public List<Product> Products { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }

    }
}
