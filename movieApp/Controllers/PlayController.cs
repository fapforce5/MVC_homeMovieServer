﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieApp.Controllers
{
    public class PlayController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
