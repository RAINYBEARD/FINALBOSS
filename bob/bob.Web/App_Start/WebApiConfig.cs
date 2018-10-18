using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace bob
{
    public static class WebApiConfig
    {
        public static string UrlPrefixRelative { get { return "~/api"; } }

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }
    }
}