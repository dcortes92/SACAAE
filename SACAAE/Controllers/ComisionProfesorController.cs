using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;

namespace SACAAE.Controllers
{
    public class ComisionProfesorController : Controller
    {
        //
        // GET: /ComisionProfesor/
        private RepositorioProfesor repositorioProfesor = new RepositorioProfesor();
        private RepositorioComision repositorioComision = new RepositorioComision();
        private RepositorioComisionesProfesor repositoriocomisionesprofesor = new RepositorioComisionesProfesor();
        private const string TempDataMessageKey = "Message";

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Asignar()
        {
            ViewBag.returnUrl = Request.UrlReferrer.ToString();
            ViewBag.Profesores = repositorioProfesor.ObtenerTodosProfesores().ToList<Profesore>();
            ViewBag.Comisiones = repositorioComision.ObtenerTodasComisiones().ToList<Comisione>();

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Asignar(String profesor, String comision, String dia, int HoraInicio, int HoraFin)
        {
            var creado = repositoriocomisionesprofesor.CrearComisionProfesor(profesor, comision, dia, HoraInicio, HoraFin);
            if (creado)
            {
                TempData[TempDataMessageKey] = "Profesor asignado correctamente.";
            }
            else
            {
                TempData[TempDataMessageKey] = "Ocurrió un error al asignar el profesor.";
            }
            return RedirectToAction("Asignar");
        }
    }
}
