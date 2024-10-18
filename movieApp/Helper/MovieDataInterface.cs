using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonFlatFileDataStore;
using movieApp.Models;

namespace movieApp.Helper
{
    public class MovieDataInterface
    {
        public MovieDataInterface()
        {

        }
        public static async Task<MovieModel> InsertMovieRecord(MovieModel insertRecord)
        {
            DataStore store = new DataStore("movieData.json");
            var collection = store.GetCollection<MovieModel>();

            if (await collection.InsertOneAsync(insertRecord))
                return GetNewestMovies(1).FirstOrDefault();
            else
                return null;
        }

        public static MovieModel GetMovieById(int id)
        {
            List<MovieModel> movieModels = new List<MovieModel>();

            DataStore store = new DataStore("movieData.json");
            var collection = store.GetCollection<MovieModel>();
            MovieModel results = collection.AsQueryable().Where(e => e.id  == id).FirstOrDefault();
            return results;
        }

        public static bool UpdateMovie(int id, string movieName, string movieDesc, string rating)
        {
            MovieModel movieModel = GetMovieById(id);
            movieModel.movieName = movieName;
            movieModel.movieDesc = movieDesc;
            movieModel.rating = rating;

            DataStore store = new DataStore("movieData.json");
            var collection = store.GetCollection<MovieModel>();
            return collection.UpdateOne(id, movieModel);
        }

        public static List<MovieModel> GetNewestMovies(int howMany)
        {
            List<MovieModel> movieModels = new List<MovieModel>();

            DataStore store = new DataStore("movieData.json");
            var collection = store.GetCollection<MovieModel>();
            List<MovieModel> results = collection.AsQueryable()
                .OrderByDescending(x => x.added)
                .Take(howMany)
                .ToList();
            return results;
        }

        public static List<MovieModel> GetMovieByTitle(string search, int howMany)
        {
            DataStore store = new DataStore("movieData.json");
            var collection = store.GetCollection<MovieModel>();
            List<MovieModel> results = collection.AsQueryable()
                .Where(x => x.movieName.Contains(search))
                .OrderByDescending(x => x.id)
                .Take(howMany)
                .ToList();
            return results;
        }

        public static List<string> GetAutocompleteMovies(string search, int howMany)
        {
            DataStore store = new DataStore("movieData.json");
            var collection = store.GetCollection<MovieModel>();
            List<string> results = collection.AsQueryable()
                .Where(x => x.movieName.StartsWith(search))
                .OrderByDescending(x => x.added)
                .Take(howMany)
                .Select(y => y.movieName)
                .ToList();
            return results;
        }

        public static async Task<bool> DeleteMovieCategory(int id)
        {
            DataStore store = new DataStore("movieCategoryData.json");
            var collection = store.GetCollection<MovieCategoryModel>();
            MovieCategoryModel deleteCategory = collection.AsQueryable()
                .Where(x => x.id == id)
                .FirstOrDefault();
            if (deleteCategory == null)
                return false;

            deleteCategory.active = false;
            await collection.UpdateOneAsync(id, deleteCategory);
            return true;
        }

        public static async Task<bool> UpdateMovieCategory(int id, string category)
        {
            DataStore store = new DataStore("movieCategoryData.json");
            var collection = store.GetCollection<MovieCategoryModel>();

            MovieCategoryModel checkDuplicate = collection.AsQueryable()
                .Where(x => string.Equals(x.category, category, StringComparison.OrdinalIgnoreCase) && x.id != id)
                .FirstOrDefault();
            if (checkDuplicate != null)
            {
                if (checkDuplicate.active)
                    return false;
                else
                {
                    checkDuplicate.active = true;
                    checkDuplicate.category = category;
                    await collection.UpdateOneAsync(id, checkDuplicate);
                    return true;
                }

            }

            MovieCategoryModel updateCategory = collection.AsQueryable()
                .Where(x => x.id == id)
                .FirstOrDefault();
            if (updateCategory == null)
                return false;

            updateCategory.active = true;
            updateCategory.category = category;

            await collection.UpdateOneAsync(id, updateCategory);
            return true;
        }

        public static async Task<bool> AddMovieCategory(MovieCategoryModel movieCategory)
        {
            DataStore store = new DataStore("movieCategoryData.json");
            var collection = store.GetCollection<MovieCategoryModel>();

            if (movieCategory.category == null)
                return false;

            if (movieCategory.category.Trim().Length == 0)
                return false;

            MovieCategoryModel checkDuplicate = collection.AsQueryable()
                .Where(x => string.Equals(x.category, movieCategory.category, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
            
            if (checkDuplicate != null)
            {
                if (checkDuplicate.active)
                    return false;
                else
                {
                    checkDuplicate.active = true;
                    await collection.UpdateOneAsync(movieCategory.id, movieCategory);
                    return true;
                }
            }
            movieCategory.id = 0;
            await collection.InsertOneAsync(movieCategory);
            return true;
        }

        public static List<MovieCategoryModel> GetMovieCategoriesAll()
        {
            DataStore store = new DataStore("movieCategoryData.json");
            var collection = store.GetCollection<MovieCategoryModel>();
            List<MovieCategoryModel> results = collection.AsQueryable()
                .Where(x => x.active)
                .OrderBy(x => x.category)
                .ToList();
            return results;
        }

        public static MovieCategoryModel GetMovieCategoryById(int id)
        {
            DataStore store = new DataStore("movieCategoryData.json");
            var collection = store.GetCollection<MovieCategoryModel>();
            MovieCategoryModel results = collection.AsQueryable()
                .Where(x => x.id == id)
                .FirstOrDefault();
            return results;
        }

        public static MovieCategoryModel GetMovieCategoryByCategory(string category)
        {
            DataStore store = new DataStore("movieCategoryData.json");
            var collection = store.GetCollection<MovieCategoryModel>();
            MovieCategoryModel results = collection.AsQueryable()
                .Where(x => string.Equals(x.category, category, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
            return results;
        }
    }
}
