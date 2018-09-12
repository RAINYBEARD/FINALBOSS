using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class CurDictionary : Dictionary<string, CurValue>
    {

        public void Add(string matcod, string fecha, string abr, string profesor, int anio, int cuatrim, string plan_id, string plan_tit)
        {
            CurValue cur = new CurValue();
            cur.Fecha = fecha;
            cur.Abr = abr;
            cur.Profesor = profesor;
            cur.Anio = anio;
            cur.Cuatrim = cuatrim;
            cur.Plan_Id = plan_id;
            cur.Plan_Tit = plan_tit;
            this.Add(matcod, cur);
        }
    }

    public class CurValue
    {
        public string Fecha { get; set; }
        public string Abr { get; set; }
        public string Profesor { get; set; }
        public int Anio { get; set; }
        public int Cuatrim { get; set; }
        public string Plan_Id { get; set; }
        public string Plan_Tit { get; set; }
    }
}
