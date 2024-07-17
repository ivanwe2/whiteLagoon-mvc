using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;
using WhiteLagoon.Web.Extensions;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillaNumberController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var villas = _context.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villas);
        }
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = CreateViewModel();

            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Create(VillaNumberVM villaNumberVM)
        {
            bool isUnique = _context.VillaNumbers.Any(v => v.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            villaNumberVM = CreateViewModel(villaNumberVM.VillaNumber);
            
            if (isUnique)
            {
                this.AddErrorMessageToTempData("Villa Number already exists!");

                return View(villaNumberVM);
            }

            if (ModelState.IsValid)
            {
                _context.VillaNumbers.Add(villaNumberVM.VillaNumber);
                _context.SaveChanges();

                this.AddSuccessMessageToTempData("Villa Number has been created successfully!");
                return RedirectToAction(nameof(Index));
            }
            return View(villaNumberVM);
        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = CreateViewModel(_context.VillaNumbers.FirstOrDefault(v => v.Villa_Number == villaNumberId));

            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                _context.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _context.SaveChanges();

                this.AddSuccessMessageToTempData("Villa Number has been updated successfully!");
                return RedirectToAction(nameof(Index));
            }

            villaNumberVM = CreateViewModel(villaNumberVM.VillaNumber);

            return View(villaNumberVM);
        }
        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = CreateViewModel();

            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            var villaNumberFromDb = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (villaNumberFromDb is not null)
            {
                _context.VillaNumbers.Remove(villaNumberFromDb);
                _context.SaveChanges();
                this.AddSuccessMessageToTempData("Villa number has been deleted successfully!");
                return RedirectToAction(nameof(Index));
            }

            this.AddErrorMessageToTempData("Error. Vill nanumber has not been deleted!");
            return View();
        }

        private VillaNumberVM CreateViewModel(VillaNumber? villaNumber = null)
            => new VillaNumberVM()
            {
                VillaList = _context.Villas.ToList().Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = villaNumber
            };
    }
}
