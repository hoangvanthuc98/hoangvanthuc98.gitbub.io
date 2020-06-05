using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using btl.Models;
using PagedList;

namespace btl.Controllers
{
    public class TimKiemController : Controller
    {
        dbtecotecEntities db = new dbtecotecEntities();
        // GET: TimKiem
        [HttpGet]
        public ActionResult KetQuaTimKiem(String key, int? page)
        {
            ViewBag.key = key;
            List<SanPham> lst = db.SanPhams.Where(l => l.TenSP.Contains(key)&&l.Active==true).ToList();
            if (lst.Count == 0)
            {
                ViewBag.thongbao = "Không tìm thấy kết quả phù hợp";
            }
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            return View(lst.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection frm,int? page)
        {
            String key = frm["txttimkiem"].ToString();
            ViewBag.key = key;
            List<SanPham> lst = db.SanPhams.Where(l=>l.TenSP.Contains(key)&&l.Active==true).ToList();
            if (lst.Count == 0)
            {
                ViewBag.thongbao = "Không tìm thấy kết quả phù hợp";
            }
            int pageNumber = (page ?? 1);
            int pageSize = 12;
            return View(lst.OrderBy(n=>n.TenSP).ToPagedList(pageNumber,pageSize));
        }
    }
}