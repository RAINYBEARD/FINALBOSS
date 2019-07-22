using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Arbol
{
    public class Arbol
    {
        public List<Nodo> nodos { get; set; }
        public List<Arcos> arcos { get; set; }
        public int total = 0;
        public int aprobadas = 0;
    }
}
