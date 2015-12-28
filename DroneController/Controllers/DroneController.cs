using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccessLibrary.Models;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary;
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
            IEntityRepository<DroneEntity> repo = RepositoryFactory.getDroneRepository();
            return Ok(repo.getAll());
        }

        [HttpGet]
        [Route("drones/{id}")]
        public IHttpActionResult getById(string id)
        {
            IEntityRepository<DroneEntity> repo = RepositoryFactory.getDroneRepository();
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

        [HttpGet]
        [Route("drones/discover")]
        public IHttpActionResult discover()
        {
            /*
            String settingName = "discoveryPollLength";
            IEntityRepository<SettingEntity> settingsRepo = RepositoryFactory.getSettingRepository();
            SettingEntity pollLength = settingsRepo.getByName(settingName);
            if (null == pollLength)
            {
                pollLength = new SettingEntity();
                pollLength.name = settingName;
                pollLength.value = "60";
                pollLength = settingsRepo.create(pollLength);
                return Ok("Setting Created");
            }
            */
            droneMgr.discover();
            return Ok();
        }


    }
}
