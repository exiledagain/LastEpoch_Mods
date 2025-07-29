using HarmonyLib;
using Il2Cpp;
using Il2CppLE.Services.Visuals;
using Il2CppLE.Services.Visuals.Items;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Cosmetics
{
    [RegisterTypeInIl2Cpp]
    public class Items_Visuals : MonoBehaviour
    {
        public static Items_Visuals instance { get; private set; }
        public Items_Visuals(System.IntPtr ptr) : base(ptr) { }

        //Used to make a list of skin
        /*[HarmonyPatch(typeof(ItemVisualDatabase), "BuildLookup")]
        public class ItemVisualDatabase_BuildLookup
        {
            [HarmonyPostfix]
            static void Postfix(ref Il2CppLE.Services.Visuals.Items.ItemVisualDatabase __instance, Il2CppSystem.Collections.Generic.Dictionary<Il2CppLE.Services.Models.Items.ItemVisualKey, Il2CppLE.Services.Visuals.VisualDatabase<Il2CppLE.Services.Models.Items.ItemVisualKey, Il2CppLE.Services.Visuals.Items.ItemVisual>.DatabaseEntry> __result)
            {
                Main.logger_instance?.Msg("ItemVisualDatabase:BuildLookup");
                Main.logger_instance?.Msg("__result, count = " + __result.Count);

                UniqueList.getUnique(0); //force initialize uniques

                int i = 0;
                foreach (Il2CppSystem.Collections.Generic.KeyValuePair<Il2CppLE.Services.Models.Items.ItemVisualKey, Il2CppLE.Services.Visuals.VisualDatabase<Il2CppLE.Services.Models.Items.ItemVisualKey, ItemVisual>.DatabaseEntry> entrie in __result)
                {
                    Il2CppLE.Services.Models.Items.ItemVisualKey t_key = entrie.Key;
                    ItemList.ClassRequirement ClassReq = t_key.ClassRequirement;
                    EquipmentType type = t_key.EquipmentType;
                    bool IsUnique = t_key.IsUnique;
                    int subtype = t_key.SubType;
                    ushort unique_id = t_key.UniqueID;
                    ushort variation = t_key.Variation;

                    Il2CppLE.Services.Visuals.VisualDatabase<Il2CppLE.Services.Models.Items.ItemVisualKey, ItemVisual>.DatabaseEntry t_value = entrie.Value;
                    ItemList.ClassRequirement ClassReq2 = t_value.Key.ClassRequirement;
                    EquipmentType type2 = t_value.Key.EquipmentType;
                    bool IsUnique2 = t_value.Key.IsUnique;
                    int subtype2 = t_value.Key.SubType;
                    ushort unique_id2 = t_value.Key.UniqueID;
                    ushort variation2 = t_value.Key.Variation;

                    string classe = "Unknow";
                    if (variation2 == 8) { classe = "Acolyte"; }
                    else if (variation2 == 2) { classe = "Mage"; }
                    else if (variation2 == 1) { classe = "Primalist"; }
                    else if (variation2 == 16) { classe = "Rogue"; }
                    else if (variation2 == 4) { classe = "Sentinel"; }

                    if (IsUnique)
                    {
                        foreach (UniqueList.Entry unique in Refs_Manager.unique_list.uniques)
                        {
                            if (unique.uniqueID == unique_id)
                            {
                                Main.logger_instance?.Msg("Index = " + i + ", ItemName = " + unique.name + ", Classe = " + classe + ", Unique = true");
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (ItemList.BaseEquipmentItem base_item in Refs_Manager.item_list.EquippableItems)
                        {
                            if (base_item.type == type)
                            {
                                foreach (ItemList.EquipmentItem item in base_item.subItems)
                                {
                                    if (item.subTypeID == subtype)
                                    {
                                        Main.logger_instance?.Msg("Index = " + i + ", ItemName = " + item.name + ", Classe = " + classe + ", Unique = false");
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    i++;
                }
            }
        }*/

        //Used to replace a skin by an other
        /*[HarmonyPatch(typeof(ItemVisualDatabase), "BuildLookup")]
        public class ItemVisualDatabase_BuildLookup2
        {
            [HarmonyPostfix]
            static void Postfix(ref Il2CppLE.Services.Visuals.Items.ItemVisualDatabase __instance, Il2CppSystem.Collections.Generic.Dictionary<Il2CppLE.Services.Models.Items.ItemVisualKey, Il2CppLE.Services.Visuals.VisualDatabase<Il2CppLE.Services.Models.Items.ItemVisualKey, Il2CppLE.Services.Visuals.Items.ItemVisual>.DatabaseEntry> __result)
            {
                int wanted_skin_index = 100;
                int replace_skin_index = 200;

                Il2CppLE.Services.Visuals.Items.ItemVisual wanted_visual = new Il2CppLE.Services.Visuals.Items.ItemVisual();                
                bool found = false;
                int i = 0;
                foreach (Il2CppSystem.Collections.Generic.KeyValuePair<Il2CppLE.Services.Models.Items.ItemVisualKey, Il2CppLE.Services.Visuals.VisualDatabase<Il2CppLE.Services.Models.Items.ItemVisualKey, ItemVisual>.DatabaseEntry> entrie in __result)
                {
                    if (i == wanted_skin_index)
                    {
                        wanted_visual = entrie.Value.Visual;
                        found = true;
                        break;
                    }
                    i++;
                }

                if (found)
                {
                    Il2CppSystem.Collections.Generic.Dictionary<Il2CppLE.Services.Models.Items.ItemVisualKey, VisualDatabase<Il2CppLE.Services.Models.Items.ItemVisualKey, ItemVisual>.DatabaseEntry> new_dictionarry = new Il2CppSystem.Collections.Generic.Dictionary<Il2CppLE.Services.Models.Items.ItemVisualKey, VisualDatabase<Il2CppLE.Services.Models.Items.ItemVisualKey, ItemVisual>.DatabaseEntry>();
                    i = 0;
                    foreach (Il2CppSystem.Collections.Generic.KeyValuePair<Il2CppLE.Services.Models.Items.ItemVisualKey, Il2CppLE.Services.Visuals.VisualDatabase<Il2CppLE.Services.Models.Items.ItemVisualKey, Il2CppLE.Services.Visuals.Items.ItemVisual>.DatabaseEntry> entrie in __result)
                    {
                        if (i == replace_skin_index)
                        {
                            VisualDatabase<Il2CppLE.Services.Models.Items.ItemVisualKey, ItemVisual>.DatabaseEntry new_entrie = new VisualDatabase<Il2CppLE.Services.Models.Items.ItemVisualKey, Il2CppLE.Services.Visuals.Items.ItemVisual>.DatabaseEntry();
                            new_entrie.Key = entrie.Value.Key;
                            new_entrie.Visual = wanted_visual;
                            new_dictionarry.Add(entrie.Key, new_entrie);
                        }
                        else { new_dictionarry.Add(entrie.Key, entrie.Value); }
                        i++;
                    }

                    __result = new_dictionarry;
                }
            }
        }*/
    }
}