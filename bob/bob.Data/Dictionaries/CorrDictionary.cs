using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class CorrDictionary : Dictionary<int, List<CorrValue>>
    {

        public void Add(int materia_id, int codigo_correlativa,string plan_id,string plan_tit,string paprobar,string pcursar)
        {
            List<CorrValue> lcorr = new List<CorrValue>();
            CorrValue corr = new CorrValue();
            corr.materia_id = materia_id;
            corr.codigo_correlativa = codigo_correlativa;
            corr.plan_id = plan_id;
            corr.plan_tit = plan_tit;
            corr.paprobar = paprobar;
            corr.pcursar = pcursar;
            lcorr.Add(corr);
            this.Add(materia_id, lcorr);
        }
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
