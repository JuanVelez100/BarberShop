using BarberEntity.Entity;
using BarberShoreLogic.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberShoreLogic.Logic
{
    public class PersonaLogic
    {
        BarberShoreDataBaseContext barberShoreDataBaseContext = new BarberShoreDataBaseContext();


        public List<PersonaEntity> GetAllPerson()
        {
            List<PersonaEntity> listPersonEntites = new List<PersonaEntity>();


           var listPersonDataBase = barberShoreDataBaseContext.Personas.ToList();

            foreach (var peronDataBase in listPersonDataBase)
            {
                PersonaEntity personaEntity = new PersonaEntity();
                personaEntity.Cedula = peronDataBase.Cedula;
                personaEntity.Nombre = peronDataBase.Nombre;
                personaEntity.Celular = peronDataBase.Celular;
                personaEntity.Correo = peronDataBase.Correo;
                personaEntity.Contraseña = peronDataBase.Contraseña;


                listPersonEntites.Add(personaEntity);
            }

            return listPersonEntites;
        }




    }
}
