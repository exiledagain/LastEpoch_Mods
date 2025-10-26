//Mod From https://github.com/exiledagain

using Newtonsoft.Json;

namespace LastEpoch_Hud.Scripts.Mods.Maxroll
{
    public class Maxroll_import
    {
        public class Equipments
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            public class Root
            {
                [JsonProperty("items")]
                public Items Items;

                [JsonProperty("idols")]
                public System.Collections.Generic.List<Idol> Idols;

                [JsonProperty("blessings")]
                public System.Collections.Generic.List<Blessing> Blessings;
            }
            public class Items
            {
                [JsonProperty("body")]
                public Item Body;

                [JsonProperty("offhand")]
                public Item Offhand;

                [JsonProperty("waist")]
                public Item Waist;

                [JsonProperty("feet")]
                public Item Feet;

                [JsonProperty("finger1")]
                public Item Finger1;

                [JsonProperty("finger2")]
                public Item Finger2;

                [JsonProperty("neck")]
                public Item Neck;

                [JsonProperty("relic")]
                public Item Relic;

                [JsonProperty("hands")]
                public Item Hands;

                [JsonProperty("head")]
                public Item Head;

                [JsonProperty("weapon")]
                public Item Weapon;
            }
            public class Item
            {
                [JsonProperty("itemType")]
                public int ItemType;

                [JsonProperty("subType")]
                public int SubType;

                [JsonProperty("uniqueID")]
                public int UniqueID;

                [JsonProperty("uniqueRolls")]
                public System.Collections.Generic.List<int> UniqueRolls;

                [JsonProperty("affixes")]
                public System.Collections.Generic.List<Affix> Affixes;

                [JsonProperty("sealedAffix")]
                public Affix SealedAffix;

                [JsonProperty("primordialAffix")]
                public Affix PrimordialAffix;

                [JsonProperty("implicits")]
                public System.Collections.Generic.List<int> Implicits;
            }
            public class Idol
            {
                [JsonProperty("itemType")]
                public int ItemType;

                [JsonProperty("subType")]
                public int SubType;

                [JsonProperty("affixes")]
                public System.Collections.Generic.List<Affix> Affixes;

                [JsonProperty("uniqueID")]
                public int? UniqueID;

                [JsonProperty("uniqueRolls")]
                public System.Collections.Generic.List<int> UniqueRolls;
            }            
            public class Blessing
            {
                [JsonProperty("itemType")]
                public int ItemType;

                [JsonProperty("subType")]
                public int SubType;

                [JsonProperty("implicits")]
                public System.Collections.Generic.List<double> Implicits;
            }
            public class Affix
            {
                [JsonProperty("id")]
                public int Id;

                [JsonProperty("tier")]
                public int Tier;

                [JsonProperty("roll")]
                public int Roll;
            }
        }
        public class Passives
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            public class Root
            {
                [JsonProperty("passives")]
                public Passive Passives;

                [JsonProperty("class")]
                public int Class;

                [JsonProperty("mastery")]
                public int Mastery;
            }
            public class Passive
            {
                [JsonProperty("history")]
                public System.Collections.Generic.List<int> History;

                [JsonProperty("position")]
                public int Position;
            }
        }
        public class WeaverTree
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            public class Root
            {
                [JsonProperty("weaver")]
                public Weaver Weaver;
            }

            public class Weaver
            {
                [JsonProperty("history")]
                public System.Collections.Generic.List<int> History;

                [JsonProperty("position")]
                public int Position;
            }
        }
    }
}
