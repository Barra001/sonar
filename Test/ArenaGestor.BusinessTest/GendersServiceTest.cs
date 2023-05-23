using ArenaGestor.Business;
using ArenaGestor.DataAccessInterface;
using ArenaGestor.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace ArenaGestor.BusinessTest
{
    [TestClass]
    public class GendersServiceTest
    {
        private Mock<IGendersManagement> managementMock;
        private GendersService managementService;

        private Gender genderOk;
        private Gender genderWithProtagonists;
        private Gender genderNull;
        private IEnumerable<Gender> gendersOk;
        private IEnumerable<Gender> gendersEmpty;
        private Gender genderEmptyName;
        private Gender genderNullName;

        private int genderIdZero;
        private int genderIdInexistant;

        [TestInitialize]
        public void InitTest()
        {
            managementMock = new Mock<IGendersManagement>(MockBehavior.Strict);
            managementService = new GendersService(managementMock.Object);

            genderOk = new Gender()
            {
                GenderId = 1,
                Name = "Rock"
            };

            genderWithProtagonists = new Gender()
            {
                GenderId = 1,
                Name = "Rock",
                MusicalProtagonists = new List<MusicalProtagonist>()
                {
                     new Soloist()
                     {
                         Name = "Test soloist"
                     }
                }
            };

            genderEmptyName = new Gender()
            {
                GenderId = 1,
                Name = ""
            };

            genderNullName = new Gender()
            {
                GenderId = 1,
                Name = null
            };

            genderNull = null;
            gendersOk = new List<Gender>() { genderOk };
            gendersEmpty = new List<Gender>() { };
            genderIdZero = 0;
            genderIdInexistant = 2;
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GetGenderByIdInvalidIdTest()
        {
            managementService.GetGenderById(genderIdZero);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void GetGenderByIdNonExistTest()
        {
            managementMock.Setup(x => x.GetGenderById(genderIdInexistant)).Returns(genderNull);
            managementService.GetGenderById(genderIdInexistant);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetGenderByIdOkTest()
        {
            managementMock.Setup(x => x.GetGenderById(genderOk.GenderId)).Returns(genderOk);
            managementService.GetGenderById(genderOk.GenderId);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetAllGendersTest()
        {
            managementMock.Setup(x => x.GetGenders()).Returns(gendersOk);
            managementService.GetGenders();
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetFilterGenderTest()
        {
            managementMock.Setup(x => x.GetGenders(It.IsAny<Func<Gender, bool>>())).Returns(gendersOk);
            managementService.GetGenders(genderOk);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetFilterGenderEmptyNameTest()
        {
            managementMock.Setup(x => x.GetGenders()).Returns(gendersOk);
            managementService.GetGenders(genderEmptyName);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetFilterGenderNullNameTest()
        {
            managementMock.Setup(x => x.GetGenders()).Returns(gendersOk);
            managementService.GetGenders(genderNullName);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertNullTest()
        {
            managementService.InsertGender(genderNull);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertEmptyNameTest()
        {
            managementService.InsertGender(genderEmptyName);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertNullNameTest()
        {
            managementService.InsertGender(genderNullName);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertGenderNameExists()
        {
            managementMock.Setup(x => x.GetGenders(It.IsAny<Func<Gender, bool>>())).Returns(gendersOk);
            managementService.InsertGender(genderOk);
        }

        [TestMethod]
        public void InsertGenderOkTest()
        {
            managementMock.Setup(x => x.GetGenders(It.IsAny<Func<Gender, bool>>())).Returns(gendersEmpty);
            managementMock.Setup(x => x.InsertGender(genderOk));
            managementMock.Setup(x => x.Save());
            managementService.InsertGender(genderOk);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void UpdateNullTest()
        {
            managementService.UpdateGender(null);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void UpdateEmptyNameTest()
        {
            managementService.UpdateGender(genderEmptyName);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void UpdateNullNameTest()
        {
            managementService.UpdateGender(genderNullName);
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void UpdateGenderNonExistTest()
        {
            managementMock.Setup(x => x.GetGenderById(genderOk.GenderId)).Returns(genderNull);
            managementService.UpdateGender(genderOk);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void UpdateGenderOkTest()
        {
            managementMock.Setup(x => x.GetGenderById(It.IsAny<int>())).Returns(genderOk);
            managementMock.Setup(x => x.UpdateGender(genderOk));
            managementMock.Setup(x => x.Save());
            managementService.UpdateGender(genderOk);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void DeleteGenderInvalidIdTest()
        {
            managementService.DeleteGender(genderIdZero);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void DeleteGenderNonExistTest()
        {
            managementMock.Setup(x => x.GetGenderById(genderIdInexistant)).Returns(genderNull);
            managementService.DeleteGender(genderIdInexistant);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void DeleteGenderWithProtagonistsTest()
        {
            managementMock.Setup(x => x.GetGenderById(genderIdInexistant)).Returns(genderWithProtagonists);
            managementService.DeleteGender(genderIdInexistant);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void DeleteGenderOkTest()
        {
            managementMock.Setup(x => x.GetGenderById(genderOk.GenderId)).Returns(genderOk);
            managementMock.Setup(x => x.DeleteGender(genderOk));
            managementMock.Setup(x => x.Save());
            managementService.DeleteGender(genderOk.GenderId);
            managementMock.VerifyAll();
        }
    }
}
