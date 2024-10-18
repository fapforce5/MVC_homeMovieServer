using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movieApp.Models;
using movieApp.Helper;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace movieApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetAllUserIcons()
        {
            List<UserIconModel> userIconList = Helper.UserDataInterface.GetAllUserIcons();
            return Json(userIconList);
        }

        [HttpPost]
        public ActionResult AddUser(string userName, int userIconId)
        {
            UserIconModel icon;
            UserModel insertedUser = Helper.UserDataInterface.AddUser(userName, userIconId);
            if (insertedUser != null)
            {
                icon = Helper.UserDataInterface.GetUserIconById(insertedUser.userIconId);
                return Json(new { userIcon = icon, user = insertedUser });
            }
            return null;
        }

        [HttpPost]
        public ActionResult SwitchUser(int? userId)
        {
            if (userId == null)
            {
                DeleteUserCookie();
                return Json(new { success = true });
            }
            else
            {
                UserModel user = UserDataInterface.GetUserById((int)userId);
                SetUserCookie((int)userId, user.userIconId);
                return Json(new { success = true });
            }
        }

        [HttpGet]
        public ActionResult GetLoggedInUser()
        {
            UserModel loggedInUser;
            UserIconModel loggedInUserIcon;
            (loggedInUser, loggedInUserIcon) = GetLoggedInUserCookie();
            return Json(new { user = loggedInUser, userIcon = loggedInUserIcon });
        }

        [HttpGet]
        public ActionResult GetAllUsers()
        {
            return Json(UserDataInterface.GetAllUsers());
        }

        private void DeleteUserCookie()
        {
            string userCookieKey = Helper.UserDataInterface.GetUserCookieKey();
            string iconCookieKey = Helper.UserDataInterface.GetUserIconCookieKey();

            Response.Cookies.Append(userCookieKey, null, new CookieOptions { MaxAge = TimeSpan.FromDays(-1) });
            Response.Cookies.Append(iconCookieKey, null, new CookieOptions { MaxAge = TimeSpan.FromDays(-1) });
        }

        private void SetUserCookie(int userId, int iconId)
        {
            HttpCookie cookie = UserDataInterface.GetIconCookie(iconId);
            Response.Cookies.Append(cookie.Name, cookie.Value, new CookieOptions { MaxAge = TimeSpan.FromDays(365) });

            cookie = Helper.UserDataInterface.GetUserCookie(userId);
            Response.Cookies.Append(cookie.Name, cookie.Value, new CookieOptions { MaxAge = TimeSpan.FromDays(365) });
        }

        private (UserModel, UserIconModel) GetLoggedInUserCookie()
        {
            UserModel user = null;
            UserIconModel icon = null;

            string userKey = Helper.UserDataInterface.GetUserCookieKey();
            string userValue = Request.Cookies[userKey];

            string iconKey = UserDataInterface.GetUserIconCookieKey();
            string iconValue = Request.Cookies[iconKey];
            
            if (userValue != null && iconValue != null)
            {
                try
                {
                    icon = Newtonsoft.Json.JsonConvert.DeserializeObject<UserIconModel>(iconValue);
                    user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserModel>(userValue);
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }
            return (user, icon);
        }
    }
}
