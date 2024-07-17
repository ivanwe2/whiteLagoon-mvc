using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.Extensions;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
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
                if(villa.Image is not null)
                {
                    string fileName = SaveImageToRootAndReturnName(villa);

                    villa.ImageUrl = @"\images\VillaImage\" + fileName;
                }
                else
                {
                    villa.ImageUrl = "https://placehold.co/600x400";
                }
                _unitOfWork.Villa.Add(villa);
                _unitOfWork.Save();

                this.AddSuccessMessageToTempData("Villa has been created successfully!");
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        

        public IActionResult Update(int villaId)
        {
            var villa = _unitOfWork.Villa.Get(x => x.Id == villaId);
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
                if (villa.Image is not null)
                {
                    DeleteImageIfExists(villa);

                    var fileName = SaveImageToRootAndReturnName(villa);

                    villa.ImageUrl = @"\images\VillaImage\" + fileName;
                }
                else
                {
                    villa.ImageUrl = "https://placehold.co/600x400";
                }

                _unitOfWork.Villa.Update(villa);
                _unitOfWork.Save();

                this.AddSuccessMessageToTempData("Villa has been updated successfully!");
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Delete(int villaId)
        {
            var villa = _unitOfWork.Villa.Get(x => x.Id == villaId);
            if (villa == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }
        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            var villaFromDb = _unitOfWork.Villa.Get(x => x.Id == villa.Id);
            if (villaFromDb is not null)
            {
                DeleteImageIfExists(villaFromDb);

                _unitOfWork.Villa.Remove(villaFromDb);
                _unitOfWork.Save();

                this.AddSuccessMessageToTempData("Villa has been deleted successfully!");
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        private string SaveImageToRootAndReturnName(Villa villa)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");
            using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
            {
                villa.Image.CopyTo(fileStream);
            }

            return fileName;
        }


        private void DeleteImageIfExists(Villa? villaFromDb)
        {
            if (!string.IsNullOrEmpty(villaFromDb.ImageUrl))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villaFromDb.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
        }
    }
}
