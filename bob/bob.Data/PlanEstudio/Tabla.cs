using System.Collections.Generic;

namespace bob.Data.PlanEstudio
{
    public class Tabla
    {
        public Tabla(List<Materias> materias = null, int total = 0, int aprobadas= 0)
        {
            this.materias = materias;
            this.total = total;
            this.aprobadas = aprobadas;
        }
        public List<Materias> materias { get; set; }
        public int total = 0;
        public int aprobadas = 0;
    }
}
