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
        public Estadisticas(int total = 0, int aprobadas = 0, int cursadas = 0, List<AprobadasPorAnio> lista = null)
        {
            Total = total;
            Aprobadas = aprobadas;
            Cursadas = cursadas;
            Lista = lista;
        }
        public int Total { get; set; }
        public int Aprobadas { get; set; }
        public int Cursadas { get; set; }
        public List<AprobadasPorAnio> Lista { get; set; }
    }
}
