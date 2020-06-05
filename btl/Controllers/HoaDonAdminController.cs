using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using btl.Models;
using PagedList;

namespace btl.Controllers
{
    public class HoaDonAdminController : Controller
    {
        private dbtecotecEntities db = new dbtecotecEntities();
        public ActionResult Index(int? page)
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                TempData["Message"] = "Bạn cần phải đăng nhập để sử dụng chức năng quản trị !";
                TempData["Type"] = "wait";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["TaiKhoan"];
                if (nd.MaQuyen != 1)
                {
                    TempData["Message"] = "Bạn cần đăng nhập bằng tài khoản của quản trị viên !";
                    TempData["Type"] = "error";
                    return RedirectToAction("DangNhap", "TaiKhoan");
                }
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var hoaDons = db.HoaDons.Include(h => h.NguoiDung).Where(h=>h.TrangThai==false);
            return View(hoaDons.OrderBy(n=>n.MaHD).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Edit(int id, string url, FormCollection frm)
        {
            HoaDon hd = db.HoaDons.FirstOrDefault(n => n.MaHD == id);
            var KH = db.NguoiDungs.Where(n => n.MaND == hd.MaND).FirstOrDefault();
            string s = frm["trangthai"];
            bool tt;
            if (s == "Đã nhận hàng") tt = true;
            else tt = false;
            hd.TrangThai = tt;
            if (ModelState.IsValid)
            {
                db.Entry(hd).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Sửa trạng thái hóa đơn thành công thành công";
                TempData["Type"] = "success";
            }
            if (hd.TrangThai == true)
            {
                var send = new SendMailController().Send(KH.Email);
            }            
            //if (send == true)
            //{
            //    TempData["Message"] = "Đã gửi mail cho khách hàng "+KH.Ho+" "+KH.Ten;
            //    TempData["Type"] = "success";
            //}
            //else
            //{
            //    TempData["Message"] = "Chưa gửi được mail cho khách hàng " + KH.Ho + " " + KH.Ten;
            //    TempData["Type"] = "wait";
            //}
            return Redirect(url);
        }
        public ActionResult TimKiemByMaHD()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult TimKiemByMaHD(int? page,FormCollection frm)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            int ma;
            if (int.TryParse(frm["mahd"].ToString(), out ma) == false)
            {
                TempData["Message"] = "Có lỗi xảy ra, bạn hãy kiểm tra lại mã đơn hàng !";
                TempData["Type"] = "error";
                return RedirectToAction("Index");
            }
            var hoaDons = db.HoaDons.Include(h => h.NguoiDung).Where(h => h.MaHD == ma);
            return View(hoaDons.OrderBy(n => n.MaHD).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult TimKiemByNgayDH()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult TimKiemByNgayDH(int? page, FormCollection frm)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            DateTime ngay;
            if (DateTime.TryParse(frm["datetimkiem"].ToString(), out ngay) == false)
            {
                TempData["Message"] = "Có lỗi xảy ra, bạn hãy kiểm tra lại ngày nhập!";
                TempData["Type"] = "error";
                return RedirectToAction("Index");
            }
            var hoaDons = db.HoaDons.Include(h => h.NguoiDung).Where(h => h.NgayLap == ngay);
            return View(hoaDons.OrderBy(n => n.MaHD).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }
        [HttpPost]
        public ActionResult Details(int id)
        {
            HoaDon hd = db.HoaDons.SingleOrDefault(l => l.MaHD == id);
            return View(hd);
        }
        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public bool Delete(int id)
        {
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon != null)
            {
                db.HoaDons.Remove(hoaDon);
                var cthd = db.ChiTietHoaDons.Where(n => n.MaHD == id).ToList();
                if (cthd != null)
                {
                    foreach (var item in cthd)
                    {
                        db.ChiTietHoaDons.Remove(item);
                    }
                }
                db.SaveChanges();
                TempData["Message"] = "Xóa thành công !";
                TempData["Type"] = "success";
                return true;
            }
            else
            {
                TempData["Message"] = "Xóa không thành công";
                TempData["Type"] = "error";
                return false;
            }
        }

        // GET: HoaDonAdmin/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    HoaDon hoaDon = db.HoaDons.Find(id);
        //    if (hoaDon == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(hoaDon);
        //}

        //// POST: HoaDonAdmin/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    HoaDon hoaDon = db.HoaDons.Find(id);
        //    db.HoaDons.Remove(hoaDon);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
