using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;
using WhiteLagoon.Web.Extensions;
using WhiteLagoon.Application.Common.Interfaces;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villas = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
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
            bool isUnique = _unitOfWork.VillaNumber.Any(v => v.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            villaNumberVM = CreateViewModel(villaNumberVM.VillaNumber);
            
            if (isUnique)
            {
                this.AddErrorMessageToTempData("Villa Number already exists!");

                return View(villaNumberVM);
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Add(villaNumberVM.VillaNumber);
                _unitOfWork.Save();

                this.AddSuccessMessageToTempData("Villa Number has been created successfully!");
                return RedirectToAction(nameof(Index));
            }
            return View(villaNumberVM);
        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = CreateViewModel(_unitOfWork.VillaNumber.Get(v => v.Villa_Number == villaNumberId));

            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfWork.Save();

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
            var villaNumberFromDb = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (villaNumberFromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(villaNumberFromDb);
                _unitOfWork.Save();
                this.AddSuccessMessageToTempData("Villa number has been deleted successfully!");
                return RedirectToAction(nameof(Index));
            }

            this.AddErrorMessageToTempData("Error. Vill nanumber has not been deleted!");
            return View();
        }

        private VillaNumberVM CreateViewModel(VillaNumber? villaNumber = null)
            => new VillaNumberVM()
            {
                VillaList = _unitOfWork.Villa.GetAll().ToList().Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = villaNumber
            };
    }
}
