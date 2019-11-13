using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSimple.Samples.Controllers
{
    public class HomeController: Controller
    {
        public JsonResult Index()
        {
            return Json(new { name ="tom", age ="18"});
        }
    }
}
