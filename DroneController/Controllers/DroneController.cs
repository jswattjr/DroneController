using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DroneManager.Models;
using DroneManager;
using NLog;
using DataAccessLibrary.Models;

namespace DroneController.Controllers
{
    public class DroneController : ApiController
    {
        static DroneManagementStation droneMgr = new DroneManagementStation();
        static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        [Route("drones")]
        public IHttpActionResult get()
        {
            logger.Debug("Entering /drones GET");

            return Ok(this.getActiveRecords());
        }

        private List<DroneEntity> getActiveRecords()
        {
            // return list of active connection data records (trying to serialize the pure connection object will fail due to the data stream)
            return droneMgr.connections.Select(x => x.data).ToList();
        }

        [HttpGet]
        [Route("drones/{id}")]
        public IHttpActionResult getById(string id)
        {
            logger.Debug("Fetching /drones/{0}", id);
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
            logger.Debug("Entering /discover");
            droneMgr.discover();
            return Ok();
        }

    }
}
