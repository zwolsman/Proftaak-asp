using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Diagnostics;
using System.Web.Script.Serialization;
using DatabaseLibrary.Extensions;
using DatabaseLibraryTests;
using DatabaseLibraryTests.Classes;

namespace DatabaseLibrary.Tests
{
    [TestClass]
    public class DatabaseManagerTests
    {

        public const string DATABASE_USER = "applicatie";
        public const string DATABASE_PASSWORD = "wachtwoord";
        public const string DATABASE_SERVER = "192.168.20.23";
        [TestMethod]
        public void InitializeTest()
        {
            //Assert.IsTrue(DatabaseManager.Initialize("dbi292440", "Wachtwoord2", "fhictora01.fhict.local"));
            Assert.IsTrue(DatabaseManager.Initialize(DATABASE_USER, DATABASE_PASSWORD, DATABASE_SERVER));
        }

        [TestMethod]
        public void ToInstanceTest()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id");
            table.Columns.Add("gebruikersnaam");
            table.Columns.Add("email");
            table.Columns.Add("activatiehash");
            table.Columns.Add("geactiveerd");

            table.Rows.Add(1, "admin", "admin@bla.nl", "af1a67e7919c0e9539fac8eb0828f208", 1);

            Account account = DatabaseManager.ToInstance<Account>(table.Rows[0]);

            Assert.AreEqual(account.ID, 1);
            Assert.AreEqual(account.Gebruikersnaam, "admin");
            Assert.AreEqual(account.Email, "admin@bla.nl");
        }

        [TestMethod]
        public void GetItemsTest()
        {
            Assert.IsTrue(DatabaseManager.Initialize(DATABASE_USER, DATABASE_PASSWORD, DATABASE_SERVER));
            foreach (Bijdrage bijdrage in DatabaseManager.GetItems<Bijdrage>())
            {
                Assert.IsNotNull(bijdrage.Account);
            }
        }

        [TestMethod]
        public void MapAsTest()
        {
            Test t = new Test();

            Assert.AreEqual("Test", t.GetType().Name);
            Assert.AreEqual("NewName", t.GetType().RealName());
        }

        [TestMethod]
        public void LoginTest()
        {
            Assert.IsTrue(DatabaseManager.Initialize(DATABASE_USER, DATABASE_PASSWORD, DATABASE_SERVER));

            SearchCriteria searchCriteria = new SearchCriteria()
            {
                {"gebruikersnaam", "admin" },
                {"email", "admin@bla.nl" }
            };

            Account account = DatabaseManager.GetItem<Account>(searchCriteria);

            Assert.IsNotNull(account);
            Assert.AreEqual(account.Gebruikersnaam, "admin");
            Assert.AreEqual(account.Email, "admin@bla.nl");


            Account account2 = DatabaseManager.GetItem<Account>(new SearchCriteria()
            {
                {"ID", 10}
            });
            Assert.IsNull(account2);
        }

        [TestMethod]
        public void InsertDeleteItemTest()
        {
            Assert.IsTrue(DatabaseManager.Initialize(DATABASE_USER, DATABASE_PASSWORD, DATABASE_SERVER));

            Account newAccount = new Account();
            newAccount.Email = "mr-z@live.nl";
            newAccount.Gebruikersnaam = "marvin2";
            newAccount.ActivateHash = "randomstring";
            newAccount.Activated = 0;

            DatabaseManager.InsertItem(newAccount);
            Assert.IsNotNull(newAccount.ID);
            
            Assert.IsTrue(DatabaseManager.DeleteItem(newAccount));

        }

        [TestMethod]
        public void PrimaryAndForgeinKeyTest()
        {
            Assert.IsTrue(DatabaseManager.Initialize(DATABASE_USER, DATABASE_PASSWORD, DATABASE_SERVER));

            foreach (Bericht bericht in DatabaseManager.GetItems<Bericht>())
            {
                Debug.WriteLine("Titel:  " + bericht.Titel);
                Debug.WriteLine("Inhoud: " + bericht.Inhoud);
            }
/*            Bericht bericht = new Bericht();
            bericht.Inhoud = "Test bericht";
            bericht.Titel = "Test tiel";

            Bijdrage bijdrage = new Bijdrage();
            bijdrage.ID = 1;
            bijdrage.Datum = DateTime.Now;
            bijdrage.Soort = "bericht";
            bericht.Bijdrage = bijdrage;

            Debug.WriteLine("Primary key: " + bericht.PrimaryKey());
            Assert.AreEqual("bijdrage_id", bericht.PrimaryKey());

            Assert.AreEqual(1, bericht.PrimaryKeyValue());*/
        }

        [TestMethod]
        public void JsonTest()
        {
            Assert.IsTrue(DatabaseManager.Initialize(DATABASE_USER, DATABASE_PASSWORD, DATABASE_SERVER));
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Categorie> categories = new List<Categorie>(DatabaseManager.GetItems<Categorie>());

            Debug.WriteLine(serializer.Serialize(categories));
            foreach (Categorie categorie in categories)
            {
                Debug.Write(categorie.Naam);
            }
        }
    }
}