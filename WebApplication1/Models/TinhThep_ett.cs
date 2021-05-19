using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TinhThep_ett
    {
        public double As { get; set; }
        public double As1 { get; set; }
        public double Am { get; set; }
        public double Ar { get; set; }
        public double Anbt { get; set; }
        public double Akbt { get; set; }
        public string Comment { get; set; }
        public string ChiuNen { get; set; }
        public string ChiuKeo { get; set; }
        public string CuongDo { get; set; }
        public double Qnc { get; set; }
        public string SSQnc { get; set; }
        public double Q { get; set; }
        public double Ad { get; set; }
        public double Stt { get; set; }
        public double Sct { get; set; }
        public double Smax { get; set; }
        public double S { get; set; }
        public string SSAmAr { get; set; }

        public TinhThep_ett()
        {
            As = 0;
            As1 = 0;
            Am = 0;
            Ar = 0;
            Anbt = 0;
            Akbt = 0;
            Qnc = 0;
            Q = 0;
            Ad = 0;
            Stt = 0;
            Smax = 0;
            S = 0;
            Comment = string.Empty;
            ChiuNen = string.Empty;
            ChiuKeo = string.Empty;
            CuongDo = string.Empty;
            SSQnc = string.Empty;
            SSAmAr = string.Empty;
        }
    }

    public class TinhThepCot_ett : TinhThep_ett {
        public double Lo_ih { get; set; }
        public double Lo_ib { get; set; }
        public double MuyH { get; set; }
        public double MuyB { get; set; }
        public double MH { get; set; }
        public double MB { get; set; }
        public string TinhTheoPhuong { get; set; }
        public double Mx { get; set; }
        public double E1 { get; set; }
        public double Ea { get; set; }
        public double E0 { get; set; }
        public double E { get; set; }
        public double Epxilon { get; set; }
        public string SSEpxilon { get; set; }
        public double X1 { get; set; }
        public string SSX1 { get; set; }
        public double XR { get; set; }
        public double Muy { get; set; }
        public string MuyCmt { get; set; }
        public string SSMuyCmt { get; set; }
        public string SSABT { get; set; }

        public TinhThepCot_ett()
        {
            Lo_ih = 0;
            Lo_ib = 0;
            MuyH = 0;
            MuyB = 0;
            MH = 0;
            MB = 0;
            TinhTheoPhuong = string.Empty;
            Mx = 0;
            E1 = 0;
            Ea = 0;
            E0 = 0;
            E = 0;
            Epxilon = 0;
            SSEpxilon = string.Empty;
            X1 = 0;
            SSX1 = string.Empty;
            XR = 0;
            Muy = 0;
            MuyCmt = string.Empty;
            SSMuyCmt = string.Empty;
            SSABT = string.Empty;
        }

    }
}