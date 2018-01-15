using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Auth.Models;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    public class SignInController : Controller
    {
        [HttpPost]
        public int Post([FromBody]Auth.Models.User value)
        {
          TestContext context = HttpContext.RequestServices.GetService(typeof(Auth.Models.TestContext)) as TestContext;
          int s = context.PostUser(value);
          return s;

        }
    }
}
