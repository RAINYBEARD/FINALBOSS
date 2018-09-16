﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class PenDictionary : Dictionary<string, PenValue>
    {

        public void Add(string matcod, string fecha, string abr, string plan_id, string plan_tit)
        {
            PenValue equiv = new PenValue();
            equiv.Fecha = fecha;
            equiv.Abr = abr;
            equiv.Plan_Id = plan_id;
            equiv.Plan_Tit = plan_tit;
            this.Add(matcod, equiv);
        }
    }

    public class PenValue
    {
        public string Fecha { get; set; }
        public string Abr { get; set; }
        public string Plan_Id { get; set; }
        public string Plan_Tit { get; set; }
    }
}