// Controls to handle HTTP response

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Repositories;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {
        // We call the repo which handles the Sql request.
        private readonly IOwnerRepository _ownerRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public OwnersController(IOwnerRepository ownerRepository)
        {
            _ownerRepo = ownerRepository;
        }

        /* Action Results first described in the 'HomController'. */
        // GET: Owners a List of all the owners in our a database
        // After we instance the IOwnerRepository
        // Helps to serve where the request of an action should go. (Return Type Method)
        public ActionResult Index()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();

            return View(owners);
        }

        // GET: Owner/Details/5
        // Action method that will loop through the list of owners and return a response to you if there is a owner attached to that data
        // or is triggered by a button click
        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            if (owner == null)
            {
                return NotFound();
            }
            return View(owner);
        }
    }
}
