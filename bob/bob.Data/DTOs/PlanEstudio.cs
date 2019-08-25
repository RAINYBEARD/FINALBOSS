using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.DTOs
{
    /// <summary>
    /// Maps from PlanDeEstudio JSON 
    /// </summary>
    public class PlanEstudio
    {
        public int materia_id { get; set; }
        public string mat_des { get; set; }
        public string plan_tit { get; set; }
        public int titulo_id { get; set; }
        public string plan_id { get; set; }
        public short mat_anio { get; set; }
        public short mat_cuatrim { get; set; }
        public float mat_modulos { get; set; }
        public string pcursar { get; set; }
        public string paprobar { get; set; }
        public int codigo_correlativa { get; set; }
        public string abr_titulo { get; set; }

        public string descripcion_correlativa { get; set; }
    }
}