using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class CorrDictionary : Dictionary<int, List<CorrValue>>
    {
        
    }

    public class CorrValue
    {
        public int materia_id { get; set; }
        public int codigo_correlativa { get; set; }
        public string plan_id { get; set; }
        public string plan_tit { get; set; }
        public string paprobar { get; set; }
        public string pcursar { get; set; }
    }
}
