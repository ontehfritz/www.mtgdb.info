using NUnit.Framework;
using System;
using MtgDb.Info;
using SuperSimple.Auth;

namespace Test_MtgDb.Info
{
    [TestFixture ()]
    public class Test
    {
        private MtgDb.Info.IRepository repository = 
            new MongoRepository ("mongodb://localhost");

        SuperSimpleAuth ssa = 
            new SuperSimpleAuth ("mtgdb.info", 
                "4e5844a9-6444-415d-b06d-6f29f52fbd0e");

        private Planeswalker mtgdbUser; 

        [SetUp()]
        public void Init()
        {
            //Super simple auth can't delete from here. 
            try
            {
                Guid id = repository.AddPlaneswalker ("mtgdb_tester", "test123", "mtgdb_tester@test.com");
            }
            catch(Exception e)
            {
                System.Console.WriteLine (e.Message);
            }

            User ssaUser = ssa.Authenticate ("mtgdb_tester", "test123", "127.0.0.1");

            mtgdbUser = new Planeswalker 
            {
                UserName = ssaUser.UserName,
                AuthToken = ssaUser.AuthToken,
                Email = ssaUser.Email,
                Id = ssaUser.Id,
                Claims = ssaUser.Claims,
                Roles = ssaUser.Roles,
                Profile = repository.GetProfile(ssaUser.Id)
            };
        }

        [TearDown] 
        public void Dispose()
        { 

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

