using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.Extensions;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var Amenitys = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(Amenitys);
        }
        public IActionResult Create()
        {
            AmenityVM AmenityVM = CreateViewModel();

            return View(AmenityVM);
        }
        [HttpPost]
        public IActionResult Create(AmenityVM amenityVM)
        {

            amenityVM = CreateViewModel(amenityVM.Amenity);

            //if (isUnique)
            //{
            //    this.AddErrorMessageToTempData("Amenity Number already exists!");

            //    return View(amenityVM);
            //}

            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(amenityVM.Amenity);
                _unitOfWork.Save();

                this.AddSuccessMessageToTempData("Amenity has been created successfully!");
                return RedirectToAction(nameof(Index));
            }
            return View(amenityVM);
        }

        public IActionResult Update(int AmenityId)
        {
            AmenityVM AmenityVM = CreateViewModel(_unitOfWork.Amenity.Get(v => v.Id == AmenityId));

            return View(AmenityVM);
        }
        [HttpPost]
        public IActionResult Update(AmenityVM AmenityVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(AmenityVM.Amenity);
                _unitOfWork.Save();

                this.AddSuccessMessageToTempData("Amenity has been updated successfully!");
                return RedirectToAction(nameof(Index));
            }

            AmenityVM = CreateViewModel(AmenityVM.Amenity);

            return View(AmenityVM);
        }
        public IActionResult Delete(int AmenityId)
        {
            AmenityVM AmenityVM = CreateViewModel();

            return View(AmenityVM);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            var AmenityFromDb = _unitOfWork.Amenity.Get(x => x.Id == amenityVM.Amenity.Id);
            if (AmenityFromDb is not null)
            {
                _unitOfWork.Amenity.Remove(AmenityFromDb);
                _unitOfWork.Save();

                this.AddSuccessMessageToTempData("Amenity has been deleted successfully!");
                return RedirectToAction(nameof(Index));
            }

            this.AddErrorMessageToTempData("Error. Amenity has not been deleted!");
            return View();
        }

        private AmenityVM CreateViewModel(Amenity? Amenity = null)
            => new AmenityVM()
            {
                VillaList = _unitOfWork.Amenity.GetAll().ToList().Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = Amenity
            };
    }
}
