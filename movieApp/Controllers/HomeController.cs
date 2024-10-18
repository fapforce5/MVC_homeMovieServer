using Microsoft.AspNetCore.Mvc;
using movieApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace movieApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetNewestMovies(int howMany)
        {
            List<MovieModel> movieModels = Helper.MovieDataInterface.GetNewestMovies(howMany);
            return Json(movieModels);
        }

        [HttpGet]
        public ActionResult GetMovieById(int id)
        {
            MovieModel movieModel = Helper.MovieDataInterface.GetMovieById(id);
            return Json(movieModel);
        }
        
        [HttpPost]
        public ActionResult UpdateMovie(int id, string movieName, string movieDesc, string rating)
        {
            bool success = Helper.MovieDataInterface.UpdateMovie(id, movieName, movieDesc, rating);
            return Json(new { success = success });
        }

        [HttpGet]
        public ActionResult GetMovieCategoriesAll()
        {
            List<MovieCategoryModel> movieCategories = Helper.MovieDataInterface.GetMovieCategoriesAll();
            return Json(movieCategories);
        }

        [HttpGet]
        public ActionResult GetMovieCategoryById(int id)
        {
            MovieCategoryModel movieCategory = Helper.MovieDataInterface.GetMovieCategoryById(id);
            return Json(movieCategory);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteMovieCategory(int id)
        {
            bool success = await Helper.MovieDataInterface.DeleteMovieCategory(id);
            return Json(new { success = success });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateMovieCategory(int id, string category)
        {
            bool success = await Helper.MovieDataInterface.UpdateMovieCategory(id, category);
            return Json(new { success = success });
        }

        [HttpPost]
        public async Task<ActionResult> AddMovieCategory(string category)
        {
            MovieCategoryModel newCategory = new MovieCategoryModel
            {
                id = 0,
                category = category,
                active = true,
                added = DateTime.Now,
                addedBy = "UNK"
            };
            bool success = await Helper.MovieDataInterface.AddMovieCategory(newCategory);
            return Json(new { success = success });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
