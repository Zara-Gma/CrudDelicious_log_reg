using System;
using System.Collections.Generic;
using System.Linq;
using crudDelicious.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace crudDelicious.Controllers
{
    public class DishesController : Controller
    {
        private crudDeliciousContext _db;
        private int? _uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }
        private bool _isLoggedIn
        {
            get
            {
                int? uid = _uid;

                if (uid != null)
                {
                    User loggedInUser =
                        _db.Users.FirstOrDefault(u => u.UserId == uid);

                    HttpContext.Session
                        .SetString("FullName", loggedInUser.FullName());
                }
                return uid != null;
            }
        }

        public DishesController(crudDeliciousContext context)
        {
            _db = context;
        }

        [HttpGet("dishes")]
        public IActionResult All()
        {
            List<Dish> allDishes = _db.Dishes
            .Include(dish => dish.Creator)
            .ToList();
            return View(allDishes);
        }

        [HttpGet("dish/details/{id}")]
        public IActionResult Details(int id)
        {
            Dish selectedDish = _db.Dishes
                .FirstOrDefault(d => d.DishId == id);

            // in case user manually types URL
            if (selectedDish == null)
                RedirectToAction("All");

            ViewBag.Uid = _uid;
            return View(selectedDish);
        }


        [HttpGet("dishes/new")]
        public IActionResult New()
        {
            NewDishPage newDishPage = new NewDishPage();
            newDishPage.AllChefs = _db.Chefs.ToList();
            return View(newDishPage);

        }

        [HttpPost("dishes/create")]
        public IActionResult Create(NewDishPage newDishPage)
        {
            NewDishPage dishPage = new NewDishPage();
            //newDishPage.AllChefs = dishPage.AllChefs;
            if (ModelState.IsValid)
            {
                //! Update it to Chef (this one is for user in session NOT needed it anymore)
                if (_uid != null)
                {
                    newDishPage.NewDish.ChefId = newDishPage.SelectedChefId;

                    _db.Dishes.Add(newDishPage.NewDish);
                    _db.SaveChanges();
                    return RedirectToAction("All");
                }
                else // no one in session
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                dishPage.AllChefs = _db.Chefs.ToList();
                return View("New", dishPage);
            }
        }

        [HttpGet("dishes/edit")]
        public IActionResult Edit(int id)
        {
            Dish toEdit = _db.Dishes.FirstOrDefault(d => d.DishId == id);

            if (toEdit == null)
                return RedirectToAction("All");
            return View(toEdit);
        }

        [HttpPost("dishes/update")]
        public IActionResult Update(Dish editedDish, int id)
        {

            if (ModelState.IsValid)
            {
                Dish dbDish = _db.Dishes.FirstOrDefault(d => d.DishId == id);

                if (dbDish != null)
                {
                    dbDish.Name = editedDish.Name;
                    // dbDish.Chef = editedDish.Chef;
                    dbDish.Tastiness = editedDish.Tastiness;
                    dbDish.Calories = editedDish.Calories;
                    dbDish.Description = editedDish.Description;
                    dbDish.UpdatedAt = DateTime.Now;

                    _db.Dishes.Update(dbDish);
                    _db.SaveChanges();

                    return RedirectToAction("Details", new { id = dbDish.DishId });
                }
            }
            // so error messages will be displayed if any
            return View("Edit", editedDish);
        }


        [HttpGet("dishes/delete/{id}")]
        public IActionResult Delete(int id)
        {
            Dish dishromDb = _db.Dishes.FirstOrDefault(dish => dish.DishId == id);

            if (dishromDb != null)
            {
                _db.Dishes.Remove(dishromDb);
                _db.SaveChanges();
            }

            return RedirectToAction("All");
        }
    }
}
