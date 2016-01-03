using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccessLibrary.Repositories;
using DataAccessLibrary.Models;

namespace DroneController.Controllers
{
    public class LogsController : ApiController
    {
        [HttpGet]
        [Route("logs")]
        public IHttpActionResult get()
        {
            LogRepository logRepo = DataAccessLibrary.RepositoryFactory.getLogsRepository();
            return Ok(logRepo.getAll());
        }
    }
}
