using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Finales
{
    public class CursadoStatus
    {
        public string materiaCod;
        public string fechaCursada;
        public string fechaVencimiento;
        public bool porVencerse;
        public string abr;
        public string descrip;
        public int nCorrelativas;
        public string reprobado;
        public List<CorrelativasCursadas> correlativasCursadas;
    }

    public class CorrelativasCursadas
    {
        public string materiaCod;
        public string abr;
    }
}
