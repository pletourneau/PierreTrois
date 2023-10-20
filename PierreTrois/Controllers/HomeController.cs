﻿using Microsoft.AspNetCore.Mvc;
using PierreTrois.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PierreTrois.Controllers
{

  public class HomeController : Controller
  {
    private readonly PierreTroisContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(UserManager<ApplicationUser> userManager, PierreTroisContext db)
    {
    _userManager = userManager;
    _db = db;
    }

    public ActionResult Index()
    {
      Treat[] treats = _db.Treats.ToArray();
      Flavor[] flavors = _db.Flavors.ToArray();
      Dictionary<string,object[]> model = new Dictionary<string, object[]>();
      model.Add("treats", treats);
      model.Add("flavors", flavors);
      return View(model);
    }
  }

}

//  private readonly ILogger<HomeController> _logger;

//     public HomeController(ILogger<HomeController> logger)
//     {
//         _logger = logger;
//     }

//     public IActionResult Index()
//     {
//         return View();
//     }

//     public IActionResult Privacy()
//     {
//         return View();
//     }

//     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//     public IActionResult Error()
//     {
//         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//     }