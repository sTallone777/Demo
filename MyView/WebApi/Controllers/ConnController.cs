using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.DBModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnController : ControllerBase
    {
        private readonly DBContext _DbContext;

        public ConnController(DBContext dbContext)
        {
            _DbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MDriver>>> Get()
        {
            return await _DbContext.MDriver.ToListAsync();
        }
    }
}