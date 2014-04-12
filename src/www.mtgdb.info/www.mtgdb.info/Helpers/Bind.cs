using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using MtgDbAdminDriver;

namespace MtgDb.Info
{
    public static class Bind
    {
        public static MtgDbAdminDriver.Ruling[] Rulings(Request request)
        {
            var dictionary =    request.Form as IDictionary<string, object>;
            string releasedAt = "{0}.ReleasedAt";
            string rule =       "{0}.Rule";

            string[] keys = dictionary.Select(x => x.Key)
                .Where(x => x.StartsWith("Ruling"))
                .ToArray();

            keys = keys.Select(x => x.Substring(0,x.IndexOf('.'))).Distinct().ToArray();

            List<MtgDbAdminDriver.Ruling> rulings = new List<MtgDbAdminDriver.Ruling>();

            if(keys != null && keys.Length > 0)
            {
                keys = keys.OrderBy(x => x).ToArray();

                foreach(string key in keys)
                {
                    rulings.Add(new MtgDbAdminDriver.Ruling()
                    {
                        ReleasedAt =  dictionary[string.Format(releasedAt, key)].ToString(),
                        Rule = dictionary[string.Format(rule, key)].ToString()
                    });
                }
            }

            return rulings.ToArray();
        }

        public static MtgDbAdminDriver.Format[] Formats(Request request)
        {
            var dictionary = request.Form as IDictionary<string, object>;
            string name = "{0}.Name";
            string legality = "{0}.Legality";

            string[] keys = dictionary.Select(x => x.Key)
                .Where(x => x.StartsWith("Format"))
                .ToArray();

            keys = keys.Select(x => x.Substring(0,x.IndexOf('.'))).Distinct().ToArray();

            List<MtgDbAdminDriver.Format> formats = 
                new List<MtgDbAdminDriver.Format>();

            if(keys != null && keys.Length > 0)
            {
                keys = keys.OrderBy(x => x).ToArray();

                foreach(string key in keys)
                {
                    formats.Add(new MtgDbAdminDriver.Format(){
                        Name = dictionary[string.Format(name, key)].ToString(),
                        Legality = dictionary[string.Format(legality, key)].ToString()
                    });
                }
            }

            return formats.ToArray();
        }
    }
}

