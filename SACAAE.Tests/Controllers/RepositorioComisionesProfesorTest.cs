using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SACAAE.Models;

namespace SACAAE.Tests.Controllers
{
    [TestClass]
    public class RepositorioComisionesProfesorTest
    {
        private RepositorioComisionesProfesor repo;

        [TestInitialize]
        [TestMethod]
        public void Iniciar()
        {
            repo = new RepositorioComisionesProfesor();
            Assert.IsNotNull(repo);
        }

        [TestMethod]
        public void ObtenerPeriodoActual()
        {
            int periodo = repo.ObtenerPeriodoActual();

            // El periodo actual configurado es 3
            Assert.AreEqual(3, periodo);
        }

        [TestMethod]
        public void ExisteComisionProfesor()
        {
            ComisionesXProfesor comision = new ComisionesXProfesor()
            {
                Comision = 1, //ID de la comisión Consejo de Escuela
                Profesor = 7, //ID del profesor Alejandro Masis
                Periodo = 3 //ID del periodo Actual
            };

            bool result = repo.ExisteComisionProfesor(comision);

            Assert.IsTrue(result); //Ya el profesor Alejandro Masis
                                   //Esta en la comisión
                                   //Consejo de Escuela
        }

        [TestMethod]
        public void CrearComisionProfesor()
        {
            bool resultado = repo.CrearComisionProfesor
                ("Daniel Cortes", "Consejo de Escuela de Administracion",
                "Martes", 930, 1130);

            Assert.IsTrue(resultado);
        }

        [TestCleanup]
        [TestMethod]
        public void Finalizar()
        {
            repo = null;
        }
    }
}
