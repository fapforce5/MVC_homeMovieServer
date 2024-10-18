using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieApp.Models
{
    public class MovieModel
    {
        public int id { get; set; }
        public string movieName { get; set; }
        public string movieDesc { get; set; }
        public TimeSpan duration { get; set; }
        public string durationString { get { return duration.ToString(@"hh\:mm"); } }
        public string fileName { get; set; }
        public string thumbnail { get; set; }
        public List<int> categoryId { get; set; }
        public string rating { get; set; }
        public bool active { get; set; }
        public DateTime added { get; set; }
        public string addedBy { get; set; }
    }
}
