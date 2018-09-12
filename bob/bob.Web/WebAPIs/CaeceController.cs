using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using bob.Data;
using bob.Data.DTOs;
using bob.Data.Dictionaries;
using Newtonsoft.Json.Linq;
using bob.CaeceWS;
using System.Web.Configuration;
using bob.Mocks;
namespace bob.Controllers
{
    public class CaeceController : Controller
    {
        private readonly CaeceDBContext context = new CaeceDBContext();
        //private string _token = WebConfigurationManager.AppSettings.Get("CaeceWSToken");
        //private CaeceWS.wbsTrans caeceWS = new CaeceWS.wbsTrans();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetPlanDeEstudio/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("GetPlanDeEstudio/{matricula}")]
        public void GetPlanDeEstudio(string matricula)
        {
            var PlanDeEstudioJSON = MockService.LoadJson<PlanEstudio>(MockMethod.PlanDeEstudio);

            using (var context = new CaeceDBContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (PlanEstudio dato in PlanDeEstudioJSON)
                        {
                            // Cargo a la base los datos de las materias
                            bool resultado = context.Materias_Descripciones.Any(a => a.Materia_Id == dato.materia_id);

                            if (resultado == false)
                            {
                                var materia_descripcion = context.Materias_Descripciones.Create();
                                AutoMapper.Mapper.Map(dato, materia_descripcion);
                                context.Materias_Descripciones.Add(materia_descripcion);
                                context.SaveChanges();
                            }

                            // Cargo a la base los datos de los titulos
                            resultado = context.Titulos.Any(a => a.Plan_Tit == dato.plan_tit && a.Titulo_Id == dato.titulo_id);

                            if (resultado == false)
                            {
                                var titulo = context.Titulos.Create();
                                AutoMapper.Mapper.Map(dato, titulo);
                                context.Titulos.Add(titulo);
                                context.SaveChanges();
                            }

                            // Cargo a la base la relacion materia titulo
                            resultado = context.Materias.Any(a => a.Materia_Id == dato.materia_id && a.Plan_Id == dato.plan_id && a.Plan_Tit == dato.plan_tit && a.Titulo_Id == dato.titulo_id);
                            if (resultado == false)
                            {
                                var materia = context.Materias.Create();
                                AutoMapper.Mapper.Map(dato, materia);
                                context.Materias.Add(materia);
                                context.SaveChanges();
                            }

                            // Cargo a la base las materias correlativas
                            var correlativa = context.Correlativas.Create();
                            AutoMapper.Mapper.Map(dato, correlativa);
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
        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetDictionaries/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("GetDictionaries/{matricula}")]
        public void GetDictionaries(string matricula)
        {
            var aprDictionary = new AprDictionary();
            var curDictionary = new CurDictionary();
            var equivDictionary = new EquivDictionary();
            var notCurDictionary = new NotCurDictionary();
            var mesaFinalDictionary = new MesaFinalDictionary();
            var cursosDictionary = new CursosDictionary();

            foreach (HistoriaAcademica dato in MockService.LoadJson<HistoriaAcademica>(MockMethod.HistoriaAcademica))
            {
                string estado_materia = dato.Descrip;
                string matcod = dato.Matcod;
                switch (estado_materia)
                {
                    case ("APR"):
                        AprValue apr = new AprValue();
                        AutoMapper.Mapper.Map(dato, apr);
                        aprDictionary.Add(matcod, apr);
                        break;
                    case ("CUR"):
                        CurValue cur = new CurValue();
                        AutoMapper.Mapper.Map(dato, cur);
                        curDictionary.Add(matcod, cur);
                        break;
                    case ("PEN"):
                        EquivValue equiv = new EquivValue();
                        AutoMapper.Mapper.Map(dato, equiv);
                        equivDictionary.Add(matcod, equiv);
                        break;
                    default:
                        if ((estado_materia.Equals("   ")) || (estado_materia.Equals("?  ")))
                        {
                            NotCurValue notcur = new NotCurValue();
                            AutoMapper.Mapper.Map(dato, notcur);
                            notCurDictionary.Add(matcod, notcur);
                        }
                        break;
                }
            }
        }


    }
}























