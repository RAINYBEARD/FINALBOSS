using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class NotCurDictionary : Dictionary<string, NotCurValue>
    {

    }

    public class NotCurValue
    {
        public string Fecha { get; set; }
        public string Abr { get; set; }
        public int Anio { get; set; }
        public int Cuatrim { get; set; }
        public string Plan_Id { get; set; }
        public string Plan_Tit { get; set; }
    }
}
