using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        public HomeController(MyContext context)
        {
            _context =context;
        }

        public IActionResult Index()
        {
            ViewBag.AllDishes = _context.Dishes.ToList();

            return View();
        }

        [HttpGet("form")]
        public IActionResult DishForm()
        {
            return View();
        }

        [HttpPost("submit")]
        public IActionResult Submit(Dish newDish)
        {
            if(ModelState.IsValid)
            {
                _context.Dishes.Add(newDish);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("DishForm");
        }

        [HttpGet("dishes/{id}")]
        public IActionResult SingleDish(int id)
        {
            Console.WriteLine(id);

            ViewBag.SingleDish = _context.Dishes
                .FirstOrDefault(d => d.DishId == id);
            return View();
        }

        [HttpGet("dishes/{id}/delete")]
        public IActionResult DeleteDish(int id)
        {
            Dish deleteMe = _context.Dishes
                .FirstOrDefault(d => d.DishId == id);

            _context.Dishes.Remove(deleteMe);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet("dishes/{id}/edit")]
        public IActionResult EditForm(int id)
        {
            Dish displayMe = _context.Dishes
                .FirstOrDefault(d => d.DishId == id);

            return View(displayMe);
        }

        [HttpPost("dishes/submitEdit")]
        public IActionResult SubmitEdit(Dish editedDish)
        {
            Dish editMe = _context.Dishes
                .FirstOrDefault(d => d.DishId == editedDish.DishId);

                editMe.Name = editedDish.Name;
                editMe.Chef = editedDish.Chef;
                editMe.Tastiness = editedDish.Tastiness;
                editMe.Calories = editedDish.Calories;
                editMe.Description = editedDish.Description;

                _context.SaveChanges();

                return RedirectToAction("SingleDish", new {id = editedDish.DishId});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
