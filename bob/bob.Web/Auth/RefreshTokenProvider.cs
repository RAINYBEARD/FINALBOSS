using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using bob.Data;
using bob.Data.Auth;
namespace bob.Auth
{
    public class RefreshTokenProvider: IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (CaeceDBContext db = new CaeceDBContext())
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                var token = new RefreshToken()
                {
                    Id = Helpers.PasswordHash.HashPassword(refreshTokenId),
                    Client_Id = clientid,
                    Subject = context.Ticket.Identity.Name,
                    Issued_Utc = DateTime.UtcNow,
                    Expires_Utc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = token.Issued_Utc;
                context.Ticket.Properties.ExpiresUtc = token.Expires_Utc;

                token.Protected_Ticket = context.SerializeTicket();

                var result = db.RefreshTokens.Add(token);
                db.SaveChanges();

                if (result != null)
                {
                    context.SetToken(refreshTokenId);
                }

            }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = Helpers.PasswordHash.HashPassword(context.Token);

            using (CaeceDBContext db = new CaeceDBContext())
            {
                var refreshToken = db.RefreshTokens.Find(hashedTokenId);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.Protected_Ticket);
                    var result = db.RefreshTokens.Remove(refreshToken);
                    db.SaveChanges();
                }
            }
        }
    }
}