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
using System.IO;

namespace btl.Controllers
{
    public class SanPhamAdminController : Controller
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
            //var sanPhams = db.SanPhams.Include(s => s.LoaiSanPham);
            //return View(sanPhams.ToList());
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(db.SanPhams.Include(s => s.LoaiSanPham).OrderByDescending(s => s.MaSP).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }
        [HttpPost]
        public ActionResult Details(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(l => l.MaSP == id);
            return View(sp);
        }

        public ActionResult Create()
        {
            ViewBag.MaLSP = new SelectList(db.LoaiSanPhams, "MaLSP", "TenLSP");
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "MaSP,MaLSP,TenSP,DonGia,SoLuong,HinhAnh")] SanPham sanPham)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.SanPhams.Add(sanPham);
        //        db.SaveChanges();
        //        TempData["Message"] = "Thêm sản phẩm thành công";
        //        TempData["Type"] = "success";
        //        return RedirectToAction("Index");
        //    }         
        //    TempData["Message"] = "Có lỗi xảy ra khi thêm sản phẩm";
        //    TempData["Type"] = "error";
        //    ViewBag.MaLSP = new SelectList(db.LoaiSanPhams, "MaLSP", "TenLSP", sanPham.MaLSP);
        //    return View(sanPham);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sp, HttpPostedFileBase file)
        {
            if (file == null)
            {
                TempData["Message"] = "Bạn cần chọn hình ảnh sản phẩm";
                TempData["Type"] = "error";
                return RedirectToAction("Create");
            }
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/images"), fileName);
            ViewBag.MaLSP = new SelectList(db.LoaiSanPhams.ToList(), "MaLSP", "TenLSP");
            if (ModelState.IsValid)
            {
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "hình ảnh đã tồn tại";
                }
                else
                {
                    file.SaveAs(path);
                }
                sp.HinhAnh = file.FileName;
                sp.Active = true;
                db.SanPhams.Add(sp);
                db.SaveChanges();
                TempData["Message"] = "Thêm sản phẩm thành công";
                TempData["Type"] = "success";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Có lỗi xảy ra khi thêm sản phẩm";
            TempData["Type"] = "error";
            ViewBag.MaLSP = new SelectList(db.LoaiSanPhams, "MaLSP", "TenLSP", sp.MaLSP);
            return View(sp);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLSP = new SelectList(db.LoaiSanPhams, "MaLSP", "TenLSP", sanPham.MaLSP);
            return View(sanPham);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(SanPham sanPham)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(sanPham).State = EntityState.Modified;
        //        db.SaveChanges();
        //        TempData["Message"] = "Sửa sản phẩm thành công";
        //        TempData["Type"] = "success";
        //        return RedirectToAction("Index");
        //    }
        //    TempData["Message"] = "Có lỗi xảy ra khi sửa sản phẩm";
        //    TempData["Type"] = "error";
        //    ViewBag.MaLSP = new SelectList(db.LoaiSanPhams, "MaLSP", "TenLSP", sanPham.MaLSP);
        //    return View(sanPham);
        //}
        [HttpPost]
        public ActionResult Edit(SanPham sp, HttpPostedFileBase file,FormCollection frm)
        {
            string fileName = "";
            string path = "";
            if (file == null)
            {
                fileName = frm["ha"].ToString();
            }
            else
            {
                fileName = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~/images"), fileName);
            }
            ViewBag.MaLSP = new SelectList(db.LoaiSanPhams.ToList(), "MaLSP", "TenLSP");
            string s = frm["trangthai"].ToString();
            bool tt;
            if (s == "Đang bán") tt = true;
            else tt = false;
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "hình ảnh đã tồn tại";
                    }
                    else
                    {
                        file.SaveAs(path);
                    }
                }              
                sp.HinhAnh = fileName;
                sp.Active = tt;
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Sửa sản phẩm thành công";
                TempData["Type"] = "success";
                return Redirect(frm["url"].ToString());
            }
            TempData["Message"] = "Có lỗi xảy ra khi sửa sản phẩm";
            TempData["Type"] = "error";
            sp.HinhAnh= frm["ha"].ToString();
            var a = frm["tt"].ToString();
            sp.Active = Boolean.Parse(a);
            ViewBag.MaLSP = new SelectList(db.LoaiSanPhams, "MaLSP", "TenLSP", sp.MaLSP);
            return View(sp);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public bool Delete(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham != null)
            {
                db.SanPhams.Remove(sanPham);
                var tssp = db.ThongSoSanPhams.Where(n=>n.MaSP==id).ToList();
                if (tssp != null)
                {
                    foreach(var item in tssp)
                    {
                        db.ThongSoSanPhams.Remove(item);
                    }
                }
                //var hd = db.ChiTietHoaDons.Where(n => n.MaSP == id).ToList();
                bool hd = db.ChiTietHoaDons.Any(n => n.MaSP == id);
                if (hd == true)
                {
                    TempData["Message"] = "Không xóa được vì sản phẩm này đang tồn tại trong các hóa đơn";
                    TempData["Type"] = "error";
                    return false;
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
        public ActionResult CreateThongSo(int id)
        {
            ThongSoSanPham ttsp = new ThongSoSanPham();
            ttsp.MaSP = id;
            ViewBag.MaTSKT = new SelectList(db.ThongSoKiThuats, "MaTSKT", "TenTSKT");
            return View(ttsp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateThongSo(ThongSoSanPham tssp, int id)
        {
            if (ModelState.IsValid)
            {
                tssp.MaSP = id;
                bool result = db.ThongSoSanPhams.Where(n=>n.MaSP==tssp.MaSP).Any(n=>n.MaTSKT==tssp.MaTSKT);
                if (result == true)
                {
                    TempData["Message"] = "Thông số kĩ thuật này đã tồn tại";
                    TempData["Type"] = "error";
                    return RedirectToAction("CreateThongSo");
                }
                db.ThongSoSanPhams.Add(tssp);
                db.SaveChanges();
                TempData["Message"] = "Thêm thông số sản phẩm thành công";
                TempData["Type"] = "success";
                return RedirectToAction("CreateThongSo");
            }
            TempData["Message"] = "Có lỗi xảy ra khi thêm thông số sản phẩm";
            TempData["Type"] = "error";
            ViewBag.MaTSKT = new SelectList(db.ThongSoKiThuats, "MaTSKT", "TenTSKT");
            return View(tssp);
        }
        public ActionResult EditThongSo(int id, int maTS, string url,FormCollection frm)
        {
            ThongSoSanPham tssp = db.ThongSoSanPhams.SingleOrDefault(l => l.MaSP == id && l.MaTSKT == maTS);
            if (tssp != null)
            {
                tssp.GiaTri = frm["txtgiatri"].ToString();
            }
            if (ModelState.IsValid)
            {
                db.Entry(tssp).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Sửa thông số sản phẩm thành công";
                TempData["Type"] = "success";
            }
            return Redirect(url);
        }
        [HttpPost]
        public bool DeleteThongSo(int id, int maTS/*,string url*/)
        {
            ThongSoSanPham tssp = db.ThongSoSanPhams.SingleOrDefault(l => l.MaSP == id && l.MaTSKT == maTS);
            if (tssp != null)
            {
                db.ThongSoSanPhams.Remove(tssp);
                db.SaveChanges();
                TempData["Message"] = "Xóa thông số sản phẩm thành công";
                TempData["Type"] = "success";
                return true;            
            }
            else
            {
                TempData["Message"] = "Có lỗi xảy ra khi xóa thông số sản phẩm";
                TempData["Type"] = "error";
                return false;
            }          
        }
        [HttpGet]
        public ActionResult KetQuaTimKiem(String key, int? page)
        {
            ViewBag.key = key;
            List<SanPham> lst = db.SanPhams.Where(l => l.TenSP.Contains(key)).ToList();
            if (lst.Count == 0)
            {
                ViewBag.thongbao = "Không tìm thấy kết quả phù hợp";
            }
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            return View(lst.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection frm, int? page)
        {
            String key = frm["txttimkiem"].ToString();
            if (key == "")
            {
                TempData["Message"] = "Bạn chưa nhập thông tin tìm kiếm";
                TempData["Type"] = "error";
                return RedirectToAction("Index");
            }
            ViewBag.key = key;
            List<SanPham> lst = db.SanPhams.Where(l => l.TenSP.Contains(key)).ToList();
            if (lst.Count == 0)
            {
                ViewBag.thongbao = "Không tìm thấy kết quả phù hợp";
            }
            int pageNumber = (page ?? 1);
            int pageSize = 12;
            return View(lst.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
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
