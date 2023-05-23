using ArenaGestor.API.Controllers;
using ArenaGestor.APIContracts.Soloist;
using ArenaGestor.BusinessInterface;
using ArenaGestor.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace ArenaGestor.APITest
{
    [TestClass]
    public class SoloistsControllerTest
    {
        private Mock<ISoloistsService> mock;
        private Mock<IMapper> mockMapper;
        private SoloistsController api;

        private Soloist soloistOk;
        private IEnumerable<Soloist> soloistsOk;
        private Gender genderOk;
        private Artist artistOk;

        private SoloistResultSoloistDto resultSoloistDto;
        private IEnumerable<SoloistResultSoloistDto> resultSoloistsDto;
        private SoloistResultArtistDto resultArtistDto;
        private SoloistResultGenderDto resultGenderDto;
        private SoloistResultConcertDto resultConcertDto;
        private SoloistGetSoloistsDto getSoloistsDto;
        private SoloistGetArtistsDto getArtistsDto;
        private SoloistInsertSoloistDto insertSoloistDto;
        private SoloistUpdateSoloistDto updateSoloistDto;

        [TestInitialize]
        public void InitTest()
        {
            mock = new Mock<ISoloistsService>(MockBehavior.Strict);
            mockMapper = new Mock<IMapper>(MockBehavior.Strict);

            api = new SoloistsController(mock.Object, mockMapper.Object);

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

            soloistOk = new Soloist()
            {
                MusicalProtagonistId = 1,
                Name = "Nirvana",
                Artist = artistOk,
                Gender = genderOk,
                StartDate = new DateTime(1987, 08, 01)
            };

            soloistsOk = new List<Soloist>() { soloistOk };

            resultArtistDto = new SoloistResultArtistDto()
            {
                ArtistId = 1,
                Name = "Kurt Cobain"
            };

            resultGenderDto = new SoloistResultGenderDto()
            {
                GenderId = 1,
                Name = "Rock"
            };

            resultConcertDto = new SoloistResultConcertDto()
            {
                ConcertId = 1,
                TourName = "Olé Tour",
                Date = DateTime.Now.AddDays(10),
                Price = 100,
                TicketCount = 500
            };

            resultSoloistDto = new SoloistResultSoloistDto()
            {
                MusicalProtagonistId = 1,
                Name = "Nirvana",
                Artist = resultArtistDto,
                Gender = resultGenderDto,
                StartDate = new DateTime(1987, 08, 01),
                Concerts = new List<SoloistResultConcertDto>()
                {
                    resultConcertDto
                },
                RoleArtist = new SoloistResultRoleArtistDto()
                {
                    Name = RoleArtistCode.Cantante.ToString()
                }
            };

            getSoloistsDto = new SoloistGetSoloistsDto()
            {
                Name = "Nirvana"
            };

            resultSoloistsDto = new List<SoloistResultSoloistDto>()
            {
                resultSoloistDto
            };

            getArtistsDto = new SoloistGetArtistsDto()
            {
                Name = "Kurt Cobain"
            };

            insertSoloistDto = new SoloistInsertSoloistDto()
            {
                ArtistId = 1,
                GenderId = 1,
                Name = "Kurt Cobain",
                StartDate = new DateTime(1987, 08, 01),
                RoleArtistId = (int)RoleArtistCode.Cantante
            };

            updateSoloistDto = new SoloistUpdateSoloistDto()
            {
                ArtistId = 1,
                GenderId = 1,
                Name = "Kurt Cobain",
                StartDate = new DateTime(1987, 08, 01),
                RoleArtistId = (int)RoleArtistCode.Cantante
            };
        }

        [TestMethod]
        public void GetSoloistByIdOkTest()
        {
            mock.Setup(x => x.GetSoloistById(soloistOk.MusicalProtagonistId)).Returns(soloistOk);
            mockMapper.Setup(x => x.Map<SoloistResultSoloistDto>(soloistOk)).Returns(resultSoloistDto);

            var result = api.GetSoloistById(soloistOk.MusicalProtagonistId);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void GetSoloistsOkTest()
        {
            mock.Setup(x => x.GetSoloists(soloistOk)).Returns(soloistsOk);
            mockMapper.Setup(x => x.Map<Soloist>(getSoloistsDto)).Returns(soloistOk);
            mockMapper.Setup(x => x.Map<IEnumerable<SoloistResultSoloistDto>>(soloistsOk)).Returns(resultSoloistsDto);

            var result = api.GetSoloists(getSoloistsDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void GetSoloistsByArtistOkTest()
        {
            mock.Setup(x => x.GetSoloistsByArtist(artistOk)).Returns(soloistsOk);
            mockMapper.Setup(x => x.Map<Artist>(getArtistsDto)).Returns(artistOk);
            mockMapper.Setup(x => x.Map<IEnumerable<SoloistResultSoloistDto>>(soloistsOk)).Returns(resultSoloistsDto);

            var result = api.GetSoloistsByArtist(getArtistsDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void PostSoloistOkTest()
        {
            mock.Setup(x => x.InsertSoloist(soloistOk)).Returns(soloistOk);
            mockMapper.Setup(x => x.Map<Soloist>(insertSoloistDto)).Returns(soloistOk);
            mockMapper.Setup(x => x.Map<SoloistResultSoloistDto>(soloistOk)).Returns(resultSoloistDto);

            var result = api.PostSoloist(insertSoloistDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void PutSoloistOkTest()
        {
            mock.Setup(x => x.UpdateSoloist(soloistOk)).Returns(soloistOk);
            mockMapper.Setup(x => x.Map<Soloist>(updateSoloistDto)).Returns(soloistOk);
            mockMapper.Setup(x => x.Map<SoloistResultSoloistDto>(soloistOk)).Returns(resultSoloistDto);

            var result = api.PutSoloist(updateSoloistDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void DeleteSoloistOkTest()
        {
            mock.Setup(x => x.DeleteSoloist(It.IsAny<int>()));
            var result = api.DeleteSoloist(It.IsAny<int>());
            var objectResult = result as OkResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

    }
}
