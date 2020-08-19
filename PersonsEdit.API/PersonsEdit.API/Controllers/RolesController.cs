using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonsEdit.API.Entity;
using PersonsEdit.API.Models;
using PersonsEdit.API.Repositories;

namespace PersonsEdit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public ActionResult<List<Role>> Get()
        {
            try
            {
                return Ok(RolesRepo.GetAllRoles());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
