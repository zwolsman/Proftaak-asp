using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DatabaseLibrary;
using DatabaseWebApi.Models;

namespace DatabaseWebApi.Controllers
{
    public class BerichtenController : ApiController
    {
        // GET: api/Berichten
        public IEnumerable<Bericht> Get()
        {
            return DatabaseManager.GetItems<Bericht>();
        }

        // GET: api/Berichten/5
        public Bericht Get(int id)
        {
            return DatabaseManager.GetItem<Bericht>(new SearchCriteria
            {
                {"ID", id}
            });
        }

        // POST: api/Berichten
        public void Post([FromBody]string value)
        {

        }

        // PUT: api/Berichten/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Berichten/5
        public void Delete(int id)
        {
        }
    }
}
