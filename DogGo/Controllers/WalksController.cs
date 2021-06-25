using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Controllers
{
    public class WalksController : Controller
    {
        private readonly IWalksRepository _walksRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;

        public WalksController(IWalksRepository walksRepository, IDogRepository dogRepository, IWalkerRepository walkerRepository)
        {
            _walksRepo = walksRepository;
            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
        }

        public ActionResult Index()
        {
            List<Walks> walks = _walksRepo.GetAll();
            return View(walks);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            List<Dog> dogs = _dogRepo.GetAllDogs();

            WalksFormViewModel vm = new WalksFormViewModel()
            {
                Walk = new Walks(),
                Dogs = dogs
            };

            return View(vm);
        }

        // POST: WalksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Walks walk)
        {
            try
            {
                _walksRepo.AddWalk(walk);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(walk);
            }
        }
    }
}
