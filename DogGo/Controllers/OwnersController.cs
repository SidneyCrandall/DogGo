// Controls to handle HTTP response
// ActionResult/IActionResult

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

        // GET: the form for a new owner to fill out and be added to the database
        public ActionResult Create()
        {
           // Return the empty form.
            return View();
        }

        // POST: Owners/Create
        // Above, we just GET a form. At that point the form will not POST data of a new owner.
        // This a response to the GET request. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }

        // GET: Owners/Delete/5
        // Similar to above except this time we just want to delete one owner
        // Let's invoke the GetOwnerById, so we can prompt users
        public ActionResult Delete(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            return View(owner);
        }

        // POST: Owners/Delete/5
        // We need a response for the GET above..
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }

        // GET: Owners/Edit/5
        public ActionResult Edit(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Owner owner)
        {
            try
            {
                _ownerRepo.UpdateOwner(owner);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }
    }
}
