using System;
using System.Collections.Generic;

#nullable disable

namespace BarbershopLogic.DataBase
{
    public partial class Client
    {
        public string Id { get; set; }
        public int? TypeAffiliation { get; set; }

        public virtual Person IdNavigation { get; set; }
        public virtual Contract TypeAffiliationNavigation { get; set; }
    }
}
