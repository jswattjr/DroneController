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
    public class SettingsController : ApiController
    {
        [HttpGet]
        [Route("settings")]
        public IHttpActionResult get()
        {
            IEntityRepository<SettingEntity> repo = RepositoryFactory.getSettingRepository();
            return Ok(repo.getAll());
        }

        [HttpGet]
        [Route("settings/{setting}")]
        public IHttpActionResult getSettingValue(string setting)
        {
            IEntityRepository<SettingEntity> repo = RepositoryFactory.getSettingRepository();
            SettingEntity settingEntity = repo.getByName(setting);
            if (null == settingEntity)
            {
                return NotFound();
            }
            return Ok(settingEntity);
        }

    }
}
