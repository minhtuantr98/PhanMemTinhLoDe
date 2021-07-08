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

        public ActionResult TinhDamCN()
        {
            return View();
        }

        public ActionResult TinhDamCT()
        {
            return View();
        }

        public ActionResult TinhCotBTCT()
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

            #region lấy dữ liệu từ tham số truyền vào
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
            string Rbt_str = Request["Rbt"];
            string Q_str = Request["Q"];
            string Rsw_str = Request["Rsw"];
            string n_str = Request["n"];
            string fi3_str = Request["fi3"];
            #endregion

            #region phần để chuyển dữ liệu từ dạng chuỗi string sang dạng số thực double
            double Un = float.Parse(Un_str);
            double fi1n = float.Parse(fi1n_str);
            double Vn = float.Parse(Vn_str);
            double fi2n = float.Parse(fi2n_str);



            double Uk = float.Parse(Uk_str);
            double fi1k = float.Parse(fi1k_str);
            double Vk = float.Parse(Vk_str);
            double fi2k = float.Parse(fi2k_str);
            double Rbt = float.Parse(Rbt_str);
            double Q = float.Parse(Q_str);
            double Rsw = float.Parse(Rsw_str);
            double n = float.Parse(n_str);
            double fi3 = float.Parse(fi3_str);



            int Rs = int.Parse(Rs_str);
            double Rb = float.Parse(Rb_str);
            int h = int.Parse(h_str);
            int a = int.Parse(a_str);
            int b = int.Parse(b_str);
            //int bf = int.Parse(bf_str);
            double M = double.Parse(M_str) * Math.Pow(10, 6);
            //int C = int.Parse(C_str);
            //int C = 2500;
            double C = 0;
            int Rsc = int.Parse(Rsc_str);

            int Osc = (Rs <= 400) ? Rs : 400;
            double w = 0.85 - 0.008 * Rb;
            int h0 = h - a;
            double Cr = w / (1 + (Rs / Osc) * (1 - (w / 1.1)));
            double Ar = Cr * (1 - 0.5 * Cr);
            double Am = (M / (Rb * b * h0)) / 100;
            C = 1 - Math.Sqrt(1 - 2 * Am);
            double As = 0;
            double As1 = 0;
            #endregion


            #region phần tính toán theo thuật toán + công thức
            tinhThep_Ett.Am = Am;
            tinhThep_Ett.Ar = Ar;

            if (Am <= Ar)
            {
                As = M / (double)(Rs * C * h0);
                As1 = 0;
                tinhThep_Ett.Comment = "Chỉ cần đặt cốt đơn";
                //rs.Data.Comment = "Chỉ cần đặt cốt đơn";
                tinhThep_Ett.SSAmAr = "≤";
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
                tinhThep_Ett.SSAmAr = ">";
            }

            rs.ErrCode = EnumErrCode.Success;
            tinhThep_Ett.As = As / 100;
            tinhThep_Ett.As1 = As1;
            tinhThep_Ett.Anbt = (Un * 3.14 * Math.Pow((fi1n / 2), 2) + Vn * 3.14 * Math.Pow((fi2n / 2), 2)) / 100;
            tinhThep_Ett.Akbt = (Uk * 3.14 * Math.Pow((fi1k / 2), 2) + Vk * 3.14 * Math.Pow((fi2k / 2), 2)) / 100;
            if ((fi1n <= b / 10) && (fi2n <= b / 10) && (fi1n - fi2n <= 6))
            {
                tinhThep_Ett.ChiuNen = "Bố trí thép chịu nén hợp lý";
            }
            else
            {
                tinhThep_Ett.ChiuNen = "Bố trí thép chịu nén chưa hợp lý";
            }
            if ((fi1k <= b / 10) && (fi2k <= b / 10) && (fi1k - fi2k <= 6))
            {
                tinhThep_Ett.ChiuKeo = "Bố trí thép chịu kéo hợp lý";
            }
            else
            {
                tinhThep_Ett.ChiuKeo = "Bố trí thép chịu kéo chưa hợp lý";
            }

            //Tính cốt đai
            double Sw = (Rbt * b * Math.Pow(h, 2)) / (Q * Math.Pow(10, 4));
            double qsw = (Rsw * (n * Math.PI * Math.Pow((fi3 / 2), 2) / 4)) / Sw;
            double Qsw = 0.75 * qsw * 2 * h0;
            double Qb = 0;
            if (qsw > 0.25 * Rbt * b)
            {
                Qb = (1.5 * Rbt * b * Math.Pow(h0, 2)) / (2 * h0);
            }
            else
            {
                Qb = (4 * 1.5 * Math.Pow(h0, 2) * qsw) / (2 * h0);
            }

            double Qnc = Qb + Qsw;
            tinhThep_Ett.Qnc = Qnc / 100;
            tinhThep_Ett.Q = Q;
            if (Qnc >= Q)
            {
                tinhThep_Ett.CuongDo = "dầm đủ khả năng chịu ứng suất nén chính";
                tinhThep_Ett.SSQnc = "≥";
            }
            else
            {
                tinhThep_Ett.CuongDo = "dầm không đủ khả năng chịu ứng suất nén chính";
                tinhThep_Ett.SSQnc = "<";
            }

            double Stt = (Rsw * (n * Math.PI * Math.Pow(fi3, 2)) / 4) * qsw / 100;
            tinhThep_Ett.Stt = Stt;

            double Sct = (h0 / 2) < 300 ? (h0 / 2) : 300;
            tinhThep_Ett.Sct = Sct;

            double Smax = (Rbt * b * h0) / Q / 100;
            tinhThep_Ett.Smax = Smax;

            double s = Stt < Sct ? Stt : (Sct < Smax ? Sct : Smax);
            tinhThep_Ett.S = s;

            double Ad = (n * Math.PI * Math.Pow(fi3, 2)) / 4 / 100;
            tinhThep_Ett.Ad = Ad;
            #endregion
            #region trả dữ liệu cho hàm ajax để trình bày lên view
            rs.Data = tinhThep_Ett;
            return JsonConvert.SerializeObject(rs);
            #endregion
        }

        [HttpPost]
        public string TinhThepDamCT()
        {
            Result_ett<TinhThep_ett> rs = new Result_ett<TinhThep_ett>();
            TinhThep_ett tinhThep_Ett = new TinhThep_ett();

            string Rs_str = Request["Rs"];
            string Rb_str = Request["Rb"];
            string h_str = Request["h"];
            string a_str = Request["a"];
            string b_str = Request["b"];
            string bf_str = Request["bf"];
            string hf_str = Request["hf"];
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
            string Rbt_str = Request["Rbt"];
            string Q_str = Request["Q"];
            string Rsw_str = Request["Rsw"];
            string n_str = Request["n"];
            string fi3_str = Request["fi3"];

            double Un = float.Parse(Un_str);
            double fi1n = float.Parse(fi1n_str);
            double Vn = float.Parse(Vn_str);
            double fi2n = float.Parse(fi2n_str);



            double Uk = float.Parse(Uk_str);
            double fi1k = float.Parse(fi1k_str);
            double Vk = float.Parse(Vk_str);
            double fi2k = float.Parse(fi2k_str);
            double Rbt = float.Parse(Rbt_str);
            double Q = float.Parse(Q_str);
            double Rsw = float.Parse(Rsw_str);
            double n = float.Parse(n_str);
            double fi3 = float.Parse(fi3_str);



            int Rs = int.Parse(Rs_str);
            double Rb = float.Parse(Rb_str);
            int h = int.Parse(h_str);
            int a = int.Parse(a_str);
            int b = int.Parse(b_str);
            //int bf = int.Parse(bf_str);
            double M = double.Parse(M_str) * Math.Pow(10, 7);
            //int C = int.Parse(C_str);
            double C = 0;
            int Rsc = int.Parse(Rsc_str);

            int Osc = (Rs <= 400) ? Rs : 400;
            double w = 0.85 - 0.008 * Rb;
            int h0 = h - a;
            double Cr = w / (1 + (Rs / Osc) * (1 - (w / 1.1)));
            double Ar = Cr * (1 - 0.5 * Cr);

            double As = 0;
            double As1 = 0;


            int bs = b - 60;
            int bf = int.Parse(bf_str);
            int hf = int.Parse(hf_str);
            int Cs = 1; // để mặc định bằng 1.

            double Am = 0;
            double Mf = Rb * bf * hf * (h0 - 0.5 * hf);
            int Sc = 0;

            if (M <= Mf)
            {
                Am = M / (Rb * bf * h0);
                C = 1 - Math.Sqrt(1 - 2 * Am);
                if (Am <= Ar)
                {
                    As = M / (Rs * C * h0);
                    tinhThep_Ett.SSAmAr = "≤";
                    tinhThep_Ett.Comment = "Chỉ cần đặt cốt đơn";
                }
                else
                {
                    if (Am <= 0.5)
                    {
                        As1 = (M - Ar * Rb * bf * h0 * h0) / (Rsc * (h0 - a));
                        As = ((Cr * Rb * bf * h0) / Rs) + ((Rsc / Rs) * As1);
                        tinhThep_Ett.Comment = "Cần đặt cốt kép";
                    }
                    else
                    {
                        tinhThep_Ett.Comment = "Tiết diện không hợp lý.";
                    }
                    tinhThep_Ett.SSAmAr = ">";
                }
            }
            else
            {
                Am = (M - Rb * (bf - b) * hf * (h0 - 0.5 * hf)) / (Rb * b * h0 * h0);
                if (Am <= Ar)
                {
                    As = (Rb / Rs) * (Cs * bs * h0 + (b - bs) * bf);
                    tinhThep_Ett.SSAmAr = "≤";
                    tinhThep_Ett.Comment = "Chỉ cần đặt cốt đơn";
                }
                else
                {
                    if (Am <= 0.5)
                    {
                        double a1 = hf / 2;
                        As1 = ((M - Ar * Rb * bf * h0 * h0) - Rb * (bf - b) * hf * (h0 - 0.5 * hf)) / (Rsc * (h0 - a1)) / 100;
                        As = ((Cr * Rb * bf * h0) / Rs) + (Rsc / Rs) * As1;
                        tinhThep_Ett.Comment = "Cần đặt cốt kép";
                    }
                    else
                    {
                        tinhThep_Ett.Comment = "Tiết diện không hợp lý.";
                    }
                    tinhThep_Ett.SSAmAr = ">";
                }

            }

            tinhThep_Ett.Am = Am;
            tinhThep_Ett.As = As / 100;
            tinhThep_Ett.Ar = Ar;
            tinhThep_Ett.As1 = As1;
            tinhThep_Ett.Anbt = (Un * 3.14 * Math.Pow((fi1n / 2), 2) + Vn * 3.14 * Math.Pow((fi2n / 2), 2)) / 100;
            tinhThep_Ett.Akbt = (Uk * 3.14 * Math.Pow((fi1k / 2), 2) + Vk * 3.14 * Math.Pow((fi2k / 2), 2)) / 100;
            if ((fi1n <= b / 10) && (fi2n <= b / 10) && (fi1n - fi2n <= 6))
            {
                tinhThep_Ett.ChiuNen = "Bố trí thép chịu nén hợp lý";
            }
            else
            {
                tinhThep_Ett.ChiuNen = "Bố trí thép chịu nén chưa hợp lý";
            }
            if ((fi1k <= b / 10) && (fi2k <= b / 10) && (fi1k - fi2k <= 6))
            {
                tinhThep_Ett.ChiuKeo = "Bố trí thép chịu kéo hợp lý";
            }
            else
            {
                tinhThep_Ett.ChiuKeo = "Bố trí thép chịu kéo chưa hợp lý";
            }

            //Tính cốt đai
            double Sw = (Rbt * b * Math.Pow(h, 2)) / (Q * Math.Pow(10, 4));
            double qsw = (Rsw * (n * Math.PI * Math.Pow((fi3 / 2), 2) / 4)) / Sw;
            double Qsw = 0.75 * qsw * 2 * h0;
            double Qb = 0;
            if (qsw > 0.25 * Rbt * b)
            {
                Qb = (1.5 * Rbt * b * Math.Pow(h0, 2)) / (2 * h0);
            }
            else
            {
                Qb = (4 * 1.5 * Math.Pow(h0, 2) * qsw) / (2 * h0);
            }

            double Qnc = Qb + Qsw;
            tinhThep_Ett.Qnc = Qnc / 100;
            tinhThep_Ett.Q = Q;
            if (Qnc >= Q)
            {
                tinhThep_Ett.CuongDo = "dầm đủ khả năng chịu ứng suất nén chính";
                tinhThep_Ett.SSQnc = "≥";
            }
            else
            {
                tinhThep_Ett.CuongDo = "dầm không đủ khả năng chịu ứng suất nén chính";
                tinhThep_Ett.SSQnc = "<";
            }

            double Stt = (Rsw * (n * Math.PI * Math.Pow(fi3, 2)) / 4) * qsw / 100;
            tinhThep_Ett.Stt = Stt;

            double Sct = (h0 / 2) < 300 ? (h0 / 2) : 300;
            tinhThep_Ett.Sct = Sct;

            double Smax = (Rbt * b * h0) / Q / 100;
            tinhThep_Ett.Smax = Smax;

            double s = Stt < Sct ? Stt : (Sct < Smax ? Sct : Smax);
            tinhThep_Ett.S = s;

            double Ad = (n * Math.PI * Math.Pow(fi3, 2)) / 4 / 100;
            tinhThep_Ett.Ad = Ad;

            rs.ErrCode = EnumErrCode.Success;
            rs.Data = tinhThep_Ett;
            return JsonConvert.SerializeObject(rs);
        }


        [HttpPost]
        public string TinhThepCot()
        {
            Result_ett<TinhThepCot_ett> rs = new Result_ett<TinhThepCot_ett>();
            TinhThepCot_ett tinhThep_Ett = new TinhThepCot_ett();
            int typeOfCal = 0;

            string l_str = Request["l"];
            string b_str = Request["b"];
            string wuy_str = Request["wuy"];
            string mx_str = Request["mx"];
            string my_str = Request["my"];
            string h_str = Request["h"];
            string e0_str = Request["e0"];
            string a_str = Request["a"];
            string N_str = Request["N"];
            string Rb1_str = Request["Rb1"];
            string Rs_str = Request["Rs"];
            string dk1_str = Request["dk1"];
            string dk2_str = Request["dk2"];
            string sl1_str = Request["sl1"];
            string sl2_str = Request["sl2"];

            double l = float.Parse(l_str);
            double b = float.Parse(b_str);
            double wuy = float.Parse(wuy_str);
            double mx = float.Parse(mx_str);
            double my = float.Parse(my_str);
            double h = float.Parse(h_str);
            double a = float.Parse(a_str);
            double e0 = double.Parse(e0_str);
            double N = double.Parse(N_str);
            double dk1 = double.Parse(dk1_str);
            double dk2 = double.Parse(dk2_str);
            double sl1 = double.Parse(sl1_str);
            double sl2 = double.Parse(sl2_str);
            double h0 = h - a;

            double l0 = wuy * l;
            double lambda = l0 / 0.288 * b;
            double n = 0;

            double Rb1 = double.Parse(Rb1_str);
            double Rs = double.Parse(Rs_str);
            double w = 0.85 - 0.008 * Rb1;
            double Cr = w / (1 + (Rs / 500) * (1 - (w / 1.1)));
            double Ncr = 0;


            if (lambda < 28)
            {
                n = 1;
                tinhThep_Ett.Comment = "Bỏ qua ảnh hưởng của uốn dọc";

            }
            else
            {
                Ncr = ((1472 * Math.Pow(10, 3)) / Math.Pow(l0, 2)) * (((0.11 / 0.1 + e0 / h) * ((b * Math.Pow(h, 3)) / 12)) / 1.6) + Math.Pow(9.13 * 0.00047 * b * h0 * (0.5 * h - a), 2);
                n = 1 / (1 - ((N * Math.Pow(10, 4)) / Ncr));
                rs.ErrCode = EnumErrCode.Empty;
                tinhThep_Ett.Comment = "Phải kể đến ảnh hưởng của uốn dọc";
            }
            double mx1 = n * mx;
            double my1 = n * my;

            double cx = b;
            double cy = h;

            double m1 = 0;
            double m2 = 0;

            if ((mx1 / cx) < (my1 / cy))
            {
                m1 = mx1;
                m2 = my1;
            }
            else
            {
                m1 = my1;
                m2 = mx1;
            }


            if ((e0 / h0) < 0.3)
            {
                tinhThep_Ett.Comment = "Tính toán theo cấu kiện chịu nén đúng tâm";
                typeOfCal = 1;
            }
            else
            {

                if (((N * Math.Pow(10, 4)) / (Rb1 * b)) > (Cr * h0))
                {
                    tinhThep_Ett.Comment = "Tính toán theo cấu kiện chịu lệch tâm bé";
                    typeOfCal = 2;
                }
                else
                {
                    tinhThep_Ett.Comment = "Tính toán theo cấu kiện chịu lệch tâm lớn";
                    typeOfCal = 3;
                }
            }
            rs.ErrCode = EnumErrCode.Success;
            //}
            //else
            //{
            //    rs.ErrCode = EnumErrCode.Empty;
            //    tinhThep_Ett.Comment = "Bỏ qua ảnh hưởng của uốn dọc";
            //}

            double Lo_ih = (l0 * Math.Pow(10, 3)) / (0.288 * h);
            tinhThep_Ett.Lo_ih = Lo_ih;

            double Lo_ib = (l0 * Math.Pow(10, 3)) / (0.288 * b);
            tinhThep_Ett.Lo_ib = Lo_ib;

            //if (Lo_ih > 28)
            //{
            //    tinhThep_Ett.
            //}

            //if (Lo_ib > 28)
            //{
            //    tinhThep_Ett.
            //}

            tinhThep_Ett.MuyH = 1 / (1 - (N / Ncr));
            tinhThep_Ett.MuyB = 1 / (1 - (N / Ncr));

            double mh = tinhThep_Ett.MuyH * my;
            tinhThep_Ett.MH = mh;

            double mb = tinhThep_Ett.MuyB * mx;
            tinhThep_Ett.MB = mb;
            tinhThep_Ett.MHH = mh / h;
            tinhThep_Ett.MBB = mb / b;

            if (mh / h <= mb / b)
            {
                tinhThep_Ett.TinhTheoPhuong = "Cạnh B";
            }
            else
            {
                tinhThep_Ett.TinhTheoPhuong = "Cạnh H";
            }

            tinhThep_Ett.Mx = mx;

            double e1 = (mx * Math.Pow(10, 4)) / (N * Math.Pow(10, 4));
            tinhThep_Ett.E1 = e1;

            double ea = (l * 1000 / 600) < (h / 30) ? (h / 30) : (l * 1000 / 600);
            tinhThep_Ett.Ea = ea;

            tinhThep_Ett.E0 = tinhThep_Ett.E1 < tinhThep_Ett.Ea ? tinhThep_Ett.Ea : tinhThep_Ett.E1;

            double e = 0.5 * h - a + n * tinhThep_Ett.E0;
            tinhThep_Ett.E = e;

            double epxilon = tinhThep_Ett.E0 / h0;
            tinhThep_Ett.Epxilon = epxilon;
            if (epxilon <= 0.3)
            {
                tinhThep_Ett.SSEpxilon = "≤ 0.3";
            }
            else
            {
                tinhThep_Ett.SSEpxilon = "> 0.3";
            }

            double x1 = N * Math.Pow(10, 4) / Rb1;
            tinhThep_Ett.X1 = x1 / Math.Pow(10, 3);

            double XR = Cr * h0;
            tinhThep_Ett.XR = XR;
            tinhThep_Ett.SSX1 = x1 > (XR) ? ">" : (x1 < (XR) ? "<" : "=");
            double k = 0.4;

            switch (typeOfCal)
            {
                case 1:
                    double fi = lambda <= 14 ? 1 : 1.028 - 0.0000288 * Math.Pow(lambda, 2) - 0.0016 * lambda;
                    double fie = fi + ((1 - fi) * tinhThep_Ett.Epxilon) / 0.3;
                    double gamaE = 1 / ((0.5 - epxilon) * (2 + epxilon));
                    double Ast = (((gamaE * N * Math.Pow(10, 4)) / fie - Rb1 * b * h) / (Rs - Rb1)) * Math.Pow(10, -4);
                    tinhThep_Ett.As = Ast;
                    break;
                case 2:
                    double x = (Cr + (1 - Cr) / (1 + 50 * epxilon)) * h0;
                    tinhThep_Ett.As = (N * Math.Pow(10, 4) * e - Rb1 * b * x * (h0 - x / 2)) / (k * Rs * (h0 - a));
                    break;
                case 3:
                    double x11 = (N * Math.Pow(10, 4)) / (Rb1 * b);
                    double e11 = e0 + h / 2 - a;
                    tinhThep_Ett.As = ((N * Math.Pow(10, 4)) * (e + 0.5 * x11 - h0)) / (k * Rs * (h0 - a));
                    break;
                default:
                    break;
            }
            double anbt = sl1 * Math.PI * Math.Pow(dk1 / 2, 2) + sl2 * Math.PI * Math.Pow(dk2 / 2, 2);
            tinhThep_Ett.Anbt = anbt / Math.Pow(10, 2);

            if (tinhThep_Ett.Anbt >= tinhThep_Ett.As)
            {
                tinhThep_Ett.SSABT = "≥";
            }
            else
            {
                tinhThep_Ett.SSABT = "<";
            }

            tinhThep_Ett.Muy = (tinhThep_Ett.Anbt * Math.Pow(10, 6) / (b * h0 ))/100;
            if (tinhThep_Ett.Muy >= 0.05 && tinhThep_Ett.Muy <= 0.4)
            {
                tinhThep_Ett.MuyCmt = "Bố trí thép hợp lí";
            }
            else
            {
                tinhThep_Ett.MuyCmt = "Bố trí thép không hợp lí";
            }

            if (tinhThep_Ett.Muy <= 4)
            {
                tinhThep_Ett.SSMuyCmt = "≤";
            }
            else
            {
                tinhThep_Ett.SSMuyCmt = ">";
            }

            rs.Data = tinhThep_Ett;
            return JsonConvert.SerializeObject(rs);
        }
    }
}