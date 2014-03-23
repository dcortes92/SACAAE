using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;


namespace SACAAE.Models
{
    public class RepositorioSACAAE
    {
        private SACAAEEntities entidades = new SACAAEEntities();

        private const string FaltaUsuario = "Usuario no existe";
        private const string MuchoUsuario = "Usuario ya existe";


        public int NumeroUsuarios
        {
            get
            {
                return this.entidades.Usuarios.Count();
            }
        }
 
        public RepositorioSACAAE()
        {
            this.entidades = new SACAAEEntities();
        }
 
 
        public IQueryable<Usuario> ObtenerTodosUsuarios()
        {
            return from user in entidades.Usuarios
                   orderby user.NombreUsuario
                   select user;
        }
 
        public Usuario ObtenerUsuario(int id)
        {
            return entidades.Usuarios.SingleOrDefault(user => user.ID_Usuario == id);
        }
 
        public Usuario ObtenerUsuario(string NombreUsuario)
        {
            return entidades.Usuarios.SingleOrDefault(user => user.NombreUsuario == NombreUsuario);
        }        
 
        private void AgregarUsuario(Usuario usuario)
        {
            if (ExisteUsuario(usuario))
                throw new ArgumentException(MuchoUsuario);
 
            entidades.Usuarios.Add(usuario);
        }
 
        public void CrearUsuario(string nombreUsuario, string nombre, string contrasenia, string correo)
        {
            if (string.IsNullOrEmpty(nombreUsuario.Trim()))
                throw new ArgumentException("El nombre de usuario no es válido. Por favor, intente de nuevo.");
            if (string.IsNullOrEmpty(nombre.Trim()))
                throw new ArgumentException("El nombre no es válido. Por favor, intente de nuevo.");
            if (string.IsNullOrEmpty(contrasenia.Trim()))
                throw new ArgumentException("La contraseña no es válida. Por favor, ingrese una contraseña válida.");
            if (string.IsNullOrEmpty(correo.Trim()))
                throw new ArgumentException("El correo no es válido. Por favor, ingrese un correo válido");
            if (this.entidades.Usuarios.Any(user => user.NombreUsuario == nombreUsuario))
                throw new ArgumentException("El nombre de usuario ya existe. Por favor, especifique un nuevo nombre de usuario.");
 
            Usuario nuevoUsuario = new Usuario()
            {
                NombreUsuario = nombreUsuario,
                Nombre = nombre,
                Contrasenia = FormsAuthentication.HashPasswordForStoringInConfigFile(contrasenia.Trim(), "md5"),
                Correo = correo,
            };
 
            try
            {
                AgregarUsuario(nuevoUsuario);
            }
            catch (ArgumentException ae)
            {
                throw ae;
            }
            catch (Exception e)
            {
                throw new ArgumentException("El proveedor de autenticación retornó un error. Por favor, intente de nuevo. " +
                    "Si el problema persiste, por favor contacte un administrador.");
            }
 
            // Immediately persist the user data
            Save();
        }
 
        public void BorrarUsuario(Usuario usuario)
        {
            if (!ExisteUsuario(usuario))
                throw new ArgumentException(FaltaUsuario);
 
            entidades.Usuarios.Remove(usuario);
        }
 
        public void BorrarUsuario(string nombreUsuario)
        {
            BorrarUsuario(ObtenerUsuario(nombreUsuario));
        }
  
        public void Save()
        {
            entidades.SaveChanges();
        }        
 
        public bool ExisteUsuario(Usuario usuario)
        {
            if (usuario == null)
                return false;
 
            return (entidades.Usuarios.SingleOrDefault(u => u.ID_Usuario == usuario.ID_Usuario || 
                u.NombreUsuario == usuario.NombreUsuario) != null);
        }

    }
}