using ArenaGestor.API.Controllers;
using ArenaGestor.APIContracts.Users;
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
    public class UsersControllerTest
    {
        private Mock<IUsersService> mock;
        private Mock<IMapper> mockMapper;

        private UsersController api;

        private User userOk;
        private UserChangePassword userChangePasswordOk;
        private IEnumerable<User> usersOk;

        private UserGetUsersDto getUsersDto;
        private UserUpdateUserDto updateUserDto;
        private UserInsertUserDto insertUserDto;
        private UserResultUserDto resultUserDto;
        private IEnumerable<UserResultUserDto> resultUsersDto;
        private UserChangePasswordDto userChangePasswordDto;
        private string randomToken;

        [TestInitialize]
        public void InitTest()
        {
            mock = new Mock<IUsersService>(MockBehavior.Strict);
            mockMapper = new Mock<IMapper>(MockBehavior.Strict);

            api = new UsersController(mock.Object, mockMapper.Object);

            randomToken = BusinessHelpers.StringGenerator.GenerateRandomToken(64);

            userOk = new User()
            {
                UserId = 1,
                Name = "Test",
                Surname = "User",
                Email = "test@user.com",
                Password = "testuser123",
                Roles = new List<UserRole>() {
                    new UserRole()
                    {
                        RoleId = RoleCode.Administrador
                    }
                }
            };

            usersOk = new List<User>() { userOk };

            getUsersDto = new UserGetUsersDto()
            {
                Name = "Test",
                Surname = "User",
                Email = "test@user.com"
            };

            resultUserDto = new UserResultUserDto()
            {
                UserId = 1,
                Name = "Test",
                Surname = "User",
                Email = "test@user.com"
            };

            resultUsersDto = new List<UserResultUserDto>()
            {
                resultUserDto
            };

            updateUserDto = new UserUpdateUserDto()
            {
                UserId = 1,
                Name = "Test",
                Surname = "User",
                Roles = new List<UserRoleDto>() {
                    new UserRoleDto()
                    {
                        RoleId = 1
                    }
                }
            };

            insertUserDto = new UserInsertUserDto()
            {
                Name = "Test",
                Surname = "User",
                Email = "test@user.com",
                Password = "testuser123",
                Roles = new List<UserRoleDto>() {
                    new UserRoleDto()
                    {
                        RoleId = 1
                    }
                }
            };

            userChangePasswordDto = new UserChangePasswordDto()
            {
                Email = "test@user.com",
                OldPassword = "testuser123",
                NewPassword = "newuser123"
            };

            userChangePasswordOk = new UserChangePassword("test@user.com", "testuser123", "newuser123");
        }

        [TestMethod]
        public void GetUsersOkTest()
        {
            mock.Setup(x => x.GetUsers(userOk)).Returns(usersOk);
            mockMapper.Setup(x => x.Map<User>(getUsersDto)).Returns(userOk);
            mockMapper.Setup(x => x.Map<IEnumerable<UserResultUserDto>>(usersOk)).Returns(resultUsersDto);

            var result = api.GetUsers(getUsersDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void GetUserByIdOkTest()
        {
            mock.Setup(x => x.GetUserById(userOk.UserId)).Returns(userOk);
            mockMapper.Setup(x => x.Map<UserResultUserDto>(userOk)).Returns(resultUserDto);

            var result = api.GetUserById(userOk.UserId);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void PostUserOkTest()
        {
            mock.Setup(x => x.InsertUser(userOk)).Returns(userOk);
            mockMapper.Setup(x => x.Map<User>(insertUserDto)).Returns(userOk);
            mockMapper.Setup(x => x.Map<UserResultUserDto>(userOk)).Returns(resultUserDto);

            var result = api.PostUser(insertUserDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void PutUserOkTest()
        {
            mock.Setup(x => x.UpdateUser(userOk)).Returns(userOk);
            mockMapper.Setup(x => x.Map<User>(updateUserDto)).Returns(userOk);
            mockMapper.Setup(x => x.Map<UserResultUserDto>(userOk)).Returns(resultUserDto);

            var result = api.PutUser(updateUserDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void DeleteUserOkTest()
        {
            mock.Setup(x => x.DeleteUser(It.IsAny<int>()));
            var result = api.DeleteUser(It.IsAny<int>());
            var objectResult = result as OkResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void ChangePasswordOkTest()
        {
            mock.Setup(x => x.ChangePassword(userChangePasswordOk));
            mockMapper.Setup(x => x.Map<UserChangePassword>(userChangePasswordDto)).Returns(userChangePasswordOk);

            var result = api.PutUserPassword(userChangePasswordDto);
            var objectResult = result as OkResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void ChangePasswordLoggedInOkTest()
        {
            mock.Setup(x => x.ChangePassword(It.IsAny<string>(), userChangePasswordOk));
            mockMapper.Setup(x => x.Map<UserChangePassword>(userChangePasswordDto)).Returns(userChangePasswordOk);
            api.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            api.ControllerContext.HttpContext.Request.Headers["token"] = randomToken;

            var result = api.PutUserLoggedInPassword(userChangePasswordDto);
            var objectResult = result as OkResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }

        [TestMethod]
        public void PutUserLoggedInOkTest()
        {
            mock.Setup(x => x.UpdateUser(It.IsAny<string>(), It.IsAny<User>())).Returns(userOk);
            mockMapper.Setup(x => x.Map<User>(updateUserDto)).Returns(userOk);
            mockMapper.Setup(x => x.Map<UserResultUserDto>(userOk)).Returns(resultUserDto);
            api.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            api.ControllerContext.HttpContext.Request.Headers["token"] = randomToken;
            var result = api.PutUserLoggedIn(updateUserDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
        }
    }
}
