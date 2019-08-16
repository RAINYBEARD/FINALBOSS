using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Arbol
{
    public class Tabla
    {
        public List<Materias> materias { get; set; }
        public int total = 0;
        public int aprobadas = 0;
    }

    public class Materias
    {
        public int materia_id { get; set; }
        public string plan_id { get; set; }
        public string mat_des { get; set; }
        public int mat_anio { get; set; }
        public int mat_cuatrim { get; set; }
        public string estado { get; set; }

        public List<Correlativas> correlativas { get; set;}

    //public string plan_tit { get; set; }
    //public int titulo_id { get; set; }
    //public string plan_id { get; set; }
    //public string mat_des { get; set; }
    //public int mat_anio { get; set; }
    //public int mat_cuatrim { get; set; }
    //public string descrip { get; set; }
    //public string abr_titulo { get; set; }
    }

    public class Correlativas
    {
        public int materia_id { get; set; }
        public string materia_des { get; set}
    }
}
