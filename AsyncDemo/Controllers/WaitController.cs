using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AsyncDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WaitController : ControllerBase
    {
        private readonly ILogger<WaitController> _logger;

        public WaitController(ILogger<WaitController> logger)
        {
            _logger = logger;
        }

        [HttpGet("async/{t:int}")]
        public async Task<bool> WaitAsync(int t)
        {
            await Task.Delay(t * 1000);
            return true;
        }

        [HttpGet("async-block/{t:int}")]
        public async Task<bool> WaitBlockAsync(int t)
        {
            Task.Delay(t * 1000).Wait();
            return true;
        }

        [HttpGet("sync/{t:int}")]
        public bool Wait(int t)
        {
            Thread.Sleep(t * 1000);
            return true;
        }
    }
}
