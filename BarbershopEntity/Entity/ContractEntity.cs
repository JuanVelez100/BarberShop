using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BarbershopEntity.Helper.Emun;

namespace BarbershopEntity.Entity
{
    public class ContractEntity
    {
        public TypeAffiliation TypeAffiliation { get; set; }
        public TimeSpan StartAttentionTime { get; set; }
        public TimeSpan EndAttentionTime { get; set; }
        public int MonthlyPayment { get; set; }
        public int DiscountRate { get; set; }
    }
}
