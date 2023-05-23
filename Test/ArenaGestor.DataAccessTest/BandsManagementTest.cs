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
    public class BandsManagementTest : ManagementTest
    {

        private DbContext context;
        private BandsManagement management;

        private Band bandOk;
        private Band bandNotExists;
        private List<Band> bandsOk;
        private List<Band> bandsAdded;
        private Gender genderOk;
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

            bandOk = new Band()
            {
                MusicalProtagonistId = 1,
                Name = "Nirvana",
                Artists = new List<ArtistBand>() {
                    new ArtistBand()
                    {
                        ArtistId = 1,
                        Artist = new Artist()
                        {
                            ArtistId = 1,
                            Name = "Dummy artist"
                        },
                        RoleArtist = roleArtistOk
                    }
                },
                Gender = genderOk,
                GenderId = genderOk.GenderId
            };

            bandNotExists = new Band()
            {
                MusicalProtagonistId = 2,
                Name = "Foo Fighters",
                Artists = new List<ArtistBand>() {
                    new ArtistBand()
                    {
                        ArtistId = 2,
                        Artist = new Artist()
                        {
                            ArtistId = 2,
                            Name = "Dummy artist 2"
                        },
                        RoleArtist = roleArtistOk
                    }
                },
                Gender = genderOk,
                GenderId = genderOk.GenderId
            };

            bandsOk = new List<Band>
            {
                bandOk
            };

            bandsAdded = new List<Band>
            {
                bandOk,
                bandNotExists
            };

            CreateDataBase();
        }

        private void CreateDataBase()
        {
            context = CreateDbContext();
            context.Set<Band>().Add(bandOk);
            foreach (ArtistBand artist in bandOk.Artists)
            {
                context.Set<Artist>().Add(artist.Artist);
            }
            foreach (ArtistBand artist in bandNotExists.Artists)
            {
                context.Set<Artist>().Add(artist.Artist);
            }
            context.SaveChanges();

            management = new BandsManagement(context);
        }

        [TestMethod]
        public void GetTest()
        {
            var result = management.GetBands().ToList();
            Assert.IsTrue(bandsOk.SequenceEqual(result));
        }

        [TestMethod]
        public void GetWithFilterThatExistsTest()
        {
            Func<Band, bool> filter = new Func<Band, bool>(x => x.Name == bandOk.Name);
            var result = management.GetBands(filter).ToList();
            Assert.IsTrue(bandsOk.SequenceEqual(result));
        }

        [TestMethod]
        public void GetWithFilterThatNotExistsTest()
        {
            Func<Band, bool> filter = new Func<Band, bool>(x => x.Name == bandNotExists.Name);
            int size = management.GetBands(filter).ToList().Count;
            Assert.AreEqual(0, size);
        }

        [TestMethod]
        public void GetWithFilterByIdTest()
        {
            Func<Band, bool> filter = new Func<Band, bool>(x => x.MusicalProtagonistId == bandOk.MusicalProtagonistId);
            var result = management.GetBands(filter).ToList();
            Assert.IsTrue(bandsOk.SequenceEqual(result));
        }

        [TestMethod]
        public void GetById()
        {
            Band band = management.GetBandById(bandOk.MusicalProtagonistId);
            Assert.AreEqual(bandOk, band);
        }

        [TestMethod]
        public void InsertTest()
        {
            management.InsertBand(bandNotExists);
            management.Save();
            var result = management.GetBands().ToList();
            Assert.IsTrue(bandsAdded.SequenceEqual(result));
        }

        [TestMethod]
        public void UpdateTest()
        {
            bandOk.Name = "Nirvanak";
            management.UpdateBand(bandOk);
            management.Save();
            string newName = management.GetBands().Where(g => g.MusicalProtagonistId == bandOk.MusicalProtagonistId).First().Name;
            Assert.AreEqual("Nirvanak", newName);
        }

        [TestMethod]
        public void DeleteTest()
        {
            management.DeleteBand(bandOk);
            management.Save();
            int size = management.GetBands().ToList().Count;
            Assert.AreEqual(0, size);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.context.Database.EnsureDeleted();
        }
    }
}
