using System;
using System.Collections.Generic;

#nullable disable

namespace BarbershopLogic.DataBase
{
    public partial class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Passwork { get; set; }
        public string Mobile { get; set; }
        public string Mail { get; set; }

        public virtual Barber Barber { get; set; }
        public virtual Client Client { get; set; }
    }
}
