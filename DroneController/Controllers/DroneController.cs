using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DroneManager.Models;
using DroneManager;
using NLog;
using DroneController.DataTransferObjects;

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

        private List<DroneDTO> getActiveRecords()
        {
            // return list of active connection data records (trying to serialize the pure connection object will fail due to the data stream)
            return droneMgr.connections.Select(x => new DroneDTO(x)).ToList();
        }

        [HttpGet]
        [Route("drones/discover")]
        public IHttpActionResult discover()
        {
            logger.Debug("Entering /discover");
            droneMgr.discover();
            return Ok();
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
        [Route("drones/{id}/arm")]
        public IHttpActionResult arm (string id)
        {
            logger.Debug("Arming /drones/{0}", id);
            Drone target = droneMgr.getById(new Guid(id));
            if (null != target)
            {
                target.arm();
                return Ok(target);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("drones/{id}/disarm")]
        public IHttpActionResult disarm(string id)
        {
            logger.Debug("disarming /drones/{0}", id);
            Drone target = droneMgr.getById(new Guid(id));
            if (null != target)
            {
                target.disarm();
                return Ok(new DroneDTO(target));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("drones/{id}/land")]
        public IHttpActionResult land(string id)
        {
            logger.Debug("Landing /drones/{0}", id);
            Drone target = droneMgr.getById(new Guid(id));
            if (null != target)
            {
                target.land();
                return Ok(new DroneDTO(target));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("drones/{id}/returnToLand")]
        public IHttpActionResult returnToLand(string id)
        {
            logger.Debug("Returning /drones/{0} to launch point", id);
            Drone target = droneMgr.getById(new Guid(id));
            if (null != target)
            {
                target.returnToLand();
                return Ok(new DroneDTO(target));
            }
            else
            {
                return NotFound();
            }
        }

    }
}
