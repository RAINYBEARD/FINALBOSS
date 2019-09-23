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
        //public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        //{

        //    string clientId = string.Empty;
        //    string clientSecret = string.Empty;
        //    Client client = null;

        //    if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
        //    {
        //        context.TryGetFormCredentials(out clientId, out clientSecret);
        //    }

        //    if (context.ClientId == null)
        //    {
        //        //Remove the comments from the below line context.SetError, and invalidate context 
        //        //if you want to force sending clientId/secrets once obtain access tokens. 
        //        //context.Validated();
        //        context.SetError("invalid_clientId", "ClientId should be sent.");
        //    }
        //    try
        //    {
        //        using (var db = new CaeceDBContext())
        //        {
        //            client = db.Clients.Find(context.ClientId);
        //        }

        //        if (client == null)
        //        {
        //            context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
        //        }

        //        if (client.Application_Type == ApplicationTypes.NativeConfidential)
        //        {
        //            if (string.IsNullOrWhiteSpace(clientSecret))
        //            {
        //                context.SetError("invalid_clientId", "Client secret should be sent.");
        //            }
        //            else
        //            {
        //                if (client.Secret != PasswordHash.HashPassword(clientSecret))
        //                {
        //                    context.SetError("invalid_clientId", "Client secret is invalid.");
        //                }
        //            }
        //        }

        //        if (!client.Active)
        //        {
        //            context.SetError("invalid_clientId", "Client is inactive.");
        //        }

        //        context.OwinContext.Set<string>("as:clientAllowedOrigin", client.Allowed_Origin);
        //        context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.Refresh_Token_Life_Time.ToString());

        //        context.Validated();

        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}

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
                        var user = db.Alumnos.First(a => a.Matricula == username);

                        if (user == null)
                        {
                            context.SetError("invalid_grant", "The user name or password is incorrect.");
                            return;
                        }

                        var validPass = PasswordHash.ValidatePassword(context.Password, user.Password);

                        if (!validPass)
                        {
                            context.SetError("invalid_grant", "The user name or password is incorrect.");
                            return;
                        }
                    }
                    else {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim("sub", context.UserName));
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

        //public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        //{
        //    foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
        //    {
        //        context.AdditionalResponseParameters.Add(property.Key, property.Value);
        //    }

        //    return Task.FromResult<object>(null);
        //}

        //public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        //{
        //    var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
        //    var currentClient = context.ClientId;

        //    if (originalClient != currentClient)
        //    {
        //        context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
        //        return Task.FromResult<object>(null);
        //    }

        //    // Change auth ticket for refresh token requests
        //    var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
        //    newIdentity.AddClaim(new Claim("newClaim", "newValue"));

        //    var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
        //    context.Validated(newTicket);

        //    return Task.FromResult<object>(null);
        //}
    }
}