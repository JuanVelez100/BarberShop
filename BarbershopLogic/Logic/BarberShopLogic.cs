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


    }
}
