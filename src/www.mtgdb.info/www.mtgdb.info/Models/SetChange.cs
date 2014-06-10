using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Reflection;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class SetChange : PageModel
    {
        public static string DateFormat = "yyyy-MM-dd";

        [BsonId]
        public Guid Id                      { get; set; }
        [BsonElement]
        public Guid UserId                  { get; set; }
        [BsonElement]
        public int Version                  { get; set; } //0 - is the original 
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedAt          { get; set; }
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt           { get; set; }
        [BsonElement]
        public string Comment               { get; set; }
        [BsonElement]
        public string[] FieldsUpdated       { get; set; }
        [BsonElement]
        public string[] ChangesCommitted    { get; set; }
        [BsonElement]
        public string[] ChangesOverwritten  { get; set; }
        //Approved
        //Pending
        //Closed
        [BsonElement]
        public string Status                { get; set; }

        [BsonIgnore]
        public Profile User                 { get; set; }

        [BsonElement]
        public string SetId                 { get; set; }
        [BsonElement]
        public string Name                  { get; set; }
        [BsonElement]
        public string Block                 { get; set; }
        [BsonElement]
        public string Type                  { get; set; }
        [BsonElement]
        public string Description           { get; set; }
        [BsonElement]
        public int Common                   { get; set; }
        [BsonElement]
        public int Uncommon                 { get; set; }
        [BsonElement]
        public int Rare                     { get; set; }
        [BsonElement]
        public int MythicRare               { get; set; }
        [BsonElement]
        public int BasicLand                { get; set; }
        [BsonElement]
        public string ReleasedAt            { get; set; }


        public SetChange() : base(){}


        public static SetChange MapSet(CardSet set)
        {
            SetChange change =          new SetChange ();
            change.SetId =              set.Id;
            change.Name =               set.Name;
            change.Type =               set.Type;
            change.Block =              set.Block;
            change.Description =        set.Description;
            change.ReleasedAt =         set.ReleasedAt.ToString(DateFormat);
            change.Common =             set.Common;
            change.Uncommon =           set.Uncommon;
            change.Rare =               set.Rare;
            change.MythicRare =         set.MythicRare;
           
            return change;
        }


        public bool IsAccepted(string field)
        {
            if(ChangesCommitted == null)
            {
                return false;
            }

            foreach(string change in ChangesCommitted)
            {
                if(field.ToLower() == change.ToLower())
                {
                    return true;
                }
            }

            return false; 
        }

        public bool IsOverwritten(string field)
        {
            if(ChangesOverwritten == null)
            {
                return false;
            }

            foreach(string change in ChangesOverwritten)
            {
                if(field.ToLower() == change.ToLower())
                {
                    return true;
                }
            }

            return false; 
        }

        public bool IsFieldChanged(string name)
        {
            name = name.ToLower();
            foreach(string field in FieldsUpdated)
            {
                if(field.ToLower() == name)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetFieldValue(string field)
        {
            Type type = this.GetType();
            dynamic value = type.GetProperty(field, BindingFlags.IgnoreCase |  BindingFlags.Public |
                BindingFlags.Instance).GetValue(this, null);

            if(value == null)
            {
                return null;
            }
                
            return value.ToString();
        }

        public string FieldState(string field)
        {
            string state = "";

            if(Version != 0 && !this.IsFieldChanged(field))
            {
                state = "nochange";
            }
            else if(Status == "Closed")
            {
                state = "closed";
            }
            else if(this.IsOverwritten(field))
            {
                state = "overwritten";
            }
            else if(this.IsAccepted(field))
            {
                state = "accepted";
            }
            else if(Version != 0 && this.IsFieldChanged(field))
            {
                state = "changed";
            }

            return state;
        }

        public static string[] FieldsChanged(CardSet set, SetChange change)
        {
            List<string> fields = new List<string> ();
           
            if(change.Name != set.Name){ fields.Add("name");}
            change.Description = change.Description ?? "";
            set.Description = set.Description ?? "";

            if(change.Description.Replace("\r",string.Empty) != 
                set.Description.Replace("\r",string.Empty))
            { fields.Add("description");}
//            if(change.Description.Replace("\r",string.Empty) != 
//                card.Description.Replace("\r",string.Empty))
//            { fields.Add("description");}
//            change.Flavor = change.Flavor ?? "";
//            card.Flavor = card.Flavor ?? "";

//            if(change.Flavor.Replace("\r",string.Empty) != 
//                card.Flavor.Replace("\r",string.Empty)){ fields.Add("flavor"); }
           
            if(change.Block != set.Block){ fields.Add("block");}
            if(change.Type != set.Type){ fields.Add("type");}
            if(change.ReleasedAt != set.ReleasedAt.ToString(DateFormat)){ fields.Add("releasedAt");}
            if(change.Common != set.Common){ fields.Add("common");}
            if(change.Uncommon != set.Uncommon){ fields.Add("uncommon");}
            if(change.Rare != set.Rare){ fields.Add("rare");}
            if(change.MythicRare != set.MythicRare){ fields.Add("mythicRare");}
           
            return fields.ToArray();
        }


    }
}

