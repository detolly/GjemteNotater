using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GjemteNotaterApiIi.Controllers
{
    [Produces("application/json")]
    [Route("api/notat")]
    public class NotatController : Controller
    {
        // GET: api/notat

        Dictionary<string, string[]> notater = new Dictionary<string, string[]> {
            {
                "Yeetings",
                new string[] {
                    "Greetings!",
                    "Greetings2!",
                    "Greetings3"
                }
            },
            {
                "Yeetings2",
                new string[] {
                    "Greetings2!",
                    "Greetings3!",
                    "Greetings4"
                }
            },
        };

        [HttpGet]
        public Dictionary<string, string[]> Get()
        {
            return notater;
        }

        [HttpPost]
        public string Post([FromBody]string name, [FromBody]string[] content)
        {
            try
            {
                notater.Add(name, content);
            } catch
            {
                return "{\"returned\":\"fail\"}";
            }
            return "{\"returned\":\"success\"}";
        }
    }
}
