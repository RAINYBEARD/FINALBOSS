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
        public int Aprobadas { get; set; }
        public int Total { get; set; }
        public decimal Porcentaje_Aprobado { get; set; }
        public decimal Porcentaje_Faltante { get; set; }
    }
}
