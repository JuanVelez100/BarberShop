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
                        return GetResponseBaseEntity("Ya existe un usuario con este Id", TypeMessage.danger);
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


        public ResponseBaseEntity CreateReservation(ReservationEntity reservationEntity)
        {

            using (var dbContextTransaction = barberShopDataBaseContext.Database.BeginTransaction())
            {
                try
                {

                    //Calcular el Costo

                    //Buscamos el cliente para saber si existe y levantar su tipo de afiliación
                    var client = barberShopDataBaseContext.Clients.FirstOrDefault(x=> x.Id == reservationEntity.IdClient);
                    if(client == null) return GetResponseBaseEntity("El cliente no existe ! ", TypeMessage.danger);
                    var contract = barberShopDataBaseContext.Contracts.FirstOrDefault(x=> x.TypeAffiliation == client.TypeAffiliation);
                    if (contract == null) return GetResponseBaseEntity("El cliente no tiene asociado un contracto ! ", TypeMessage.danger);

                    //Buscamos la barberia para saber sus costos bases por servicio
                    var barbershop = barberShopDataBaseContext.Barbershops.FirstOrDefault(x => x.Id == reservationEntity.IdBarberShop);
                    if (barbershop == null) return GetResponseBaseEntity("La Barberia no existe ! ", TypeMessage.danger);

                    //Buscamos al barbero y luego su tipo de servicio para saber el costo de este
                    var barber = barberShopDataBaseContext.Barbers.FirstOrDefault(x => x.Id == reservationEntity.IdBarber);
                    if (barber == null) return GetResponseBaseEntity("El barbero no existe ! ", TypeMessage.danger);
                    var service = barberShopDataBaseContext.Services.FirstOrDefault(x => x.Id == barber.IdServicio);
                    if (service == null) return GetResponseBaseEntity("El barbero no tiene un servicio existente ! ", TypeMessage.danger);


                    var cost = 0;
                    if (client.TypeAffiliation == (int)TypeAffiliation.Afiliado)
                    {
                       cost = (int)(barbershop.AffiliateBaseCost + service.AffiliateCost);
                    }
                    else
                    {
                      cost = (int)(barbershop.ParticularBaseCost + service.ParticularCost);
                    }

                    if (contract.DiscountRate > 0)
                    {
                        cost = cost - (cost * (int)contract.DiscountRate / 100);
                    }

                    reservationEntity.Cost = cost;


                    reservationEntity.Id = barberShopDataBaseContext.Reservations.Max(x => x.Id) + 1;
                    barberShopDataBaseContext.Reservations.Add(ConvertReservationEntityToReservation(reservationEntity));
                    barberShopDataBaseContext.SaveChanges();
                    dbContextTransaction.Commit();

                    return GetResponseBaseEntity("Reserva creada con exito con un costo de "+ cost +" mil pesos y "+ service.Description, TypeMessage.success);

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return GetResponseBaseEntity(ex.Message, TypeMessage.danger);
                }
            }


        } 

        public List<HistoryEntity> GetAllHistoricForClient(string idclient)
        {
            List<HistoryEntity> listHistoryEntities = new List<HistoryEntity>();

            var client = barberShopDataBaseContext.People.FirstOrDefault(x => x.Id == idclient);
            var listReservation = barberShopDataBaseContext.Reservations.Where(x => x.IdClient == idclient).OrderByDescending(x => x.Date).ToList();

            foreach (var reservation in listReservation)
            {
                HistoryEntity historyEntity = new HistoryEntity();
                historyEntity.Id = reservation.Id;
                historyEntity.Client = client.Name + " " + client.LastName;
                historyEntity.Date = reservation.Date;
                historyEntity.Hour = reservation.Hour;
                historyEntity.Cost = reservation.Cost;
                historyEntity.Observation = reservation.Observation;

                historyEntity.City = barberShopDataBaseContext.Cities.FirstOrDefault(x => x.Id == reservation.IdCity).Name;
                historyEntity.BarberShop = barberShopDataBaseContext.Barbershops.FirstOrDefault(x => x.Id == reservation.IdBarberShop).Name;

                var barberPerson = barberShopDataBaseContext.People.FirstOrDefault(x => x.Id == reservation.IdBarber);
                historyEntity.Barber = barberPerson.Name +" "+ barberPerson.LastName;

                var barber = barberShopDataBaseContext.Barbers.FirstOrDefault(x => x.Id == reservation.IdBarber);
                historyEntity.Service = barberShopDataBaseContext.Services.FirstOrDefault(x => x.Id == barber.IdServicio).Description;

                listHistoryEntities.Add(historyEntity);
            }

            return listHistoryEntities;
        }



        private ResponseBaseEntity GetResponseBaseEntity(string message, TypeMessage typeMessage)
        {
            ResponseBaseEntity responseBaseEntity = new ResponseBaseEntity();
            responseBaseEntity.Message = message;
            responseBaseEntity.Type = typeMessage;
            return responseBaseEntity;
        }

        private Reservation ConvertReservationEntityToReservation(ReservationEntity reservationEntity)
        {
            Reservation reservation = new Reservation();
            reservation.Id = reservationEntity.Id;
            reservation.IdClient = reservationEntity.IdClient;
            reservation.IdBarberShop = reservationEntity.IdBarberShop;
            reservation.IdCity = reservationEntity.IdCity;
            reservation.IdBarber = reservationEntity.IdBarber;
            reservation.Observation = reservationEntity.Observation;
            reservation.Cost = reservationEntity.Cost;
            reservation.Date = reservationEntity.Date;
            reservation.Hour = reservationEntity.Hour;
            return reservation;
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
