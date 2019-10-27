using System.Collections.Generic;

namespace bob.Data.PlanEstudio
{
    public class Materias
    {
        public int materia_id { get; set; }
        public string plan_id { get; set; }
        public string mat_des { get; set; }
        public int mat_anio { get; set; }
        public string mat_cuatrim { get; set; }
        public string estado { get; set; }

        public List<Correlativas> correlativas { get; set; }
    }
}
