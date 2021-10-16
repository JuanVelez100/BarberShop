using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbershopEntity.Entity
{
    public class ReservationEntity
    {
        public int Id { get; set; }
        public int? IdCity { get; set; }
        public int? IdBarberShop { get; set; }
        public string IdClient { get; set; }
        public string IdBarber { get; set; }
        public TimeSpan? Hour { get; set; }
        [Required(ErrorMessage = "Fecha Requerida")]
        public DateTime? Date { get; set; }
        public int? Cost { get; set; }
        public string Observation { get; set; }

    }
}
