using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieApp.Models
{
    public class RatingModel
    {

        private readonly List<string> ratingList = new List<string>()
        {
            "G", "PG-13", "R", "MA", "UR"
        };

        public List<string> Ratings { get => ratingList; }
    }
}
