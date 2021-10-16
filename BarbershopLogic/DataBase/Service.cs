using System;
using System.Collections.Generic;

#nullable disable

namespace BarbershopLogic.DataBase
{
    public partial class Service
    {
        public Service()
        {
            Barbers = new HashSet<Barber>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AffiliateCost { get; set; }
        public int? ParticularCost { get; set; }

        public virtual ICollection<Barber> Barbers { get; set; }
    }
}
