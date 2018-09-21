using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class RepDictionary : Dictionary<string, string>
    {
        public new void Add(string matcod, string fecha)
        {
            this.Add(matcod, fecha);
        }
    }

}
