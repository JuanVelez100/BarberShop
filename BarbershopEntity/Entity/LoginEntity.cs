using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbershopEntity.Entity
{
    public class LoginEntity
    {
        [Required(ErrorMessage ="Identificacion Requerida") ]
        public string Id { get; set; }
        [Required(ErrorMessage = "Contraseña Requerida")]
        public string Passwork { get; set; }

    }
}
