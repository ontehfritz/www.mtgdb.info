using NUnit.Framework;
using System;
using MtgDb.Info;
using SuperSimple.Auth;
using MtgDb.Info.Driver;

namespace Test_MtgDb.Info
{
    [TestFixture ()]
    public class Test
    {
        private SuperSimpleAuth ssa;
        private IRepository repository;
        private Planeswalker mtgdbUser; 
        private Db mtgdb; 

        [SetUp()]
        public void Init()
        {
            ssa =  new SuperSimpleAuth ("testing_mtgdb.info", 
                "ae132e62-570f-4ffb-87cc-b9c087b09dfb");

            mtgdb = new Db();

            repository =  new MongoRepository ("mongodb://localhost", ssa);

            //Super simple auth can't delete from here. 
            try
            {
                repository.AddPlaneswalker ("mtgdb_tester", 
                    "test123", "mtgdb_tester@test.com");
            }
            catch(Exception e)
            {
                System.Console.WriteLine (e.Message);
            }

            User ssaUser = ssa.Authenticate ("mtgdb_tester", 
                "test123", "127.0.0.1");

            mtgdbUser = new Planeswalker 
            {
                UserName = ssaUser.UserName,
                AuthToken = ssaUser.AuthToken,
                Email = ssaUser.Email,
                Id = ssaUser.Id,
                Claims = ssaUser.Claims,
                Roles = ssaUser.Roles,
                Profile = repository.GetProfile (ssaUser.Id)
            };
        }


        [Test()]
        public void Make_card_change()
        {
            CardChange change = new CardChange();

            Card card = mtgdb.GetCard(2);

            change = CardChange.MapCard(card);
            change.Comment = "test";
            change.Description = "test";
            change.UserId = Guid.NewGuid();

            Guid id = repository.AddCardChangeRequest(change);


            change = repository.GetCardChangeRequest(id);

            Assert.AreEqual(id, change.Id );
        }


        [Test()]
        public void Get_card_change()
        {
            CardChange change = new CardChange();

            Card card = mtgdb.GetCard(2);

            change = CardChange.MapCard(card);
            change.Comment = "test";
            change.UserId = Guid.NewGuid();

            Guid id = repository.AddCardChangeRequest(change);

            change = repository.GetCardChangeRequest(id);

            Assert.AreEqual(id, change.Id );
        }


        [Test()]
        public void Get_card_changes()
        {
            CardChange change = new CardChange();

            Card card = mtgdb.GetCard(2);

            change = CardChange.MapCard(card);
            change.Comment = "test";
            //change.UserId = Guid.NewGuid();

            repository.AddCardChangeRequest(change);

            CardChange[] changes = repository.GetCardChangeRequests(2);

            Assert.Greater(changes.Length, 0);
        }

        [Test()]
        public void Add_card_change()
        {
            CardChange change = new CardChange();

            Card card = mtgdb.GetCard(1);

            change = CardChange.MapCard(card);
            change.Comment = "test";
            change.UserId = Guid.NewGuid();

            Guid newId = repository.AddCardChangeRequest(change);

            Assert.AreNotEqual(Guid.Empty,newId);
        }
            
        [Test ()]
        public void Get_UserCards_by_setId ()
        {
            repository.AddUserCard(mtgdbUser.Id, 1, 1);
            repository.AddUserCard(mtgdbUser.Id, 2, 3);

            UserCard [] userCards = repository.GetUserCards(mtgdbUser.Id, "LEA");

            Assert.AreEqual (2, userCards.Length);
        }

        [Test ()]
        public void Get_UserCards ()
        {
            repository.AddUserCard(mtgdbUser.Id, 1, 1);
            repository.AddUserCard(mtgdbUser.Id, 2, 3);

            UserCard [] userCards = repository.GetUserCards(mtgdbUser.Id,new int[]{1, 2});

            Assert.AreEqual (2, userCards.Length);
        }

        [Test ()]
        public void Add_UserCard ()
        {
            UserCard card = repository.AddUserCard(mtgdbUser.Id, 1, 1);
            Assert.AreEqual (1, card.Amount);
        }

        [Test ()]
        public void Add_Planeswalker ()
        {
            string user = Guid.NewGuid ().ToString ();
            Guid id = repository.AddPlaneswalker (user,"test12345", user + "@test.com");
            Assert.IsNotNull (id);

            //Clean up
            repository.RemovePlaneswalker (id);
        }

        [Test ()]
        public void Remove_Planeswalker ()
        {
            string user = Guid.NewGuid ().ToString ();
            Guid id = repository.AddPlaneswalker (user,"test12345", user + "@test.com");
            Assert.IsNotNull (id);
            //Clean up
            repository.RemovePlaneswalker (id);

            Assert.IsNull(repository.GetProfile (id));
        }

        [Test ()]
        public void Get_Profile ()
        {
            Profile profile = repository.GetProfile (mtgdbUser.Id);
            Assert.AreEqual (mtgdbUser.Id, profile.Id);
        }

        [Test ()]
        public void Update_Planeswalker ()
        {
            mtgdbUser.Profile.Email = "change@email.com";
            mtgdbUser = repository.UpdatePlaneswalker(mtgdbUser);
            Assert.AreEqual (mtgdbUser.Email, "change@email.com");

            mtgdbUser.Profile.Email = "mtgdb_tester@test.com";
            mtgdbUser = repository.UpdatePlaneswalker(mtgdbUser);
            Assert.AreEqual (mtgdbUser.Email, "mtgdb_tester@test.com");

        }
    }
}

