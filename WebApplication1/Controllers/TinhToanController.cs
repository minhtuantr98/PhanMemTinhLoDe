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
            var listVatLieu = db.tbl_VatLieus.Where(o => o.Type.Equals(Constants.ThepDai_Type));
            return JsonConvert.SerializeObject(listVatLieu);
        }

        [HttpPost]
        public string GetDetailVatLieu(Guid id)
        {
            var obj = db.tbl_VatLieus.Where(o => o.ID.Equals(id));
            return JsonConvert.SerializeObject(obj);
        }

        [HttpPost]
        public string TinhThepDamCN()
        {
            Result_ett<TinhThep_ett> rs = new Result_ett<TinhThep_ett>();
            TinhThep_ett tinhThep_Ett = new TinhThep_ett();

            string Rs_str = Request["Rs"];
            string Rb_str = Request["Rb"];
            string h_str = Request["h"];
            string a_str = Request["a"];
            string b_str = Request["b"];
            //string bf_str = Request["bf"];
            string M_str = Request["M"];
            //string C_str = Request["C"]; //???
            string Rsc_str = Request["Rsc"];

            string Un_str = Request["Un"];
            string fi1n_str = Request["fi1n"];
            string Vn_str = Request["Vn"];
            string fi2n_str = Request["fi2n"];
            string Uk_str = Request["Uk"];
            string fi1k_str = Request["fi1k"];
            string Vk_str = Request["Vk"];
            string fi2k_str = Request["fi2k"];

            double Un = float.Parse(Un_str);
            double fi1n = float.Parse(fi1n_str);
            double Vn = float.Parse(Vn_str);
            double fi2n = float.Parse(fi2n_str);



            double Uk = float.Parse(Uk_str);
            double fi1k = float.Parse(fi1k_str);
            double Vk = float.Parse(Vk_str);
            double fi2k = float.Parse(fi2k_str);



            int Rs = int.Parse(Rs_str);
            double Rb = float.Parse(Rb_str);
            int h = int.Parse(h_str);
            int a = int.Parse(a_str);
            int b = int.Parse(b_str);
            //int bf = int.Parse(bf_str);
            double M = double.Parse(M_str) * Math.Pow(10,6);
            //int C = int.Parse(C_str);
            int C = 2500;
            int Rsc = int.Parse(Rsc_str);

            int Osc = (Rs <= 400) ? Rs : 400;
            double w = 0.85 - 0.008 * Rb;
            int h0 = h - a;
            double Cr = w / (1 + (Rs / Osc) * (1 - (w / 1.1)));
            double Ar = Cr * (1 - 0.5 * Cr);
            double Am = M / (Rb * b * h0);

            double As = 0;
            double As1 = 0;

            tinhThep_Ett.Am = Am;
            tinhThep_Ett.Ar = Ar;

            if (Am <= Ar)
            {
                As = M / (double)(Rs * C * h0);
                As1 = 0;
                tinhThep_Ett.Comment = "Chỉ cần đặt cốt đơn";
                //rs.Data.Comment = "Chỉ cần đặt cốt đơn";
                
            }
            else
            {
                if (Am <= 0.5)
                {
                    As1 = (M - Ar * Rb * b * h0 * h0) / (Rsc * (h0 - a));
                    As = ((Cr * Rb * b * h0) / Rs) + ((Rsc / Rs) * As1);
                    tinhThep_Ett.Comment = "Cần đặt cốt kép";
                }
                else
                {
                    tinhThep_Ett.Comment = "Tiết diện không hợp lý.";
                }
            }

            rs.ErrCode = EnumErrCode.Success;
            tinhThep_Ett.As = As;
            tinhThep_Ett.As1 = As1;
            tinhThep_Ett.Anbt = Un * 3.14 * Math.Pow((fi1n / 2), 2) + Vn * 3.14 * Math.Pow((fi2n / 2), 2);
            tinhThep_Ett.Akbt = Uk * 3.14 * Math.Pow((fi1k / 2), 2) + Vk * 3.14 * Math.Pow((fi2k / 2), 2);
            if ((fi1n <= b / 10) && (fi2n <= b / 10) && (fi1n - fi2n <= 6))
            {
                tinhThep_Ett.ChiuNen = "Bố trí thép chịu nén hợp lý";
            }
            else
            {
                tinhThep_Ett.ChiuNen = "Bố trí thép chịu nén chưa hợp lý";
            }
            if ((fi1k <= b / 10) && (fi2k <= b / 10) && (fi1k- fi2k <= 6))
            {
                tinhThep_Ett.ChiuKeo = "Bố trí thép chịu kéo hợp lý";
            }
            else
            {
                tinhThep_Ett.ChiuKeo = "Bố trí thép chịu kéo chưa hợp lý";
            }
            rs.Data = tinhThep_Ett;
            return JsonConvert.SerializeObject(rs);
        }

        //[HttpPost]
        //public string TinhThepDamCT()
        //{
        //    string Rs_str = Request["Rs"];
        //    string Rb_str = Request["Rb"];
        //    string h_str = Request["h"];
        //    string a_str = Request["a"];
        //    string b_str = Request["b"];
        //    string bf_str = Request["beRongCanh"];
        //    string M_str = Request["thongSoNoiNuc5"];
        //    //string C_str = Request["C"]; //???
        //    string Rsc_str = Request["Rsc"];

        //    int Rs = int.Parse(Rs_str);
        //    double Rb = float.Parse(Rb_str);
        //    int h = int.Parse(h_str);
        //    int a = int.Parse(a_str);
        //    int b = int.Parse(b_str);
        //    int bs = b - 60;
        //    int bf = int.Parse(bf_str);
        //    int M = int.Parse(M_str);
        //    int C = 2500;
        //    int Rsc = int.Parse(Rsc_str);
        //    int Cs = 1; // để mặc định bằng 1.

        //    int Osc = (Rs <= 400) ? Rs : 400;
        //    double w = 0.85 - 0.008 * Rb;
        //    int h0 = h - a;
        //    double Cr = w / (1 + (Rs / Osc) * (1 - (w / 1.1)));
        //    double Ar = Cr * (1 - 0.5 * Cr);
        //    double Am = 0;
        //    double Mf = Rb * b * bf * (h0 - 0.5 * bf);
        //    double As = 0;
        //    double As1 = 0;
        //    int hf1 = 120;
        //    int Sc = 0;

        //    if (M <= Mf)
        //    {
        //        Am = M / (Rb * b * h0);
        //        if (Am <= Ar)
        //        {
        //            As = M / (Rs * C * h0);
        //        }
        //        else
        //        {
        //            if (Am <= 0.5)
        //            {
        //                As1 = (M - Ar * Rb * b * h0 * h0) / (Rsc * (h0 - a));
        //                As = ((Cr * Rb * b * h0) / Rs) + ((Rsc / Rs) * As1);
        //            }
        //            else
        //            {
        //                return "Tiết diện không hợp lý.";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Am = (M - Rb * (b - bs) * bf * (h0 - 0.5 * bf)) / (Rb * b * h0 * h0);
        //        if (Am <= Ar)
        //        {
        //            As = (Rb / Rs) * (Cs * bs * h0 + (b - bs) * bf);
        //        }
        //        else
        //        {
        //            if (Am <= 0.5)
        //            {
        //                As1 = ((M - Ar * Rb * b * h0 * h0) - Rb * (bf1 - b) * hf1 * (h0 - 0.5 * hf1)) / (Rsc * (h0 - a1));
        //                As = ((Cr * Rb * b * h0) / Rs) + (Rsc / Rs) * As1;
        //            }
        //        }

        //    }

        //    return string.Empty;
        //}
    }
}