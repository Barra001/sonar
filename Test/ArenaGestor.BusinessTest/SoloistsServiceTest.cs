using ArenaGestor.Business;
using ArenaGestor.BusinessInterface;
using ArenaGestor.DataAccessInterface;
using ArenaGestor.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArenaGestor.BusinessTest
{
    [TestClass]
    public class SoloistsServiceTest
    {
        private Mock<ISoloistsManagement> managementMock;
        private SoloistsService managementService;
        private Mock<IGendersService> serviceGenderMock;
        private Mock<IArtistsService> serviceArtistMock;

        private Soloist soloistOk;
        private Soloist soloistWithConcerts;
        private Soloist soloistNull;
        private Soloist soloistEmptyName;
        private Soloist soloistNullName;
        private Soloist soloistNullArtist;
        private Soloist soloistArtistNonExist;
        private Soloist soloistNullGender;
        private Soloist soloistGenderNonExist;
        private Soloist soloistNonValidStartDateFuture;
        private Soloist soloistNonValidStartDatePast;
        private RoleArtist roleArtistOk;

        private IEnumerable<Soloist> soloistsOk;
        private IEnumerable<Soloist> soloistsEmpty;
        private Artist artistOk;
        private Artist artistNonExist;

        private Gender genderOk;
        private Gender genderNull;
        private Artist artistNull;

        private int soloistIdZero;
        private int soloistIdInexistant;

        [TestInitialize]
        public void InitTest()
        {
            serviceArtistMock = new Mock<IArtistsService>(MockBehavior.Strict);
            serviceGenderMock = new Mock<IGendersService>(MockBehavior.Strict);

            managementMock = new Mock<ISoloistsManagement>(MockBehavior.Strict);
            managementService = new SoloistsService(managementMock.Object, serviceGenderMock.Object, serviceArtistMock.Object);

            roleArtistOk = new RoleArtist()
            {
                Name = RoleArtistCode.Cantante.ToString(),
                RoleArtistId = RoleArtistCode.Cantante
            };

            genderOk = new Gender()
            {
                GenderId = 1,
                Name = "Rock"
            };

            artistOk = new Artist()
            {
                ArtistId = 1,
                Name = "Kurt Cobain"
            };

            artistNonExist = new Artist()
            {
                ArtistId = 2,
                Name = "Robert Smith"
            };

            soloistEmptyName = new Soloist()
            {
                MusicalProtagonistId = 1,
                Name = "",
                Gender = genderOk,
                Artist = artistOk,
                StartDate = new DateTime(1987, 08, 01),
                RoleArtist = roleArtistOk
            };

            soloistNullName = new Soloist()
            {
                MusicalProtagonistId = 1,
                Name = null,
                Gender = genderOk,
                Artist = artistOk,
                StartDate = new DateTime(1987, 08, 01),
                RoleArtist = roleArtistOk
            };

            soloistNullArtist = new Soloist()
            {
                MusicalProtagonistId = 1,
                Name = "Kurt Cobain",
                Gender = genderOk,
                Artist = null,
                StartDate = new DateTime(1987, 08, 01),
                RoleArtist = roleArtistOk
            };

            soloistArtistNonExist = new Soloist()
            {
                MusicalProtagonistId = 1,
                Name = "Kurt Cobain",
                Gender = genderOk,
                GenderId = 1,
                Artist = artistNonExist,
                ArtistId = 10,
                StartDate = new DateTime(1987, 08, 01),
                RoleArtistId = roleArtistOk.RoleArtistId
            };

            soloistNullGender = new Soloist()
            {
                MusicalProtagonistId = 1,
                Name = "Kurt Cobain",
                Gender = null,
                Artist = artistOk,
                StartDate = new DateTime(1987, 08, 01),
                RoleArtist = roleArtistOk
            };

            soloistGenderNonExist = new Soloist()
            {
                MusicalProtagonistId = 1,
                Name = "Kurt Cobain",
                GenderId = 2,
                ArtistId = 1,
                StartDate = new DateTime(1987, 08, 01),
                RoleArtistId = roleArtistOk.RoleArtistId
            };

            soloistNonValidStartDateFuture = new Soloist()
            {
                MusicalProtagonistId = 1,
                Name = "Kurt Cobain",
                Gender = genderOk,
                Artist = artistOk,
                StartDate = DateTime.Now.AddDays(1),
                RoleArtist = roleArtistOk
            };

            soloistNonValidStartDatePast = new Soloist()
            {
                MusicalProtagonistId = 1,
                Name = "Kurt Cobain",
                Gender = genderOk,
                Artist = artistOk,
                StartDate = DateTime.Now.AddDays(-1).AddYears(-50),
                RoleArtist = roleArtistOk
            };

            soloistOk = new Soloist()
            {
                MusicalProtagonistId = 1,
                Name = "Kurt Cobain",
                GenderId = 1,
                ArtistId = 1,
                StartDate = new DateTime(1987, 08, 01),
                RoleArtist = roleArtistOk,
                RoleArtistId = roleArtistOk.RoleArtistId
            };

            soloistWithConcerts = new Soloist()
            {
                MusicalProtagonistId = 1,
                Name = "Kurt Cobain",
                GenderId = 1,
                ArtistId = 1,
                StartDate = new DateTime(1987, 08, 01),
                RoleArtist = roleArtistOk,
                Concerts = new List<ConcertProtagonist>()
                {
                    new ConcertProtagonist()
                    {
                        Concert = new Concert()
                        {
                            TourName = "Test concert"
                        }
                    }
                }
            };

            soloistNull = null;
            genderNull = null;
            artistNull = null;
            soloistsOk = new List<Soloist>() { soloistOk };
            soloistsEmpty = new List<Soloist>();
            soloistIdZero = 0;
            soloistIdInexistant = 2;
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GetSoloistByIdInvalidIdTest()
        {
            managementService.GetSoloistById(soloistIdZero);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void GetSoloistByIdNonExistTest()
        {
            managementMock.Setup(x => x.GetSoloistById(soloistIdInexistant)).Returns(soloistNull);
            managementService.GetSoloistById(soloistIdInexistant);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetSoloistByIdOkTest()
        {
            managementMock.Setup(x => x.GetSoloistById(soloistOk.MusicalProtagonistId)).Returns(soloistOk);
            managementService.GetSoloistById(soloistOk.MusicalProtagonistId);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetAllSoloistsTest()
        {
            managementMock.Setup(x => x.GetSoloists()).Returns(soloistsOk);
            managementService.GetSoloists();
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertNullTest()
        {
            managementService.InsertSoloist(soloistNull);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertEmptyNameTest()
        {
            managementService.InsertSoloist(soloistEmptyName);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertNullNameTest()
        {
            managementService.InsertSoloist(soloistNullName);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertNullArtistTest()
        {
            managementService.InsertSoloist(soloistNullArtist);
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]

        public void InsertNonExistArtistTest()
        {
            serviceArtistMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistNull);
            serviceGenderMock.Setup(x => x.GetGenderById(It.IsAny<int>())).Returns(genderOk);

            managementService.InsertSoloist(soloistArtistNonExist);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]

        public void InsertNullGenderTest()
        {
            serviceArtistMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistOk);
            serviceGenderMock.Setup(x => x.GetGenderById(It.IsAny<int>())).Returns(genderNull);

            managementService.InsertSoloist(soloistNullGender);
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]

        public void InsertNonExistGenderTest()
        {
            serviceArtistMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistOk);
            serviceGenderMock.Setup(x => x.GetGenderById(It.IsAny<int>())).Returns(genderNull);

            managementService.InsertSoloist(soloistGenderNonExist);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertNonValidStartDateFutureTest()
        {
            serviceArtistMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistOk);
            serviceGenderMock.Setup(x => x.GetGenderById(It.IsAny<int>())).Returns(genderOk);

            managementService.InsertSoloist(soloistNonValidStartDateFuture);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertNonValidStartDatePastTest()
        {
            serviceArtistMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistOk);
            serviceGenderMock.Setup(x => x.GetGenderById(It.IsAny<int>())).Returns(genderOk);

            managementService.InsertSoloist(soloistNonValidStartDatePast);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertSoloistNameExists()
        {
            serviceArtistMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistOk);
            serviceGenderMock.Setup(x => x.GetGenderById(It.IsAny<int>())).Returns(genderOk);
            managementMock.Setup(x => x.GetSoloists(It.IsAny<Func<Soloist, bool>>())).Returns(soloistsOk);
            managementService.InsertSoloist(soloistOk);
        }

        [TestMethod]
        public void InsertSoloistOkTest()
        {
            managementMock.Setup(x => x.GetSoloists(It.IsAny<Func<Soloist, bool>>())).Returns(soloistsEmpty);
            serviceArtistMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistOk);
            serviceGenderMock.Setup(x => x.GetGenderById(It.IsAny<int>())).Returns(genderOk);
            managementMock.Setup(x => x.InsertSoloist(soloistOk));
            managementMock.Setup(x => x.Save());
            managementService.InsertSoloist(soloistOk);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void UpdateNullTest()
        {
            managementService.UpdateSoloist(soloistNull);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void UpdateEmptyNameTest()
        {
            managementService.UpdateSoloist(soloistEmptyName);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void UpdateNullNameTest()
        {
            managementService.UpdateSoloist(soloistNullName);
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void UpdateSoloistNonExistTest()
        {
            serviceArtistMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistOk);
            serviceGenderMock.Setup(x => x.GetGenderById(It.IsAny<int>())).Returns(genderOk);
            managementMock.Setup(x => x.GetSoloistById(soloistOk.MusicalProtagonistId)).Returns(soloistNull);
            managementService.UpdateSoloist(soloistOk);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void UpdateSoloistOkTest()
        {
            serviceArtistMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistOk);
            serviceGenderMock.Setup(x => x.GetGenderById(It.IsAny<int>())).Returns(genderOk);
            managementMock.Setup(x => x.GetSoloistById(It.IsAny<int>())).Returns(soloistOk);
            managementMock.Setup(x => x.UpdateSoloist(soloistOk));
            managementMock.Setup(x => x.Save());
            managementService.UpdateSoloist(soloistOk);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void DeleteSoloistInvalidIdTest()
        {
            managementService.DeleteSoloist(soloistIdZero);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void DeleteSoloistNonExistTest()
        {
            managementMock.Setup(x => x.GetSoloistById(soloistIdInexistant)).Returns(soloistNull);
            managementService.DeleteSoloist(soloistIdInexistant);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void DeleteSoloistWithConcertsTest()
        {
            managementMock.Setup(x => x.GetSoloistById(soloistIdInexistant)).Returns(soloistWithConcerts);
            managementService.DeleteSoloist(soloistIdInexistant);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void DeleteSoloistOkTest()
        {
            managementMock.Setup(x => x.GetSoloistById(soloistOk.MusicalProtagonistId)).Returns(soloistOk);
            managementMock.Setup(x => x.DeleteSoloist(soloistOk));
            managementMock.Setup(x => x.Save());
            managementService.DeleteSoloist(soloistOk.MusicalProtagonistId);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetFilterSoloistNullNameTest()
        {
            managementMock.Setup(x => x.GetSoloists()).Returns(soloistsOk);
            managementService.GetSoloists(soloistNullName);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetFilterSoloistEmptyNameTest()
        {
            managementMock.Setup(x => x.GetSoloists()).Returns(soloistsOk);
            managementService.GetSoloists(soloistEmptyName);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetFilterSoloistTest()
        {
            managementMock.Setup(x => x.GetSoloists(It.IsAny<Func<Soloist, bool>>())).Returns(soloistsOk);
            managementService.GetSoloists(soloistOk);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void SoloistByArtistNull()
        {
            IEnumerable<Soloist> soloists = managementService.GetSoloistsByArtist(null);
            Assert.AreEqual(0, soloists.Count());
        }

        [TestMethod]
        public void SoloistByArtistOk()
        {
            managementMock.Setup(x => x.GetSoloists(It.IsAny<Func<Soloist, bool>>())).Returns(soloistsOk);
            IEnumerable<Soloist> soloists = managementService.GetSoloistsByArtist(artistOk);
            managementMock.VerifyAll();
        }

    }
}
