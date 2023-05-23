using ArenaGestor.DataAccess.Managements;
using ArenaGestor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ArenaGestor.DataAccessTest
{
    [TestClass]
    public class SnacksManagementTest : ManagementTest
    {
        private DbContext context;
        private SnackManagement management;
        private Snack okSnack;
        private Snack nonExistSnack;
        private List<Snack> okSnacks;

        [TestInitialize]
        public void InitTest()
        {
            okSnack = new Snack()
            {
                Id = 1,
                Description = "Papitas",
                Price = 100
            };
            okSnacks = new List<Snack>
            {
                okSnack
            };
            nonExistSnack = new Snack()
            {
                Id = 2,
                Description = "Agua",
                Price = 50
            };
            CreateDataBase();
        }
        private void CreateDataBase()
        {
            context = CreateDbContext();
            context.Set<Snack>().Add(okSnack);
            context.SaveChanges();

            management = new SnackManagement(context);
        }
        [TestMethod]
        public void GetTest()
        {
            var result = management.GetSnacks().ToList();
            Assert.IsTrue(okSnacks.SequenceEqual(result));
        }
        [TestMethod]
        public void InsertTest()
        {
            management.InsertSnack(nonExistSnack);
            var result = management.GetSnacks();
            okSnacks.Add(nonExistSnack);
            Assert.IsTrue(okSnacks.SequenceEqual(result));
        }
        [TestMethod]
        public void DeleteTest()
        {
            management.DeleteSnack(okSnack);
            var result = management.GetSnacks();
            okSnacks.Remove(okSnack);
            Assert.IsTrue(okSnacks.SequenceEqual(result));
        }
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void DeleteNonExistingTest()
        {
            management.DeleteSnack(nonExistSnack);
        }
        [TestCleanup]
        public void TestCleanup()
        {
            this.context.Database.EnsureDeleted();
        }
    }
}
