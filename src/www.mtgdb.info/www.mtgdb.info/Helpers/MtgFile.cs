using System;
using Nancy;
using System.IO;
using MtgDb.Info.Driver;
using System.Linq;
using System.Configuration;

namespace MtgDb.Info
{
    public class MtgFile
    {
        public static Db magicdb = 
            new Db (ConfigurationManager.AppSettings.Get("api"));
        //Format: ignore "//"
        // amount cardname
        // 4 Giant Growth
        //SB: amount cardname
        //SB: 4 Murder 
        public static Deck ImportDec(HttpFile file)
        {
            Deck deck = new Deck();
            Stream stream = file.Value;
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

    }
}

