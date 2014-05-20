using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;
using Newtonsoft.Json;


namespace SACAAE.Controllers
{
    public class ProfesoresProyectosController : Controller
    {

        private  RepositorioProyecto repoProyectos = new RepositorioProyecto();
        private RepositorioProfesor repoProfesores = new RepositorioProfesor();
        RepositorioProfesorProyecto repoProfesProyectos = new RepositorioProfesorProyecto();
        private const string TempDataMessageKey = "Message";

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Asignar()
        {
            ViewBag.profesores = repoProfesores.ObtenerTodosProfesores();
            ViewBag.proyectos = repoProyectos.ObtenerTodosProyectos(); 
            return View(); 
        }

        [Authorize]
        [HttpPost]
        public ActionResult Asignar(String sltProfesor, String sltProyecto, String dia, String HoraInicio, String HoraFin)
        {

            var creado = repoProfesProyectos.CrearProyectoProfesor(sltProfesor, sltProyecto, dia, HoraInicio, HoraFin);
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
            List<Profesore> ListaProfesores = repoProfesores.ObtenerTodosProfesores().ToList<Profesore>();

            if (ListaProfesores.Count > 0)
                ViewBag.Profesores = ListaProfesores;
            else
                ViewBag.Profesores = null;

            return View();
        }


        public ActionResult ObtenerProyectosXProfesor(int idProfesor)
        {
            IQueryable listaComisiones = repoProfesProyectos.ObtenerProyectosXProfesor(idProfesor);
            if (HttpContext.Request.IsAjaxRequest())
            {
                var json = JsonConvert.SerializeObject(listaComisiones);

                return Content(json);
            }
            return View(listaComisiones);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Revocar(int sltProyectos)
        {
            var revocado = false;

            revocado = repoProfesProyectos.revocarProfesor(sltProyectos);

            if (revocado)
            {
                TempData[TempDataMessageKey] = "Profesor revocado de proyecto correctamente.";
            }
            else
            {
                TempData[TempDataMessageKey] = "Ocurrió un error al revocar el profesor.";
            }

            return RedirectToAction("Revocar");
        }
	}
}