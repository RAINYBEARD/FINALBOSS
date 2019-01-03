using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Estadisticas
{
    /// <summary>
    /// Devuelve los datos para los porcentajes de aprobado y cursado
    /// </summary>
    public class Estadisticas
    {
        public int Total { get; set; }
        public int Aprobadas { get; set; }
        public int Cursadas { get; set; }

        public List<AprobadasPorAnio> Lista { get; set; }
    }
}