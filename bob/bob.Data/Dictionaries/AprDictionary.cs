using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class AprDictionary : Dictionary<string, AprValue>
    {

    }

    public class AprValue
    {
        public string Fecha { get; set; }
        public string Abr { get; set; }
        public string Descrip { get; set; }
        public int Calificacion { get; set; }
        public string Profesor { get; set; }
        public string Acta_Id { get; set; }
        public int Anio { get; set; }
        public int Cuatrim { get; set; }
        
    }
}
