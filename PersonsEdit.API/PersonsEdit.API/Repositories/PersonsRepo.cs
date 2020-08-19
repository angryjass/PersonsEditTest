using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PersonsEdit.API.Entity;
using PersonsEdit.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonsEdit.API.Repositories
{
    public class PersonsRepo
    {
        private static PersonsContext context = new PersonsContext();
        public static PersonDto GetPerson(string login, string password)
        {
            var person = context.Persons.FirstOrDefault(p => p.Login == login && p.Password == password);

            if (person != null)
            {
                var rolesIds = context.PersonsRoles.Where(pr => pr.PersonId == person.Id).Select(r => r.RoleId);
                var roles = context.Roles.Where(r => rolesIds.Contains(r.Id)).ToList();

                return new PersonDto(person, roles);
            }

            return null;
        }

        public static List<PersonDto> GetAllPersons()
        {
            var persons = context.Persons.ToList();
            var personsAndRoles = new List<PersonDto>();

            foreach (var person in persons)
            {
                var rolesIds = context.PersonsRoles.Where(pr => pr.PersonId == person.Id).Select(r => r.RoleId).ToList();
                var roles = context.Roles.Where(r => rolesIds.Contains(r.Id)).ToList();

                personsAndRoles.Add(new PersonDto(person, roles));
            }

            return personsAndRoles;
        }

        public static void DeletePerson(int personId)
        {
            var person = context.Persons.First(p => p.Id == personId);

            context.PersonsRoles.RemoveRange(context.PersonsRoles.Where(p => p.PersonId == personId));
            context.Persons.Remove(person);
            context.SaveChanges();
        }

        public static PersonDto EditOrCreatePerson(Person person, List<Role> roles)
        {
            var personForEdit = context.Persons.FirstOrDefault(p => p.Id == person.Id);
            bool isNewPerson = false;

            if (personForEdit == null)
            {
                context.Persons.Add(person);
                isNewPerson = true;
            }
            else
            {
                personForEdit.Login = person.Login;
                personForEdit.Name = person.Name;
                personForEdit.Password = person.Password;
                personForEdit.Email = person.Email;
                
                context.PersonsRoles.RemoveRange(context.PersonsRoles.Where(p => p.PersonId == personForEdit.Id));
            }

            context.SaveChanges();

            foreach (var role in roles)
            {
                var personsRole = new PersonsRoles() { PersonId = person.Id, RoleId = role.Id };
                context.PersonsRoles.Add(personsRole);
            }
            context.SaveChanges();

            if (isNewPerson)
            {
                return new PersonDto(person, roles);
            }
            else
            {
                return null;
            }

        }
    }
}
