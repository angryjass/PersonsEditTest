using PersonsEdit.API.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonsEdit.API.Models
{
    public class PersonDto
    {
        public PersonDto(Person person, List<Role> roles)
        {
            Person = person;
            Roles = roles;
        }
        public Person Person { get; set; }
        public List<Role> Roles { get; set; }
    }
}
