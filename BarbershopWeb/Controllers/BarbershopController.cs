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

            if (barberShopLogic.VefirySession(loginEntity))
            {
                HttpContext.Session.SetString("token", loginEntity.Id);
                VerifySession();
            }
            else
            {
                return RedirectToAction("Login", "Barbershop", routeValues: new { loginError = "El usuario no existe" });
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

        public IActionResult CreateAppointment()
        {
            VerifySession();
            return View();
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


    }
}
