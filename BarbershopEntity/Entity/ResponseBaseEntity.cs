using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BarbershopEntity.Helper.Emun;

namespace BarbershopEntity.Entity
{
    public class ResponseBaseEntity
    {
        public string Message { get; set; }
        public TypeMessage Type { get; set; }
    }
}
