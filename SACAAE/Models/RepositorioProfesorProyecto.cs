using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class RepositorioProfesorProyecto
    {
        
        private SACAAEEntities entidades;

        public RepositorioProfesorProyecto()
        {
            entidades = new SACAAEEntities();
        }


        public bool CrearProyectoProfesor(string sltProfesor, string sltProyecto, string dia,
                        string horainicio, string horafin)
        {

            bool retorno = false; 
            using (var transaccion = new System.Transactions.TransactionScope())
            {
                RepositorioHorario repoHorario = new RepositorioHorario();
                RepositorioProfesor repoProfesor = new RepositorioProfesor();
                RepositorioProyecto repoProyecto = new RepositorioProyecto();

                var IDProfesor = repoProfesor.ObtenerProfesor(Int16.Parse(sltProfesor));
                var IDProyecto = repoProyecto.ObtenerProyecto(Int16.Parse(sltProyecto));
                var IDPeriodo = ObtenerPeriodoActual();
                ProyectosXProfesor proyectosProfesor = new ProyectosXProfesor
                {
                    Proyecto = Int16.Parse(IDProyecto.ID.ToString()),
                    Profesor = Int16.Parse(IDProfesor.ID.ToString()),
                    Periodo = IDPeriodo
                };
                if (ExisteProyectoProfesor(proyectosProfesor))
                {
                    var IDHorario = repoHorario.ObtenerUltimoHorario();

                    Dia nuevoDia = new Dia()
                    {
                        Dia1 = dia,
                        Horario = IDHorario,
                        Hora_Inicio = Int16.Parse(horainicio),
                        Hora_Fin = Int16.Parse(horafin),

                    };

                    entidades.Dias.Add(nuevoDia);
                    entidades.SaveChanges();
                    retorno = true;
                    transaccion.Complete();
                }
                else
                {
                    var IDHorario = repoHorario.CrearHorario();
                    proyectosProfesor.Horario = IDHorario;
                    Dia lala = new Dia(); 
                    
                    Dia nuevoDia = new Dia()
                    {
                        Dia1 = dia, 
                        Horario = IDHorario,
                        Hora_Inicio = Int16.Parse(horainicio),
                        Hora_Fin = Int16.Parse(horafin),

                    };

                    entidades.ProyectosXProfesors.Add(proyectosProfesor);
                    entidades.SaveChanges();
                    entidades.Dias.Add(nuevoDia);
                    entidades.SaveChanges();
                    retorno = true;
                    transaccion.Complete();
                }   
            }
            return retorno;
        }

        public int ObtenerPeriodoActual()
        {
            var query = from ajustes in entidades.Ajustes
                        select ajustes;

            List<Ajuste> config = query.ToList();

            return config[0].IDPeriodoActual;
        }

        public bool ExisteProyectoProfesor(ProyectosXProfesor proyectoprofesor)
        {
            if (proyectoprofesor == null)
                return false;
            else
                return (entidades.ProyectosXProfesors.SingleOrDefault(cp => cp.Profesor == proyectoprofesor.Profesor &&
                    cp.Proyecto == proyectoprofesor.Proyecto) != null);
        }

        /// <summary>
        /// Obtiene la lista de comisiones a las que está asociado un profesor.
        /// </summary>
        /// <param name="idProfesor">El id del profesor.</param>
        /// <returns>La lista de comisiones a las que está asociado el profesor.</returns>
        internal IQueryable ObtenerProyectosXProfesor(int idProfesor)
        {
            return from profesores in entidades.Profesores
                   join proyectosxprofesor in entidades.ProyectosXProfesors on profesores.ID equals proyectosxprofesor.Profesor
                   join proyectos in entidades.Proyectos on proyectosxprofesor.Proyecto equals proyectos.ID
                   where profesores.ID == idProfesor
                   select new { proyectosxprofesor.ID, proyectos.Nombre };
        }

        /// <summary>
        /// Revoca un profesor de un proyecto determinado.
        /// </summary>
        /// <param name="idProyecto">El id del proyecto por profesor.</param>
        /// <returns>True si se logra revocar.</returns>
        public bool revocarProfesor(int idProyecto)
        {
            var retorno = false;

            var proyecto = entidades.ProyectosXProfesors.Find(idProyecto);

            if (proyecto != null)
            {
                entidades.ProyectosXProfesors.Remove(proyecto);

                entidades.SaveChanges();

                retorno = true;
            }

            return retorno;
        }
    }
}