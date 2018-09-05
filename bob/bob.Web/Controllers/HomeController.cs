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
        private string _token = WebConfigurationManager.AppSettings.Get("CaeceWSToken");

        public ActionResult Index()
        {
            var PlanDeEstudioJSON = LoadJson();

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

        public List<PlanDeEstudio> LoadJson()
        {
            //CaeceWS.wbsTrans service = new bob.CaeceWS.wbsTrans();
            //var json = service.getPlanEstudioJSON(_token, " 951282");
            var jobject = JObject.Parse(MockService.MockService.PlanDeEstudio());
            var sCursos = (JArray)jobject["PlanEstudio"];
            return sCursos.ToObject<List<PlanDeEstudio>>();
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
