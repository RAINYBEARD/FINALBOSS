using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.DTOs
{
    /// <summary>
    /// Maps from HistoriaAcademica JSON
    /// </summary>
    public class HistoriaAcademica
    {
        public string Descrip { get; set; }
        public string Matcod { get; set; }
        public string Materia_Id { get; set; }
        public string Plan_Id { get; set; }
        public string Fecha { get; set; }
        public string Abr { get; set; }
        public int Calificacion { get; set; }
        public string Profesor { get; set; }
        public string Acta_Id { get; set; }
        public int Anio { get; set; }
        public int Cuatrim { get; set; }
        public string Plan_Tit { get; set; }
    }
}
