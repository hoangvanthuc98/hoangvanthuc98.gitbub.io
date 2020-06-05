using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using btl.Models;
namespace btl.Models
{
    public class GioHang
    {
        dbtecotecEntities db = new dbtecotecEntities();
        public int iMaSP { get; set; }
        public String sTenSP { get; set; }
        public String sHinhAnh { get; set; }
        public double fDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double ThanhTien
        {
            get { return fDonGia * iSoLuong; }
        }
        public GioHang(int id)
        {
            iMaSP = id;
            SanPham sp = db.SanPhams.Single(n => n.MaSP==iMaSP);
            sTenSP = sp.TenSP;
            sHinhAnh = sp.HinhAnh;
            fDonGia = sp.DonGia-(double)((sp.DonGia*sp.KhuyenMai)/100);
            iSoLuong = 1;
        }
    }
}