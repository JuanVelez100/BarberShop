using System;
using System.Collections.Generic;

#nullable disable

namespace BarbershopLogic.DataBase
{
    public partial class Contract
    {
        public Contract()
        {
            Clients = new HashSet<Client>();
        }

        public int TypeAffiliation { get; set; }
        public TimeSpan StartAttentionTime { get; set; }
        public TimeSpan EndAttentionTime { get; set; }
        public int? MonthlyPayment { get; set; }
        public int? DiscountRate { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}
