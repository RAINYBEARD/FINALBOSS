using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Configuration;

namespace bob.MockService
{
    public class MockService
    {
        private static readonly string _repoPath = HttpContext.Current.Server.MapPath("~/App_Data");

        public static string HistoriaAcademica() {

            using (StreamReader r = new StreamReader(Path.Combine(_repoPath, "HistoriaAcademica.json")))
            {
                return r.ReadToEnd();
            }
        }

        public static string Cursos()
        {

            using (StreamReader r = new StreamReader(Path.Combine(_repoPath, "Cursos.json")))
            {
                return r.ReadToEnd();
            }
        }

        public static string MesasFinal()
        {

            using (StreamReader r = new StreamReader(Path.Combine(_repoPath, "MesasFinal.json")))
            {
                return r.ReadToEnd();
            }
        }

        public static string PlanDeEstudio()
        {

            using (StreamReader r = new StreamReader(Path.Combine(_repoPath, "PlanDeEstudio.json")))
            {
                return r.ReadToEnd();
            }
        }

        public static string Autenticacion()
        {

            using (StreamReader r = new StreamReader(Path.Combine(_repoPath, "Autenticacion.json")))
            {
                return r.ReadToEnd();
            }
        }
    }
}