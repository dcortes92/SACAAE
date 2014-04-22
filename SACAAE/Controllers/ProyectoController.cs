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
            repositorio.CrearProyecto(nuevoProyecto.Nombre, nuevoProyecto.Inicio, nuevoProyecto.Fin, nuevoProyecto.Link);
            TempData[TempDataMessageKey] = "Proyecto creado correctamente.";
            return RedirectToAction("Index");
            
        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            var model = repositorio.ObtenerProyecto(id);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Eliminar(Proyecto proyecto)
        {
            repositorio.BorrarProyecto(proyecto);
            TempData[TempDataMessageKey] = "Proyecto eliminado correctamente.";
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            var model = repositorio.ObtenerProyecto(id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Editar(Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                repositorio.Actualizar(proyecto);
                TempData[TempDataMessageKey] = "Proyecto editado correctamente.";
            }
            
            
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Detalles(int id)
        {
            var model = repositorio.ObtenerProyecto(id);
            return View(model);
        }

    }
}
