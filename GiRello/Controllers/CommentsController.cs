using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GiRello.Controllers
{
    public class CommentsController : ApiController
    {
        // GET api/comments
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/comments/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/comments
        public void Post([FromBody]string value)
        {
        }

        // PUT api/comments/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/comments/5
        public void Delete(int id)
        {
        }
    }
}
