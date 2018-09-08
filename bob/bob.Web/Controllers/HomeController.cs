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

        public ActionResult Nacho()
        {
           var dict = new AprDictionary();
           var dict1 = new CurDictionary();
           var dict2 = new EquivDictionary();
           var dict3 = new NotCurDictionary();
           var dict4 = new MesaFinalDictionary();
           var dict5 = new CursosDictionary();
            //dict.Add()
           foreach (HistoriaAcademica dato in LoadJson<HistoriaAcademica>(MockMethod.HistoriaAcademica))
           {
              string estado_materia = dato.Descrip;
              string matcod = dato.Matcod;
              switch (estado_materia)
              {
                    case ("APR"):
                        AprValue apr = new AprValue();
                        apr.Fecha = dato.Fecha;
                        apr.Calificacion = dato.Calificacion;
                        apr.Profesor = dato.Profesor;
                        apr.Acta_Id = dato.Acta_Id;
                        apr.Anio = dato.Anio;
                        apr.Cuatrim = dato.Cuatrim;
                        dict.Add(matcod, apr);
                        System.Diagnostics.Debug.WriteLine("MateriaID:" + matcod + "Nombre Materia" + apr.Fecha);
                        break;
                    case ("CUR"):
                        CurValue cur = new CurValue();
                        cur.Fecha = dato.Fecha;
                        cur.Abr = dato.Abr;
                        cur.Profesor = dato.Profesor;
                        cur.Anio = dato.Anio;
                        cur.Cuatrim = dato.Cuatrim;
                        cur.Plan_Id = dato.Plan_Id;
                        cur.Plan_Tit = dato.Plan_Tit;
                        dict1.Add(matcod, cur);
                        break;
                    case ("PEN"):
                        EquivValue equiv = new EquivValue();
                        equiv.Fecha = dato.Fecha;
                        equiv.Abr = dato.Abr;
                        equiv.Plan_Id = dato.Plan_Id;
                        equiv.Plan_Tit = dato.Plan_Tit;
                        dict2.Add(matcod, equiv);
                        break;
                    default:
                        if ((estado_materia.Equals("   ")) || (estado_materia.Equals("?  ")))
                        {
                            NotCurValue notcur = new NotCurValue();
                            notcur.Abr = dato.Abr;
                            notcur.Anio = dato.Anio;
                            notcur.Cuatrim = dato.Cuatrim;
                            notcur.Plan_Id = dato.Plan_Id;
                            notcur.Plan_Tit = dato.Plan_Tit;
                            dict3.Add(matcod, notcur);
                        }
                        break;
              }

                
            }
            //foreach (MesaFinal dato in LoadJson<MesaFinal>(MockMethod.MesasFinal))
            // {

            //        MesaFinalValue mesafinal = new MesaFinalValue();
            //        string materia_id = dato.Matcod;
            //        mesafinal.Turno_Id = dato.Turno_Id;
            //        mesafinal.Plan_Id = dato.Plan_Id;
            //        mesafinal.Profesor = dato.Profesor;
            //        mesafinal.Sede_Id = dato.Sede_Id;
            //        dict4.Add(materia_id, mesafinal);

            // }
            // foreach (Cursos dato in LoadJson<Cursos>(MockMethod.Cursos))
            // {
            //   char turno_materia = dato.Turno_Id;
            //   string dia_materia = dato.Dia;
            //    if ((turno_materia == 'N') || (dia_materia.Equals("0000010")) || (dia_materia.Equals("0000020")) || (dia_materia.Equals("0000030")) || (dato.Materia_Id.Equals("2091")))
            //    {
            //      CursosValue cursos = new CursosValue();
            //      string materia_id = dato.Matcod; 
            //      cursos.Turno_Id = dato.Turno_Id;
            //      cursos.Dia = dato.Dia;
            //      cursos.M_Acobrar = dato.M_Acobrar;
            //      cursos.Plan_Id = dato.Plan_Id;
            //      dict5.Add(materia_id, cursos);
            //    }


            //}
            return View("Index");
        }

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
    public string Descrip;
    public string Matcod;
    public string Materia_Id;
    public string Plan_Id;
    public string Fecha;
    public string Abr;
    public int Calificacion;
    public string Profesor;
    public string Acta_Id;
    public int Anio;
    public int Cuatrim;
    public string  Plan_Tit;
    }

public class MesaFinal
{
    public string Materia_Id;
    public string Plan_Id;
    public char Turno_Id;
    public string Profesor;
    public char Sede_Id;
}

public class Cursos
{
    public string Materia_Id;
    public string Plan_Id;
    public char Turno_Id;
    public string Dia;
    public int M_Acobrar;


}

public class AprValue
{
    public string Fecha { get; set; }
    public string Abr { get; set; }
    public int Calificacion { get; set; }
    public string Profesor { get; set; }
    public string Acta_Id { get; set; }
    public int Anio { get; set; }
    public int Cuatrim { get; set; }
}

public class CurValue
{
    public string Fecha { get; set; }
    public string Abr { get; set; }
    public string Profesor { get; set; }
    public int Anio { get; set; }
    public int Cuatrim { get; set; }
    public string Plan_Id { get; set; }
    public string Plan_Tit { get; set; }
}

public class EquivValue {
    public string Fecha { get; set; }
    public string Abr { get; set; }
    public string Plan_Id { get; set; }
    public string Plan_Tit { get; set; }
}

public class NotCurValue
{
    public string Fecha { get; set; }
    public string Abr { get; set; }
    public int Anio { get; set; }
    public int Cuatrim { get; set; }
    public string Plan_Id { get; set; }
    public string Plan_Tit { get; set; }
}

public class MesaFinalValue
{
    public char Turno_Id { get; set; }
    public string Plan_Id { get; set; }
    public string Profesor { get; set; }
    public char Sede_Id { get; set; }
}

public class CursosValue
{
    public string Plan_Id { get; set; }
    public char Turno_Id { get; set; }
    public string Dia { get; set; }
    public int M_Acobrar { get; set; }
}

class AprDictionary : Dictionary<string, AprValue>
{

    public void Add(string matcod, string fecha, string abr, int calificacion, string profesor, string acta_id, int anio, int cuatrim)
    {

        AprValue apr = new AprValue();
        apr.Fecha = fecha;
        apr.Abr = abr;
        apr.Calificacion = calificacion;
        apr.Profesor = profesor;
        apr.Acta_Id = acta_id;
        apr.Anio = anio;
        apr.Cuatrim = cuatrim;
        this.Add(matcod, apr);
    }
}

class CurDictionary : Dictionary<string, CurValue>
{

    public void Add(string matcod, string fecha, string abr, string profesor,  int anio, int cuatrim, string plan_id, string plan_tit)
    {
        CurValue cur = new CurValue();
        cur.Fecha = fecha;
        cur.Abr = abr;
        cur.Profesor = profesor;
        cur.Anio = anio;
        cur.Cuatrim = cuatrim;
        cur.Plan_Id = plan_id;
        cur.Plan_Tit = plan_tit;
        this.Add(matcod, cur);
    }
}

class EquivDictionary : Dictionary<string, EquivValue>
{

    public void Add(string matcod, string fecha, string abr, string plan_id, string plan_tit)
    {
        EquivValue equiv = new EquivValue();
        equiv.Fecha = fecha;
        equiv.Abr = abr;     
        equiv.Plan_Id = plan_id;
        equiv.Plan_Tit = plan_tit;
        this.Add(matcod, equiv);
    }
}

class NotCurDictionary : Dictionary<string, NotCurValue>
{

    public void Add(string matcod, string abr, int anio, int cuatrim, string plan_id, string plan_tit)
    {
        NotCurValue notcur = new NotCurValue();
        notcur.Abr = abr;
        notcur.Anio = anio;
        notcur.Cuatrim = cuatrim;
        notcur.Plan_Id = plan_id;
        notcur.Plan_Tit = plan_tit;
        this.Add(matcod, notcur);
    }
}

 class MesaFinalDictionary : Dictionary<string, MesaFinalValue>
 {
     public void Add(string matcod, char turno_id, string plan_id, string profesor, char sede_id)
   {
      MesaFinalValue mesafinal = new MesaFinalValue();
      mesafinal.Turno_Id = turno_id;
      mesafinal.Plan_Id = plan_id;
      mesafinal.Profesor = profesor;
      mesafinal.Sede_Id= sede_id;
      this.Add(matcod, mesafinal);
   }
  }

 class CursosDictionary : Dictionary<string, CursosValue>
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

