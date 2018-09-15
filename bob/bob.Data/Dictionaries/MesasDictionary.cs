using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class MesasDictionary : Dictionary<string, MesasValue>
    {
        public void Add(string matcod, char turno_id, string plan_id, string profesor, char sede_id)
        {
            MesasValue mesafinal = new MesasValue();
            mesafinal.Turno_Id = turno_id;
            mesafinal.Plan_Id = plan_id;
            mesafinal.Profesor = profesor;
            mesafinal.Sede_Id = sede_id;
            this.Add(matcod, mesafinal);
        }
    }

    public class MesasValue
    {
        public char Turno_Id { get; set; }
        public string Plan_Id { get; set; }
        public string Profesor { get; set; }
        public char Sede_Id { get; set; }
    }
}
