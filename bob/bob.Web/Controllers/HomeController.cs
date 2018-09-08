using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using bob.Data;
using Newtonsoft.Json.Linq;
using bob.CaeceWS;
using System.Web.Configuration;

namespace bob.Controllers
{
    public class HomeController : Controller
    {
        //private string _token = WebConfigurationManager.AppSettings.Get("CaeceWSToken");

        public ActionResult Index()
        {
            var PlanDeEstudioJSON = LoadJson<PlanDeEstudio>(MockMethod.PlanDeEstudio);

            using (var context = new CaeceDBContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (PlanDeEstudio dato in PlanDeEstudioJSON)
                        {
                            // Cargo a la base los datos de las materias
                            bool resultado = context.Materias_Descripciones.Any(a => a.Materia_Id == dato.materia_id);

                            if (resultado == false)
                            {
                                var materia_descripcion = context.Materias_Descripciones.Create();
                                materia_descripcion.Materia_Id = dato.materia_id;
                                materia_descripcion.Mat_Des = dato.mat_des.Trim();
                                context.Materias_Descripciones.Add(materia_descripcion);
                                context.SaveChanges();
                            }

                            // Cargo a la base los datos de los titulos
                            resultado = context.Titulos.Any(a => a.Plan_Tit == dato.plan_tit && a.Titulo_Id == dato.titulo_id);

                            if (resultado == false)
                            {
                                var titulo = context.Titulos.Create();
                                titulo.Plan_Tit = dato.plan_tit;
                                titulo.Titulo_Id = dato.titulo_id;
                                titulo.Tit_Des = dato.abr_titulo;
                                context.Titulos.Add(titulo);
                                context.SaveChanges();
                            }

                            // Cargo a la base la relacion materia titulo
                            resultado = context.Materias.Any(a => a.Materia_Id == dato.materia_id && a.Plan_Id == dato.plan_id && a.Plan_Tit == dato.plan_tit && a.Titulo_Id == dato.titulo_id);
                            if (resultado == false)
                            {
                                var materia = context.Materias.Create();
                                materia.Materia_Id = dato.materia_id;
                                materia.Plan_Id = dato.plan_id;
                                materia.Plan_Tit = dato.plan_tit;
                                materia.Titulo_Id = dato.titulo_id;
                                materia.Anio = dato.mat_anio;
                                materia.Cuatrim = dato.mat_cuatrim;
                                materia.Mat_Modulos = dato.mat_modulos;
                                context.Materias.Add(materia);
                                context.SaveChanges();
                            }

                            // Cargo a la base las materias correlativas
                            var correlativa = context.Correlativas.Create();
                            correlativa.Codigo_Correlativa = dato.codigo_correlativa;
                            correlativa.Materia_Id = dato.materia_id;
                            correlativa.PAprobar = dato.paprobar.Trim();
                            correlativa.PCursar = dato.pcursar.Trim();
                            correlativa.Plan_Id = dato.plan_id;
                            correlativa.Plan_Tit = dato.plan_tit;
                            correlativa.Titulo_Id = dato.titulo_id;
                            context.Correlativas.Add(correlativa);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return View();
        }

        //public ActionResult Nacho()
        //{
        //    var dict = new AprDictionary();
        //    var dict = new CurDictionary();
        //    var dict = new EquivDictionary();
        //    var dict = new NotCurDictionary();
        //    var dict = new MesaFinalDictionary();
        //    var dict = new CursosDictionary();
        //    //dict.Add()
        //    foreach (HistoriaAcademica dato in LoadJson<HistoriaAcademica>(MockMethod.HistoriaAcademica))
        //    {
        //        char estado_materia = char.Parse(dato.descrip);
        //        if (estado_materia == 'APR')
        //        {

        //            AprValue apr;
        //            string materia_id = dato.materia_id;
        //            apr.Fecha = dato.fecha;
        //            apr.Calificacion = dato.calificacion;
        //            apr.Descripcion = dato.descrip;
        //            apr.Profesor = dato.profesor;
        //            apr.Acta_id = dato.acta_id;
        //            apr.Anio = dato.anio;
        //            apr.Cuatrim = dato.cuatrim;
        //            dict.Add(materia_id, apr);
        //        }
        //        else {
        //               if (estado_materia == 'CUR') 
        //               {
        //                 CurValue cur;
        //                 cur.Fecha = fecha;
        //                 cur.Abr = abr;
        //                 cur.Profesor = profesor;
        //                 cur.Anio = anio;
        //                 cur.Cuatrim = cuatrim;
        //                 cur.Plan_id = plan_id;
        //                 cur.Plan_tit = plan_tit;
        //                 this.Add(materia_id, cur);
        //               }
        //               else 
        //               {
        //                  if (estado_materia == 'PEN') 
        //                  {
        //                    EquivVale equiv;
        //                    cur.Fecha = fecha;
        //                    cur.Abr = abr;     
        //                    cur.Plan_id = plan_id;
        //                    cur.Plan_tit = plan_tit;
        //                    this.Add(materia_id, equiv);
        //                  }
        //                  else 
        //                  {
        //                    NotCurValue notcur;
        //                    notcur.Abr = abr;
        //                    notcur.Profesor = profesor;
        //                    notcur.Anio = anio;
        //                    notcur.Cuatrim = cuatrim;
        //                    notcur.Plan_id = plan_id;
        //                    notcur.Plan_tit = plan_tit;
        //                    this.Add(materia_id, notcur);
        //                  }
        //               }
        //             }
        //    }
        //    foreach (MesaFinal dato in LoadJson<MesaFinal>(MockMethod.MesasFinal))
        //     { 
        //       MesaFinalValue  mesafinal;
        //       string materia_id = dato.materia_id;
        //       mesafinal.Turno_id = dato.Turno_id;
        //       mesafinal.Plan_id = dato.Plan_id
        //       mesafinal.Profesor = dato.Profesor;
        //       mesafinal.Sede_id = dato.Sede_id;
        //       dict.Add(materia_id, mesafinal);
        //     }
        //     foreach (Cursos dato in LoadJson<Cursos>(MockMethod.Cursos))
        //     {
        //       char turno_materia = char.Parse(dato.turno_id);
        //       string dia_materia = dato.dia;
        //       if (turno_materia == 'N') || ((dia_materia == '0000010') || (dia_materia == '0000020') !! (dia_materia == '0000030')) || (dato.materia_id = '2091')
        //       {
        //          CursosValue cursos;
        //          string materia_id = dato.materia_id;
        //          cursos.Turno_id = dato.turno_id;
        //          cursos.Dia = dato.dia;
        //          cursos.M_acobrar = dato.m_acobrar;
        //          cursos.Plan_id = dato.plan_id;
        //          dict.Add(materia_id, cursos);
        //       }
        //       
        //         
        //     }
        //    return View("Index.cshtml");
        //}

        public List<T> LoadJson<T>(MockMethod mm)
        {
            //CaeceWS.wbsTrans service = new bob.CaeceWS.wbsTrans();
            //var json = service.getPlanEstudioJSON(_token, " 951282");

            switch (mm)
            {
                case MockMethod.Autenticacion:
                    return ((JArray)JObject.Parse(MockService.MockService.Autenticacion())["autenticacion"]).ToObject<List<T>>();

                case MockMethod.Cursos:
                    return ((JArray)JObject.Parse(MockService.MockService.Cursos())["Cursos"]).ToObject<List<T>>();

                case MockMethod.HistoriaAcademica:
                    return ((JArray)JObject.Parse(MockService.MockService.HistoriaAcademica())["HistoriaAcademica"]).ToObject<List<T>>();

                case MockMethod.MesasFinal:
                    return ((JArray)JObject.Parse(MockService.MockService.MesasFinal())["Mesas"]).ToObject<List<T>>();

                case MockMethod.PlanDeEstudio:
                    return ((JArray)JObject.Parse(MockService.MockService.PlanDeEstudio())["PlanEstudio"]).ToObject<List<T>>();
            }
            return new List<T>();
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

    public class PlanDeEstudio
    {
        public int materia_id { get; set; }
        public string mat_des { get; set; }
        public string plan_tit { get; set; }
        public int titulo_id { get; set; }
        public string plan_id { get; set; }
        public short mat_anio { get; set; }
        public short mat_cuatrim { get; set; }
        public float mat_modulos { get; set; }
        public string pcursar { get; set; }
        public string paprobar { get; set; }
        public int codigo_correlativa { get; set; }
        public string abr_titulo { get; set; }
    }

    public class Curso
    {
        public string periodo_id { get; set; }
        public string materia_id { get; set; }
        public string perso_id { get; set; }
    }
}








public class HistoriaAcademica
{
    public string descrip;
    public string materia_id;
    public string fecha;
    public string abr;
    public int calificacion;
    public string profesor;
    public string acta_id;
    public int anio;
    public int cuatrim;
}

public class MesaFinal
{
    public string materia_id;
    string plan_id;
    char turno_id;
    string profesor;
    char sede_id;
}

public class Cursos
{
    public string materia_id;
    string plan_id;
    char turno_id;
    string dia;
    int m_acobrar;


}

//struct AprValue {
//    string Fecha  
//    string Abr
//    int Calificacion
//    string Profesor
//    string Acta_id
//    int Anio 
//    int Cuatrim
//}

//struct CurValue {
//    string Fecha  
//    string Abr
//    string Profesor
//    int Anio 
//    int Cuatrim
//    string Plan_id
//    int Plan_tit
//}

//struct EquivValue {
//    string Fecha  
//    string Abr
//    string Plan_id
//    int Plan_tit
//}

//struct NotCurValue {
//    string Fecha  
//    string Abr
//    int Anio 
//    int Cuatrim
//    string Plan_id
//    int Plan_tit
//}

//struct MesaFinalValue {
//    char Turno_id 
//    string Plan_id
//    string Profesor 
//    char sede_id 
//}

//struct CursosValue {
//    string Plan_id
//    char Turno_id
//    string Dia 
//    int M_acobrar
//}

//public class AprDictionary : Dictionary<string, AprValue>
//{

//    public void Add(string materia_id, string fecha, string abr, int calificacion, string profesor, string acta_id, int anio, int cuatrim)
//    {

//        AprValue apr;
//        apr.Fecha = fecha;
//        apr.Abr = abr;
//        apr.Calificacion = calificacion;
//        apr.Profesor = profesor;
//        apr.Acta_id = acta_id;
//        apr.Anio = anio;
//        apr.Cuatrim = cuatrim;
//        this.Add(materia_id, apr);
//    }
//}
//public class CurDictionary : Dictionary<string, CurValue>
//{

//    public void Add(string materia_id, string fecha, string abr, string profesor,  int anio, int cuatrim, string plan_id, int plan_tit)
//    {
//        CurValue cur;
//        cur.Fecha = fecha;
//        cur.Abr = abr;
//        cur.Profesor = profesor;
//        cur.Anio = anio;
//        cur.Cuatrim = cuatrim;
//        cur.Plan_id = plan_id;
//        cur.Plan_tit = plan_tit;
//        this.Add(materia_id, cur);
//    }
//}

//public class EquivDictionary : Dictionary<string, EquivValue>
//{

//    public void Add(string materia_id, string fecha, string abr, string plan_id, int plan_tit)
//    {
//        EquivValue equiv;
//        cur.Fecha = fecha;
//        cur.Abr = abr;     
//        cur.Plan_id = plan_id;
//        cur.Plan_tit = plan_tit;
//        this.Add(materia_id, equiv);
//    }
//}

//public class NotCurDictionary : Dictionary<string, NotCurValue>
//{

//    public void Add(string materia_id, string abr, int anio, int cuatrim, string plan_id, int plan_tit)
//    {
//        NotCurValue notcur;
//        notcur.Abr = abr;
//        notcur.Profesor = profesor;
//        notcur.Anio = anio;
//        notcur.Cuatrim = cuatrim;
//        notcur.Plan_id = plan_id;
//        notcur.Plan_tit = plan_tit;
//        this.Add(materia_id, notcur);
//    }
//}

// public class MesaFinalDictionary : Dictionary<string, MesaFinalValue>
//  {
//     public void Add(string materia_id, char turno_id, string plan_id, string profesor, char sede_id)
//   {
//      MesaFinalValue mesafinal
//      mesafinal.Turno_id = turno_id;
//      mesafinal.Plan_id = plan_id;
//      mesafinal.Profesor = profesor;
//      mesafinal.Sede_id= sede_id;
//      this.Add(materia_id, mesafinal);
//   }
//  }

// public class CursosDictionary : Dictionary<string, Cursosvalue>
// {
//   public void Add(string materia_id, char turno_id, string dia, int m_acobrar, string plan_id, int plan_tit)
//    {
//      CursosValue cursos
//      cursos.Turno_id = turno_id;
//      cursos.Dia = dia;
//      cursos.M_acobrar = m_acobrar;
//      cursos.Plan_id = plan_id;
//      this.Add(materia_id, cursos);
//    }
// }