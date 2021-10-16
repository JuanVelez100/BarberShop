using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BarbershopEntity.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using static BarbershopEntity.Helper.Emun;
using System.Text;
using Microsoft.AspNetCore.Http;
using BarbershopLogic.Logic;

namespace BarbershopWeb.Controllers
{
    public class BarbershopController : Controller
    {
        private readonly ILogger<BarbershopController> logger;
        private BarberShopLogic barberShopLogic = new BarberShopLogic();

        public BarbershopController(ILogger<BarbershopController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Login(string loginError="")
        {
            HttpContext.Session.Clear();

            if (!string.IsNullOrEmpty(loginError))
            {
                ViewBag.LoginError = loginError;
            }

            return View();
        }

        [HttpPost]
        public IActionResult Home(LoginEntity loginEntity)
        {
            var session = HttpContext.Session.GetString("token");

            if (session == null)
            {
                if (barberShopLogic.VefirySession(loginEntity))
                {
                    HttpContext.Session.SetString("token", loginEntity.Id);
                    VerifySession();
                }
                else
                {
                    return RedirectToAction("Login", "Barbershop", routeValues: new { loginError = "El usuario no existe" });
                }
            }
            
            return View();
        }

        public IActionResult Signup()
        {
            List<SelectListItem> listTypeAffiliation = new List<SelectListItem>();

            foreach (int item in Enum.GetValues(typeof(TypeAffiliation)))
            {
                listTypeAffiliation.Add(new SelectListItem { Value = ((int)item).ToString() , Text = Enum.ToObject(typeof(TypeAffiliation), (int)item).ToString() });
            }

            ViewBag.ListCompanyDocumentType = listTypeAffiliation;

            return View();
        }

        [HttpPost]
        public IActionResult SignupCreate(ClientEntity clientEntity)
        {
            var responseBase = barberShopLogic.CreateClient(clientEntity);

            ViewBag.Message = responseBase.Message;
            ViewBag.Type = Enum.ToObject(typeof(TypeMessage), (int)responseBase.Type).ToString();

            return View();
        }

        public IActionResult Reservation()
        {
            VerifySession();

            //Obtener valores del cliente logeeado con la session
            var idClient = HttpContext.Session.GetString("token");
            ViewBag.Id = idClient;
            var client = barberShopLogic.GetClientEntityForId(idClient);
            ViewBag.Name = client.Name + " "+ client.Lastname;

            //Lista de ciudades
            List<SelectListItem> listCities = new List<SelectListItem>();
            listCities.Add(new SelectListItem { Value = "0", Text = "Seleccione Ciudad" });

            var cities = barberShopLogic.GetAllCities();

            foreach (var city in cities)
            {
                listCities.Add(new SelectListItem { Value = city.Id.ToString() , Text = city.Name });
            }

            ViewBag.ListCities = listCities;

            //Lista de Barberias
            List<SelectListItem> listBarbershop = new List<SelectListItem>();
            ViewBag.ListBarbershop = listBarbershop;

            //Lista de Barberos
            List<SelectListItem> listBarbers = new List<SelectListItem>();
            ViewBag.ListBarbers = listBarbers;


            //Lista de Horas disponibles por Dia de un Barbero
            List<SelectListItem> listHour = new List<SelectListItem>();
            ViewBag.ListHour = listHour;


            return View();
        }

        public IActionResult BarberForBarbershop()
        {
            var listBarber = barberShopLogic.GetAllBarbers(1);

            return View(listBarber);
        }

        private void VerifySession()
        {
            var session = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(session))
            {
                HttpContext.Session.Clear();
                ViewBag.LoginError = "El usuario no esta logeado";
            }
        }


        public JsonResult GetAllBarbershopForCity(int city)
        {
            var resultado = barberShopLogic.GetAllBarbershop(city);
            return Json(resultado.ToList());
        }

        public JsonResult GetAllBarbersForBarbershop(int barbershop)
        {
            var resultado = barberShopLogic.GetAllBarbers(barbershop);
            return Json(resultado.ToList());
        }

        public JsonResult GetAllHoursAvailableForDate(DateTime date, string idBarber)
        {
            var resultado = barberShopLogic.GetAllHoursAvailableForDate(date, idBarber);
            return Json(resultado.ToList());
        }

        [HttpGet]
        public string Prueba(string prueba)
        {
            return prueba + " Todo anda muy bien";
        }

        [HttpPost]
        public LoginEntity Prueba2(LoginEntity loginEntity)
        {
            loginEntity.Id = loginEntity.Id + " :Id ";
            loginEntity.Passwork = loginEntity.Passwork + " :Contraseña "; ;
            return  loginEntity;
        }


    }
}
