﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exceptionless.Logging;
using Exceptionless;
using Microsoft.Extensions.Options;
using AspNetCoreWebApi.Samples.Models;

namespace AspNetCoreWebApi.Samples.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IOptions<Person> _options;
        public ValuesController(IOptions<Person> options)
        {
            _options = options;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            
            Exceptionless.ExceptionlessClient.Default.CreateLog("GET", "api/values", LogLevel.Debug).AddTags("CoreApi").Submit();
            return new string[] { "value1", "value2" + _options.Value.Name };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            Exceptionless.ExceptionlessClient.Default.CreateLog("GET", "api/values/5", LogLevel.Warn).AddTags("CoreApi").Submit();
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
