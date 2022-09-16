﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AccountAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

       
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

       
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5      --comment
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5    ---comment
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
