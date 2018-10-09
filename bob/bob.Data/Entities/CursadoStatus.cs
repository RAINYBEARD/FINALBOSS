using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Entities
{
    public class CursadoStatus
    {
        public string materia_cod;
        public string fecha_cursada;
        public string fecha_vencimiento;
        public string abr;
        public int n_correlativas;
        public bool reprobado;
        public List<CorrelativasCursadas> correlativascursadas;
    }

    public class CorrelativasCursadas
    {
        public string materia_cod;
        public string abr;
    }
}
