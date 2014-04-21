using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SACAAE.Models
{
    public class RepositorioComision
    {
        private SACAAEEntities entidades = new SACAAEEntities();

        private const String FaltaComision = "Comisión no existe";
        private const String MuchaComision = "Comisión ya existe";

        public int NumeroComisiones
        {
            get
            {
                return this.entidades.Comisiones.Count();
            }
        }

        public RepositorioComision()
        {
            entidades = new SACAAEEntities();
        }

        public IQueryable<Comisione> ObtenerTodasComisiones()
        {
            return from comision in entidades.Comisiones
                   orderby comision.Nombre
                   select comision;
        }

        public Comisione ObtenerComision(int id)
        {
            return entidades.Comisiones.SingleOrDefault(comision => comision.ID == id);
        }

        public Comisione ObtenerComision(string nombre)
        {
            return entidades.Comisiones.SingleOrDefault(comision => comision.Nombre == nombre);
        }

        private void AgregarComision(Comisione comision)
        {
            if (ExisteComision(comision))
                throw new ArgumentException(MuchaComision);
            entidades.Comisiones.Add(comision);
        }

        public void CrearComision(String nombre, DateTime inicio, DateTime fin)
        {
            if (string.IsNullOrEmpty(nombre.Trim()))
                throw new ArgumentException("El nombre de la comisión no es válida. Por favor, inténtelo de nuevo");

            Comisione comisionNueva = new Comisione()
            {
                Nombre = nombre,
                Inicio = inicio,
                Fin = fin,
                Estado = 1
            };

            try
            {
                AgregarComision(comisionNueva);
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

        public void BorrarComision(Comisione comision)
        {
            if (!ExisteComision(comision))
                throw new ArgumentException(FaltaComision);

            var temp = entidades.Comisiones.Find(comision.ID);
            if (temp != null)
            {
                entidades.Entry(temp).Property(c => c.Estado).CurrentValue = 2;
            }
            Save();
        }

        public void BorrarComision(string nombre)
        {
            BorrarComision(ObtenerComision(nombre));
        }

        public void Actualizar(Comisione comision)
        {
            if (!ExisteComision(comision))
                AgregarComision(comision);

            var temp = entidades.Comisiones.Find(comision.ID);

            if (temp != null)
            {
                entidades.Entry(temp).Property(c => c.Nombre).CurrentValue = comision.Nombre;
                entidades.Entry(temp).Property(c => c.Inicio).CurrentValue = comision.Inicio;
                entidades.Entry(temp).Property(c => c.Fin).CurrentValue = comision.Fin;
            }

            Save();
        }

        /*Helpers*/
        public bool ExisteComision(Comisione comision)
        {
            if (comision == null)
                return false;
            return (entidades.Comisiones.SingleOrDefault(c => c.ID == comision.ID ||
                c.Nombre == comision.Nombre) != null);
        }

        public void Save()
        {
            entidades.SaveChanges();
        }
    }
}