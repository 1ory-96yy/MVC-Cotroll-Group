using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_Cotroll_Group.Models;

namespace MVC_Cotroll_Group.Controllers
{
    public class CategoryController : Controller
    {
        private static List<Category> _categories = new()
        {
            new Category { Id = 1, Name = "��������" },
            new Category { Id = 2, Name = "���������" },
            new Category { Id = 3, Name = "��������" }
        };

        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View( _categories);
        }
    }
}
