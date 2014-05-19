using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SACAAE.Models;

namespace SACAAE.Models
{
    public class RepositorioCursoProfesor
    {
        private SACAAEEntities entidades;

        public RepositorioCursoProfesor()
        {
            entidades = new SACAAEEntities();
        }

        public bool crearCursoProfesor(String profesor, int horas)
        {
            var retorno = false;

            /*Empieza la transacción*/
            using (var transaccion = new System.Transactions.TransactionScope())
            {
                try
                {
                    /* Se necesitan los repositorios de Horario y Profesor */
                    RepositorioProfesor repositorioProfesor = new RepositorioProfesor();
                    RepositorioHorario repositorioHorario = new RepositorioHorario();

                    var idProfesor = repositorioProfesor.ObtenerProfesor(profesor);
                    var idHorario = repositorioHorario.ObtenerUltimoHorario();

                    /*Primero se crea el registro en la tabla ProfesoresXCurso*/
                    ProfesoresXCurso profesoresXCurso = new ProfesoresXCurso()
                    {
                        Profesor = idProfesor.ID,
                        Horas = horas
                    };

                    entidades.ProfesoresXCursoes.Add(profesoresXCurso);
                    entidades.SaveChanges();
                    retorno = true;
                }
                catch (Exception ex)
                {
                    retorno = false;
                }
            }

            return retorno;
        }

        /// <summary>
        /// Retorna el último registro insertado en profesores x curso
        /// </summary>
        /// <returns>Id del último registro</returns>
        public int ObtenerUltimoProfesoresXCurso()
        {
            ProfesoresXCurso last = (from profesoresXCurso in entidades.ProfesoresXCursoes
                                     orderby profesoresXCurso.Id descending
                                     select profesoresXCurso).First();

            return last.Id;
        }

        /// <summary>
        /// Se obtienen todas las sedes.
        /// </summary>
        /// <returns>Lista de sedes.</returns>
        public IQueryable<Sede> obtenerTodasSedes()
        {
            return from sede in entidades.Sedes
                   orderby sede.Nombre
                   select sede;
        }

        /// <summary>
        /// Se obtienen las modalidades de los planes.
        /// </summary>
        /// <returns>Lista de modalidades.</returns>
        public IQueryable<Modalidade> obtenerTodasModalidades()
        {
            return from modalidad in entidades.Modalidades
                   orderby modalidad.Nombre
                   select modalidad;
        }

        /// <summary>
        /// Obtiene los planes de estudio de una determinada sede y modalidad
        /// </summary>
        /// <param name="sede">El ID de la sede.</param>
        /// <param name="modalidad">El ID de la modalidad.</param>
        /// <returns>Lista de planes de estudio.</returns>
        public IQueryable obtenerPlanesEstudio(int sede, int modalidad)
        {
            return from sedes in entidades.Sedes
                   join planesporsede in entidades.PlanesDeEstudioXSedes on sedes.ID equals planesporsede.Sede
                   join planesestudio in entidades.PlanesDeEstudios on planesporsede.PlanDeEstudio equals planesestudio.ID
                   join modalidades in entidades.Modalidades on planesestudio.Modalidad equals modalidades.ID
                   where (sedes.ID == sede) && (modalidades.ID == modalidad)
                   select new { planesestudio.ID, planesestudio.Nombre };

        }

        /// <summary>
        /// Obtiene los cursos abiertos para un determinado plan de estudio.
        /// </summary>
        /// <param name="plan">El id del plan del curso.</param>
        /// <returns>La lista de cursos abiertos para un plan de estudio.</returns>
        public IQueryable obtenerCursos(int plan)
        {
            return from cursos in entidades.Cursos
                   join cursosporgrupo in entidades.CursosXGrupoes on cursos.ID equals cursosporgrupo.Curso
                   where (cursos.PlanDeEstudio == plan)
                   select new { cursos.ID, cursos.Nombre, cursos.Codigo };
        }


        /// <summary>
        /// Obtiene la información de los grupos de un curso determinado.
        /// </summary>
        /// <param name="curso">El id del curso.</param>
        /// <returns>Lista de grupos abiertos de ese curso.</returns>
        public IQueryable obtenerGrupos(int curso)
        {
            return from grupos in entidades.Grupoes
                   join cursosxgrupo in entidades.CursosXGrupoes on grupos.ID equals cursosxgrupo.Grupo
                   where cursosxgrupo.Curso == curso
                   select new { cursosxgrupo.ID, grupos.Nombre };
        }

        /// <summary>
        /// Obtiene el cupo y el aula de un curso por grupo.
        /// </summary>
        /// <param name="cursoxgrupo">El id del curso por grupo.</param>
        /// <returns>El cupo y el aula del curso por grupo.</returns>
        public IQueryable obtenerInfo(int cursoxgrupo)
        {
            return from detalle in entidades.Detalle_Curso
                   where detalle.Curso == cursoxgrupo
                   select new { detalle.Id, detalle.Curso, detalle.Aula, detalle.Cupo };
        }

        /// <summary>
        /// Obtiene el horario de un detalle de curso.
        /// </summary>
        /// <param name="cursoxgrupo">El id del curso por grupo.</param>
        /// <returns>Id del horario.</returns>
        public int obtenerHorario(int cursoxgrupo)
        {
            var query = from detalle in entidades.Detalle_Curso
                        where detalle.Curso == cursoxgrupo
                        select detalle;

            List<Detalle_Curso> config = query.ToList();


            return config[0].Horario;
        }


        /// <summary>
        /// Obtiene la información del horario de un curso.
        /// </summary>
        /// <param name="horario">El id del horario.</param>
        /// <returns>La lista de días en que se imparte el horario con sus horas.</returns>
        public IQueryable obtenerInfoHorario(int horario)
        {
            return from dia in entidades.Dias
                   where dia.Horario == horario
                   select new { dia.Horario, dia.Dia1, dia.Hora_Inicio, dia.Hora_Fin };
        }


        /// <summary>
        /// Se actualiza la información de profesor x curso
        /// </summary>
        /// <param name="id">El id del registro de profesor x curso</param>
        /// <param name="idProfesor">El id del profesor que será asignado a ese curso</param>
        /// <param name="horas">La cantidad de horas del profesor.</param>
        /// <returns>True si se completa la operación.</returns>
        public bool asignarProfesor(int id, int idProfesor, int horas)
        {
            var retorno = false;
            var temp = entidades.ProfesoresXCursoes.Find(id);

            if (temp != null)
            {
                entidades.Entry(temp).Property(p => p.Profesor).CurrentValue = idProfesor;
                entidades.Entry(temp).Property(p => p.Horas).CurrentValue = horas;
                retorno = true;
            }

            entidades.SaveChanges();

            return retorno;
        }


        /// <summary>
        /// Obtiene el id del profesor por curso de un detalle_curso.
        /// </summary>
        /// <param name="idCursoXGrupo">El id del curso x grupo.</param>
        /// <returns>El id del profesor por curso.</returns>
        public int obtenerIdProfesorXGrupo(int idCursoXGrupo)
        {
            var query = from detalle in entidades.Detalle_Curso
                        where detalle.Curso == idCursoXGrupo
                        select detalle;

            List<Detalle_Curso> config = query.ToList();


            return config[0].Profesor;
        }


        /// <summary>
        /// Obtiene los cursos que está impartiendo un profesor.
        /// </summary>
        /// <param name="idProfesor">El id del profesor.</param>
        /// <returns>La lista de cursos.</returns>
        public IQueryable obtenerCursosPorProfesor(int idProfesor)
        {
            
            return from profesores in entidades.Profesores
                   join profesoresxcurso in entidades.ProfesoresXCursoes on profesores.ID equals profesoresxcurso.Profesor
                   join detallecurso in entidades.Detalle_Curso on profesoresxcurso.Id equals detallecurso.Profesor
                   join cursosxgrupo in entidades.CursosXGrupoes on detallecurso.Curso equals cursosxgrupo.ID
                   join cursos in entidades.Cursos on cursosxgrupo.Curso equals cursos.ID
                   where profesores.ID == idProfesor
                   select new { profesoresxcurso.Id, cursos.Nombre, cursos.Codigo};
        }

        /// <summary>
        /// Revoca la asignación de un profesor a un curso.
        /// </summary>
        /// <param name="idProfesorXCurso">El id del profesor x curso.</param>
        /// <returns>True si logra revocarlo.</returns>
        public bool revocarProfesor(int idProfesorXCurso)
        {
            var retorno = false;
            var temp = entidades.ProfesoresXCursoes.Find(idProfesorXCurso);

            if (temp != null)
            {
                entidades.Entry(temp).Property(p => p.Profesor).CurrentValue = 1;
                entidades.Entry(temp).Property(p => p.Horas).CurrentValue = 0;
                retorno = true;
            }

            entidades.SaveChanges();

            return retorno;
        }
    }
}