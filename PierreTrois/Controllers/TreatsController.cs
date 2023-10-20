using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PierreTrois.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PierreTrois.Controllers
{
  [Authorize]
  public class TreatsController : Controller
  {
    private readonly PierreTroisContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public RecipesController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    
    {
      _userManager = userManager;
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    { 
      ViewBag.Title = "Treat Menu";
      List<Treat> model = _db.Treats.ToList();
      return View(model);
    }
    
    public ActionResult Create()
    {
      ViewBag.Title = "Add a new treat";
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Treat treat, int FlavorId)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.Title = "Add a new treat";
        ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
        return View(treat);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        recipe.User = currentUser;
        _db.Recipes.Add(treat);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      ViewBag.Title = "Treat Details";
      Treat targetTreat = _db.Treats.Include(entry => entry.JoinEntities)
                                       .ThenInclude(join => join.Flavor)
                                       .FirstOrDefault(entry => entry.TreatId == id);
      return View(targetTreat);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.Title = "Edit Treat";
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Treats.Update(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      ViewBag.Title = "Delete Recipe";
      Recipe targetRecipe = _db.Recipes.FirstOrDefault(entry => entry.RecipeId == id);
      return View(targetRecipe);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat targetTreat = _db.Treats.FirstOrDefault(entry => entry.TreatId == id);
      _db.Treats.Remove(targetTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddFlavor(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(model => model.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddFlavor(Treat treat, int flavorId)
    {
      #nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join =>(join.FlavorId == flavorId && join.TreatId == treat.TreatId));
      #nullable disable
      if (joinEntity == null && flavorId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() {TreatId = treat.TreatId, FlavorId = flavorId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new {id = treat.TreatId});
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      TreatFlavor joinEntry = _db.TreatFlavors.FirstOrDefault(entry => entry.TreatFlavorId == joinId);
      _db.TreatFlavors.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntry.TreatId });
    }
  }
}