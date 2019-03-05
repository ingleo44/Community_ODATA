using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adventureworks.Model;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adventureworks.Controllers
{
    [Route("api/[controller]")]
    [ODataRoutePrefix("EntityWithEnum")]
    public class EntityWithEnumController : ODataController
    {
        private List<PhoneNumberTypeWithEnum.EntityWithEnum> someData = new List<PhoneNumberTypeWithEnum.EntityWithEnum>();

        public EntityWithEnumController()
        {
            someData.Add(new PhoneNumberTypeWithEnum.EntityWithEnum { Description = "test1", Name = "Van", PhoneNumberType = PhoneNumberTypeWithEnum.PhoneNumberTypeEnum.Home });
            someData.Add(new PhoneNumberTypeWithEnum.EntityWithEnum { Description = "test2", Name = "Bill", PhoneNumberType = PhoneNumberTypeWithEnum.PhoneNumberTypeEnum.Work });
            someData.Add(new PhoneNumberTypeWithEnum.EntityWithEnum { Description = "test3", Name = "Rob", PhoneNumberType = PhoneNumberTypeWithEnum.PhoneNumberTypeEnum.Cell });
        }

        [ODataRoute]
        [EnableQuery(PageSize = 20)]
        public IActionResult Get()
        {
            return Ok(someData);
        }

        [ODataRoute]
        [EnableQuery(PageSize = 20)]
        public IActionResult Get([FromODataUri] string key)
        {
            if (someData.Exists(t => t.Name == key))
            {
                return Ok(someData.FirstOrDefault(t => t.Name == key));
            }

            return BadRequest("key does not key");
        }

        [HttpPost]
        [ODataRoute]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IActionResult Post(PhoneNumberTypeWithEnum.EntityWithEnum entityWithEnum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            return Created(entityWithEnum);
        }

        [HttpGet]
        [EnableQuery(PageSize = 20, AllowedQueryOptions = AllowedQueryOptions.All)]
        [ODataRoute("Default.PersonSearchPerPhoneType(PhoneNumberTypeEnum={phoneNumberTypeEnum})")]
        public IActionResult PersonSearchPerPhoneType([FromODataUri] PhoneNumberTypeWithEnum.PhoneNumberTypeEnum phoneNumberTypeEnum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(someData.Where(t => t.PhoneNumberType.Equals(phoneNumberTypeEnum)));
        }

    }
}