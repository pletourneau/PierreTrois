using Microsoft.AspNetCore.Mvc;
using PierreTrois.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace PierreTrois.Controllers
{
  [Authorize]
  public class FlavorsController : Controller
  {
    private readonly PierreTroisContext _db;

    public FlavorsController(PierreTroisContext db)
    {
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {

      return View(_db.Flavors.ToList());
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Flavor flavor)
    {
      if (!ModelState.IsValid)
      {
        return View(flavor);
      }
      else
      {
        _db.Flavors.Add(flavor);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      Flavor targetFlavor = _db.Flavors.Include(entry => entry.JoinEntities)
                                              .ThenInclude(join => join.Treat)
                                              .FirstOrDefault(entry => entry.FlavorId == id);
      return View(targetFlavor);
    }

    public ActionResult Edit(int id)
    {
      Flavor targetFlavor = _db.Flavors.FirstOrDefault(entry => entry.FlavorId == id);
      return View(targetFlavor);
    }

    [HttpPost]
    public ActionResult Edit(Flavor flavorToEdit)
    {
      if (!ModelState.IsValid)
        {
          return View(flavorToEdit);
        }
      
      else
      {
        _db.Flavors.Update(flavorToEdit);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
    public ActionResult Delete(int id)
    {
      Flavor targetFlavor = _db.Flavors.FirstOrDefault(entry => entry.FlavorId == id);
      return View(targetFlavor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Flavor targetFlavor = _db.Flavors.FirstOrDefault(entry => entry.FlavorId == id);
      _db.Flavors.Remove(targetFlavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddTreat(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(model => model.FlavorId == id);
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      return View(thisFlavor);
    }

    [HttpPost]
    public ActionResult AddTreat(Flavor flavor, int treatId)
    {
#nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join => (join.TreatId == treatId && join.FlavorId == flavor.FlavorId));
#nullable disable
      if (joinEntity == null && treatId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { TreatId = treatId, FlavorId = flavor.FlavorId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = flavor.FlavorId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      TreatFlavor joinEntry = _db.TreatFlavors.FirstOrDefault(entry => entry.TreatFlavorId == joinId);
      _db.TreatFlavors.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntry.FlavorId });
    }
  }
}