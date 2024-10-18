using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieApp.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public int displayOrder { get; set; }
        public int userIconId { get; set; }
        public DateTime added { get; set; }
        public bool active { get; set; }
    }

    public class UserIconModel
    {
        public int id { get; set; }
        public string filename { get; set; }
        public bool active { get; set; }
        public DateTime added { get; set; }
        public int addedUserId { get; set; }
    }

    public class UserIconJoinModel
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string filename { get; set; }
        public int displayOrder { get; set; }
    }
}
