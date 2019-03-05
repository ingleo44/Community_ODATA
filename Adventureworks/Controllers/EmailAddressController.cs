using System.Linq;
using Infrastructure;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adventureworks.Controllers
{
    public class EmailAddressController : ODataController
    {
        private AdventureWorks2017Context _db;

        public EmailAddressController(AdventureWorks2017Context AdventureWorks2017Context)
        {
            _db = AdventureWorks2017Context;
        }

        [EnableQuery(PageSize = 20)]
        public IActionResult Get()
        {
            return Ok(_db.EmailAddress.AsQueryable());
        }

        [EnableQuery(PageSize = 20)]
        public IActionResult Get([FromODataUri] int key)
        {
            return Ok(_db.EmailAddress.Find(key));
        }
    }
}