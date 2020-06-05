using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using btl.Models;
using PagedList;
namespace btl.Controllers
{
    public class DonHangController : Controller
    {
        dbtecotecEntities db = new dbtecotecEntities();
        // GET: DonHang
        public ActionResult DonHang(int? page)
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                TempData["Message"] = "Bạn cần phải đăng nhập để xem danh sách đơn hàng";
                TempData["Type"] = "wait";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            NguoiDung nd = (NguoiDung)Session["TaiKhoan"];
            Method mt = new Method();
            return View(mt.GetHoaDon(nd.MaND).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult TimKiemHD()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TimKiemHD(int? page, FormCollection frm)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            DateTime ngay;
            if(DateTime.TryParse(frm["datetimkiem"].ToString(), out ngay)==false)
            {
                TempData["Message"] = "Có lỗi xảy ra, bạn hãy kiểm tra lại ngày nhập!";
                TempData["Type"] = "error";
                RedirectToAction("DonHang","DonHang");
            }

            NguoiDung nd = (NguoiDung)Session["TaiKhoan"];
            Method mt = new Method();
            var hd = mt.GetHoaDonByNgay(nd.MaND, ngay);
            return View(hd.ToPagedList(pageNumber, pageSize));
            //TempData["Message"] = "Có lỗi xảy ra,bạn hãy kiểm tra lại ngày nhập!";
            //TempData["Type"] = "error";
            //RedirectToAction("DonHang");    
        }
    }
}