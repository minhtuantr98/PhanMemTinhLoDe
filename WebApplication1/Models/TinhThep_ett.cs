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

        public TinhThep_ett()
        {
            As = 0;
            As1 = 0;
            Am = 0;
            Ar = 0;
            Anbt = 0;
            Akbt = 0;
            Comment = string.Empty;
            ChiuNen = string.Empty;
            ChiuKeo = string.Empty;
        }
    }

    public class TinhThepCot_ett : TinhThep_ett {
        }
}