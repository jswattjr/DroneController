using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DroneManager.Models;
using DroneManager;

namespace DroneController.Controllers
{
    public class DroneController : ApiController
    {
        static DroneManagementStation droneMgr = new DroneManagementStation();

        [HttpGet]
        [Route("drones")]
        public IHttpActionResult get()
        {
            return Ok(droneMgr.connections);
        }

        [HttpGet]
        [Route("drones/{id}")]
        public IHttpActionResult getById(string id)
        {
            Drone target = droneMgr.getById(new Guid(id));
            if (null != target)
            {
                return Ok(target);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("drones/discover")]
        public IHttpActionResult discover()
        {
            droneMgr.discover();
            return Ok();
        }

    }
}
