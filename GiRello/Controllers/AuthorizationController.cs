using GiRello.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Management;

namespace GiRello.Controllers
{
    public class AuthorizationController : ApiController
    {
        private AuthContext db = new AuthContext();

        // GET api/Authorization
        public IEnumerable<Auth> GetAuths()
        {
            return db.Auths.AsEnumerable();
        }

        // GET api/Authorization/5
        public Auth GetAuth(string id)
        {
            new LogEvent(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString).Raise();
            Auth auth = db.Auths.Find(id);
            if (auth == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return auth;
        }

        // PUT api/Authorization/5
        public HttpResponseMessage PutAuth(string id, Auth auth)
        {
            if (ModelState.IsValid && id == auth.Token)
            {
                db.Entry(auth).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Authorization
        public HttpResponseMessage PostAuth([FromBody] Auth auth)
        {
            if (ModelState.IsValid && auth != null)
            {
                db.Auths.Add(auth);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, auth);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = auth.Token }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Authorization/5
        public HttpResponseMessage DeleteAuth(string id)
        {
            Auth auth = db.Auths.Find(id);
            if (auth == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Auths.Remove(auth);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, auth);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private class LogEvent : WebRequestErrorEvent
        {
            public LogEvent(string message)
                : base(null, null, 100001, new Exception(message))
            { 
            }
        }
    }
}