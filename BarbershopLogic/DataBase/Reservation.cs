using System;
using System.Collections.Generic;

#nullable disable

namespace BarbershopLogic.DataBase
{
    public partial class Reservation
    {
        public int Id { get; set; }
        public int? IdCity { get; set; }
        public string IdClient { get; set; }
        public string IdBarber { get; set; }
        public TimeSpan? Hour { get; set; }
        public DateTime? Date { get; set; }
        public int? Cost { get; set; }
        public string Observation { get; set; }
        public int? IdBarberShop { get; set; }

        public virtual Barber IdBarberNavigation { get; set; }
        public virtual Barbershop IdBarberShopNavigation { get; set; }
        public virtual City IdCityNavigation { get; set; }
        public virtual Client IdClientNavigation { get; set; }
    }
}
