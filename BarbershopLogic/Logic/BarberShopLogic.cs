using BarbershopEntity.Entity;
using BarbershopLogic.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BarbershopEntity.Helper.Emun;

namespace BarbershopLogic.Logic
{
    public class BarberShopLogic
    {
        BarberShopDataBaseContext barberShopDataBaseContext = new BarberShopDataBaseContext();

        public bool VefirySession(LoginEntity loginEntity)
        {
            var client = barberShopDataBaseContext.Clients.Where(x => x.Id == loginEntity.Id).FirstOrDefault();
            if (client == null) return false;
            var person = barberShopDataBaseContext.People.Where(x => x.Id == loginEntity.Id && x.Passwork == loginEntity.Passwork).FirstOrDefault();
            if (person == null) return false;

            return true;
        }

        public ResponseBaseEntity CreateClient(ClientEntity clientEntity)
        {
            using (var dbContextTransaction = barberShopDataBaseContext.Database.BeginTransaction())
            {
                try
                {
                    var client = barberShopDataBaseContext.Clients.Where(x => x.Id == clientEntity.Id).FirstOrDefault();

                    if (client != null)
                    {
                        return GetResponseBaseEntity("Ya esxiste un usuario con este Id", TypeMessage.danger);
                    }

                    barberShopDataBaseContext.People.Add(ConvertClientEntityToPerson(clientEntity));
                    barberShopDataBaseContext.Clients.Add(ConvertClientEntityToClient(clientEntity));
                    barberShopDataBaseContext.SaveChanges();
                    dbContextTransaction.Commit();

                    return GetResponseBaseEntity("Cliente Creado con exito !", TypeMessage.success);

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return GetResponseBaseEntity(ex.Message, TypeMessage.danger);
                }
            }
        }

        public ClientEntity GetClientEntityForId(string id)
        {

            var person = barberShopDataBaseContext.People.Where(x => x.Id == id).FirstOrDefault();

            if (person != null)
            {
                return ConvertPersonToClientEntity(person);
            }

           return new ClientEntity();
        }

        public List<City> GetAllCities()
        {
            List<City> cities = new List<City>();
            cities = barberShopDataBaseContext.Cities.ToList();
            return cities;
        }

        public List<Barbershop> GetAllBarbershop(int city)
        {
            List<Barbershop> barbershops = new List<Barbershop>();
            barbershops = barberShopDataBaseContext.Barbershops.Where(x=> x.IdCity == city).ToList();
            return barbershops;
        }

        public List<BarberEntity> GetAllBarbers(int barbershop)
        {
            List<BarberEntity> barbers = new List<BarberEntity>();

            var barbersDataBase = barberShopDataBaseContext.Barbers.Where(x => x.IdBarberShop == barbershop).ToList();

            foreach (var barberDataBase in barbersDataBase)
            {
                BarberEntity barberEntity = new BarberEntity();
                barberEntity.Id = barberDataBase.Id;
                barberEntity.IdBarberShop = barberDataBase.IdBarberShop;
                barberEntity.StartAttentionTime = barberDataBase.StartAttentionTime;
                barberEntity.EndAttentionTime = barberDataBase.EndAttentionTime;
                barberEntity.IdServicio = barberDataBase.IdServicio;

                var personDataBase = barberShopDataBaseContext.People.FirstOrDefault(x=> x.Id == barberDataBase.Id);

                barberEntity.Name = personDataBase.Name;
                barberEntity.Lastname = personDataBase.LastName;
                barberEntity.Passwork = personDataBase.Passwork;
                barberEntity.Mobile = personDataBase.Mobile;
                barberEntity.Mail = personDataBase.Mail;

                var barbershopDataBase = barberShopDataBaseContext.Barbershops.FirstOrDefault(x => x.Id == barberEntity.IdBarberShop);

                barberEntity.NameBarberShop = barbershopDataBase.Name;

                barbers.Add(barberEntity);
            }

            return barbers;
        }

        public List<string> GetAllHoursAvailableForDate(DateTime date, string idBarber)
        {
            List<string> hours = new List<string>();

            //Traer reservas en esa fecha para ese barbero

            var reservations = barberShopDataBaseContext.Reservations.Where(x => x.Date == date && x.IdBarber == idBarber).ToList();
            var hoursAvailable = new List<TimeSpan?>();
            if (reservations.Any()) hoursAvailable = reservations.Select(x=> x.Hour).ToList();

            //Traer Horas de atencion de un barbero
            var barber = barberShopDataBaseContext.Barbers.FirstOrDefault(x => x.Id == idBarber);

            var horaAuxiliar = barber.StartAttentionTime;

            while (horaAuxiliar <= barber.EndAttentionTime)
            {
                if (hoursAvailable.Any())
                {

                    if (!hoursAvailable.Where(x => x.Value == horaAuxiliar).Any())
                    {
                        hours.Add(horaAuxiliar.ToString());
                    }

                }
                else
                {
                    hours.Add(horaAuxiliar.ToString());
                }

               
                horaAuxiliar += (new TimeSpan(1, 0, 0));

            }

            return hours;
        }


        private ResponseBaseEntity GetResponseBaseEntity(string message, TypeMessage typeMessage)
        {
            ResponseBaseEntity responseBaseEntity = new ResponseBaseEntity();
            responseBaseEntity.Message = message;
            responseBaseEntity.Type = typeMessage;
            return responseBaseEntity;
        }

        private Client ConvertClientEntityToClient(ClientEntity clientEntity)
        {
            Client client = new Client();
            client.Id = clientEntity.Id;
            client.TypeAffiliation = (int)clientEntity.TypeAffiliation;
            return client;
        }

        private Person ConvertClientEntityToPerson(ClientEntity clientEntity)
        {
            Person person = new Person();
            person.Id = clientEntity.Id;
            person.Name = clientEntity.Name;
            person.LastName = clientEntity.Lastname;
            person.Passwork = clientEntity.Passwork;
            person.Mobile = clientEntity.Mobile;
            person.Mail = clientEntity.Mail;
            return person;
        }

        private ClientEntity ConvertPersonToClientEntity(Person person)
        {
            ClientEntity clientEntity = new ClientEntity();
            clientEntity.Id = person.Id;
            clientEntity.Name = person.Name;
            clientEntity.Lastname = person.LastName;
            clientEntity.Passwork = person.Passwork;
            clientEntity.Mobile = person.Mobile;
            clientEntity.Mail = person.Mail;
            return clientEntity;
        }

    }
}
