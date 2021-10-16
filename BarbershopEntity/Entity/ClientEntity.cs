using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BarbershopEntity.Helper.Emun;

namespace BarbershopEntity.Entity
{
    public class ClientEntity : PersonEntity 
    {
        public TypeAffiliation TypeAffiliation { get; set; }
    }
}
