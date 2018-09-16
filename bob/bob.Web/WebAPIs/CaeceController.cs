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
using bob.Helpers;

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
        /// CARGA LOS DICCIONARIOS
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("GetDictionaries/{matricula}")]
        public void GetDictionaries(string matricula)
        {
             var aprDictionary = new AprDictionary();
             var curDictionary = new CurDictionary();
             var penDictionary = new PenDictionary();
             var notCurDictionary = new NotCurDictionary();
             var mesaFinalDictionary = new MesasDictionary();
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
                        PenValue equiv = new PenValue();
                        AutoMapper.Mapper.Map(dato, equiv);
                        penDictionary.Add(matcod, equiv);
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
            //ACA SE CARGAN LOS DICCIONARIOS 
            SessionManager.DiccionarioAprobadas = aprDictionary;
            SessionManager.DiccionarioCursadas = curDictionary;
            SessionManager.DiccionarioPendientes = penDictionary;
            SessionManager.DiccionarioNoCursadas = notCurDictionary;

        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetCorrelativas/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("GetCorrelativas/{matricula}")]
        public void GetCorrelativas(string matricula)
        {
            GetDictionaries(matricula);
            System.Diagnostics.Debug.WriteLine("Entro en el controller Cursos");
            //CHEQUEAR QUE LOS DICCIONARIOS ESTEN CARGADOS ANTES DE EMPEZAR A PROCESAR
            //if(Helpers.SessionManager.DiccionarioCursadas != null);
            List<int> materias_a_cursar = new List<int>();
            int materia_ant = 0;
            var query1 = BuscarUltimasMateriasDelPlanDeEstudio();

            foreach (var resultado0 in query1)
            {
                var query2 = BuscarCorrelativa(resultado0.Materia_Id);

                foreach (var resultado in query2)
                {
                    if (resultado.Materia_Id != resultado.Codigo_Correlativa)
                    {
                        BuscarMateriasACursar(resultado,ref materias_a_cursar,materia_ant);
                    }
                }
            }
            foreach (var materia_a_cursar in materias_a_cursar)
            {
                System.Diagnostics.Debug.WriteLine("Materia que puede cursar : " + materia_a_cursar);
            }
        }
        
        public List<Data.Entities.Correlativa> BuscarCorrelativa(int idmateria)
        {
            using (var context = new CaeceDBContext())
            {
                return context.Correlativas.Where(a => a.Materia_Id == idmateria).ToList();
            }
        }

        public List<Data.Entities.Materia> BuscarUltimasMateriasDelPlanDeEstudio()
        {
            using (var context = new CaeceDBContext())
            {
                short ultimo_anio_de_la_carrera = (short)(context.Materias.Max(a => a.Anio));
                return context.Materias.Where(a => a.Anio == ultimo_anio_de_la_carrera && a.Cuatrim == 2).ToList();
            }
        }

        private void BuscarMateriasACursar(Data.Entities.Correlativa correlativa,ref List<int> materias_a_cursar,int materia_ant)
        {
            //CHEQUEAR QUE LOS DICCIONARIOS ESTEN CARGADOS ANTES DE EMPEZAR A PROCESAR
            if (SessionManager.DiccionarioCursadas != null)
            {
                // Print the node.  
                //System.Diagnostics.Debug.WriteLine("Materia : " + correlativa.Materia_Id + " Correlativa : " + correlativa.Codigo_Correlativa);

                //Dictionary key = matcod = 8015/10S, value = AprValue  = {Abr, etc, etc} 

                // Busco las correlativas dentro del diccionario de las materias no cursadas
                if (SessionManager.DiccionarioNoCursadas.ContainsKey(correlativa.Materia_Id + "/" + correlativa.Plan_Id))
                {
                    // Verifico asignaturas que requieren cantidad de materias aprobadas
                    if (correlativa.Codigo_Correlativa < 100 && correlativa.Codigo_Correlativa <= (SessionManager.DiccionarioAprobadas.Count() + SessionManager.DiccionarioCursadas.Count()) )
                    {
                        materias_a_cursar.Add(correlativa.Materia_Id);
                    }

                    // Almaceno la materia para evaluar si la correlativa es la que le resta cursar al alumno
                    materia_ant = correlativa.Materia_Id;

                    var resulcorrelativa = BuscarCorrelativa(correlativa.Codigo_Correlativa);

                    foreach (var resultado in resulcorrelativa)
                    {
                        if (resultado.Materia_Id != resultado.Codigo_Correlativa)
                        {
                            // Hago llamada recursiva para recorrer el arbol de materias correlativas
                            BuscarMateriasACursar(resultado, ref materias_a_cursar,materia_ant);
                        }
                    }
                }
                else
                {
                    if (!materias_a_cursar.Contains(materia_ant))
                        materias_a_cursar.Add(materia_ant);
                }
            }
        }
    }
}























