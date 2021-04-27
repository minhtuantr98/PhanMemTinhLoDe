using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Shareds;
using System.Web.Security;

namespace WebApplication1.Controllers
{
    public class TinhToanController : Controller
    {
        private DatabaseDataContext db;

        public TinhToanController()
        {
            db = new DatabaseDataContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetListBeTong()
        {
            var listVatLieu = db.tbl_VatLieus.Where(o => o.Type.Equals(Constants.BeTong_Type));
            return JsonConvert.SerializeObject(listVatLieu);
        }

        [HttpPost]
        public string GetListThepDoc()
        {
            var listVatLieu = db.tbl_VatLieus.Where(o => o.Type.Equals(Constants.ThepDoc_Type));
            return JsonConvert.SerializeObject(listVatLieu);
        }

        [HttpPost]
        public string GetListThepDai()
        {
            var listVatLieu = db.tbl_VatLieus.Where(o => o.Type.Equals(Constants.ThepDoc_Type));
            return JsonConvert.SerializeObject(listVatLieu);
        }

        [HttpPost]
        public string GetDetailVatLieu(Guid id)
        {
            var obj = db.tbl_VatLieus.Where(o => o.ID.Equals(id));
            return JsonConvert.SerializeObject(obj);
        }

        [HttpPost]
        public string TinhThep()
        {
            string Rs_str = Request["Rs"];
            string Rb_str = Request["Rb"];
            string h_str = Request["h"];
            string a_str = Request["a"];
            string b_str = Request["b"];
            string bf_str = Request["bf"];

            int Rs = int.Parse(Rs_str);
            double Rb = float.Parse(Rb_str);
            int h = int.Parse(h_str);
            int a = int.Parse(a_str);
            int b = int.Parse(b_str);
            int bf = int.Parse(bf_str);

            int Osc = (Rs <= 400) ? Rs : 400;
            double w = 0.85 - 0.008 * Rb;
            int h0 = h - a;
            double Cr = w / (1 + (Rs / Osc) * (1 - (w / 1.1)));
            double Ar = Cr * (1 - 0.5 * Cr);
            double Mf = Rb * b * bf * (h0 - 0.5 * bf);


            return string.Empty;

        }
    }
}