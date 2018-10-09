using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class MesasDictionary : Dictionary<string, MesasValue>
    {
        
    }

    public class MesasValue
    {
        public char Turno_Id { get; set; }
        public string Plan_Id { get; set; }
        public string Profesor { get; set; }
        public char Sede_Id { get; set; }
    }
}
