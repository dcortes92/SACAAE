using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;
using Newtonsoft.Json;

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
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }
            List<Profesore> ListaProfesores = repositorioProfesor.ObtenerTodosProfesores().ToList<Profesore>();
            List<Comisione> ListaComisiones = repositorioComision.ObtenerTodasComisiones().ToList<Comisione>();

            if (ListaProfesores.Count > 0)
                ViewBag.Profesores = ListaProfesores;
            else
                ViewBag.Profesores = null;

            if (ListaComisiones.Count > 0)
                ViewBag.Comisiones = ListaComisiones;
            else
                ViewBag.Comisiones = null;
                       

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

        [Authorize]
        public ActionResult Revocar()
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }

            /* Se obtiene la lista de profesores */
            List<Profesore> ListaProfesores = repositorioProfesor.ObtenerTodosProfesores().ToList<Profesore>();

            if (ListaProfesores.Count > 0)
                ViewBag.Profesores = ListaProfesores;
            else
                ViewBag.Profesores = null;
            
            return View();
        }

        public ActionResult ObtenerComisionesXProfesor(int idProfesor)
        {
            IQueryable listaComisiones = repositoriocomisionesprofesor.ObtenerComisionesXProfesor(idProfesor);
            if (HttpContext.Request.IsAjaxRequest())
            {                
                var json = JsonConvert.SerializeObject(listaComisiones);

                return Content(json);
            }
            return View(listaComisiones);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Revocar(int sltComisiones)
        {
            var revocado = false;

            revocado = repositoriocomisionesprofesor.revocarProfesor(sltComisiones);

            if (revocado)
            {
                TempData[TempDataMessageKey] = "Profesor revocado de comisión correctamente.";
            }
            else
            {
                TempData[TempDataMessageKey] = "Ocurrió un error al revocar el profesor.";
            }

            return RedirectToAction("Revocar");
        }
    }
}
