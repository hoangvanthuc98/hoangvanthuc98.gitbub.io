using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using btl.Models;
using PagedList;

namespace btl.Controllers
{
    public class SanPhamController : Controller
    {
        dbtecotecEntities db = new dbtecotecEntities();
        Method mt = new Method();
        // GET: SanPham
        public ActionResult Index()
        {
            return View();
        }
        public ViewResult XemDanhSachSanPham(int? page, int? idsp)
        {
            //int pageSize = 12;
            //int pageNumber = (page ?? 1);
            //return View(db.SanPhams.OrderBy(n=>n.MaSP).ToPagedList(pageNumber,pageSize));
            int ma = (idsp ?? -1);
            if (ma == -1)
            {
                int pageSize = 12;
                int pageNumber = (page ?? 1);
                ViewBag.tenLsp = "Tất cả sản phẩm";
                return View(db.SanPhams.Where(n=>n.Active==true).OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                int pageSize = 12;
                int pageNumber = (page ?? 1);
                int data = db.SanPhams.Where(n => n.MaSP == idsp).Select(n => n.MaLSP).FirstOrDefault();
                ViewBag.tenLsp = db.LoaiSanPhams.Where(n => n.MaLSP == data).Select(n => n.TenLSP);
                return View(mt.GetByLoaiSanPhamId(ma).ToPagedList(pageNumber, pageSize));
            }
        }
        public ViewResult XemChiTietSanPham(int id=0)
        {
            var sanpham = mt.GetSanPhamById(id);
            return View(sanpham);
        }
        //[HttpPost]
        //public ViewResult XemDanhSachSanPham(int? page,int? idsp)
        //{
        //    int ma = (idsp ?? -1);
        //    if (ma == -1)
        //    {
        //        int pageSize = 12;
        //        int pageNumber = (page ?? 1);
        //        return View(db.SanPhams.OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        //    }
        //    else
        //    {
        //        int pageSize = 12;
        //        int pageNumber = (page ?? 1);
        //        return View(mt.GetByLoaiSanPhamId(ma).ToPagedList(pageNumber, pageSize));
        //    }
        //}
    }
}