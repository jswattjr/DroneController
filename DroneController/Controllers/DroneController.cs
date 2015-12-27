using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccessLibrary.Models;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary;

namespace DroneController.Controllers
{
    public class DroneController : ApiController
    {
        [HttpGet]
        [Route("drones")]
        public IHttpActionResult get()
        {
            IDroneRepository repo = RepositoryFactory.getDroneRepository();
            return Ok(repo.getAll());
        }

        [HttpGet]
        [Route("drones/{id}")]
        public IHttpActionResult getById(string id)
        {
            IDroneRepository repo = RepositoryFactory.getDroneRepository();
            DroneEntity target = repo.getById(new Guid(id));
            if (null != target)
            {
                return Ok(target);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
