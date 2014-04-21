using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class RepositorioHorario
    {
        private SACAAEEntities entidades;

        public RepositorioHorario()
        {
            entidades = new SACAAEEntities();
        }

        public int CrearHorario()
        {
            Horario horario = new Horario();
            entidades.Horarios.Add(horario);
            entidades.SaveChanges();
            return horario.Id;
        }

        public int ObtenerUltimoHorario()
        {
            Horario last = (from horario in entidades.Horarios
                             orderby horario.Id descending
                             select horario).First();

            return last.Id;
        }
    }
}