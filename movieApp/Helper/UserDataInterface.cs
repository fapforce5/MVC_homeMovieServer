using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using JsonFlatFileDataStore;
using movieApp.Models;

namespace movieApp.Helper
{
    public class UserDataInterface
    {
        private static string _userCookie = "loggedInUser";
        private static string _iconCookie = "loggedInUserIcon";
        public UserDataInterface()
        {

        }

        public static List<UserIconJoinModel> GetAllUsers()
        {
            DataStore store = new DataStore("userData.json");
            var collection = store.GetCollection<UserModel>();
            var iconCollection = store.GetCollection<UserIconModel>();
            List<UserIconJoinModel> userIconJoins = new List<UserIconJoinModel>();
            string defaultIconFilename = GetDefaultIcon().filename;

            List<UserModel> userList = collection.AsQueryable()
                .Where(x => x.active)
                .OrderBy(x => x.displayOrder)
                .ToList();

            List<UserIconModel> iconList = GetAllUserIconsWithDefault();

            userIconJoins = (from u in userList
                             join i in iconList on u.userIconId equals i.id into userIconJoin
                             from uij in userIconJoin.DefaultIfEmpty()
                             select new UserIconJoinModel
                             {
                                 userId = u.id,
                                 username = u.username,
                                 filename = uij.filename ?? defaultIconFilename,
                                 displayOrder = u.displayOrder
                             }).OrderBy(x => x.displayOrder).ToList();

            return userIconJoins;
        }

        public static UserModel GetUserByNameDescending(string userName)
        {
            DataStore store = new DataStore("userData.json");
            var collection = store.GetCollection<UserModel>();
            return collection.AsQueryable()
                .Where(x => x.username == userName)
                .OrderByDescending(x => x.id)
                .FirstOrDefault();
        }

        public static UserModel GetUserById(int id)
        {
            DataStore store = new DataStore("userData.json");
            var collection = store.GetCollection<UserModel>();
            return collection.AsQueryable()
                .Where(x => x.id == id && x.active)
                .FirstOrDefault();
        }

        public static UserModel AddUser(string username, int userIconId)
        {
            int displayOrder = 1;
            DataStore store = new DataStore("userData.json");
            var collection = store.GetCollection<UserModel>();

            username = new string(username.ToCharArray().Where(x => char.IsLetterOrDigit(x) || x == '_' || x == ' ' || x == '-').ToArray());

            UserModel result = collection.AsQueryable()
                .OrderByDescending(x => x.displayOrder)
                .FirstOrDefault();
            if (result != null)
                displayOrder = result.displayOrder + 1;

            UserModel insertRecord = new UserModel
            {
                id = 0,
                username = username,
                userIconId = userIconId,
                displayOrder = displayOrder,
                added = DateTime.Now,
                active = true
            };
            if (collection.InsertOne(insertRecord))
            {
                return GetUserByNameDescending(username);
            }
            return null;
        }

        public static bool UpdateUser(int id, string userName, int userIconId)
        {
            UserModel updateUser = GetUserById(id);
            if (updateUser == null)
                return false;

            updateUser.username = userName;
            updateUser.userIconId = userIconId;
            updateUser.active = true;

            DataStore store = new DataStore("userData.json");
            var collection = store.GetCollection<UserModel>();
            return collection.UpdateOne(id, updateUser);
        }

        public static bool DeleteUserById(int id)
        {
            UserModel deleteUser = GetUserById(id);
            if (deleteUser == null)
                return false;

            deleteUser.active = false;

            DataStore store = new DataStore("userData.json");
            var collection = store.GetCollection<UserModel>();
            return collection.UpdateOne(id, deleteUser);
        }

        public static List<UserIconModel> GetAllUserIcons()
        {
            DataStore store = new DataStore("userIconData.json");
            var collection = store.GetCollection<UserIconModel>();
            List<UserIconModel> iconList = collection.AsQueryable()
                .Where(x => x.active)
                .OrderByDescending(x => x.id)
                .ToList();
            if (iconList.Count() == 0)
                iconList.Add(GetDefaultIcon());
            return iconList;
        }

        public static List<UserIconModel> GetAllUserIconsWithDefault()
        {
            DataStore store = new DataStore("userIconData.json");
            var collection = store.GetCollection<UserIconModel>();
            List<UserIconModel> iconList = collection.AsQueryable()
                .Where(x => x.active)
                .OrderByDescending(x => x.id)
                .ToList();
            iconList.Add(GetDefaultIcon());
            return iconList;
        }

        private static UserIconModel GetDefaultIcon()
        {
            return new UserIconModel
            {
                id = -1,
                filename = "-1.jpg",
                active = true,
                added = DateTime.Now,
                addedUserId = -1
            };
        } 

        public static UserIconModel GetUserIconById(int id)
        {
            if (id == -1)
                return GetDefaultIcon();

            DataStore store = new DataStore("userIconData.json");
            var collection = store.GetCollection<UserIconModel>();
            return collection.AsQueryable()
                .Where(x => x.id == id && x.active)
                .FirstOrDefault() ?? GetDefaultIcon();
        }

        public static UserModel GetCurrentUser(HttpContext httpContext)
        {
            return CookiesInterface.GetLoggedInUser(httpContext);
        }
        
        public static HttpCookie GetUserCookie(int userId, int expireDays = 365)
        {
            UserModel user = GetUserById(userId);
            string userJson = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            HttpCookie cookie = new HttpCookie(_userCookie, userJson);
            cookie.Expires = DateTime.Now.AddDays(expireDays);
            return cookie;
        }

        public static HttpCookie GetIconCookie(int iconId, int expireDays = 365)
        {
            UserIconModel userIcon = GetUserIconById(iconId);
            string iconJson = Newtonsoft.Json.JsonConvert.SerializeObject(userIcon);
            HttpCookie cookie = new HttpCookie(_iconCookie, iconJson);
            cookie.Expires = DateTime.Now.AddDays(expireDays);
            return cookie;
        }

        public static string GetUserCookieKey()
        {
            return _userCookie;
        }

        public static string GetUserIconCookieKey()
        {
            return _iconCookie;
        }
    }
}
