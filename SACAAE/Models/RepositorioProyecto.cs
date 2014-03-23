using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SACAAE.Models
{
    public class RepositorioProyecto
    {
        private SACAAEEntities entidades = new SACAAEEntities();

        private const String FaltaProyecto = "Proyecto no existe";
        private const String MuchoProyecto = "Proyecto ya existe";

        /// <summary>
        /// Obtener cantidad de proyectos
        /// </summary>
        public int NumeroProyectos
        {
            get
            {
                return this.entidades.Proyectos.Count();
            }
        }

        public RepositorioProyecto()
        {
            this.entidades = new SACAAEEntities();
        }

        /// <summary>
        /// Se obtiene la lista de proyectos usando LINQ
        /// </summary>
        /// <returns></returns>
        public IQueryable<Proyecto> ObtenerTodosProyectos()
        {
            return from proyecto in entidades.Proyectos
                   orderby proyecto.Nombre
                   select proyecto;
        }

        /// <summary>
        /// Se obtiene un proyecto de acuerdo a un Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Proyecto ObtenerProyecto(int id)
        {
            return entidades.Proyectos.SingleOrDefault(proyecto => proyecto.ID == id);
        }

        /// <summary>
        /// Se obtiene un proyecto de acuerdo a su nombre.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public Proyecto ObtenerProyecto(string nombre)
        {
            return entidades.Proyectos.SingleOrDefault(proyecto => proyecto.Nombre == nombre);
        }


        private void AgregarProyecto(Proyecto proyecto)
        {
            if (ExisteProyecto(proyecto))
                throw new ArgumentException(MuchoProyecto);

            entidades.Proyectos.Add(proyecto);
        }

        public void CrearProyecto(string nombre, DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (string.IsNullOrEmpty(nombre.Trim()))
                throw new ArgumentException("El nombre del proyecto no es válido. Por favor, intente de nuevo.");

            Proyecto proyectoNuevo = new Proyecto()
            {
                Nombre = nombre,
                Inicio = fechaInicio,
                Fin = fechaFin
            };

            try
            {
                AgregarProyecto(proyectoNuevo);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new ArgumentException("El proveedor de autenticación retornó un error. Por favor, intente de nuevo. " +
                    "Si el problema persiste, por favor contacte un administrador.\n" + e.Message);
            }

            Save();
        }

        /// <summary>
        /// Comprueba que exista un proyecto determinado.
        /// </summary>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public bool ExisteProyecto(Proyecto proyecto)
        {
            if (proyecto == null)
                return false;
            return (entidades.Proyectos.SingleOrDefault(p => p.ID == proyecto.ID ||
                p.Nombre == proyecto.Nombre) != null);
        }

        public void Save()
        {
            entidades.SaveChanges();
        }

        public void BorrarProyecto(Proyecto proyecto)
        {
            if (!ExisteProyecto(proyecto))
                throw new ArgumentException(FaltaProyecto);

            entidades.Proyectos.Remove(proyecto);
        }

        public void BorrarProyecto(String nombre)
        {
            BorrarProyecto(ObtenerProyecto(nombre));
        }
    }
}