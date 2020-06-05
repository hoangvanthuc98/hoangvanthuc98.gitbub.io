using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using btl.Models;
namespace btl.Controllers
{
    public class GioHangController : Controller
    {
        dbtecotecEntities db = new dbtecotecEntities();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int id, String strUrl)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.Find(n => n.iMaSP == id);
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                lstGioHang.Add(sanpham);
                return Redirect(strUrl);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strUrl);
            }
        }
        public ActionResult SuaGioHang(int id, FormCollection frm)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaSP == id);
            if (sanpham != null)
            {
                if (int.Parse(frm["txtSoLuong"].ToString()) > 0)
                {
                    sanpham.iSoLuong = int.Parse(frm["txtSoLuong"].ToString());
                }
                else
                {
                    TempData["Message"] = "Số lượng phải lớn hơn 0 !";
                    TempData["Type"] = "error";
                }
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaSP == id);
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSP==id);     
            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("XemDanhSachSanPham", "SanPham");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                TempData["Message"] = "Giỏ hàng đang trống, bạn vui lòng chọn sản phẩm !";
                TempData["Type"] = "wait";
                return RedirectToAction("XemDanhSachSanPham", "SanPham");
            }
            return View(lstGioHang); 
        }
        public int TongSoLuong()
        {
            int tong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return tong;
        }
        public double TongTien()
        {
            double tong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tong = lstGioHang.Sum(n => n.ThanhTien);
            }
            return tong;
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView();
        }
        [HttpPost]
        public ActionResult HoaDon()
        {
            if(Session["TaiKhoan"]==null || Session["TaiKhoan"].ToString() == "")
            {
                TempData["Message"] = "Bạn cần đăng nhập để đặt hàng";
                TempData["Type"] = "wait";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            if (Session["GioHang"] == null || Session["GioHang"].ToString()=="")
            { 
                return RedirectToAction("XemDanhSachSanPham", "SanPham"); 
            }
            HoaDon hd = new HoaDon();
            NguoiDung nd = (NguoiDung)Session["TaiKhoan"];
            List<GioHang> lstgiohang = LayGioHang();
            hd.MaND = nd.MaND;
            hd.NgayLap = DateTime.Now;
            hd.TrangThai = false;
            db.HoaDons.Add(hd);
            db.SaveChanges();
            foreach(var item in lstgiohang)
            {
                ChiTietHoaDon cthd = new ChiTietHoaDon();
                cthd.MaHD = hd.MaHD;
                cthd.MaSP = item.iMaSP;
                cthd.SoLuong = item.iSoLuong;
                cthd.DonGia = item.fDonGia;
                db.ChiTietHoaDons.Add(cthd);
            }
            db.SaveChanges();
            TempData["Message"] = "Đặt hàng thành công, bạn có thể xem chi tiết tại mục đơn hàng !";
            TempData["Type"] = "success";
            Session["GioHang"] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}