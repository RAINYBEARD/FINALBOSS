using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class MesaFinalDictionary : Dictionary<string, MesaFinalValue>
    {
        public void Add(string matcod, char turno_id, string plan_id, string profesor, char sede_id)
        {
            MesaFinalValue mesafinal = new MesaFinalValue();
            mesafinal.Turno_Id = turno_id;
            mesafinal.Plan_Id = plan_id;
            mesafinal.Profesor = profesor;
            mesafinal.Sede_Id = sede_id;
            this.Add(matcod, mesafinal);
        }
    }

    public class MesaFinalValue
    {
        public char Turno_Id { get; set; }
        public string Plan_Id { get; set; }
        public string Profesor { get; set; }
        public char Sede_Id { get; set; }
    }
}
