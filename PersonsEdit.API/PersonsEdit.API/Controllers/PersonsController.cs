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
    public class PersonsController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public ActionResult<List<PersonDto>> Get()
        {
            try
            {
                return Ok(PersonsRepo.GetAllPersons());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult<PersonDto> Post([FromBody] object obj)
        {
            try
            {
                var person = JsonConvert.DeserializeObject<PersonDto>(obj.ToString());
                return Ok(PersonsRepo.EditOrCreatePerson(person.Person, person.Roles));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        public ActionResult Delete([FromQuery] int personId)
        {
            try
            {
                PersonsRepo.DeletePerson(personId);
                return Ok(PersonsRepo.GetAllPersons());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
