using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SACAAE.Models;

namespace SACAAE.Tests.Controllers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {            
            RepositorioHorario repo = new RepositorioHorario();
            int resul = repo.CrearHorario();
            Console.WriteLine(resul);
        }
    }
}
