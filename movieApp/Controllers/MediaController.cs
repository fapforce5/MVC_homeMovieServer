using Microsoft.AspNetCore.Mvc;
using movieApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using NReco;
using movieApp.Helper;
using System.Threading.Tasks;

namespace movieApp.Controllers
{
    public class MediaController : Controller
    {
        public MediaController()//IWebHostEnvironment environment)
        {
            //_hostingEnvironment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Upload()
        {
            return View();
        }


        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Category()
        {
            return View();
        }

        public IActionResult Manage()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadRawMovie(IFormFile rawUpload)
        {
            TimeSpan duration = TimeSpan.FromSeconds(0);
            List<string> validFileExtensions = new List<string>()
            {
                "avi", "mp4", "webm", "mov", "wmv", "mkv"
            };
            string filename = rawUpload.FileName;
            if (!filename.Contains("."))
                return Json(new { validUpload = false, error = "Invalid File Extension" });

            string fileExtension = filename.Substring(filename.LastIndexOf(".")).Replace(".", "").ToLower();
            if (validFileExtensions.Where(x => x == fileExtension).FirstOrDefault() == null)
                return Json(new { validUpload = false, error = "Invalid File Extension: " + fileExtension });

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = path.Replace("movieApp.exe", "");
            string basePath = path.Substring(0, path.LastIndexOf("movieapp", StringComparison.OrdinalIgnoreCase) + 8);
            string newPath = "\\wwwroot\\media\\";
            string saveFileName = Guid.NewGuid().ToString();
            string fullSavePath = basePath + newPath + saveFileName + "." + fileExtension;
            string fullSaveThumbnailPath = basePath + newPath + saveFileName + ".jpg";
            
            string webPathMovie = ("/media/" + saveFileName + "." + fileExtension).Replace("\\", "/");
            string webPathThumbnail = ("/media/" + saveFileName + ".jpg").Replace("\\", "/");

            FileStream fileStream = new FileStream(fullSavePath, FileMode.Create);
                rawUpload.CopyTo(fileStream);

            MemoryStream thumbJpegStream = new MemoryStream();
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            var ffProbe = new NReco.VideoInfo.FFProbe();
            
            ffMpeg.GetVideoThumbnail(fullSavePath, fullSaveThumbnailPath, 5);
            try
            {
                var videoInfo = ffProbe.GetMediaInfo(fullSavePath);
                duration = videoInfo.Duration;
            }
            catch { }

            MovieModel insertedMovie = await MovieDataInterface.InsertMovieRecord(new MovieModel
            {
                id = 0,
                movieName = "",
                movieDesc = "",
                duration = duration,
                active = true,
                fileName = webPathMovie,
                thumbnail = webPathThumbnail,
                categoryId = new List<int>(),
                rating = "UR",
                added = DateTime.Now,
                addedBy = "UNK"
            }); 

            return Json(insertedMovie);
        }

        [HttpGet]
        public ActionResult GetAutocompleteMovies(string search, int howMany)
        {
            return Json(MovieDataInterface.GetAutocompleteMovies(search, howMany));
        }

        public ActionResult GetMovieByTitle(string search, int howMany)
        {
            return Json(MovieDataInterface.GetMovieByTitle(search, howMany));
        }

    }
}