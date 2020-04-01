using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TaskSchedulingWithHangfire.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HangfireController : ControllerBase
    {

        private readonly ILogger<HangfireController> _logger;

        public HangfireController(ILogger<HangfireController> logger)
        {
            _logger = logger;
        }
    }
}
