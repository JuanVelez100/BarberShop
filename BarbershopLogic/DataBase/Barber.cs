using System;
using System.Collections.Generic;

#nullable disable

namespace BarbershopLogic.DataBase
{
    public partial class Barber
    {
        public Barber()
        {
            Reservations = new HashSet<Reservation>();
        }

        public string Id { get; set; }
        public int? IdServicio { get; set; }
        public TimeSpan? StartAttentionTime { get; set; }
        public TimeSpan? EndAttentionTime { get; set; }
        public int? IdBarberShop { get; set; }

        public virtual Barbershop IdBarberShopNavigation { get; set; }
        public virtual Person IdNavigation { get; set; }
        public virtual Service IdServicioNavigation { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
