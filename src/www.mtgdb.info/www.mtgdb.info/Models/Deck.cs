using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MtgDb.Info.Driver;
using System.Configuration;
using System.Linq;

namespace MtgDb.Info
{
    public class Deck
    {
        [BsonId]
        public Guid Id                  { get; set; }
        [BsonElement]
        public Guid UserId              { get; set; }
        [BsonElement]
        public string Name              { get; set; }
        [BsonElement]
        public string Description       { get; set; }
        [BsonElement]
        public bool IsPublic            { get; set; }
        [BsonElement]
        public List<DeckCard> Cards     { get; set; }
        [BsonElement]
        public List<DeckCard> SideBar   { get; set; }
        [BsonElement]
        public DateTime CreatedAt       { get; set; }
        [BsonElement]
        public DateTime ModifiedAt      { get; set; }

        [BsonIgnore]
        private Db mtgDb; 

        public Deck ()
        {
            mtgDb =     new Db (ConfigurationManager.AppSettings.Get("api"));
            Cards =     new List<DeckCard>();
            SideBar =   new List<DeckCard>();
        }

        public void SetCards(int [] mvids)
        {
            Dictionary<int, int> deckCard = new Dictionary<int, int>();

            foreach(int mvid in  mvids)
            {
                if(deckCard.ContainsKey(mvid))
                {
                    deckCard[mvid] = deckCard[mvid] + 1;
                }
                else
                {
                    deckCard.Add(mvid,1);
                }
            }


            Cards = deckCard.Select(c => new DeckCard { 
                MultiverseId = c.Key, Amount = c.Value 
            }).ToList();
        }


        public void SetSideBar(int [] mvids)
        {
            Dictionary<int, int> deckCard = new Dictionary<int, int>();

            foreach(int mvid in  mvids)
            {
                if(deckCard.ContainsKey(mvid))
                {
                    deckCard[mvid] = deckCard[mvid] + 1;
                }
                else
                {
                    deckCard.Add(mvid,1);
                }
            }


            SideBar = deckCard.Select(c => new DeckCard { 
                MultiverseId = c.Key, Amount = c.Value 
            }).ToList();
        }

        public Card[] GetCards()
        {
            if(Cards != null)
            {
                int [] multiverseIds = Cards
                    .Select(x => x.MultiverseId)
                    .ToArray();

                return mtgDb.GetCards(multiverseIds).ToArray();
            }

            return null;
             
        }

        public Card[] GetSideBarCards()
        {
            if(Cards != null)
            {
                int [] multiverseIds = SideBar
                    .Select(x => x.MultiverseId)
                    .ToArray();

                return mtgDb.GetCards(multiverseIds).ToArray();
            }

            return null;
        }
    }

    public class DeckCard
    {
        [BsonElement]
        public int MultiverseId { get; set; }
        [BsonElement]
        public int Amount       { get; set; }
    }
}

