using NUnit.Framework;
using System;
using MtgDb.Info;

namespace Test_MtgDb.Info
{
    [TestFixture ()]
    public class Test
    {
        private MtgDb.Info.IRepository repository = 
            new MongoRepository ("mongodb://localhost");

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
        public void Get_Planeswalker ()
        {
            string user = Guid.NewGuid ().ToString ();
            Guid id = repository.AddPlaneswalker (user,"test12345", user + "@test.com");
            Assert.IsNotNull (id);
            Assert.IsNotNull(repository.GetProfile (id));
            repository.RemovePlaneswalker (id);
        }

        [Test ()]
        public void Update_Planeswalker ()
        {
            string user = Guid.NewGuid ().ToString ();
            Guid id = repository.AddPlaneswalker (user,"test12345", user + "@test.com");
            Assert.IsNotNull (id);
            Assert.IsNotNull(repository.GetProfile (id));
            repository.RemovePlaneswalker (id);
        }
    }
}

