#region

using System.Collections.Generic;
using System.Web.Http;
using Highway.Data.Tests.TestDomain;

#endregion

namespace Highway.Data.Rest.ExampleAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new[] {"value1", "value2"};
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

    public class FooController : ApiController
    {
        public IEnumerable<Foo> Get()
        {
            return new List<Foo>
            {
                new Foo {Address = "Test", Id = 1, Name = "Test"},
                new Foo {Address = "Devlin", Id = 1, Name = "Devlin"}
            };
        }
    }
}