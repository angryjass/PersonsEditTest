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
    public class RolesRepo
    {
        private static PersonsContext context = new PersonsContext();
        
        public static List<Role> GetAllRoles()
        {
            return context.Roles.ToList();
        }
    }
}
