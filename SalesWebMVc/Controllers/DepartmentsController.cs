using Microsoft.AspNetCore.Mvc;
using SalesWebMVc.Models;

namespace SalesWebMVc.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            List<Department> list = new List<Department>();
            list.Add(new Department { Id = 1, Name = "Eletronics" });
            list.Add(new Department { Id = 2, Name = "Fashion" });
            //Pass a list of departments to the view (Views//Departments//Index.cshtml)
            return View(list);
        }
    }
}
