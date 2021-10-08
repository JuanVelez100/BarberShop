using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbershopEntity.Entity
{
    public class PersonEntity
    {
        [Required(ErrorMessage = "Identificacion Requerida")]
        [RegularExpression("[0-9]*")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Nombre Requerido")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Apellido Requerido")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Contraseña Requerida")]
        [StringLength(100, MinimumLength = 10)]
        public string Passwork { get; set; }

        [Required(ErrorMessage = "Celular Requerido")]
        [Phone]
        [StringLength(10, MinimumLength = 7)]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Correo Requerido")]
        [EmailAddress]
        public string Mail { get; set; }

    }
}
