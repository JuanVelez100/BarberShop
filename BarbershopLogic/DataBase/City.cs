using System;
using System.Collections.Generic;

#nullable disable

namespace BarbershopLogic.DataBase
{
    public partial class City
    {
        public City()
        {
            Barbershops = new HashSet<Barbershop>();
            Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Barbershop> Barbershops { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
