using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ArenaGestor.Business;
using ArenaGestor.Domain;
using ArenaGestor.DataAccessInterface;
using Moq;

namespace ArenaGestor.BusinessTest
{
    [TestClass]
    public class SnackServiceTest
    {
       
        private SnackService snackService;
        private Mock<ISnackManagement> managementMock;
        private Snack okSnack;
        private Snack missingDescriptionSnack;
        private Snack invalidPriceSnack;
        private List<Snack> snackList;


        [TestInitialize]
        public void InitTest()
        {

            managementMock = new Mock<ISnackManagement>(MockBehavior.Strict);
            snackService = new SnackService(managementMock.Object);
            snackList = new List<Snack>(){okSnack};
            invalidPriceSnack = new Snack()
            {
                Id = 1,
                Description = "Papitas",
                Price = -100
            };
            missingDescriptionSnack = new Snack()
            {
                Id = 1,
                Price = 100
            };
            okSnack = new Snack()
            {
                Id = 1,
                Description = "Papitas",
                Price = 100
            };
        }
        [TestMethod]
        public void AddValidSnackOkTest()
        {
            managementMock.Setup(x => x.Save());
            managementMock.Setup(x => x.InsertSnack(okSnack));
            snackService.AddSnack(okSnack);
            managementMock.VerifyAll();
        }
        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void AddNullSnackTest()
        {
            managementMock.Setup(x => x.InsertSnack(null)).Throws(new Exception());
            snackService.AddSnack(null);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void AddMissingDescriptionSnackTest()
        {
            managementMock.Setup(x => x.InsertSnack(missingDescriptionSnack)).Throws(new Exception());
            snackService.AddSnack(missingDescriptionSnack);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void AddInvalidPriceSnackTest()
        {
            managementMock.Setup(x => x.InsertSnack(invalidPriceSnack)).Throws(new Exception());
            snackService.AddSnack(invalidPriceSnack);
            managementMock.VerifyAll();
        }
        [TestMethod]
        public void RemoveValidSnackOkTest()
        {
            managementMock.Setup(x => x.Save());
            managementMock.Setup(x => x.DeleteSnack(It.IsAny<Snack>()));
            snackService.RemoveSnack(okSnack.Id);
            managementMock.VerifyAll();
        }
        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public void RemoveNonExistingSnackTest()
        {
            managementMock.Setup(x => x.Save());
            managementMock.Setup(x => x.DeleteSnack(It.IsAny<Snack>())).Throws(new KeyNotFoundException());
            snackService.RemoveSnack(okSnack.Id);
            managementMock.VerifyAll();
        }
        [TestMethod]
        public void GetSnackTest()
        {
            
            managementMock.Setup(x => x.GetSnacks()).Returns(snackList);
            snackService.GetSnacks();
            Assert.AreEqual(snackList, snackService.GetSnacks());
            managementMock.VerifyAll();
        }


    }
}
