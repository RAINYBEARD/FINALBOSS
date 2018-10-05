using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class CursosDictionary : Dictionary<string, CursosValue>
    {
       
    }

    public class CursosValue
    {
        public string Plan_Id { get; set; }
        public char Turno_Id { get; set; }
        public string Dia { get; set; }
        public int M_Acobrar { get; set; }
    }
}
