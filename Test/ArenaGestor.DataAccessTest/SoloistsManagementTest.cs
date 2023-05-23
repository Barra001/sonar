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
    public class SoloistsManagementTest : ManagementTest
    {

        private DbContext context;
        private SoloistsManagement management;

        private Soloist soloistOk;
        private Soloist soloistNotExists;
        private List<Soloist> soloistsOk;
        private List<Soloist> soloistsAdded;
        private Gender genderOk;
        private Artist artistOk;
        private RoleArtist roleArtistOk;

        [TestInitialize]
        public void InitTest()
        {
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
                Name = "Dummy artist"
            };

            soloistOk = new Soloist()
            {
                MusicalProtagonistId = 1,
                Name = "Kurt Cobain",
                Gender = genderOk,
                GenderId = genderOk.GenderId,
                Artist = artistOk,
                ArtistId = artistOk.ArtistId,
                RoleArtist = roleArtistOk
            };

            soloistNotExists = new Soloist()
            {
                MusicalProtagonistId = 2,
                Name = "Dave Grohl",
                Gender = genderOk,
                GenderId = genderOk.GenderId,
                Artist = artistOk,
                ArtistId = artistOk.ArtistId,
                RoleArtist = roleArtistOk
            };

            soloistsOk = new List<Soloist>
            {
                soloistOk
            };

            soloistsAdded = new List<Soloist>
            {
                soloistOk,
                soloistNotExists
            };

            CreateDataBase();
        }

        private void CreateDataBase()
        {
            context = CreateDbContext();
            context.Set<Soloist>().Add(soloistOk);
            context.SaveChanges();

            management = new SoloistsManagement(context);
        }

        [TestMethod]
        public void GetTest()
        {
            var result = management.GetSoloists().ToList();
            Assert.IsTrue(soloistsOk.SequenceEqual(result));
        }

        [TestMethod]
        public void GetWithFilterThatExistsTest()
        {
            Func<Soloist, bool> filter = new Func<Soloist, bool>(x => x.Name == soloistOk.Name);
            var result = management.GetSoloists(filter).ToList();
            Assert.IsTrue(soloistsOk.SequenceEqual(result));
        }

        [TestMethod]
        public void GetWithFilterThatNotExistsTest()
        {
            Func<Soloist, bool> filter = new Func<Soloist, bool>(x => x.Name == soloistNotExists.Name);
            int size = management.GetSoloists(filter).ToList().Count;
            Assert.AreEqual(0, size);
        }

        [TestMethod]
        public void GetWithFilterByIdTest()
        {
            Func<Soloist, bool> filter = new Func<Soloist, bool>(x => x.MusicalProtagonistId == soloistOk.MusicalProtagonistId);
            var result = management.GetSoloists(filter).ToList();
            Assert.IsTrue(soloistsOk.SequenceEqual(result));
        }

        [TestMethod]
        public void GetById()
        {
            Soloist soloist = management.GetSoloistById(soloistOk.MusicalProtagonistId);
            Assert.AreEqual(soloistOk, soloist);
        }

        [TestMethod]
        public void InsertTest()
        {
            management.InsertSoloist(soloistNotExists);
            management.Save();
            var result = management.GetSoloists().ToList();
            Assert.IsTrue(soloistsAdded.SequenceEqual(result));
        }

        [TestMethod]
        public void UpdateTest()
        {
            soloistOk.Name = "Kurt Cobaink";
            management.UpdateSoloist(soloistOk);
            management.Save();
            string newName = management.GetSoloists().Where(g => g.MusicalProtagonistId == soloistOk.MusicalProtagonistId).First().Name;
            Assert.AreEqual("Kurt Cobaink", newName);
        }

        [TestMethod]
        public void DeleteTest()
        {
            management.DeleteSoloist(soloistOk);
            management.Save();
            int size = management.GetSoloists().ToList().Count;
            Assert.AreEqual(0, size);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.context.Database.EnsureDeleted();
        }
    }
}
