using HouseMates.Business;
using HouseMates.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HouseMates.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHouseBusiness _houseBusiness;

        public HomeController(ILogger<HomeController> logger, IHouseBusiness houseBusiness)
        {
            _logger = logger;
            _houseBusiness = houseBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Search(string searchedString)
        {
            var houses =  _houseBusiness.SearchHouses(searchedString);
            _logger.LogInformation($"Search string received: {searchedString}");

            foreach (var house in houses)
            {
                Console.WriteLine($"House found: Location: {house.location}, Bedrooms: {house.numberOfBedrooms}, Bathrooms: {house.numberOfBathrooms}, Description: {house.description}");
            }
            return View("SearchResults", houses);
        }
    }
}
