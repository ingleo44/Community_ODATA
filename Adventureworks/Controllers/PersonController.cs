using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adventureworks.Core.Supervisor.Classes;
using Adventureworks.Core.Supervisor.Interfaces;
using Adventureworks.Entities.Converters;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adventureworks.Controllers
{
    [ODataRoutePrefix("Person")]
    public class PersonController : ODataController
    {
        private readonly IPersonSupervisor _personSupervisor;

        public PersonController(IPersonSupervisor personSupervisor)
        {
            _personSupervisor = personSupervisor;
        }


        [ODataRoute]
        [EnableQuery(PageSize = 20, AllowedQueryOptions = AllowedQueryOptions.All)]
        public async Task<IActionResult> Get()
        {
            var result = await _personSupervisor.Getall();
            return Ok(result.AsQueryable());
        }

        [ODataRoute("({key})")]
        [EnableQuery(PageSize = 20, AllowedQueryOptions = AllowedQueryOptions.All)]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var result = await _personSupervisor.Getall();
            return Ok(result.Where(q => q.BusinessEntityId == key).AsQueryable());
            
        }

        [EnableQuery(PageSize = 20, AllowedQueryOptions = AllowedQueryOptions.All)]
        [ODataRoute("Default.MyFirstFunction")]
        [HttpGet]
        public async Task<IActionResult> MyFirstFunction()
        {
            var result = await _personSupervisor.Getall();
            return   Ok(result.Where(q => q.FirstName.StartsWith("K")).AsQueryable());           
        }


    }
}
