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

            foreach (var dato in MockService.LoadJson<Curso>(MockMethod.Cursos))
            {
                if (notCurDictionary.ContainsKey(dato.Materia_Id + "/" + dato.Plan_Id))
                {
                    CursosValue curso = new CursosValue();
                    AutoMapper.Mapper.Map(dato, curso);
                    cursosDictionary.Add(dato.Materia_Id, curso);
                }

            }

            //ACA SE CARGAN LOS DICCIONARIOS 
            SessionManager.DiccionarioAprobadas = aprDictionary;
            SessionManager.DiccionarioCursadas = curDictionary;
            SessionManager.DiccionarioPendientes = penDictionary;
            SessionManager.DiccionarioNoCursadas = notCurDictionary;
            SessionManager.DiccionarioCursos = cursosDictionary;

        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetMateriasACursar/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("GetMateriasACursar/{matricula}")]
        public List<string> GetMateriasACursar(string matricula)
        {
            var tiempoinicio=DateTime.Now;
            GetDictionaries(matricula);
            System.Diagnostics.Debug.WriteLine("Entro en el controller Cursos");
            //CHEQUEAR QUE LOS DICCIONARIOS ESTEN CARGADOS ANTES DE EMPEZAR A PROCESAR
            //if(Helpers.SessionManager.DiccionarioCursadas != null);
            List<string> materias_a_cursar = new List<string>();
            List<Data.Entities.Correlativa> materias_para_buscar_correlativas = new List<Data.Entities.Correlativa>();
            string materia_ant = "";

            foreach (var resultado0 in SessionManager.DiccionarioNoCursadas)
            {
                // Descompongo la materiaid del planid
                string[] matriculaid = resultado0.Key.Split(new Char[] { '/' });
                var query2 = BuscarCorrelativa(int.Parse(matriculaid[0]));

                // Evaluacion para la materias que no tienen correlativas
                if (query2.Count == 1)
                    materias_a_cursar.Add(query2[0].Materia_Id +"/"+query2[0].Plan_Id);

                foreach (var resultado in query2)
                {
                    if (resultado.Materia_Id != resultado.Codigo_Correlativa)
                    {
                        // Elimino los resultados repetidos
                        if (!materias_para_buscar_correlativas.Any(x => x.Materia_Id == resultado.Materia_Id))
                            materias_para_buscar_correlativas.Add(resultado);
                    }
                }
            }
            foreach (var materia in materias_para_buscar_correlativas)
             {
                //System.Diagnostics.Debug.WriteLine("Busco correlativas de la materia : " + materia.Materia_Id);
                BuscarMateriasACursar(materia, ref materias_a_cursar, materia_ant);
             }
            //foreach (var materia_a_cursar in materias_a_cursar)
            //{
            //    System.Diagnostics.Debug.WriteLine("Materia que puede cursar : " + materia_a_cursar);
            //}
            var tiempofin = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("tardo : " + (tiempofin-tiempoinicio));
            return materias_a_cursar;
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

        private void BuscarMateriasACursar(Data.Entities.Correlativa correlativa,ref List<string> materias_a_cursar,string materia_ant)
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
                        materias_a_cursar.Add(correlativa.Materia_Id + "/" + correlativa.Plan_Id);
                    }

                    // Almaceno la materia para evaluar si la correlativa es la que le resta cursar al alumno
                    materia_ant = correlativa.Materia_Id + "/" + correlativa.Plan_Id;

                    var resulcorrelativa = BuscarCorrelativa(correlativa.Codigo_Correlativa);
                    bool flag_materia_a_cursar = true;
                    foreach (var resultado in resulcorrelativa)
                    {
                        if (resultado.Materia_Id != resultado.Codigo_Correlativa)
                        {
                            if (!SessionManager.DiccionarioAprobadas.ContainsKey(resultado.Materia_Id + "/" + resultado.Plan_Id) && !SessionManager.DiccionarioCursadas.ContainsKey(resultado.Materia_Id + "/" + resultado.Plan_Id))
                            {
                                flag_materia_a_cursar = false;
                            }

                            // Hago llamada recursiva para recorrer el arbol de materias correlativas
                            BuscarMateriasACursar(resultado, ref materias_a_cursar, materia_ant);

                        }
                    }

                    if (flag_materia_a_cursar)
                    {
                        if (!materias_a_cursar.Contains(materia_ant))
                            materias_a_cursar.Add(materia_ant);
                    }

                }
            }
        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetMateriasACursarCuatrimestreActual/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("GetMateriasACursarCuatrimestreActual/{matricula}")]
        public List<Curso> GetMateriasACursarCuatrimestreActual(string matricula)
        {
            GetDictionaries(matricula);
            List<string> materias_a_cursar = GetMateriasACursar(matricula);
            List<Curso> materias_a_cursar_este_cuatri = new List<Curso>();
            

            foreach (var materia in materias_a_cursar)
            {
                // Descompongo la materiaid del planid
                string[] materiaid = materia.Split(new Char[] { '/' });

                // Verifico los cursos que puede cursar este cuatrimestre
                if (SessionManager.DiccionarioCursos.ContainsKey(materiaid[0]))
                {
                    Curso cursomateria = new Curso();
                    cursomateria.Materia_Id = materiaid[0];
                    cursomateria.Dia = SessionManager.DiccionarioCursos[materiaid[0]].Dia;
                    cursomateria.M_Acobrar = SessionManager.DiccionarioCursos[materiaid[0]].m_acobrar;
                    cursomateria.Plan_Id = SessionManager.DiccionarioCursos[materiaid[0]].Plan_Id;
                    cursomateria.Turno_Id = SessionManager.DiccionarioCursos[materiaid[0]].Turno_Id;

                    // Agrego a la lista los cursos a los cuales se puede inscribir
                    materias_a_cursar_este_cuatri.Add(cursomateria);                    
                    //System.Diagnostics.Debug.WriteLine("Materia que puede cursar este cuatri : " + SessionManager.DiccionarioCursos[materiaid[0]].Dia);
                }
            }
            return materias_a_cursar_este_cuatri;
        }

        public bool VerificarFiltroDias(string filtro,string diamateria)
        {
            int i = 0;

            // Verifico si el filtro de dias en los cuales se cursa una materia machea con la cantidad de dias que se cursa la materia
            while (i < 7 && ((filtro.Substring(i, 1) == "1" && diamateria.Substring(i, 1) == "1") || 
                             (filtro.Substring(i, 1) == "1" && diamateria.Substring(i, 1) == "0") ||
                             (filtro.Substring(i, 1) == "0" && diamateria.Substring(i, 1) == "0")))
            {
                i++;
            }
            if (i == 7)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static string ReplaceAtIndex(int i, char value, string word)
        {
            char[] letters = word.ToCharArray();
            letters[i] = value;
            return string.Join("", letters);
        }

        public bool AgregarDiasACursar(string dia_materia,ref string dias_a_cursar, int filtrocantdias)
        {
            int i = 0;
            List<int> dias = new List<int>();
            int cant_dias = 0;
            while ( i<7 && ((dia_materia.Substring(i, 1) == "1" && dias_a_cursar.Substring(i, 1) == "0") ||
                            (dia_materia.Substring(i, 1) == "0" && dias_a_cursar.Substring(i, 1) == "0") ||
                            (dia_materia.Substring(i, 1) == "0" && dias_a_cursar.Substring(i, 1) == "1")))
            {
                if (dia_materia.Substring(i, 1) == "1" && dias_a_cursar.Substring(i, 1) == "0")
                    dias.Add(i);
                i++;
            }
            for (int j = 0; j < 7; j++)
            {
                if (dias_a_cursar.Substring(j, 1) != "0")
                    cant_dias++;
            }

            // Le sumo la cantidad de dias de la materia nueva
            cant_dias = cant_dias + dias.Count();

            if (i == 7 && (cant_dias <= filtrocantdias))
            {
                foreach (var indice in dias)
                {
                    dias_a_cursar = ReplaceAtIndex(indice, '1', dias_a_cursar);
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Curso> MostrarMateriasACursarCuatrimestreActual(string matricula,string filtrodias,int filtrocantdias,string modo)
        {
            List<Curso> materias_a_cursar_este_cuatri = GetMateriasACursarCuatrimestreActual(matricula);
            List<Curso> mostrar_materias_a_cursar_este_cuatri = new List<Curso>();
            List<Curso> aux_materias_a_cursar = new List<Curso>();
            string dias_que_cursa = "0000000";

            if (modo == "manual")
            {
                foreach (var materia in materias_a_cursar_este_cuatri)
                {
                    // Para contar cantidad de dias que se cursa una materia
                    //string[] auxdiasacursar = materia.Dia.Split(new Char[] { '1' });
                    //int cantidad_dias_que_se_cursa_materia = auxdiasacursar.Length - 1;

                    // Verifico si el filtro de dias en los cuales se cursa una materia machea con la cantidad de dias que se cursa la materia
                    if (VerificarFiltroDias(filtrodias, materia.Dia))
                    {
                        mostrar_materias_a_cursar_este_cuatri.Add(materia);
                    }
                }
            }
            else
            {
                if (modo == "auto")
                {
                    foreach (var materia in materias_a_cursar_este_cuatri)
                    {
                        // Para contar cantidad de dias que se cursa una materia
                        string[] auxdiasacursar = materia.Dia.Split(new Char[] { '1' });
                        int cantidad_dias_que_se_cursa_materia = auxdiasacursar.Length - 1;
                        
                        if (cantidad_dias_que_se_cursa_materia > 1 && AgregarDiasACursar(materia.Dia, ref dias_que_cursa, filtrocantdias) && VerificarFiltroDias(filtrodias, materia.Dia))
                        {
                            mostrar_materias_a_cursar_este_cuatri.Insert(0, materia);
                        }
                        else
                        {
                                aux_materias_a_cursar.Add(materia);
                        }
                        
                    }
                    foreach (var materia in aux_materias_a_cursar)
                    {
                        if (AgregarDiasACursar(materia.Dia, ref dias_que_cursa, filtrocantdias) && VerificarFiltroDias(filtrodias, materia.Dia))
                        {
                            mostrar_materias_a_cursar_este_cuatri.Add(materia);
                        }
                    }
                }
            }
            return mostrar_materias_a_cursar_este_cuatri;
        }

        public string ObtenerNombreMateria(int materiaid)
        {
            using (var context = new CaeceDBContext())
            {
                return context.Materias_Descripciones.First(x => x.Materia_Id == materiaid).Mat_Des;
            }
        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/Prueba/?filtrodias=1000000&filtrocantdias=1
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("Prueba/{filtrodias}/{filtrocantdias}")]
        public void Prueba(string filtrodias, int filtrocantdias)
        {
            List<Curso> materias = MostrarMateriasACursarCuatrimestreActual("951282", filtrodias, filtrocantdias, "auto");
            //List<Curso> materias = MostrarMateriasACursarCuatrimestreActual("951282", filtrodias, 1, "manual");

            foreach (var materia in materias)
            {
                System.Diagnostics.Debug.WriteLine("Materia a cursar : " + ObtenerNombreMateria(int.Parse(materia.Materia_Id)));
            }
        }
    }
}























