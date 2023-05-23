using ArenaGestor.API.Controllers;
using ArenaGestor.APIContracts.Gender;
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
    public class GendersControllerTest
    {
        private Mock<IGendersService> mock;
        private Mock<IMapper> mockMapper;

        private GendersController api;

        private Gender genderOk;
        private IEnumerable<Gender> gendersOk;

        private GenderResultGenderDto resultGenderDto;
        private GenderGetGendersDto getGendersDto;
        private IEnumerable<GenderResultGenderDto> resultGendersDto;
        private GenderInsertGenderDto insertGenderDto;
        private GenderUpdateGenderDto updateGenderDto;

        [TestInitialize]
        public void InitTest()
        {
            mock = new Mock<IGendersService>(MockBehavior.Strict);
            mockMapper = new Mock<IMapper>(MockBehavior.Strict);

            api = new GendersController(mock.Object, mockMapper.Object);

            genderOk = new Gender()
            {
                GenderId = 1,
                Name = "Rock"
            };
            gendersOk = new List<Gender>() { genderOk };

            resultGenderDto = new GenderResultGenderDto()
            {
                GenderId = 1,
                Name = "Rock"
            };

            getGendersDto = new GenderGetGendersDto()
            {
                Name = "Rock"
            };

            resultGendersDto = new List<GenderResultGenderDto>()
            {
                 resultGenderDto
            };

            insertGenderDto = new GenderInsertGenderDto()
            {
                Name = "Rock"
            };

            updateGenderDto = new GenderUpdateGenderDto()
            {
                GenderId = 1,
                Name = "Rock"
            };
        }

        [TestMethod]
        public void GetGenderByIdOkTest()
        {
            mock.Setup(x => x.GetGenderById(genderOk.GenderId)).Returns(genderOk);
            mockMapper.Setup(x => x.Map<GenderResultGenderDto>(genderOk)).Returns(resultGenderDto);

            var result = api.GetGenderById(genderOk.GenderId);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void GetGendersOkTest()
        {
            mock.Setup(x => x.GetGenders(genderOk)).Returns(gendersOk);
            mockMapper.Setup(x => x.Map<Gender>(getGendersDto)).Returns(genderOk);
            mockMapper.Setup(x => x.Map<IEnumerable<GenderResultGenderDto>>(gendersOk)).Returns(resultGendersDto);

            var result = api.GetGenders(getGendersDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void PostGenderOkTest()
        {
            mock.Setup(x => x.InsertGender(genderOk)).Returns(genderOk);
            mockMapper.Setup(x => x.Map<Gender>(insertGenderDto)).Returns(genderOk);
            mockMapper.Setup(x => x.Map<GenderResultGenderDto>(genderOk)).Returns(resultGenderDto);

            var result = api.PostGender(insertGenderDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void PutGenderOkTest()
        {
            mock.Setup(x => x.UpdateGender(genderOk)).Returns(genderOk);
            mockMapper.Setup(x => x.Map<Gender>(updateGenderDto)).Returns(genderOk);
            mockMapper.Setup(x => x.Map<GenderResultGenderDto>(genderOk)).Returns(resultGenderDto);

            var result = api.PutGender(updateGenderDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void DeleteGenderOkTest()
        {
            mock.Setup(x => x.DeleteGender(It.IsAny<int>()));
            var result = api.DeleteGender(It.IsAny<int>());
            var objectResult = result as OkResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

    }
}
