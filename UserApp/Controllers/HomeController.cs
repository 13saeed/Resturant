using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resturant.Data;
using Resturant.Models;


namespace Resturant.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FoodsServices _foodsServices;
        private readonly AppDbContext _contex;

        public HomeController(ILogger<HomeController> logger, FoodsServices foodsServices , AppDbContext context)
        {
            _logger = logger;
            _foodsServices = foodsServices;
            _contex = context;
        }

        public async Task<IActionResult> Index()
        {
            var foods = await _foodsServices.GetAllFoodsAsync();
            return View(foods);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> DashboardAdmin()
        {
            var foods = await _foodsServices.GetAllFoodsAsync();
            return View(foods);
        }
        public async Task<IActionResult> MenuFoods()
        {
            var foods = await _foodsServices.GetAllFoodsAsync();
            return View(foods);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
