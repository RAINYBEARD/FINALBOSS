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
        private static string _repoPath = WebConfigurationManager.AppSettings.Get("LocalRepositoryPath");

        public static string HistoriaAcademicaJson() {

            using (StreamReader r = new StreamReader(_repoPath + @"\bob\bob.Web\App_Data\Cursos.json"))
            {
                return r.ReadToEnd();
            }
        }

        public static string CursosJson()
        {

            using (StreamReader r = new StreamReader(_repoPath + @"\bob\bob.Web\App_Data\Cursos.json"))
            {
                return r.ReadToEnd();
            }
        }

        public static string MesasFinal()
        {

            using (StreamReader r = new StreamReader(_repoPath + @"\bob\bob.Web\App_Data\MesasFinal.json"))
            {
                return r.ReadToEnd();
            }
        }

        public static string PlanDeEstudio()
        {

            using (StreamReader r = new StreamReader(_repoPath + @"\bob\bob.Web\App_Data\PlanDeEstudio.json"))
            {
                return r.ReadToEnd();
            }
        }
    }
}