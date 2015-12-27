using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Repositories;

namespace DataAccessLibrary
{
    public class RepositoryFactory
    {
        public static IDroneRepository getDroneRepository()
        {
            return new DroneRepository();
        }
    }
}
