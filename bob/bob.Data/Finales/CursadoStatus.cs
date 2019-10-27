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
        public string materiaCod2;
        public string fechaCursada;
        public string fechaVencimiento;
        public string abr;
        public string descrip;
        public int nCorrelativas;
        public string reprobado;
        public List<CorrelativasCursadas> correlativasFuturas;
        public List<CorrelativasCursadas> correlativasCursadas;
    }
}
