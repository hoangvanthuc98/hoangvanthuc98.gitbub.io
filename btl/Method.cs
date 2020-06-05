using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using btl.Models;
using PagedList;

namespace btl
{
    public class Method
    {
        dbtecotecEntities db = new dbtecotecEntities();
        public List<SanPham> GetByLoaiSanPhamId(int id)
        {
            try
            {
                var result = db.SanPhams.Where(l => l.MaLSP == id&&l.Active==true).OrderBy(l => l.MaSP).ToList();
                return result;
            }
            catch
            {
                return new List<SanPham>();
            }
        }
        public String GetTenLoaiSPById(int id)
        {
            try
            {
                var result = db.LoaiSanPhams.Where(l => l.MaLSP == id).FirstOrDefault();
                var kq = result.TenLSP;
                return kq;
            }
            catch
            {
                return null;
            }
        }
        public List<LoaiSanPham> GetListLoaiSP()
        {
            try
            {
                return db.LoaiSanPhams.ToList();
            }
            catch
            {
                return new List<LoaiSanPham>();
            }
        }
        public List<SanPham> GetListSanPham()
        {
            try
            {
                return db.SanPhams.ToList();
            }
            catch
            {
                return null;
            }
        }
        public SanPham GetSanPhamById(int id)
        {
            try
            {
                return (SanPham)db.SanPhams.FirstOrDefault(l => l.MaSP == id);
            }
            catch
            {
                return null;
            }
        }
        public List<ThongSoSanPham> GetTsspById(int id)
        {
            try
            {
                var result = db.ThongSoSanPhams.Where(l => l.MaSP == id).ToList();
                return result;
            }
            catch
            {
                return new List<ThongSoSanPham>();
            }
        }
        public ThongSoKiThuat GetTsktById(int id)
        {
            try
            {
                var result = db.ThongSoKiThuats.FirstOrDefault(l => l.MaTSKT == id);
                return result;
            }
            catch
            {
                return null;
            }
        }
        public List<HoaDon> GetHoaDon(int id)
        {
            try
            {
                return db.HoaDons.Where(n => n.MaND == id).OrderByDescending(n => n.NgayLap).ToList();
            }
            catch
            {
                return null;
            }
        }
        public List<HoaDon> GetHoaDonByNgay(int id, DateTime ngay)
        {
            try
            {
                return db.HoaDons.Where(n => n.MaND == id && n.NgayLap == ngay).OrderByDescending(n => n.MaHD).ToList();
            }
            catch
            {
                return null;
            }
        }
        public List<ChiTietHoaDon> GetChiTietHoaDonByID(int id)
        {
            try
            {
                return db.ChiTietHoaDons.Where(n => n.MaHD == id).ToList();
            }
            catch
            {
                return null;
            }
        }
        public List<int> GetMaTSKT(List<int> id)
        {
            var lst = new List<int>();
            lst.Add(-1);
            foreach(var i in id)
            {
                var lstMaTSKT = (from n in db.ThongSoSanPhams where n.MaSP == i select n.MaTSKT);
                foreach(var item in lstMaTSKT)
                {
                    if (lst.Contains(item)) continue;
                    else lst.Add(item);
                }
            }
            lst.Remove(-1);
            return lst;
        }
        public String GetValueThongSo(int maSP, int maTSKT)
        {
            var result = (from n in db.ThongSoSanPhams where n.MaSP == maSP && n.MaTSKT == maTSKT select n.GiaTri).FirstOrDefault();
            return result;
        }
    }
}