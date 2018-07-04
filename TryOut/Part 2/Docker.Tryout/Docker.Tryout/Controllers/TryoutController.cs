using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Docker.Tryout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TryoutController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> HeartBeat()
        {
            return new string[] { "Beat 1", "Beat 2" };
        }
    }
}
