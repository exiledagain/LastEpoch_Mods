using HarmonyLib;
using Il2Cpp;
using Il2CppLE.Data;

namespace LastEpoch_Hud.Scripts.Mods.Fixs
{
    public class Fix_LocalData
    {
        //Use this file to repair your save on load

        /*[HarmonyPatch(typeof(LocalCharacterSlots), "LoadCharacterByData")]
        public class LocalCharacterSlots_LoadCharacterByData
        {
            [HarmonyPostfix]
            static void Postfix(LocalCharacterSlots __instance, Il2CppCysharp.Threading.Tasks.UniTask __result, ref Il2CppLE.Data.CharacterData __0)
            {
                Main.logger_instance.Msg("LocalCharacterSlots.LoadCharacterByData();" + __0.CharacterName);
                //0 'characterData

                //Fix Save Items
                Il2CppSystem.Collections.Generic.List<ItemLocationPair> list = new Il2CppSystem.Collections.Generic.List<ItemLocationPair>();
                int forge_items_count = 0;
                int index = 0; //debug log
                foreach (ItemLocationPair item_location_pair in __0.SavedItems)
                {
                    if (item_location_pair.ContainerID == 23) //forge slot
                    {
                        //debug show data
                        Main.logger_instance.Msg("index = " + index);
                        Main.logger_instance.Msg("Data");
                        int i = 0;
                        foreach (byte data in item_location_pair.Data)
                        {
                            Main.logger_instance.Msg(i + " : " + data);
                            i++;
                        }

                        if (forge_items_count == 0)
                        {
                            item_location_pair.Quantity = 1; //fix quantity
                            //fix data here
                            list.Add(item_location_pair);
                        }
                        forge_items_count++;
                    }
                    else if (item_location_pair.ContainerID == 32) //Don't copy Vendor Items
                    {
                        list.Add(item_location_pair);
                    }
                    index++;
                }

                __0.SavedItems = list;                
            }
        }*/
                
        /*[HarmonyPatch(typeof(GlobalDataTracker), "LoadStashForCharacter")]
        public class GlobalDataTracker_LoadStashForCharacter
        {
            [HarmonyPrefix]
            static void Prefix(GlobalDataTracker __instance, Il2CppCysharp.Threading.Tasks.UniTask __result, Il2Cpp.CharacterDataTracker __0)
            {
                Main.logger_instance.Msg("GlobalDataTracker.LoadStashForCharacter();");
                //0 'tracker

                //Fix Stash here
            }
        }*/
    }
}
