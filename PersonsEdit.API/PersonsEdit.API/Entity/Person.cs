using System;
using System.Collections.Generic;

namespace PersonsEdit.API.Entity
{
    public partial class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
