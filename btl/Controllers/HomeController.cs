using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using btl.Models;

namespace btl.Controllers
{
    public class HomeController : Controller
    {
        Method mt = new Method();
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult MenuPatial()
        {
            return PartialView("MenuPatial", mt.GetListLoaiSP());
        }
        public ActionResult GPKT()
        {
            return View();
        }
    }
}