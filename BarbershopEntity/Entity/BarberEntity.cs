using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbershopEntity.Entity
{
    public class BarberEntity : PersonEntity
    {
        public string Id { get; set; }
        public int? IdServicio { get; set; }
        public TimeSpan? StartAttentionTime { get; set; }
        public TimeSpan? EndAttentionTime { get; set; }
        public int? IdBarberShop { get; set; }

        public string NameBarberShop { get; set; }







    }
}
