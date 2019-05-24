using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class RepDictionary : Dictionary<string, RepValue>
    {

    }
    public class RepValue
    {
        public string Fecha { get; set; }
        public string Descrip { get; set; }
    }
}
