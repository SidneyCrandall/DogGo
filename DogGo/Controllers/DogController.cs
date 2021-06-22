using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Repositories;

namespace DogGo.Controllers
{
    public class DogController : Controller
    {
        private readonly IDogRepository _dogRepo;
        public DogController(IDogRepository dogRepository)
        {
            _dogRepo = dogRepository;
        }
        // GET: Request to see all the dogs on the browser 
        public ActionResult Index()
        {
            List<Dog> dogs = _dogRepo.GetAllDogs();
            return View();
        }

        // GET: A single Dog view for the browser
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            return View(dog);
        }

        // Adding a new dog
        public ActionResult Create()
        {
            return View();
        }

        // We need the respomnse of adding the new dog
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                _dogRepo.AddDog(dog);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            return View(dog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepo.DeleteDog(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }
            return View(dog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                _dogRepo.UpdateDog(dog);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

    }
}
