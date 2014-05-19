using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SACAAE
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             "ObtenerPlanesEstudio",
             "CursoProfesor/Planes/List/{sede}/{modalidad}",
             new { Controller = "CursoProfesor", action = "ObtenerPlanesEstudio" });

            routes.MapRoute(
             "ObtenerCursos",
             "CursoProfesor/Cursos/List/{plan}",
             new { Controller = "CursoProfesor", action = "ObtenerCursos" });

            routes.MapRoute(
             "ObtenerGrupos",
             "CursoProfesor/Grupos/List/{curso}",
             new { Controller = "CursoProfesor", action = "ObtenerGrupos" });

            routes.MapRoute(
             "ObtenerInfo",
             "CursoProfesor/Grupos/Info/{cursoxgrupo}",
             new { Controller = "CursoProfesor", action = "ObtenerInfo" });

            routes.MapRoute(
             "ObtenerHorario",
             "CursoProfesor/Horarios/Info/{cursoxgrupo}",
             new { Controller = "CursoProfesor", action = "ObtenerHorario" });

            routes.MapRoute(
            "ObtenerCursosPorProfesor",
            "CursoProfesor/Profesor/Cursos/{idProfesor}",
            new { Controller = "CursoProfesor", action = "ObtenerCursosPorProfesor" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}