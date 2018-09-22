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
        public DateTime fecha_cursada;
        public DateTime fecha_vencimiento;
        public int n_correlativas;
        public bool reprobado;
        public int n_prioridad;
    }
}
