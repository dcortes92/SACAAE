using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class ComisionController : Controller
    {
        //
        // GET: /Comision/
        private const string TempDataMessageKey = "Message";
        private RepositorioComision repositorio = new RepositorioComision();

        [Authorize]
        public ActionResult Index()
        {
            var model = repositorio.ObtenerTodasComisiones();
            return View(model);
        }

        [Authorize]
        public ActionResult Crear()
        {
            var model = new Comisione();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Crear(Comisione nuevaComision)
        {
            repositorio.CrearComision(nuevaComision.Nombre, nuevaComision.Inicio, nuevaComision.Fin);
            TempData[TempDataMessageKey] = "Comisión creada correctamente.";
            return RedirectToAction("Index");

        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            var model = repositorio.ObtenerComision(id);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Eliminar(Comisione comision)
        {
            repositorio.BorrarComision(comision);
            TempData[TempDataMessageKey] = "Comisión eliminada correctamente.";
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            var model = repositorio.ObtenerComision(id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Editar(Comisione comision)
        {
            if (ModelState.IsValid)
            {
                repositorio.Actualizar(comision);
                TempData[TempDataMessageKey] = "Comisión editada correctamente.";
            }


            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Detalles(int id)
        {
            var model = repositorio.ObtenerComision(id);
            return View(model);
        }

    }
}
