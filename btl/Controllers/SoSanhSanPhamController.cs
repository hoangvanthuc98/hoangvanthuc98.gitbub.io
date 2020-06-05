using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using btl.Models;
using PagedList;
using Newtonsoft.Json;

namespace btl.Controllers
{
    public class SoSanhSanPhamController : Controller
    {
        dbtecotecEntities db = new dbtecotecEntities();
        Method mt = new Method();

        public ActionResult Index()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult TimKiem(String name)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            var sp = db.SanPhams.Where(n => n.TenSP.StartsWith(name)&&n.Active==true).ToList();
            return PartialView(sp);
        }
        public ActionResult SoSanh()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SoSanh(FormCollection frm)
        {
            var lst = frm["sanPham"].ToString();
            var str = lst.Split(',');
            List<int> lstMa = new List<int>();
            foreach(var i in str)
            {
                if (i != "")
                {
                    int t =Int32.Parse(i);
                    lstMa.Add(t);
                }
            }
            if (lstMa.Count<2)
            {
                TempData["Message"] = "Bạn cần chọn ít nhất 2 sản phẩm để so sánh !";
                TempData["Type"] = "wait";
                return RedirectToAction("Index");
            }
            return View(lstMa);
        }

    }
}