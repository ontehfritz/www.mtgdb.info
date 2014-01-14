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
            Planeswalker planeswalker = new Planeswalker ();
            planeswalker.Id = Guid.NewGuid ();
            planeswalker.Email = "test@test.com";
            planeswalker.Name = "testwalker";
        
            planeswalker = repository.AddPlaneswalker (planeswalker);
            Assert.AreEqual ("testwalker", planeswalker.Name);
            //Clean up
            repository.RemovePlaneswalker (planeswalker.Id);
        }

        [Test ()]
        public void Remove_Planeswalker ()
        {
            Planeswalker planeswalker = new Planeswalker ();
            planeswalker.Id = Guid.NewGuid ();
            planeswalker.Email = "test@test.com";
            planeswalker.Name = "testwalker";

            planeswalker = repository.AddPlaneswalker (planeswalker);
            Assert.AreEqual ("testwalker", planeswalker.Name);
            //Clean up
            repository.RemovePlaneswalker (planeswalker.Id);
        }

        [Test ()]
        public void Get_Planeswalker ()
        {
            Planeswalker planeswalker = new Planeswalker ();
            planeswalker.Id = Guid.NewGuid ();
            planeswalker.Email = "test@test.com";
            planeswalker.Name = "testwalker";

            planeswalker = repository.AddPlaneswalker (planeswalker);
            planeswalker = repository.GetPlaneswalker (planeswalker.Id);

            Assert.IsNotNull(planeswalker);
            //Clean up
            repository.RemovePlaneswalker (planeswalker.Id);
        }
    }
}

