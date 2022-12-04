using ActionCommandGame.Ui.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace ActionCommandGame.Ui.WebApp.Controllers
{
    public class HomeController : Controller
    {


        public HomeController()
        {

           
        }
        
        public IActionResult Index()
        {
            return View();
        }
        



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}