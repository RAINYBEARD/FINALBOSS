using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Pendientes
{
    public class Pendientes
    {
        public string materiaCod;
        public string abr;
        public List<CorrelativasNoAprobadas> correlativasNoAprobadas;
    }

    public class CorrelativasNoAprobadas
    {
        public string materiaCod;
        public string abr;
    }
}
