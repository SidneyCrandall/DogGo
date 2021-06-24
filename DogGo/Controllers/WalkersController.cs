// Controller to handle the HTTP request.
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// This is logic that is already built in. We are simple adding more functionality to it.
namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        // We call the repo which handles the Sql request.
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalksRepository _walksRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository, IWalksRepository walksRepository)
        {
            _walkerRepo = walkerRepository;
            _walksRepo = walksRepository;
        }

        /* Action Results first described in the 'HomController'. */
        // GET: Walkers a List of all the walkers in our a database
        // After we instance the IWalkerRepository
        // Helps to serve where the request of an action should go. (Return Type Method)
        public ActionResult Index()
        {
            List<Walker> walkers = _walkerRepo.GetAllWalkers();

            return View(walkers);
        }

        // GET: Walkers/Details/5
        // Action method that will loop through the list of walkers and return a response to you if there is a walker attached to that data
        // or is triggered by a button click
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Walks> walks = _walksRepo.GetWalkByWalkerId(walker.Id);

            WalkerViewModel vm = new WalkerViewModel()
            {
                Walker = walker,
                Walks = walks
            };

            return View(vm);
        }

        
        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Walker walker)
        {
            try
            {
                _walkerRepo.AddWalker(walker);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(walker);
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);

            if (walker == null)
            {
                return NotFound();
            }
            return View(walker);
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Walker walker)
        {
            try
            {
                _walkerRepo.UpdateWalker(walker);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(walker);
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            return View(walker);
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Walker walker)
        {
            try
            {
                _walkerRepo.DeleteWalker(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(walker);
            }
        }
    }
}
