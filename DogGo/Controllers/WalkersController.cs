// Controller to handle the HTTP request.

using DogGo.Models;
using DogGo.Repositories;
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

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository)
        {
            _walkerRepo = walkerRepository;
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

            if (walker == null)
            {
                return NotFound();
            }

            return View(walker);
        }
    }
}
