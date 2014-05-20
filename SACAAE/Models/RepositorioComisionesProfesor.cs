using System;
using System.Collections.Generic;
using System.Linq;

namespace SACAAE.Models
{
    public class RepositorioComisionesProfesor
    {
        private SACAAEEntities entidades;

        public RepositorioComisionesProfesor()
        {
            entidades = new SACAAEEntities();
        }

        public bool CrearComisionProfesor(String profesor, String comision, String dia, int horainicio, int horafin)
        {
            var retorno = false;

            /*Empieza la transacción*/
            using (var transaccion = new System.Transactions.TransactionScope())
            {
                try
                {
                    RepositorioHorario repositorioHorario = new RepositorioHorario();
                    RepositorioProfesor repositorioProfesor = new RepositorioProfesor();
                    RepositorioComision repositorioComision = new RepositorioComision();

                    var IDProfesor = repositorioProfesor.ObtenerProfesor(profesor);
                    var IDComision = repositorioComision.ObtenerComision(comision);
                    var IDPeriodo = ObtenerPeriodoActual();                    

                    ComisionesXProfesor comisionProfesor = new ComisionesXProfesor()
                    {
                        Comision = IDComision.ID,
                        Profesor = IDProfesor.ID,
                        Periodo = IDPeriodo
                    };

                    if (ExisteComisionProfesor(comisionProfesor))
                    {
                        var IDHorario = repositorioHorario.ObtenerUltimoHorario();
                        
                        Dia nuevoDia = new Dia()
                        {
                            Dia1 = dia,
                            Horario = IDHorario,
                            Hora_Inicio = horainicio,
                            Hora_Fin = horafin,

                        };

                        entidades.Dias.Add(nuevoDia);
                        entidades.SaveChanges();
                        retorno = true;
                        transaccion.Complete();
                    }
                    else
                    {
                        var IDHorario = repositorioHorario.CrearHorario();
                        comisionProfesor.Horario = IDHorario;

                        Dia nuevoDia = new Dia()
                        {
                            Dia1 = dia,
                            Horario = IDHorario,
                            Hora_Inicio = horainicio,
                            Hora_Fin = horafin,

                        };

                        entidades.ComisionesXProfesors.Add(comisionProfesor);
                        entidades.SaveChanges();
                        entidades.Dias.Add(nuevoDia);
                        entidades.SaveChanges();
                        retorno = true;
                        transaccion.Complete();
                    }          
                    
                }
                catch (Exception ex)
                {                    
                    retorno = false;                    
                }

            }

            return retorno;
        }

        public bool ExisteComisionProfesor(ComisionesXProfesor comisionprofesor)
        {
            if (comisionprofesor == null)
                return false;
            else
                return (entidades.ComisionesXProfesors.SingleOrDefault(cp => cp.Profesor == comisionprofesor.Profesor &&
                    cp.Comision == comisionprofesor.Comision) != null);
        }

        public void Save()
        {
            entidades.SaveChanges();
        }

        public int ObtenerPeriodoActual()
        {
            var query = from ajustes in entidades.Ajustes
                        select ajustes;

            List<Ajuste> config = query.ToList();

            return config[0].IDPeriodoActual;
        }

        /// <summary>
        /// Obtiene la lista de comisiones a las que está asociado un profesor.
        /// </summary>
        /// <param name="idProfesor">El id del profesor.</param>
        /// <returns>La lista de comisiones a las que está asociado el profesor.</returns>
        public IQueryable ObtenerComisionesXProfesor(int idProfesor)
        {            

            return from profesores in entidades.Profesores
                   join comisionesxprofesor in entidades.ComisionesXProfesors on profesores.ID equals comisionesxprofesor.Profesor
                   join comisiones in entidades.Comisiones on comisionesxprofesor.Comision equals comisiones.ID
                   where profesores.ID == idProfesor
                   select new { comisionesxprofesor.ID, comisiones.Nombre};
        }

        /// <summary>
        /// Revoca un profesor de una comisión determinada.
        /// </summary>
        /// <param name="idComision">El id de la comisión por profesor.</param>
        /// <returns>True si se logra revocar.</returns>
        public bool revocarProfesor(int idComision)
        {
            var retorno = false;

            var comision = entidades.ComisionesXProfesors.Find(idComision);

            if (comision != null)
            {
                entidades.ComisionesXProfesors.Remove(comision);

                entidades.SaveChanges();

                retorno = true;
            }

            return retorno;
        }
    }
}