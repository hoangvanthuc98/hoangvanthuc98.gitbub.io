using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using btl.Models;
using System.Web.Security;

namespace btl.Controllers
{
    public class TaiKhoanController : Controller
    {
        dbtecotecEntities db = new dbtecotecEntities();
        // GET: TaiKhoan
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(NguoiDung nd)
        {
            if (ModelState.IsValid)
            {
                nd.MaQuyen = 2;
                nd.TrangThai = true;
                db.NguoiDungs.Add(nd);
                db.SaveChanges();
                TempData["Message"] = "Đăng ký thành công !";
                TempData["Type"] = "success";
                return RedirectToAction("DangNhap");
            }
            else
            {
                TempData["Message"] = "Đăng ký không thành công !";
                TempData["Type"] = "error";
                return View();
            }
            
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public /*ActionResult*/bool DangNhap(FormCollection frm)
        {
            String tk = frm.Get("txtTaiKhoan").ToString();
            String mk = frm.Get("txtMatKhau").ToString();
            NguoiDung nd = db.NguoiDungs.SingleOrDefault(n => n.TaiKhoan == tk && n.MatKhau == mk);
            if(nd != null)
            {
                Session["TaiKhoan"] = nd;
                TempData["Message"] = "Xin chào : "+nd.Ho+" "+nd.Ten;
                TempData["Type"] = "wait";
                return true;
                //ViewBag.Ten = "Xin Chào : " + nd.Ten;
                //return View();
            }
            //ViewBag.ThongBao = "tài khoản hoặc mật khẩu không chính xác";
            //return View();
            return false;
        }
        public ActionResult ChangePassWord()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                TempData["Message"] = "Mời bạn đăng nhập !";
                TempData["Type"] = "wait";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassWord(FormCollection frm)
        {
            NguoiDung nd = (NguoiDung)Session["TaiKhoan"];
            var mk = frm["matkhau"];
            var mk1= frm["matkhau1"];
            var mk2 = frm["matkhau2"];
            if (nd.MatKhau != mk)
            {
                TempData["Message"] = "Mật khẩu không chính xác !";
                TempData["Type"] = "error";
                return RedirectToAction("ChangePassWord");
            }
            if (mk == "" || mk1 == "" || mk2 == "")
            {
                TempData["Message"] = "Mật khẩu không được để trống !";
                TempData["Type"] = "error";
                return RedirectToAction("ChangePassWord");
            }
            if (mk1 != mk2)
            {
                TempData["Message"] = "Mật khẩu mới không khớp !";
                TempData["Type"] = "error";
                return RedirectToAction("ChangePassWord");
            }
            var tk = db.NguoiDungs.Where(n => n.MaND == nd.MaND).FirstOrDefault();
            tk.MatKhau = mk1;
            db.SaveChanges();
            TempData["Message"] = "Đổi mật khẩu thành công !";
            TempData["Type"] = "success";
            return RedirectToAction("DangNhap", "TaiKhoan");
        }

        public ActionResult ForgotPassWord()
        {
            return View();
        }
        [HttpPost]
        public bool ForgotStep1(FormCollection frm)
        {
            String sdt = frm.Get("sdt").ToString();
            var nd = db.NguoiDungs.Where(n => n.SDT == sdt).FirstOrDefault();
            if (nd == null)
            {
                return false;
            }
            else
            {
                ViewBag.ma = nd.MaND;
                return true;
            }
        }
        [HttpPost]
        public bool ForgotStep2(FormCollection frm)
        {
            String sdt = frm.Get("sdt").ToString();
            String mk1= frm.Get("mk1").ToString();
            String mk2 = frm.Get("mk2").ToString();
            if (mk1 == "" || mk2 == "")
            {
                return false;
            }
            if (mk1 != mk2)
            {
                return false;
            }
            var nd = db.NguoiDungs.Where(n => n.SDT == sdt).FirstOrDefault();
            nd.MatKhau = mk1;
            db.SaveChanges();
            return true;
        }
        public ActionResult ThongTin()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                TempData["Message"] = "Mời bạn đăng nhập !";
                TempData["Type"] = "wait";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            NguoiDung nd = (NguoiDung)Session["TaiKhoan"];
            return View(nd);
        }
        public ActionResult LogOut()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult tkPartial()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                ViewBag.tenND = "";
                ViewBag.MaQuyen = -1;
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["TaiKhoan"];
                ViewBag.tenND = nd.Ten;
                ViewBag.MaQuyen =nd.MaQuyen;
            }
            return PartialView();
        }
    }
}