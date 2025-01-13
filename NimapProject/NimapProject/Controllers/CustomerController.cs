using Microsoft.AspNetCore.Mvc;
using NimapProject.Models;
using System.Linq;

namespace NimapProject.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            return View(this.GetCustomers(1));
        }

        [HttpPost]
        public ActionResult Index(int currentPageIndex)
        {
            return View(this.GetCustomers(currentPageIndex));
        }

        private CustomerModel GetCustomers(int currentPage)
        {
            int maxRows = 10;

            // Replace 'Data' with the actual name of your DbContext class
            using (var entities = new YourDbContext())
            {
                CustomerModel customerModel = new CustomerModel();

                customerModel.Datas = (from product in entities.Products 
                                       join category in entities.Categories 
                                       on product.ProductID equals category.ProductID
                                       select new Data
                                       {
                                           ProductId = product.ProductID,
                                           ProductName = product.ProductName, // Ensure the property name matches your model
                                           CategoryId = category.CategoryID,
                                           CategoryName = category.CategoryName // Ensure the property name matches your model
                                       })
                            .OrderBy(x => x.ProductId)
                            .Skip((currentPage - 1) * maxRows)
                            .Take(maxRows)
                            .ToList();

                double pageCount = (double)entities.Products.Count() / maxRows; // Replace 'Products' with your DbSet name
                customerModel.PageCount = (int)Math.Ceiling(pageCount);
                customerModel.CurrentPageIndex = currentPage;

                return customerModel;
            }
        }
    }
}
