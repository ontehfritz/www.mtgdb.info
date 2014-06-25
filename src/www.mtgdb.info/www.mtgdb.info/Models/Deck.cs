using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MtgDb.Info.Driver;
using System.Configuration;
using System.Linq;
using Nancy;
using System.IO;

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

        private Dictionary<string, List<string>> Types;

        [BsonIgnore]
        private Db mtgDb; 

        public Deck ()
        {
            mtgDb =     new Db (ConfigurationManager.AppSettings.Get("api"));
            Cards =     new List<DeckCard>();
            SideBar =   new List<DeckCard>();
            Types = new Dictionary<string, List<string>>();
            Types.Add("creature", new List<string>(){
                "Creature",
                "Artifact Creature",
                "Enchantment Creature",
                "Land Creature",
                "Legendary Artifact Creature",
                "Legendary Creature",
                "Legendary Enchantment Creature",
                "Snow Artifact Creature",
                "Snow Creature",
                "Planeswalker"
            });

            Types.Add("land", new List<string>(){
                "Artifact Land",
                "Basic Land",
                "Basic Snow Land",
                "Land",
                "Legendary Land",
                "Legendary Snow Land",
                "Snow Land"
            });

            Types.Add("instant", new List<string>(){
                "Instant",
                "Interrupt",
                "Tribal Instant"
            });

            Types.Add("sorcery", new List<string>(){
                "Sorcery",
                "Summon",
                "Tribal Sorcery"
            });

            Types.Add("enchantment", new List<string>(){
                "Enchant Creature",
                "Enchant Player",
                "Enchantment",
                "Legendary Enchantment"
            });


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

        public Card[] GetCards(string type)
        {
            if(Cards != null)
            {
                int [] multiverseIds = Cards
                    .Select(x => x.MultiverseId)
                    .ToArray();

                return mtgDb.GetCards(multiverseIds)
                    .Where(x => x.Type.ToLower() == type.ToLower())
                    .ToArray();
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

        public int CardCount(int multiverseId)
        {
            DeckCard card = 
                this.Cards.Find(x => x.MultiverseId == multiverseId);

            if(card != null)
            {
                return card.Amount;
            }

            return 0;
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

