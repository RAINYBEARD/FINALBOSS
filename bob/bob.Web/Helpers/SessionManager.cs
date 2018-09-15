using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bob.Data.Dictionaries;

namespace bob.Helpers
{
    public static class SessionManager
    {
        private const string DiccionarioAprobadasKey = "APR";
        private const string DiccionarioCursadasKey = "CUR";
        private const string DiccionarioPendientesKey = "PEN";
        private const string DiccionarioNoCursadasKey = "NOT_CUR";
        private const string DiccionarioCursosKey = "CURSOS";
        private const string DiccionarioMesasFinalKey = "MESAS_FINAL";

        public static AprDictionary DiccionarioAprobadas
        {
            get
            {
                return GetFromSession<AprDictionary>(DiccionarioAprobadasKey);
            }
            set
            {
                SetInSession(DiccionarioAprobadasKey, value);
            }
        }

        public static CurDictionary DiccionarioCursadas
        {
            get
            {
                return GetFromSession<CurDictionary>(DiccionarioCursadasKey);
            }
            set
            {
                SetInSession(DiccionarioCursadasKey, value);
            }
        }

        public static PenDictionary DiccionarioPendientes
        {
            get
            {
                return GetFromSession<PenDictionary>(DiccionarioPendientesKey);
            }
            set
            {
                SetInSession(DiccionarioPendientesKey, value);
            }
        }

        public static NotCurDictionary DiccionarioNoCursadas
        {
            get
            {
                return GetFromSession<NotCurDictionary>(DiccionarioNoCursadasKey);
            }
            set
            {
                SetInSession(DiccionarioNoCursadasKey, value);
            }
        }

        public static CursosDictionary DiccionarioCursos
        {
            get
            {
                return GetFromSession<CursosDictionary>(DiccionarioCursosKey);
            }
            set
            {
                SetInSession(DiccionarioCursosKey, value);
            }
        }
        public static MesasDictionary DiccionarioMesasFinal
        {
            get
            {
                return GetFromSession<MesasDictionary>(DiccionarioMesasFinalKey);
            }
            set
            {
                SetInSession(DiccionarioMesasFinalKey, value);
            }
        }

        private static T GetFromSession<T>(string key)
        {
            return (T)((HttpContext.Current != null && HttpContext.Current.Session[key] != null) ? HttpContext.Current.Session[key] : default(T));
        }

        private static void SetInSession<T>(string key, T value)
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(key);
            }
            else
            {
                HttpContext.Current.Session[key] = value;
            }
        }
    }
}