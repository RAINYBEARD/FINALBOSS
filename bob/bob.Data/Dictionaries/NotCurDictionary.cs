using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class NotCurDictionary : Dictionary<string, NotCurValue>
    {

        public void Add(string matcod, string abr, int anio, int cuatrim, string plan_id, string plan_tit)
        {
            NotCurValue notcur = new NotCurValue();
            notcur.Abr = abr;
            notcur.Anio = anio;
            notcur.Cuatrim = cuatrim;
            notcur.Plan_Id = plan_id;
            notcur.Plan_Tit = plan_tit;
            this.Add(matcod, notcur);
        }
    }

    public class NotCurValue
    {
        public string Fecha { get; set; }
        public string Abr { get; set; }
        public int Anio { get; set; }
        public int Cuatrim { get; set; }
        public string Plan_Id { get; set; }
        public string Plan_Tit { get; set; }
    }
}
