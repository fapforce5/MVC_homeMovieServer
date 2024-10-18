using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using movieApp.Models;
using Newtonsoft.Json;

namespace movieApp.Helper
{
    public class CookiesInterface
    {

        public static bool SetLoggedInUser(HttpContext context, UserModel user, int expireDay = 365)
        {
            //string userJson = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            //var cookie = new HttpCookie(key, value);
            //cookie.Expires = DateTime.Now.AddDays(expireDay);
            //SetCookie(context, "user", userJson, expireDay);
            //return true;
            return false;
        }

        public static UserModel GetLoggedInUser(HttpContext context)
        {
            string userJson = GetCookie(context, "user");
            UserModel loggedInUser = null;
            if (loggedInUser != null)
            {
                try
                {
                    loggedInUser = Newtonsoft.Json.JsonConvert.DeserializeObject<UserModel>(userJson);
                }
                catch (Exception ex)
                {

                }
            }
            return loggedInUser;
        }

        private static void SetCookie(HttpContext context, string key, string value, int expireDay = 1)
        {
            //var cookie = new HttpCookie(key, value);
            //cookie.Expires = DateTime.Now.AddDays(expireDay);
            //Response.Cookies.Add(cookie);
        }

        private static string GetCookie(HttpContext context, string key)
        {
            string value = string.Empty;
            var cookie = context.Request.Cookies[key];
            if (cookie != null)
            {
                if (string.IsNullOrWhiteSpace(cookie.Value))
                {
                    return value;
                }
                value = cookie.Value;
            }
            return value;
        }
    }
}
