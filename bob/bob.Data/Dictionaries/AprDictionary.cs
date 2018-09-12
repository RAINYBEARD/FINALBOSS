using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class AprDictionary : Dictionary<string, AprValue>
    {

        public void Add(string matcod, string fecha, string abr, int calificacion, string profesor, string acta_id, int anio, int cuatrim)
        {

            AprValue apr = new AprValue();
            apr.Fecha = fecha;
            apr.Abr = abr;
            apr.Calificacion = calificacion;
            apr.Profesor = profesor;
            apr.Acta_Id = acta_id;
            apr.Anio = anio;
            apr.Cuatrim = cuatrim;
            this.Add(matcod, apr);
        }
    }

    public class AprValue
    {
        public string Fecha { get; set; }
        public string Abr { get; set; }
        public int Calificacion { get; set; }
        public string Profesor { get; set; }
        public string Acta_Id { get; set; }
        public int Anio { get; set; }
        public int Cuatrim { get; set; }
    }
}
