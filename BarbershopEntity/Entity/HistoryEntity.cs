using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbershopEntity.Entity
{
    public class HistoryEntity
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string BarberShop { get; set; }
        public string Client { get; set; }
        public string Barber { get; set; }
        public TimeSpan? Hour { get; set; }
        public DateTime? Date { get; set; }
        public int? Cost { get; set; }
        public string Observation { get; set; }
        public string Service { get; set; }
    }
}
