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
using DroneManager.Models.MessageContainers;
using DroneController.DataTransferObjects.Commands;

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

        IHttpActionResult commandNoParams(string id, string action)
        {
            logger.Debug("running command {1} on /drones/{0}", id, action);
            Drone target = droneMgr.getById(new Guid(id));
            if (null != target)
            {
                if (!target.isConnected())
                {
                    return BadRequest("Target system is not connected, refusing request");
                }
                CommandAck result = runCommand(target, action);
                if (null == result)
                {
                    return NotFound();
                }
                return Ok(new CommandAckDTO(result));
            }
            else
            {
                return NotFound();
            }
        }

        CommandAck runCommand(Drone target, string action)
        {
            if (action.Equals("arm"))
            {
                return target.Command.arm();
            }
            if (action.Equals("disarm"))
            {
                return target.Command.disarm();
            }
            if (action.Equals("returnToLaunch"))
            {
                return target.Command.returnToLaunch();
            }
            return null;
        }

        [HttpPost]
        [Route("drones/{id}/arm")]
        public IHttpActionResult armCommand(string id)
        {
            return commandNoParams(id, "arm");
        }

        [HttpPost]
        [Route("drones/{id}/disarm")]
        public IHttpActionResult disarmCommand(string id)
        {
            return commandNoParams(id, "disarm");
        }

        [HttpPost]
        [Route("drones/{id}/returnToLaunch")]
        public IHttpActionResult rtlCommand(string id)
        {
            return commandNoParams(id, "returnToLaunch");
        }


        [HttpPost]
        [Route("drones/{id}/landAtLocation")]
        public IHttpActionResult landAtLocation(string id, [FromBody] LandAtLocationAction parameters)
        {
            String action = "landAtLocation";
            logger.Debug("running command {1} on /drones/{0}", id, action);
            Drone target = droneMgr.getById(new Guid(id));
            if (null != target)
            {
                if (!target.isConnected())
                {
                    return BadRequest("Target system is not connected, refusing request");
                }
                CommandAck result = target.Command.landAtLocation(parameters.latitude, parameters.longitude, parameters.altitude);
                if (null == result)
                {
                    return NotFound();
                }
                return Ok(new CommandAckDTO(result));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("drones/{id}/loiterTime")]
        public IHttpActionResult loiterTime(string id, [FromBody] LoiterTimeAction parameters)
        {
            String action = "loiterTime";
            logger.Debug("running command {1} on /drones/{0}", id, action);
            Drone target = droneMgr.getById(new Guid(id));
            if (null != target)
            {
                if (!target.isConnected())
                {
                    return BadRequest("Target system is not connected, refusing request");
                }
                CommandAck result = target.Command.loiterTime(parameters.loiterTimeSeconds, parameters.radiusMeters, parameters.latitude, parameters.longitude, parameters.altitude);
                if (null == result)
                {
                    return NotFound();
                }
                return Ok(new CommandAckDTO(result));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("drones/{id}/loiterTurns")]
        public IHttpActionResult loiterTurns(string id, [FromBody] LoiterTurnsAction parameters)
        {
            String action = "loiterTurns";
            logger.Debug("running command {1} on /drones/{0}", id, action);
            Drone target = droneMgr.getById(new Guid(id));
            if (null != target)
            {
                if (!target.isConnected())
                {
                    return BadRequest("Target system is not connected, refusing request");
                }
                CommandAck result = target.Command.loiterTurns(parameters.numTurns, parameters.radiusMeters, parameters.latitude, parameters.longitude, parameters.altitude);
                if (null == result)
                {
                    return NotFound();
                }
                return Ok(new CommandAckDTO(result));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("drones/{id}/loiterUnlimited")]
        public IHttpActionResult loiterUnlimited(string id, [FromBody] LoiterUnlimitedAction parameters)
        {
            String action = "loiterUnlimited";
            logger.Debug("running command {1} on /drones/{0}", id, action);
            Drone target = droneMgr.getById(new Guid(id));
            if (null != target)
            {
                if (!target.isConnected())
                {
                    return BadRequest("Target system is not connected, refusing request");
                }
                CommandAck result = target.Command.loiterUnlimited(parameters.radiusMeters, parameters.latitude, parameters.longitude, parameters.altitude);
                if (null == result)
                {
                    return NotFound();
                }
                return Ok(new CommandAckDTO(result));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("drones/{id}/navigateWaypoint")]
        public IHttpActionResult navigateWaypoint(string id, [FromBody] NavigateWaypointAction parameters)
        {
            String action = "navigateWaypoint";
            logger.Debug("running command {1} on /drones/{0}", id, action);
            Drone target = droneMgr.getById(new Guid(id));
            if (null != target)
            {
                if (!target.isConnected())
                {
                    return BadRequest("Target system is not connected, refusing request");
                }
                CommandAck result = target.Command.navigateWaypoint(parameters.latitude, parameters.longitude, parameters.altitude);
                if (null == result)
                {
                    return NotFound();
                }
                return Ok(new CommandAckDTO(result));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("drones/{id}/takeoff")]
        public IHttpActionResult takeoff(string id, [FromBody] TakeoffAction parameters)
        {
            String action = "takeoff";
            logger.Debug("running command {1} on /drones/{0}", id, action);
            Drone target = droneMgr.getById(new Guid(id));
            if (null != target)
            {
                if (!target.isConnected())
                {
                    return BadRequest("Target system is not connected, refusing request");
                }
                CommandAck result = target.Command.takeoff(parameters.heightMeters);
                if (null == result)
                {
                    return NotFound();
                }
                return Ok(new CommandAckDTO(result));
            }
            else
            {
                return NotFound();
            }
        }
    }
}
