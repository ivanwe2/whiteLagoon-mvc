using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var villas = _context.Villas.ToList();
            return View(villas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa villa)
        {
            if (ModelState.IsValid)
            {
                _context.Villas.Add(villa);
                _context.SaveChanges();

                TempData["success"] = "Villa has been created successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Update(int villaId)
        {
            var villa = _context.Villas.FirstOrDefault(x => x.Id == villaId);
            if (villa == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }
        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (ModelState.IsValid)
            {
                _context.Villas.Update(villa);
                _context.SaveChanges();

                TempData["success"] = "Villa has been updated successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int villaId)
        {
            var villa = _context.Villas.FirstOrDefault(x => x.Id == villaId);
            if (villa == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }
        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            var villaFromDb = _context.Villas.FirstOrDefault(x => x.Id == villa.Id);
            if (villaFromDb is not null)
            {
                _context.Villas.Remove(villaFromDb);
                _context.SaveChanges();
                TempData["success"] = "Villa has been deleted successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
