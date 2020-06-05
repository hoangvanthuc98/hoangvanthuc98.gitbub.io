using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using btl.Models;
namespace btl.Controllers
{
    public class TKSP
    {
        public string TenSP { get; set; }
        public int SoLuongBan { get; set; }
    }
    public class ThongKeController : Controller
    {
        dbtecotecEntities db = new dbtecotecEntities();
        // GET: ThongKe
        public ActionResult Index()
        {
            ViewBag.time = DateTime.Now.Month;         
            return View();
        }
        [HttpGet]
        public JsonResult GetJsonData()
        {
            var query = (from h in db.HoaDons
                         join ct in db.ChiTietHoaDons on h.MaHD equals ct.MaHD
                         join s in db.SanPhams on ct.MaSP equals s.MaSP
                         where /*h.NgayLap>=start && h.NgayLap<=end */h.NgayLap.Month.Equals(DateTime.Now.Month)
                         select new
                         {
                             ct.MaSP,
                             s.TenSP,
                             ct.SoLuong,
                         }).OrderBy(n => n.MaSP);
            var result = from n in query
                         group n by n.MaSP into a
                         select new
                         {
                             MaSP = a.Key,
                             TenSP = a.ToList(),
                             count = a.Count(),
                         };
            var list = new List<TKSP>();
            foreach (var item in result)
            {
                var ten = "";
                int dem = 0;
                foreach (var i in item.TenSP)
                {
                    ten = i.TenSP;
                    dem = dem + i.SoLuong;
                }
                list.Add(new TKSP { TenSP = ten, SoLuongBan = dem });
            }
            var c = list;
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetJsonData(DateTime? start, DateTime? end)
        {
            var query = (from h in db.HoaDons
                         join ct in db.ChiTietHoaDons on h.MaHD equals ct.MaHD
                         join s in db.SanPhams on ct.MaSP equals s.MaSP
                         where h.NgayLap >= start && h.NgayLap <= end
                         select new
                         {
                             ct.MaSP,
                             s.TenSP,
                             ct.SoLuong,
                         }).OrderBy(n => n.MaSP);
            var result = from n in query
                         group n by n.MaSP into a
                         select new
                         {
                             MaSP = a.Key,
                             TenSP = a.ToList(),
                             count = a.Count(),
                         };
            var list = new List<TKSP>();
            foreach (var item in result)
            {
                var ten = "";
                int dem = 0;
                foreach (var i in item.TenSP)
                {
                    ten = i.TenSP;
                    dem = dem + i.SoLuong;
                }
                list.Add(new TKSP { TenSP = ten, SoLuongBan = dem });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult TimKiem(FormCollection frm)
        {
            DateTime start, end;
            if (DateTime.TryParse(frm["dateStart"].ToString(), out start) == false || DateTime.TryParse(frm["dateEnd"].ToString(), out end) == false)
            {
                TempData["Message"] = "Bạn hãy kiểm tra lại ngày nhập!";
                TempData["Type"] = "error";
                ViewBag.start = DateTime.Now;
                ViewBag.end = DateTime.Now;
                return RedirectToAction("Index");
            }
            if (DateTime.Compare(start, end) > 0)
            {
                DateTime tg = start;
                start = end;
                end = tg;
            }
            ViewBag.start = start;
            ViewBag.end = end;
            return View();
        }
    }
}