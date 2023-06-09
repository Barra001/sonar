﻿using ArenaGestor.DataAccess.Managements;
using ArenaGestor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArenaGestor.DataAccessTest
{
    [TestClass]
    public class GendersManagementTest : ManagementTest
    {

        private DbContext context;
        private GendersManagement management;

        private Gender genderOk;
        private Gender genderNotExists;
        private List<Gender> gendersOk;
        private List<Gender> gendersAdded;

        [TestInitialize]
        public void InitTest()
        {
            genderOk = new Gender()
            {
                GenderId = 1,
                Name = "Rock"
            };

            genderNotExists = new Gender()
            {
                GenderId = 2,
                Name = "Bachata"
            };

            gendersOk = new List<Gender>
            {
                genderOk
            };

            gendersAdded = new List<Gender>
            {
                genderOk,
                genderNotExists
            };

            CreateDataBase();
        }

        private void CreateDataBase()
        {
            context = CreateDbContext();
            context.Set<Gender>().Add(genderOk);
            context.SaveChanges();

            management = new GendersManagement(context);
        }

        [TestMethod]
        public void GetTest()
        {
            var result = management.GetGenders().ToList();
            Assert.IsTrue(gendersOk.SequenceEqual(result));
        }

        [TestMethod]
        public void GetWithFilterThatExistsTest()
        {
            Func<Gender, bool> filter = new Func<Gender, bool>(x => x.Name == genderOk.Name);
            var result = management.GetGenders(filter).ToList();
            Assert.IsTrue(gendersOk.SequenceEqual(result));
        }

        [TestMethod]
        public void GetWithFilterThatNotExistsTest()
        {
            Func<Gender, bool> filter = new Func<Gender, bool>(x => x.Name == genderNotExists.Name);
            int size = management.GetGenders(filter).ToList().Count;
            Assert.AreEqual(0, size);
        }

        [TestMethod]
        public void GetWithFilterByIdTest()
        {
            Func<Gender, bool> filter = new Func<Gender, bool>(x => x.GenderId == genderOk.GenderId);
            var result = management.GetGenders(filter).ToList();
            Assert.IsTrue(gendersOk.SequenceEqual(result));
        }

        [TestMethod]
        public void GetById()
        {
            Gender gender = management.GetGenderById(genderOk.GenderId);
            Assert.AreEqual(genderOk, gender);
        }

        [TestMethod]
        public void InsertTest()
        {
            management.InsertGender(genderNotExists);
            management.Save();
            var result = management.GetGenders().ToList();
            Assert.IsTrue(gendersAdded.SequenceEqual(result));
        }

        [TestMethod]
        public void UpdateTest()
        {
            genderOk.Name = "Rockk";
            management.UpdateGender(genderOk);
            management.Save();
            string newName = management.GetGenders().Where(g => g.GenderId == genderOk.GenderId).First().Name;
            Assert.AreEqual("Rockk", newName);
        }

        [TestMethod]
        public void DeleteTest()
        {
            management.DeleteGender(genderOk);
            management.Save();
            int size = management.GetGenders().ToList().Count;
            Assert.AreEqual(0, size);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.context.Database.EnsureDeleted();
        }
    }
}
