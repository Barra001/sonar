using ArenaGestor.DataAccess.Managements;
using ArenaGestor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArenaGestor.DataAccessTest
{
    [TestClass]
    public class UsersManagementTest : ManagementTest
    {
        private DbContext context;
        private UsersManagement management;

        private User userOk;
        private User userNotExists;
        private List<User> usersOk;
        private List<User> usersAdded;

        [TestInitialize]
        public void InitTest()
        {
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
                        RoleId = RoleCode.Administrador,
                        Role = new Role()
                        {
                            RoleId = RoleCode.Administrador,
                            Name = "Administrador"
                        }
                    }
                }
            };

            userNotExists = new User()
            {
                UserId = 2,
                Name = "Test2",
                Surname = "User2",
                Email = "test2@user.com",
                Password = "testuser1233",
                Roles = new List<UserRole>() {
                    new UserRole()
                    {
                        RoleId = RoleCode.Vendedor,
                        Role = new Role()
                        {
                            RoleId = RoleCode.Vendedor,
                            Name = "Vendedor"
                        }
                    }
                }
            };

            usersOk = new List<User>
            {
                userOk
            };

            usersAdded = new List<User>
            {
                userOk,
                userNotExists
            };

            CreateDataBase();
        }

        private void CreateDataBase()
        {
            context = CreateDbContext();
            context.Set<User>().Add(userOk);

            foreach (UserRole role in userOk.Roles)
            {
                context.Set<Role>().Add(role.Role);
            }

            foreach (UserRole role in userNotExists.Roles)
            {
                context.Set<Role>().Add(role.Role);
            }

            context.SaveChanges();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            management = new UsersManagement(context);
        }

        [TestMethod]
        public void GetTest()
        {
            var result = management.GetUsers().ToList();
            Assert.IsTrue(usersOk.SequenceEqual(result));
        }

        [TestMethod]
        public void GetWithFilterThatExistsTest()
        {
            Func<User, bool> filter = new Func<User, bool>(x => x.Name == userOk.Name);
            var result = management.GetUsers(filter).ToList();
            Assert.IsTrue(usersOk.SequenceEqual(result));
        }

        [TestMethod]
        public void GetWithFilterThatNotExistsTest()
        {
            Func<User, bool> filter = new Func<User, bool>(x => x.Name == userNotExists.Name);
            int size = management.GetUsers(filter).ToList().Count;
            Assert.AreEqual(0, size);
        }

        [TestMethod]
        public void GetWithFilterByIdTest()
        {
            Func<User, bool> filter = new Func<User, bool>(x => x.UserId == userOk.UserId);
            var result = management.GetUsers(filter).ToList();
            Assert.IsTrue(usersOk.SequenceEqual(result));
        }

        [TestMethod]
        public void GetById()
        {
            User user = management.GetUserById(userOk.UserId);
            Assert.AreEqual(userOk, user);
        }

        [TestMethod]
        public void InsertTest()
        {
            management.InsertUser(userNotExists);
            management.Save();
            var result = management.GetUsers().ToList();
            Assert.IsTrue(usersAdded.SequenceEqual(result));
        }

        [TestMethod]
        public void UpdateTest()
        {
            userOk.Name = "AnotherName";
            management.UpdateUser(userOk);
            management.Save();
            string newName = management.GetUsers().Where(g => g.UserId == userOk.UserId).First().Name;
            Assert.AreEqual("AnotherName", newName);
        }

        [TestMethod]
        public void UpdateHeaderTest()
        {
            userOk.Name = "AnotherName";
            management.UpdateUserHeader(userOk);
            management.Save();
            string newName = management.GetUsers().Where(g => g.UserId == userOk.UserId).First().Name;
            Assert.AreEqual("AnotherName", newName);
        }

        [TestMethod]
        public void DeleteTest()
        {
            management.DeleteUser(userOk);
            management.Save();
            int size = management.GetUsers().ToList().Count;
            Assert.AreEqual(0, size);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.context.Database.EnsureDeleted();
        }
    }
}
