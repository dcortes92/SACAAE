using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class ProfesorController : Controller
    {
        //
        // GET: /Profesor/

        private const string TempDataMessageKey = "Message";
        private RepositorioProfesor repositorio = new RepositorioProfesor();

        [Authorize]
        public ActionResult Index()
        {
            var model = repositorio.ObtenerTodosProfesores();
            return View(model);
        }        

        [Authorize]
        public ActionResult Crear()
        {
            var model = new Profesore();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Crear(Profesore nuevoProfesor)
        {
            repositorio.CrearProfesor(nuevoProfesor.Nombre, nuevoProfesor.Plaza, nuevoProfesor.HorasEnPropiedad, nuevoProfesor.Link, 1);
            TempData[TempDataMessageKey] = "Profesor agregado correctamente.";
            return RedirectToAction("Index");

        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            var model = repositorio.ObtenerProfesor(id);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Eliminar(Profesore profesor)
        {
            repositorio.BorrarProfesor(profesor);
            TempData[TempDataMessageKey] = "Profesor borrado correctamente.";
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            var model = repositorio.ObtenerProfesor(id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Editar(Profesore profesor)
        {
            if (ModelState.IsValid)
            {
                repositorio.Actualizar(profesor);
                TempData[TempDataMessageKey] = "Profesor editado correctamente.";
            }


            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Detalles(int id)
        {
            var model = repositorio.ObtenerProfesor(id);
            return View(model);
        }
    }
}
