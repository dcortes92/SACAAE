//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SACAAE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CursosXGrupo
    {
        public CursosXGrupo()
        {
            this.ProfesoresXCursoes = new HashSet<ProfesoresXCurso>();
        }
    
        public int ID { get; set; }
        public int Curso { get; set; }
        public int Grupo { get; set; }
        public int Profesor { get; set; }
        public int Cupo { get; set; }
    
        public virtual Curso Curso1 { get; set; }
        public virtual Grupo Grupo1 { get; set; }
        public virtual ICollection<ProfesoresXCurso> ProfesoresXCursoes { get; set; }
    }
}
