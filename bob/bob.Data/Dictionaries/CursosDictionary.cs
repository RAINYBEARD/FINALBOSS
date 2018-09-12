using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.Dictionaries
{
    public class CursosDictionary : Dictionary<string, CursosValue>
    {
        public void Add(string matcod, char turno_id, string dia, int m_acobrar, string plan_id, int plan_tit)
        {
            CursosValue cursos = new CursosValue();
            cursos.Plan_Id = plan_id;
            cursos.Turno_Id = turno_id;
            cursos.Dia = dia;
            cursos.M_Acobrar = m_acobrar;
            this.Add(matcod, cursos);
        }
    }

    public class CursosValue
    {
        public string Plan_Id { get; set; }
        public char Turno_Id { get; set; }
        public string Dia { get; set; }
        public int M_Acobrar { get; set; }
    }
}
