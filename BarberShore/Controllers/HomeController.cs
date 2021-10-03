using BarberEntity.Entity;
using BarberShore.Models;
using BarberShoreLogic.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BarberShore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private PersonaLogic personaLogic = new PersonaLogic();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string nombre="")
        {
            List<PersonaEntity> listPersonaEntities = new List<PersonaEntity>();

            if (string.IsNullOrEmpty(nombre))
            {
                listPersonaEntities = personaLogic.GetAllPerson();
            }
            else
            {
                listPersonaEntities = personaLogic.GetAllPerson().Where(x => x.Nombre.ToUpper().Contains(nombre.ToUpper())).ToList();
            }

            return View(listPersonaEntities);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PersonaEntity personaEntity)
        {
            var person = personaLogic.AddPerson(personaEntity);

            ViewBag.Message = person.Message;
            ViewBag.Type = person.Type;

            return View(personaEntity);
        }


    }
}
