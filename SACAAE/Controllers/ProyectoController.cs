using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;

namespace SACAAE.Controllers
{
    public class ProyectoController : Controller
    {
        //
        // GET: /Proyecto/
        private const string TempDataMessageKey = "Message";
        private RepositorioProyecto repositorio = new RepositorioProyecto();

        [Authorize]
        public ActionResult Index()
        {
            var model = repositorio.ObtenerTodosProyectos();
            return View(model);
        }

        [Authorize]
        public ActionResult Crear()
        {
            var model = new Proyecto();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Crear(Proyecto nuevoProyecto)
        {
            repositorio.CrearProyecto(nuevoProyecto.Nombre, nuevoProyecto.Inicio, nuevoProyecto.Fin);
            TempData[TempDataMessageKey] = "Proyecto creado";
            return RedirectToAction("Index");
            
        }
    }
}
