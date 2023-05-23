using ArenaGestor.API.Controllers;
using ArenaGestor.APIContracts.Artist;
using ArenaGestor.BusinessInterface;
using ArenaGestor.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace ArenaGestor.APITest
{
    [TestClass]
    public class ArtistsControllerTest
    {
        private Mock<IArtistsService> mock;
        private Mock<IMapper> mockMapper;

        private ArtistsController api;

        private Artist artistOk;
        private Artist artistWithUserOk;
        private IEnumerable<Artist> artistsOk;

        private ArtistGetArtistsDto getArtistsDto;
        private ArtistUpdateArtistDto updateArtistDto;
        private ArtistInsertArtistDto insertArtistDto;
        private ArtistUpdateArtistDto updateArtistWithUserDto;
        private ArtistInsertArtistDto insertArtistWithUserDto;
        private ArtistResultArtistDto resultArtistDto;
        private ArtistUpdateUserDto userUpdate;
        private ArtistInsertUserDto userInsert;
        private IEnumerable<ArtistResultArtistDto> resultArtistsDto;

        [TestInitialize]
        public void InitTest()
        {
            mock = new Mock<IArtistsService>(MockBehavior.Strict);
            mockMapper = new Mock<IMapper>(MockBehavior.Strict);

            api = new ArtistsController(mock.Object, mockMapper.Object);

            artistOk = new Artist()
            {
                ArtistId = 1,
                Name = "Kurt Cobain"
            };

            artistWithUserOk = new Artist()
            {
                ArtistId = 1,
                Name = "Kurt Cobain",
                User = new User()
                {
                    UserId = 1
                }
            };

            getArtistsDto = new ArtistGetArtistsDto()
            {
                Name = "Kurt Cobain"
            };

            updateArtistDto = new ArtistUpdateArtistDto()
            {
                ArtistId = 1,
                Name = "Kurt Cobain"
            };

            insertArtistDto = new ArtistInsertArtistDto()
            {
                Name = "Kurt Cobain"
            };

            userUpdate = new ArtistUpdateUserDto()
            {
                UserId = 1
            };

            userInsert = new ArtistInsertUserDto()
            {
                UserId = 1
            };

            updateArtistWithUserDto = new ArtistUpdateArtistDto()
            {
                ArtistId = 1,
                Name = "Kurt Cobain",
                UserId = userUpdate.UserId
            };

            insertArtistWithUserDto = new ArtistInsertArtistDto()
            {
                Name = "Kurt Cobain",
                UserId = userUpdate.UserId
            };

            resultArtistDto = new ArtistResultArtistDto()
            {
                UserId = 1,
                ArtistId = 1,
                Name = "Kurt Cobain"
            };

            resultArtistsDto = new List<ArtistResultArtistDto>()
            {
                new ArtistResultArtistDto()
                {
                    UserId = 1,
                    ArtistId = 1,
                    Name = "Kurt Cobain"
                }
            };

            artistsOk = new List<Artist>() { artistOk };
        }

        [TestMethod]
        public void GetArtistByIdOkTest()
        {
            mock.Setup(x => x.GetArtistById(artistOk.ArtistId)).Returns(artistOk);
            mockMapper.Setup(x => x.Map<ArtistResultArtistDto>(artistOk)).Returns(resultArtistDto);

            var result = api.GetArtistById(artistOk.ArtistId);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void GetArtistsOkTest()
        {
            mock.Setup(x => x.GetArtists(artistOk)).Returns(artistsOk);
            mockMapper.Setup(x => x.Map<Artist>(getArtistsDto)).Returns(artistOk);
            mockMapper.Setup(x => x.Map<IEnumerable<ArtistResultArtistDto>>(artistsOk)).Returns(resultArtistsDto);

            var result = api.GetArtists(getArtistsDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void PostArtistOkTest()
        {
            mock.Setup(x => x.InsertArtist(artistOk)).Returns(artistOk);
            mockMapper.Setup(x => x.Map<Artist>(insertArtistDto)).Returns(artistOk);
            mockMapper.Setup(x => x.Map<ArtistResultArtistDto>(artistOk)).Returns(resultArtistDto);

            var result = api.PostArtist(insertArtistDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void PostArtistWithUserOkTest()
        {
            mock.Setup(x => x.InsertArtist(artistWithUserOk)).Returns(artistWithUserOk);
            mockMapper.Setup(x => x.Map<Artist>(insertArtistWithUserDto)).Returns(artistWithUserOk);
            mockMapper.Setup(x => x.Map<ArtistResultArtistDto>(artistWithUserOk)).Returns(resultArtistDto);

            var result = api.PostArtist(insertArtistWithUserDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void PutArtistOkTest()
        {
            mock.Setup(x => x.UpdateArtist(artistOk)).Returns(artistOk);
            mockMapper.Setup(x => x.Map<Artist>(updateArtistDto)).Returns(artistOk);
            mockMapper.Setup(x => x.Map<ArtistResultArtistDto>(artistOk)).Returns(resultArtistDto);

            var result = api.PutArtist(updateArtistDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void PutArtistWithUserOkTest()
        {
            mock.Setup(x => x.UpdateArtist(artistWithUserOk)).Returns(artistWithUserOk);
            mockMapper.Setup(x => x.Map<Artist>(updateArtistWithUserDto)).Returns(artistWithUserOk);
            mockMapper.Setup(x => x.Map<ArtistResultArtistDto>(artistWithUserOk)).Returns(resultArtistDto);

            var result = api.PutArtist(updateArtistWithUserDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void DeleteArtistOkTest()
        {
            mock.Setup(x => x.DeleteArtist(It.IsAny<int>()));
            var result = api.DeleteArtist(It.IsAny<int>());
            var objectResult = result as OkResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

    }
}
