using System;
using System.Collections.Generic;
using System.Linq;
using crudDelicious.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace crudDelicious.Controllers
{
    public class ChefsController : Controller
    {
        private crudDeliciousContext _db;
        private int? _uid
        {
            get
            {
                return HttpContext.Session.GetInt32("ChefId");
            }
        }

        public ChefsController(crudDeliciousContext context)
        {
            _db = context;
        }
        [HttpGet("chefs")]
        public IActionResult Chefs()
        {
            List<Chef> allChefs = _db.Chefs
            .Include(chef => chef.Dishes)
            .ToList();
            return View(allChefs);
        }

        [HttpGet("chef/details/{id}")]
        public IActionResult DetailsChef(int id)
        {
            Chef selectedChef = _db.Chefs
            .Include(chef => chef.Dishes)
            .FirstOrDefault(d => d.ChefId == id);
            return View(selectedChef);
        }

        [HttpGet("chefs/new")]
        public IActionResult NewChef()
        {
            return View();
        }

        [HttpPost("chefs/create")]
        public IActionResult CreateChef(Chef newChef)
        {

            if (ModelState.IsValid)
            {
                // if (_uid != null)
                // {
                _db.Chefs.Add(newChef);
                _db.SaveChanges();
                return RedirectToAction("Chefs");
                // }
                // else // no one in session
                // {
                //     return RedirectToAction("Index", "Home");
                // }
            }
            else
            {
                return View("NewChef");
            }
        }

        [HttpGet("chefs/edit")]
        public IActionResult Edit(int id)
        {
            Chef toEdit = _db.Chefs
            .Include(chef => chef.Dishes)
            .FirstOrDefault(d => d.ChefId == id);

            if (toEdit == null)
                return RedirectToAction("Chefs");
            return View(toEdit);
        }

        [HttpPost("chefs/update")]
        public IActionResult Update(Chef editedChef, int id)
        {
            Chef dbChef = _db.Chefs
            .Include(chef => chef.Dishes)
            .FirstOrDefault(d => d.ChefId == id);

            if (editedChef.Biography != null && editedChef.Biography.Length > 5)
            {
                dbChef.Biography = editedChef.Biography;
                dbChef.UpdatedAt = DateTime.Now;

                //_db.Chefs.Update(dbChef);
                _db.SaveChanges();

                return RedirectToAction("DetailsChef", new { id = dbChef.ChefId });
            }
            ModelState.AddModelError("Biography", "Must be at least 5 characters");
            // so error messages will be displayed if any
            return View("Edit", dbChef);
        }



        [HttpPost("chefs/delete/{id}")]
        public IActionResult DeleteChef(int id)
        {
            Chef chefromDb = _db.Chefs.FirstOrDefault(chef => chef.ChefId == id);
            if (chefromDb != null)
            {
                _db.Chefs.Remove(chefromDb);
                _db.SaveChanges();
            }

            return RedirectToAction("Chefs");
        }
    }
}