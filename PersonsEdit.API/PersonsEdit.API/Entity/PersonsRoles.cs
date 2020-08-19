using System;
using System.Collections.Generic;

namespace PersonsEdit.API.Entity
{
    public partial class PersonsRoles
    {
        public int PersonId { get; set; }
        public int RoleId { get; set; }

        public virtual Person Person { get; set; }
        public virtual Role Role { get; set; }
    }
}
