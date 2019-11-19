using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using System.Collections.Generic;
using bob.Helpers;
using System.Linq;
using bob.Data;
using bob.Data.Auth;
using System;

namespace bob.Auth
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";
            var username = context.UserName.PadLeft(7, ' ');
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            try
            {
                using (var db = new CaeceDBContext())
                {
                    if (db.Alumnos.Count() > 0)
                    {
                        var user = db.Alumnos.FirstOrDefault(a => a.Matricula == username);

                        if (user == null)
                        {
                            context.SetError("invalid_grant", "Matricula o contraseña incorrectos.");
                            return;
                        }

                        var validPass = PasswordHash.ValidatePassword(context.Password, user.Password);

                        if (!validPass)
                        {
                            context.SetError("invalid_grant", "Matricula o contraseña incorrectos.");
                            return;
                        }
                    }
                    else {
                        context.SetError("invalid_grant", "Matricula o contraseña incorrectos.");
                        return;
                    }
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim("sub", context.UserName));

                if (username == "0000000")
                    identity.AddClaim(new Claim("role", "admin"));
                else
                    identity.AddClaim(new Claim("role", "user"));

                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "username", context.UserName
                    }
                });

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);

            }
            catch (Exception e)
            {
                throw;
            }

        }

       
    }
}