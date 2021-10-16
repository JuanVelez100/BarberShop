using System;
using System.Collections.Generic;

#nullable disable

namespace BarbershopLogic.DataBase
{
    public partial class Barbershop
    {
        public Barbershop()
        {
            Barbers = new HashSet<Barber>();
            Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Capacity { get; set; }
        public int? IdCity { get; set; }
        public int? AffiliateBaseCost { get; set; }
        public int? ParticularBaseCost { get; set; }

        public virtual City IdCityNavigation { get; set; }
        public virtual ICollection<Barber> Barbers { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
