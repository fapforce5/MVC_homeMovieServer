using System;

namespace movieApp.Models
{
    public class MovieCategoryModel
    {
        public int id { get; set; }
        public string category { get; set; }
        public bool active { get; set; }
        public bool hidden { get; set; }
        public DateTime added { get; set; }
        public string addedBy { get; set; }
    }
}
