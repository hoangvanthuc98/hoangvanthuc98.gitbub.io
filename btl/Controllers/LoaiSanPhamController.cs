using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using btl.Models;

namespace btl.Controllers
{
    public class LoaiSanPhamController : Controller
    {
        private dbtecotecEntities db = new dbtecotecEntities();

        // GET: LoaiSanPham
        public ActionResult Index()
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
            return View(db.LoaiSanPhams.ToList());
        }


        // GET: LoaiSanPham/Create
        public ActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLSP,TenLSP,MoTa")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                db.LoaiSanPhams.Add(loaiSanPham);
                db.SaveChanges();
                TempData["Message"] = "Thêm loại sản phẩm thành công";
                TempData["Type"] = "success";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Có lỗi xảy ra khi thêm loại sản phẩm";
            TempData["Type"] = "error";
            return View(loaiSanPham);
        }

        // GET: LoaiSanPham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaiSanPham).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Sửa loại sản phẩm thành công";
                TempData["Type"] = "success";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Có lỗi xảy ra khi sửa loại sản phẩm";
            TempData["Type"] = "error";
            return View(loaiSanPham);
        }

        //// GET: LoaiSanPham/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
        //    if (loaiSanPham == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(loaiSanPham);
        //}

        // POST: LoaiSanPham/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
        //    db.LoaiSanPhams.Remove(loaiSanPham);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
         public bool Delete(int id)
        {
            LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
            if (loaiSanPham != null)
            {
                bool sp = db.SanPhams.Any(n=>n.MaLSP==id);
                if (sp == true)
                {
                    TempData["Message"] = "Xóa không thành công vì vẫn tồn tại sản phẩm thuộc loại này";
                    TempData["Type"] = "error";
                    return false;
                }
                db.LoaiSanPhams.Remove(loaiSanPham);
                db.SaveChanges();
                TempData["Message"] = "Xóa loại sản phẩm thành công";
                TempData["Type"] = "success";
                return true;
            }
            else
            {
                TempData["Message"] = "Xóa loại sản phẩm không thành công";
                TempData["Type"] = "error";
                return false;
            }
        }
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
