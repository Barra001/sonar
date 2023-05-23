using ArenaGestor.DataAccess.Managements;
using ArenaGestor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ArenaGestor.DataAccessTest
{

    [TestClass]
    public class CountrysManagementTest : ManagementTest
    {
        private DbContext context;
        private CountrysManagement management;

        private Country countryOk;
        private Country countryNull;
        private int countryIdInexistant;

        [TestInitialize]
        public void InitTest()
        {
            countryOk = new Country()
            {
                CountryId = 1,
                Name = "Uruguay"
            };

            countryNull = null;
            countryIdInexistant = 2;

            CreateDataBase();
        }

        private void CreateDataBase()
        {
            context = CreateDbContext();
            context.Set<Country>().Add(countryOk);
            context.SaveChanges();

            management = new CountrysManagement(context);
        }

        [TestMethod]
        public void GetById()
        {
            Country country = management.GetCountryById(countryOk.CountryId);
            Assert.AreEqual(countryOk, country);
        }

        [TestMethod]
        public void GetByCountryIdNotExists()
        {
            Country country = management.GetCountryById(countryIdInexistant);
            Assert.AreEqual(countryNull, country);
        }

        [TestMethod]
        public void GetCountrys()
        {
            var result = management.GetCountrys().ToList();
            Assert.IsTrue(result.Count == 1);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.context.Database.EnsureDeleted();
        }
    }

}
