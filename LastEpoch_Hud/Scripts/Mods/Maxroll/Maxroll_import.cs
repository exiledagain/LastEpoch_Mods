//Mod From https://github.com/exiledagain

//At this time, can import AllEquipement, Equipement, Idols, Passives and WeaverTree
//Blessing (need to unlock timeline first) Should be fixed soon
//Skills (not writen at this time)

using Il2Cpp;
using MelonLoader;
using Newtonsoft.Json;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Maxroll
{
    [RegisterTypeInIl2Cpp]
    public class Maxroll_import : MonoBehaviour
    {
        public Maxroll_import(System.IntPtr ptr) : base(ptr) { }
        public static Maxroll_import instance { get; private set; }

        private static readonly int blessing_container = 33;

        void Awake()
        {
            instance = this;
        }
        void Update()
        {

        }
        public static void Load()
        {
            string JsonString = GUIUtility.systemCopyBuffer;
            if ((JsonString.Substring(2, 5) == "items") && (JsonString.Contains("idols")) && (JsonString.Contains("blessings")))
            {
                AllEquipments AllEquipmentsData = JsonConvert.DeserializeObject<AllEquipments>(JsonString);
                Item[] items = { AllEquipmentsData.Items.Body, AllEquipmentsData.Items.Feet, AllEquipmentsData.Items.Finger1,
                    AllEquipmentsData.Items.Finger2, AllEquipmentsData.Items.Hands, AllEquipmentsData.Items.Head,
                    AllEquipmentsData.Items.Neck, AllEquipmentsData.Items.Offhand, AllEquipmentsData.Items.Relic,
                    AllEquipmentsData.Items.Waist, AllEquipmentsData.Items.Weapon };
                foreach (Item item in items)
                {
                    if (!item.IsNullOrDestroyed())
                    {
                        if (item.SealedAffix.IsNullOrDestroyed()) { item.SealedAffix = new Affix { Id = -1, Tier = -1, Roll = -1 }; }
                        if (item.PrimordialAffix.IsNullOrDestroyed()) { item.PrimordialAffix = new Affix { Id = -1, Tier = -1, Roll = -1 }; }
                        if (item.UniqueRolls.IsNullOrDestroyed()) { item.UniqueRolls = new System.Collections.Generic.List<int> { -1, -1, -1, -1, -1, -1, -1, -1 }; }
                        DropItem(item.Affixes, item.Implicits, item.ItemType, item.SealedAffix, item.PrimordialAffix, item.SubType, item.UniqueID, item.UniqueRolls);
                    }
                }
                int i = 0;
                foreach (Blessing item in AllEquipmentsData.Blessings)
                {
                    if (!item.IsNullOrDestroyed()) { SetBlessings((ushort)(i + blessing_container), item.Implicits, item.ItemType, item.SubType); }
                    i++;
                }
                foreach (Idol item in AllEquipmentsData.Idols)
                {
                    if (!item.IsNullOrDestroyed())
                    {
                        if (item.UniqueRolls.IsNullOrDestroyed())
                        {
                            item.UniqueRolls = new System.Collections.Generic.List<int> { -1, -1, -1, -1, -1, -1, -1, -1 };
                        }
                        DropIdol(item.Affixes, item.ItemType, item.SubType, item.UniqueID, item.UniqueRolls);
                    }
                }
            }
            else if (JsonString.Substring(2, 5) == "items")
            {
                Equipment EquipmentData = JsonConvert.DeserializeObject<Equipment>(JsonString);
                Item[] items = { EquipmentData.Items.Body, EquipmentData.Items.Feet, EquipmentData.Items.Finger1,
                    EquipmentData.Items.Finger2, EquipmentData.Items.Hands, EquipmentData.Items.Head,
                    EquipmentData.Items.Neck, EquipmentData.Items.Offhand, EquipmentData.Items.Relic,
                    EquipmentData.Items.Waist, EquipmentData.Items.Weapon };
                foreach (Item item in items)
                {
                    if (!item.IsNullOrDestroyed())
                    {
                        if (item.SealedAffix.IsNullOrDestroyed()) { item.SealedAffix = new Affix { Id = -1, Tier = -1, Roll = -1 }; }
                        if (item.PrimordialAffix.IsNullOrDestroyed()) { item.PrimordialAffix = new Affix { Id = -1, Tier = -1, Roll = -1 }; }
                        if (item.UniqueRolls.IsNullOrDestroyed()) { item.UniqueRolls = new System.Collections.Generic.List<int> { -1, -1, -1, -1, -1, -1, -1, -1 }; }
                        DropItem(item.Affixes, item.Implicits, item.ItemType, item.SealedAffix, item.PrimordialAffix, item.SubType, item.UniqueID, item.UniqueRolls);
                    }
                }
            }
            else if (JsonString.Substring(2, 5) == "idols")
            {
                AllIdols IdolsData = JsonConvert.DeserializeObject<AllIdols>(JsonString);
                foreach (Idol item in IdolsData.Idols)
                {
                    if (!item.IsNullOrDestroyed())
                    {
                        if (item.UniqueRolls.IsNullOrDestroyed())
                        {
                            item.UniqueRolls = new System.Collections.Generic.List<int> { -1, -1, -1, -1, -1, -1, -1, -1 };
                        }
                        DropIdol(item.Affixes, item.ItemType, item.SubType, item.UniqueID, item.UniqueRolls);
                    }
                }
            }
            else if (JsonString.Substring(2, 9) == "blessings")
            {
                AllBlessings BlessingsData = JsonConvert.DeserializeObject<AllBlessings>(JsonString);
                int i = 0;
                foreach (Blessing item in BlessingsData.Blessings)
                {
                    if (!item.IsNullOrDestroyed()) { SetBlessings((ushort)(i + blessing_container), item.Implicits, item.ItemType, item.SubType); }
                    i++;
                }
            }
            else if (JsonString.Substring(2, 8) == "passives")
            {
                AllPassives PassivesData = JsonConvert.DeserializeObject<AllPassives>(JsonString);
                if ((!Refs_Manager.player_treedata.IsNullOrDestroyed()) && (!Refs_Manager.player_data.IsNullOrDestroyed()))
                {
                    if (Refs_Manager.player_data.CharacterClass == PassivesData.Class)
                    {
                        Refs_Manager.player_data.ChosenMastery = (byte)PassivesData.Mastery;
                        Refs_Manager.player_data.ClickedUnlockMasteriesButton = true;
                        Refs_Manager.player_treedata.chosenMastery = (byte)PassivesData.Mastery;
                        Refs_Manager.player_treedata.passiveTree.nodes.Clear();
                        foreach (int node_id in PassivesData.Passives.History)
                        {
                            bool found = false;
                            foreach (LocalTreeData.NodeData node_data in Refs_Manager.player_treedata.passiveTree.nodes)
                            {
                                if (node_data.id == node_id)
                                {
                                    node_data.pointsAllocated++;
                                    found = true;
                                    break;
                                }
                            }
                            if (!found) { Refs_Manager.player_treedata.passiveTree.nodes.Add(new LocalTreeData.NodeData((byte)node_id, (byte)1)); }
                        }
                        Refs_Manager.player_treedata.updateMasteryTotals();
                        Refs_Manager.player_treedata.savePassiveTreeData();
                        Refs_Manager.player_data.SaveData();
                    }
                    else { Main.logger_instance.Error("Not the good class"); }
                }
            }
            else if (JsonString.Substring(2, 11) == "weaverItems")
            {
                WeaverTree WeaverTreeData = JsonConvert.DeserializeObject<WeaverTree>(JsonString);
                if (!Refs_Manager.player_treedata.IsNullOrDestroyed())
                {
                    Refs_Manager.player_treedata.weaverTree.nodes.Clear();
                    foreach (int node_id in WeaverTreeData.Weaver.History)
                    {
                        bool found = false;
                        foreach (LocalTreeData.NodeData node_data in Refs_Manager.player_treedata.weaverTree.nodes)
                        {
                            if (node_data.id == node_id)
                            {
                                node_data.pointsAllocated++;
                                found = true;
                                break;
                            }
                        }
                        if (!found) { Refs_Manager.player_treedata.weaverTree.nodes.Add(new LocalTreeData.NodeData((byte)node_id, (byte)1)); }
                    }
                    Refs_Manager.player_treedata.SaveWeaverTreeData();
                    Refs_Manager.player_data.SaveData();
                }
            }
            else if (JsonString.Substring(2, 10) == "skillTrees")
            {
                Skills.Root SkillsData = JsonConvert.DeserializeObject<Skills.Root>(JsonString);

            }
        }
        public static void DropItem(System.Collections.Generic.List<Affix> affixs, System.Collections.Generic.List<int> implicits,
                int item_type, Affix sealed_affix, Affix primordial_affix, int sub_type, int? unique_id, System.Collections.Generic.List<int> unique_rolls)
        {
            if ((!Refs_Manager.ground_item_manager.IsNullOrDestroyed()) && (!Refs_Manager.player_actor.IsNullOrDestroyed()))
            {
                Il2CppSystem.Collections.Generic.List<ItemAffix> item_affixes = new Il2CppSystem.Collections.Generic.List<ItemAffix>();
                foreach (Affix affix in affixs)
                {
                    ItemAffix new_affix = new ItemAffix { affixId = (ushort)affix.Id, affixTier = (byte)(affix.Tier - 1), affixRoll = (byte)affix.Roll, sealedAffixType = SealedAffixType.None };
                    item_affixes.Add(new_affix);
                }
                bool HasSeal = false;
                if ((sealed_affix.Id > -1) && (sealed_affix.Tier > -1) && (sealed_affix.Roll > -1))
                {
                    ItemAffix new_affix = new ItemAffix { affixId = (ushort)sealed_affix.Id, affixTier = (byte)(sealed_affix.Tier - 1), affixRoll = (byte)sealed_affix.Roll, sealedAffixType = SealedAffixType.Regular };
                    item_affixes.Add(new_affix);
                    HasSeal = true;
                }
                bool HasPrimo = false;
                if ((primordial_affix.Id > -1) && (primordial_affix.Tier > -1) && (primordial_affix.Roll > -1))
                {
                    ItemAffix new_affix = new ItemAffix { affixId = (ushort)primordial_affix.Id, affixTier = (byte)(primordial_affix.Tier - 1), affixRoll = (byte)primordial_affix.Roll, sealedAffixType = SealedAffixType.Primordial };
                    item_affixes.Add(new_affix);
                    HasPrimo = true;
                }
                byte item_rarity = (byte)item_affixes.Count;
                byte lp = 0; //Legendary potencial
                byte ww = 0; //Weaver will
                UniqueList.LegendaryType item_legendary_type = UniqueList.LegendaryType.LegendaryPotential;
                if (unique_id != null)
                {
                    UniqueList.Entry unique_item = UniqueList.getUnique((ushort)unique_id);
                    if (!unique_item.IsNullOrDestroyed())
                    {
                        if ((item_affixes.Count > 0) && (unique_item.isSetItem)) { item_rarity = (byte)(8); }
                        else if (item_affixes.Count > 0) { item_rarity = (byte)(7); }
                        else { item_rarity = (byte)(9); }

                        if (unique_item.legendaryType == UniqueList.LegendaryType.LegendaryPotential)
                        {
                            lp = (byte)Random.RandomRange(0f, 4f);
                        }
                        else
                        {
                            item_legendary_type = UniqueList.LegendaryType.WeaversWill;
                            ww = (byte)Random.RandomRange(0f, 28f);
                        }
                    }
                }
                ItemDataUnpacked item = new ItemDataUnpacked { itemType = (byte)item_type, subType = (ushort)sub_type, rarity = item_rarity, affixes = item_affixes, hasSealedPrimordialAffix = HasPrimo, hasSealedRegularAffix = HasSeal };
                
                int i = 0;
                foreach (int implicit_value in implicits)
                {
                    if (i < item.implicitRolls.Count) { item.implicitRolls[i] = (byte)(implicit_value * 255); }
                    else { break; }
                    i++;
                }
                if (unique_id != null)
                {
                    item.uniqueID = (ushort)unique_id;
                    if (item_legendary_type == UniqueList.LegendaryType.LegendaryPotential) { item.legendaryPotential = lp; }
                    else { item.weaversWill = ww; }
                    i = 0;
                    foreach (int unique_roll_value in unique_rolls)
                    {
                        if ((i < item.uniqueRolls.Count) && (unique_roll_value > -1)) { item.uniqueRolls[i] = (byte)(unique_roll_value * 255); }
                        else { break; }
                        i++;
                    }
                }
                item.RefreshIDAndValues();
                Refs_Manager.ground_item_manager.dropItemForPlayer(Refs_Manager.player_actor, item.TryCast<ItemData>(), Refs_Manager.player_actor.position(), false);
            }
        }
        public static void SetBlessings(ushort container_id, System.Collections.Generic.List<double> implicits, int item_type, int sub_type)
        {
            if ((!Refs_Manager.ground_item_manager.IsNullOrDestroyed()) && (!Refs_Manager.player_actor.IsNullOrDestroyed()))
            {
                ItemDataUnpacked item = new ItemDataUnpacked
                {
                    itemType = (byte)item_type,
                    subType = (ushort)sub_type,
                    rarity = 0
                };
                int i = 0;
                foreach (int implicit_value in implicits)
                {
                    if (i < item.implicitRolls.Count) { item.implicitRolls[i] = (byte)(implicit_value * 255); }
                    else { break; }
                    i++;
                }
                item.RefreshIDAndValues();

                bool found = false;
                foreach (Il2CppLE.Data.ItemLocationPair item_pair in Refs_Manager.player_data_tracker.charData.SavedItems)
                {
                    if (item_pair.ContainerID == container_id)
                    {
                        if (item_pair.Data.Count > 7)
                        {
                            if (item_pair.Data[1] == 34)
                            {
                                item_pair.Data[2] = (byte)sub_type;
                                item_pair.Data[5] = item.implicitRolls[0];
                                item_pair.Data[6] = item.implicitRolls[1];
                                item_pair.Data[7] = item.implicitRolls[2];
                                found = true;
                                break;
                            }                            
                        }
                        break;
                    }
                }
                if (!found)
                {
                    byte format_version = 2; //patch

                    Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<byte> Data = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<byte>(11);
                    Data[0] = format_version;
                    Data[1] = item.itemType;
                    Data[2] = (byte)item.subType;
                    Data[3] = 0;
                    Data[4] = 0;
                    Data[5] = item.implicitRolls[0];
                    Data[6] = item.implicitRolls[1];
                    Data[7] = item.implicitRolls[2];
                    Data[8] = 0;
                    Data[9] = 0;
                    Data[10] = 0;

                    Il2CppLE.Data.ItemLocationPair new_blessing = new Il2CppLE.Data.ItemLocationPair
                    {
                        ContainerID = container_id,
                        Data = Data,
                        FormatVersion = format_version,
                        InventoryPosition = new Il2CppLE.Data.ItemInventoryPosition(0, 0),
                        Quantity = 1,
                        TabID = 0
                    };

                    Refs_Manager.player_data_tracker.charData.SavedItems.Add(new_blessing);
                }
                Refs_Manager.player_data_tracker.charData.SaveData();
            }
        }
        public static void DropIdol(System.Collections.Generic.List<Affix> affixs, int item_type, int sub_type, int? unique_id, System.Collections.Generic.List<int> unique_rolls)
        {
            if ((!Refs_Manager.ground_item_manager.IsNullOrDestroyed()) && (!Refs_Manager.player_actor.IsNullOrDestroyed()))
            {
                Il2CppSystem.Collections.Generic.List<ItemAffix> item_affixes = new Il2CppSystem.Collections.Generic.List<ItemAffix>();
                foreach (Affix affix in affixs)
                {
                    ItemAffix new_affix = new ItemAffix { affixId = (ushort)affix.Id, affixTier = (byte)(affix.Tier - 1), affixRoll = (byte)affix.Roll, sealedAffixType = SealedAffixType.None };
                    item_affixes.Add(new_affix);
                }
                byte item_rarity = (byte)item_affixes.Count;
                byte lp = 0; //Legendary potencial
                byte ww = 0; //Weaver will
                UniqueList.LegendaryType item_legendary_type = UniqueList.LegendaryType.LegendaryPotential;
                if (unique_id != null)
                {
                    UniqueList.Entry unique_item = UniqueList.getUnique((ushort)unique_id);
                    if (!unique_item.IsNullOrDestroyed())
                    {
                        if ((item_affixes.Count > 0) && (unique_item.isSetItem)) { item_rarity = (byte)(8); }
                        else if (item_affixes.Count > 0) { item_rarity = (byte)(7); }
                        else { item_rarity = (byte)(9); }

                        if (unique_item.legendaryType == UniqueList.LegendaryType.LegendaryPotential)
                        {
                            lp = (byte)Random.RandomRange(0f, 4f);
                        }
                        else
                        {
                            item_legendary_type = UniqueList.LegendaryType.WeaversWill;
                            ww = (byte)Random.RandomRange(0f, 28f);
                        }
                    }
                }

                ItemDataUnpacked item = new ItemDataUnpacked { itemType = (byte)item_type, subType = (ushort)sub_type, affixes = item_affixes, rarity = item_rarity };

                if (unique_id != null)
                {
                    item.uniqueID = (ushort)unique_id;
                    if (item_legendary_type == UniqueList.LegendaryType.LegendaryPotential) { item.legendaryPotential = lp; }
                    else { item.weaversWill = ww; }
                }

                int i = 0;
                foreach (int unique_roll_value in unique_rolls)
                {
                    if ((i < item.uniqueRolls.Count) && (unique_roll_value > -1)) { item.uniqueRolls[i] = (byte)(unique_roll_value * 255); }
                    else { break; }
                    i++;
                }

                item.RefreshIDAndValues();
                Refs_Manager.ground_item_manager.dropItemForPlayer(Refs_Manager.player_actor, item.TryCast<ItemData>(), Refs_Manager.player_actor.position(), false);
            }
        }
        
        public class AllEquipments
        {
            [JsonProperty("items")]
            public Items Items;

            [JsonProperty("idols")]
            public System.Collections.Generic.List<Idol> Idols;

            [JsonProperty("blessings")]
            public System.Collections.Generic.List<Blessing> Blessings;
        }
        public class Equipment
        {
            [JsonProperty("items")]
            public Items Items;
        }
        public class AllIdols
        {
            [JsonProperty("idols")]
            public System.Collections.Generic.List<Idol> Idols;
        }
        public class AllBlessings
        {
            [JsonProperty("blessings")]
            public System.Collections.Generic.List<Blessing> Blessings;
        }
        public class AllPassives
        {
            [JsonProperty("passives")]
            public NodePoint Passives;

            [JsonProperty("class")]
            public int Class;

            [JsonProperty("mastery")]
            public int Mastery;
        }
        public class WeaverTree
        {
            [JsonProperty("weaverItems")] //Not needed
            public System.Collections.Generic.List<object> WeaverItems;

            [JsonProperty("weaver")]
            public NodePoint Weaver;
        }
        public class Skills
        {
            public class Root
            {
                [JsonProperty("skillTrees")]
                public SkillTrees SkillTrees;
            }
            public class SkillTrees
            {
                [JsonProperty("skillName")]
                public NodePoint Skill;
            }
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
            public int? UniqueID;

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
        public class NodePoint
        {
            [JsonProperty("history")]
            public System.Collections.Generic.List<int> History;

            [JsonProperty("position")]
            public int Position;
        }
    }
}
