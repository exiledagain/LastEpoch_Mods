using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Character
{
    public class Character_Visuals
    {
        private static int backup_class = -1;
        private static int wanted_class = -1;        
        private static bool loading_screen = false;
        private static readonly string[] CharacterClass = { "Primalist", "Mage", "Sentinel", "Acolyte", "Rogue" };

        private static int Get_WantedClass(int character_index)
        {
            int index = -1;
            string str = "";
            if (!Save_Manager.instance.data.IsNullOrDestroyed())
            {                
                if (character_index == 0) { str = Save_Manager.instance.data.Character.Visuals.Primalist; }
                else if (character_index == 1) { str = Save_Manager.instance.data.Character.Visuals.Mage; }
                else if (character_index == 2) { str = Save_Manager.instance.data.Character.Visuals.Sentinel; }
                else if (character_index == 3) { str = Save_Manager.instance.data.Character.Visuals.Acolyte; }
                else if (character_index == 4) { str = Save_Manager.instance.data.Character.Visuals.Rogue; }
                                
                int i = 0;
                foreach (string s in CharacterClass)
                {
                    if (s == str) { index = i; break; }
                    i++;
                }
            }

            return index;
        }
        private static void Set_CharacterClass(int Class)
        {
            if (!Refs_Manager.player_data_tracker.IsNullOrDestroyed())
            {
                if (!Refs_Manager.player_data_tracker.charData.IsNullOrDestroyed()) { Refs_Manager.player_data_tracker.charData.CharacterClass = Class; }
            }
        }

        //Set backup_class and wanted_class when Loading a character
        [HarmonyPatch(typeof(ActorVisuals), "CreateClassVisuals")]
        public class ActorVisuals_CreateClassVisuals
        {
            [HarmonyPrefix]
            static void Prefix(ActorVisuals __instance, ref int __0)
            {
                backup_class = __0;
                wanted_class = -1;
                int index = Get_WantedClass(__0);
                if ((index > -1) && (index < 5))
                {
                    if (__0 != index)
                    {
                        wanted_class = index;
                        __0 = index;
                    }
                }
            }
        }

        //Set all Visuals (items and cosmetics) on LoadingScreen
        [HarmonyPatch(typeof(Il2CppLE.UI.LoadingScreen), "OnBeforeSceneLoaded")]
        public class LoadingScreen_OnBeforeSceneLoaded
        {
            [HarmonyPostfix]
            static void Postfix(Il2CppLE.UI.LoadingScreen __instance, string __0, UnityEngine.SceneManagement.LoadSceneMode __1, string __2)
            {
                loading_screen = true;
                if ((wanted_class > -1) && (backup_class > -1)) { Set_CharacterClass(wanted_class); }
            }
        }

        [HarmonyPatch(typeof(Il2CppLE.UI.LoadingScreen), "Disable")]
        public class LoadingScreen_Disable
        {
            [HarmonyPostfix]
            static void Postfix(Il2CppLE.UI.LoadingScreen __instance)
            {
                loading_screen = false;
                if ((wanted_class > -1) && (backup_class > -1)) { Set_CharacterClass(backup_class); }
            }
        }

        //Replace Items Visuals when item contain ClassReq
        [HarmonyPatch(typeof(EquipmentVisualsManager), "EquipGearAsync")]
        public class EquipmentVisualsManager_EquipGearAsync
        {
            [HarmonyPrefix]
            static void Prefix(EquipmentVisualsManager __instance, Il2CppCysharp.Threading.Tasks.UniTask __result, EquipmentType __0, ref int __1, bool __2, ref ushort __3)
            {
                if ((__instance.didAwake) && (__instance.name == "v_MainPlayer") && (wanted_class > -1) && (backup_class > -1))
                {
                    //Check item class req, then set another subitem and another unique if unique
                    bool class_req = false;
                    int item_type = 0;
                    foreach (ItemList.BaseEquipmentItem baseEquipmentItem in ItemList.instance.EquippableItems)
                    {
                        if (baseEquipmentItem.type == __0)
                        {
                            if ((__1 > -1) && (__1 < baseEquipmentItem.subItems.Count))
                            {
                                if (baseEquipmentItem.subItems[__1].classRequirement != ItemList.ClassRequirement.None)
                                {
                                    class_req = true;
                                }
                            }
                            break;
                        }
                        item_type++;
                    }
                    if (class_req)
                    {
                        if (!__2) //Check for another Base item without class requirement
                        {
                            bool found = false;
                            int i = 0;
                            foreach (ItemList.BaseEquipmentItem baseEquipmentItem in ItemList.instance.EquippableItems)
                            {
                                if (baseEquipmentItem.type == __0)
                                {
                                    foreach (ItemList.EquipmentItem equipmentItem in baseEquipmentItem.subItems)
                                    {
                                        if (equipmentItem.classRequirement == ItemList.ClassRequirement.None)
                                        {
                                            found = true;
                                            break;
                                        }
                                        i++;
                                    }
                                    break;
                                }
                            }
                            if (found) { __1 = i; }
                        }
                        else //Check for another Unique without class requirement
                        {
                            bool found = false;
                            foreach (UniqueList.Entry unique in UniqueList.instance.uniques)
                            {
                                if (unique.baseType == item_type)
                                {
                                    int unique_subtype = unique.subTypes[0];
                                    foreach (ItemList.BaseEquipmentItem baseEquipmentItem in ItemList.instance.EquippableItems)
                                    {
                                        if (baseEquipmentItem.type == __0)
                                        {
                                            if ((unique_subtype > -1) && (unique_subtype < baseEquipmentItem.subItems.Count))
                                            {
                                                if (baseEquipmentItem.subItems[unique_subtype].classRequirement == ItemList.ClassRequirement.None)
                                                {
                                                    found = true;
                                                    __1 = unique_subtype;
                                                    __3 = unique.uniqueID;
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                                if (found) { break; }
                            }
                        }
                    }
                    if (!loading_screen) { Set_CharacterClass(wanted_class); }
                }
            }
            [HarmonyPostfix]
            static void Postifx(EquipmentVisualsManager __instance, Il2CppCysharp.Threading.Tasks.UniTask __result, EquipmentType __0, int __1, bool __2, ushort __3)
            {
                if ((__instance.didAwake) && (__instance.name == "v_MainPlayer") && (wanted_class > -1) && (backup_class > -1))
                {
                    if (!loading_screen) { Set_CharacterClass(backup_class); }
                }
            }
        }
    }
}
