using ArenaGestor.API.Controllers;
using ArenaGestor.BusinessInterface;
using ArenaGestor.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArenaGestor.APIContracts.Snack;

namespace ArenaGestor.APITest
{
    [TestClass]
    public class SnackControllerTest
    {
        private Mock<ISnackService> mock;
        private Mock<IMapper> mockMapper;
        private SnacksController api;
        private Snack okSnack;
        private Snack missingDescriptionSnack;
        private Snack invalidPriceSnack;
        private List<Snack> snackList;

        [TestInitialize]
        public void InitTest()
        {
            mock = new Mock<ISnackService>(MockBehavior.Strict);
            mockMapper = new Mock<IMapper>(MockBehavior.Strict);
            api = new SnacksController(mock.Object, mockMapper.Object);
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
            snackList = new List<Snack>() { okSnack };
        }
        [TestMethod]
        public void AddSnackOk() {
            SnackDto snackDto = new SnackDto()
            {
                Description = okSnack.Description,
                Price = okSnack.Price
            };
            mock.Setup(x => x.AddSnack(okSnack));
            mockMapper.Setup(x => x.Map<Snack>(snackDto)).Returns(okSnack);
            var result = api.PostSnacks(snackDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var expectedResponse = new SnackResponseDto { Message = "Snack added successfully" };
            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
            Assert.AreEqual(expectedResponse.Message, (objectResult.Value as SnackResponseDto).Message);
        }
        [TestMethod]
        public void DeleteSnackOk()
        {
            mock.Setup(x => x.RemoveSnack(okSnack.Id));
            var result = api.DeleteSnacks(okSnack.Id);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var expectedResponse = new SnackResponseDto { Message = "Snack deleted successfully" };
            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
            Assert.AreEqual(expectedResponse.Message, (objectResult.Value as SnackResponseDto).Message);
        }
        [TestMethod]
        public void GetSnackOk()
        {
            mock.Setup(x => x.GetSnacks()).Returns(snackList);
            var result = api.GetSnacks();
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
            Assert.AreEqual(snackList, objectResult.Value);
        }
    }
}
