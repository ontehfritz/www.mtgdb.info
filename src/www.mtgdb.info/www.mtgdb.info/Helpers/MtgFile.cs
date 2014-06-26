using System;
using Nancy;
using System.IO;
using MtgDb.Info.Driver;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class MtgFile
    {
        public static Db magicdb = 
            new Db (ConfigurationManager.AppSettings.Get("api"));


        public static string ExportDec(Deck deck)
        {
            StringBuilder dec = new StringBuilder();
            dec.AppendFormat("//Name: {0}\n", deck.Name);
            dec.AppendFormat("//Description: {0}\n", deck.Description);

            //deck
            foreach(var card in deck.GetCards())
            {
                dec.AppendFormat("{0} {1}\n", deck.CardCount(card.Id), 
                    card.Name.Replace("//", "/"));
            }

            //sidebar
            foreach(var card in deck.GetSideBarCards())
            {
                dec.AppendFormat("SB: {0} {1}\n",deck.SideBarCardCount(card.Id), 
                    card.Name.Replace("//", "/"));
            }
          
            return dec.ToString();
        }

        //Format: ignore "//"
        // amount cardname
        // 4 Giant Growth
        //SB: amount cardname
        //SB: 4 Murder 
        public static Deck ImportDec(Stream stream)
        {
            Deck deck = new Deck();
            StreamReader sr = new StreamReader(stream);

            string line = null;
            while ((line = sr.ReadLine()) != null) 
            {
                line = line.Trim();
                if(!line.StartsWith("//"))
                {
                    if(line.StartsWith("SB:"))
                    {
                        line = line.Replace("SB:","").Trim();
                        try
                        {
                            int first = line.IndexOf(' ');
                            string number = line.Substring(0,first);
                            string cardName = line.Substring(first);

                            int amount = int.Parse(number.Trim());
                            Card card = magicdb.GetCards(cardName.Trim())
                                .OrderByDescending(x => x.Id)
                                .FirstOrDefault();

                            deck.SideBar.Add(new DeckCard(){
                                Amount = amount,
                                MultiverseId = card.Id
                            });

                        }
                        catch(Exception exc)
                        {
                            throw exc;
                        }
                    }
                    else
                    {
                        try
                        {
                            int first = line.IndexOf(' ');
                            string number = line.Substring(0,first);
                            string cardName = line.Substring(first);


                            int amount = int.Parse(number.Trim());
                            Card card = magicdb.GetCards(cardName.Trim()).FirstOrDefault();

                            deck.Cards.Add(new DeckCard(){
                                Amount = amount,
                                MultiverseId = card.Id
                            });

                        }
                        catch(Exception exc)
                        {
                            //throw exc;
                        }
                    }
                }
                //Console.WriteLine(line);
            }
                
            return deck;
        }

        //convert coll2(Decked builder collection file to mtgdb.info collection)
        //YAML BS, No need for the overhead and dependencies for yaml! Parse this 
        //out our way. This will assit in the import of decked builder collections 
        //into "My Cards"
        public static Item [] ImportColl2(Stream stream)
        {
            StreamReader sr =   new StreamReader(stream);
            string line =       null;
            Item card =         null;
            List<Item> items =  new List<Item>();

            while ((line = sr.ReadLine()) != null) 
            {
                line = line.Replace("-","").Trim();
                if(line.Contains("r:"))
                {
                    int amount = int.Parse(line.Substring(line.IndexOf(":") + 1).Trim());
                    card.R = amount;
                }
                if(line.Contains("f:"))
                {
                    int amount = int.Parse(line.Substring(line.IndexOf(":") + 1).Trim());
                    card.F = amount;
                }
                else if(line.Contains("id:"))
                {
                    int mvid = int.Parse(line.Substring(line.IndexOf(":") + 1).Trim());
                    card = new Item();
                    card.mvid = mvid;
                    card.R = 0;
                    card.F = 0;
                }

                if(card != null)
                {
                    items.Add(card);
                }
            }
           
            return items.ToArray();
        }

        public class Item 
        {
            public int mvid { get; set; }
            public int R { get; set; }
            public int F { get; set; }
        }
    }
}

