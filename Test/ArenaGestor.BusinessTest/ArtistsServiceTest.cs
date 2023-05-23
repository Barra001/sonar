using ArenaGestor.Business;
using ArenaGestor.BusinessInterface;
using ArenaGestor.DataAccessInterface;
using ArenaGestor.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace ArenaGestor.BusinessTest
{
    [TestClass]
    public class ArtistsServiceTest
    {
        private Mock<IArtistsManagement> managementMock;
        private Mock<IUsersService> usersServiceMock;
        private ArtistsService managementService;

        private Artist artistOk;
        private Artist artistNull;
        private Artist artistEmptyName;
        private Artist artistNullName;
        private Artist artistWithBands;
        private Artist artistWithSoloists;
        private Artist artistNullBands;
        private Artist artistNullSoloists;
        private Artist artistWithUserOk;
        private User userOk;
        private User userNoArtist;

        private IEnumerable<Artist> artistsOk;
        private IEnumerable<Artist> artistsEmpty;
        private int artistIdZero;
        private int artistIdInexistant;

        [TestInitialize]
        public void InitTest()
        {
            managementMock = new Mock<IArtistsManagement>(MockBehavior.Strict);
            usersServiceMock = new Mock<IUsersService>(MockBehavior.Strict);

            managementService = new ArtistsService(managementMock.Object, usersServiceMock.Object);

            UserRole roleOk = new UserRole()
            {
                RoleId = RoleCode.Artista
            };

            List<UserRole> rolesOk = new List<UserRole>() {
                roleOk
            };

            UserRole roleNoArtist = new UserRole()
            {
                RoleId = RoleCode.Administrador
            };

            List<UserRole> rolesNoArtist = new List<UserRole>() {
                roleNoArtist
            };

            userOk = new User()
            {
                UserId = 1,
                Roles = rolesOk
            };

            userNoArtist = new User()
            {
                UserId = 1,
                Roles = rolesNoArtist
            };

            artistOk = new Artist()
            {
                ArtistId = 1,
                Name = "Kurt Cobain"
            };

            artistWithUserOk = new Artist()
            {
                ArtistId = 1,
                Name = "Kurt Cobain",
                UserId = 1,
                User = new User()
                {
                    UserId = 1
                }
            };

            artistEmptyName = new Artist()
            {
                ArtistId = 1,
                Name = ""
            };

            artistNullName = new Artist()
            {
                ArtistId = 1,
                Name = null
            };

            artistWithBands = new Artist()
            {
                ArtistId = 1,
                Name = "Kurt Cobain",
                Bands = new List<ArtistBand>()
                {
                    new ArtistBand()
                    {
                        Band = new Band(){
                            Name = "Test Band"
                        }
                    }
                }
            };

            artistWithSoloists = new Artist()
            {
                ArtistId = 1,
                Name = "Kurt Cobain",
                Soloists = new List<Soloist>()
                {
                    new Soloist()
                    {
                        Name = "Test soloist"
                    }
                }
            };

            artistNullBands = new Artist()
            {
                ArtistId = 1,
                Name = "Kurt Cobain",
                Bands = null
            };

            artistNullSoloists = new Artist()
            {
                ArtistId = 1,
                Name = "Kurt Cobain",
                Soloists = null
            };

            artistNull = null;
            artistsOk = new List<Artist>() { artistOk };
            artistsEmpty = new List<Artist>();
            artistIdZero = 0;
            artistIdInexistant = 2;
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GetArtistByIdInvalidIdTest()
        {
            managementService.GetArtistById(artistIdZero);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void GetArtistByIdNonExistTest()
        {
            managementMock.Setup(x => x.GetArtistById(artistIdInexistant)).Returns(artistNull);
            managementService.GetArtistById(artistIdInexistant);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetArtistByIdOkTest()
        {
            managementMock.Setup(x => x.GetArtistById(artistOk.ArtistId)).Returns(artistOk);
            managementService.GetArtistById(artistOk.ArtistId);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetAllArtistsTest()
        {
            managementMock.Setup(x => x.GetArtists()).Returns(artistsOk);
            managementService.GetArtists();
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetFilterArtistTest()
        {
            managementMock.Setup(x => x.GetArtists(It.IsAny<Func<Artist, bool>>())).Returns(artistsOk);
            managementService.GetArtists(artistOk);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetFilterArtistEmptyNameTest()
        {
            managementMock.Setup(x => x.GetArtists()).Returns(artistsOk);
            managementService.GetArtists(artistEmptyName);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void GetFilterArtistNullNameTest()
        {
            managementMock.Setup(x => x.GetArtists()).Returns(artistsOk);
            managementService.GetArtists(artistNullName);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertNullTest()
        {
            managementService.InsertArtist(artistNull);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertEmptyNameTest()
        {
            managementService.InsertArtist(artistEmptyName);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertNullNameTest()
        {
            managementService.InsertArtist(artistNullName);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void InsertArtistNameExists()
        {
            managementMock.Setup(x => x.GetArtists(It.IsAny<Func<Artist, bool>>())).Returns(artistsOk);
            managementService.InsertArtist(artistOk);
        }

        [TestMethod]
        public void InsertArtistWithUserOkTest()
        {
            managementMock.Setup(x => x.GetArtists(It.IsAny<Func<Artist, bool>>())).Returns(artistsEmpty);
            usersServiceMock.Setup(x => x.GetUserById(It.IsAny<int>())).Returns(userOk);

            managementMock.Setup(x => x.InsertArtist(artistWithUserOk));
            managementMock.Setup(x => x.Save());
            managementService.InsertArtist(artistWithUserOk);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void InsertArtistWithUserNoArtistTest()
        {
            managementMock.Setup(x => x.GetArtists(It.IsAny<Func<Artist, bool>>())).Returns(artistsEmpty);
            usersServiceMock.Setup(x => x.GetUserById(It.IsAny<int>())).Returns(userNoArtist);

            managementService.InsertArtist(artistWithUserOk);
        }

        [TestMethod]
        public void InsertArtistOkTest()
        {
            managementMock.Setup(x => x.GetArtists(It.IsAny<Func<Artist, bool>>())).Returns(artistsEmpty);
            managementMock.Setup(x => x.InsertArtist(artistOk));
            managementMock.Setup(x => x.Save());
            managementService.InsertArtist(artistOk);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void UpdateNullTest()
        {
            managementService.UpdateArtist(artistNull);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void UpdateEmptyNameTest()
        {
            managementService.UpdateArtist(artistEmptyName);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void UpdateNullNameTest()
        {
            managementService.UpdateArtist(artistNullName);
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void UpdateArtistNonExistTest()
        {
            managementMock.Setup(x => x.GetArtistById(artistOk.ArtistId)).Returns(artistNull);
            managementService.UpdateArtist(artistOk);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void UpdateArtistWithUserOkTest()
        {
            managementMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistOk);
            usersServiceMock.Setup(x => x.GetUserById(It.IsAny<int>())).Returns(userOk);

            managementMock.Setup(x => x.UpdateArtist(artistWithUserOk));
            managementMock.Setup(x => x.Save());
            managementService.UpdateArtist(artistWithUserOk);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void UpdateArtistWithUserNoArtistTest()
        {
            managementMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistOk);
            usersServiceMock.Setup(x => x.GetUserById(It.IsAny<int>())).Returns(userNoArtist);

            managementService.UpdateArtist(artistWithUserOk);
        }

        [TestMethod]
        public void UpdateArtistOkTest()
        {
            managementMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistOk);
            managementMock.Setup(x => x.UpdateArtist(artistOk));
            managementMock.Setup(x => x.Save());
            managementService.UpdateArtist(artistOk);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void DeleteArtistInvalidIdTest()
        {
            managementService.DeleteArtist(artistIdZero);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void DeleteArtistNonExistTest()
        {
            managementMock.Setup(x => x.GetArtistById(artistIdInexistant)).Returns(artistNull);
            managementService.DeleteArtist(artistIdInexistant);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void DeleteArtistWithBands()
        {
            managementMock.Setup(x => x.GetArtistById(artistIdInexistant)).Returns(artistWithBands);
            managementService.DeleteArtist(artistIdInexistant);
            managementMock.VerifyAll();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void DeleteArtistWithSoloists()
        {
            managementMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistWithSoloists);
            managementMock.Setup(x => x.DeleteArtist(artistOk));
            managementMock.Setup(x => x.Save());
            managementService.DeleteArtist(artistOk.ArtistId);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void DeleteArtistWithNullSoloists()
        {
            managementMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistNullSoloists);
            managementMock.Setup(x => x.DeleteArtist(artistOk));
            managementMock.Setup(x => x.Save());
            managementService.DeleteArtist(artistOk.ArtistId);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void DeleteArtistWithNullBands()
        {
            managementMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistNullBands);
            managementMock.Setup(x => x.DeleteArtist(artistOk));
            managementMock.Setup(x => x.Save());
            managementService.DeleteArtist(artistOk.ArtistId);
            managementMock.VerifyAll();
        }

        [TestMethod]
        public void DeleteArtistOkTest()
        {
            managementMock.Setup(x => x.GetArtistById(It.IsAny<int>())).Returns(artistOk);
            managementMock.Setup(x => x.DeleteArtist(artistOk));
            managementMock.Setup(x => x.Save());
            managementService.DeleteArtist(artistOk.ArtistId);
            managementMock.VerifyAll();
        }
    }
}
