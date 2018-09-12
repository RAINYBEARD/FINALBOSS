using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Configuration;
using Newtonsoft.Json.Linq;

namespace bob.Mocks
{
    public class MockService
    {
        private static readonly string _repoPath = HttpContext.Current.Server.MapPath("~/App_Data");

        public static string HistoriaAcademica()
        {

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

            using (StreamReader r = new StreamReader(Path.Combine(_repoPath, "Mesas.json")))
            {
                return r.ReadToEnd();
            }
        }

        public static string PlanDeEstudio()
        {

            using (StreamReader r = new StreamReader(Path.Combine(_repoPath, "PlanEstudio.json")))
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

        public static List<T> LoadJson<T>(MockMethod mm)
        {
            //CaeceWS.wbsTrans service = new bob.CaeceWS.wbsTrans();
            //var json = service.getPlanEstudioJSON(_token, " 951282");

            switch (mm)
            {
                case MockMethod.Autenticacion:
                    return ((JArray)JObject.Parse(Autenticacion())["autenticacion"]).ToObject<List<T>>();

                case MockMethod.Cursos:
                    return ((JArray)JObject.Parse(Cursos())["Cursos"]).ToObject<List<T>>();

                case MockMethod.HistoriaAcademica:
                    return ((JArray)JObject.Parse(HistoriaAcademica())["HistoriaAcademica"]).ToObject<List<T>>();

                case MockMethod.MesasFinal:
                    return ((JArray)JObject.Parse(MesasFinal())["Mesas"]).ToObject<List<T>>();

                case MockMethod.PlanDeEstudio:
                    return ((JArray)JObject.Parse(PlanDeEstudio())["PlanEstudio"]).ToObject<List<T>>();
            }
            return new List<T>();
        }


    }
    public enum MockMethod
    {
        Autenticacion,
        Cursos,
        HistoriaAcademica,
        MesasFinal,
        PlanDeEstudio
    }
}