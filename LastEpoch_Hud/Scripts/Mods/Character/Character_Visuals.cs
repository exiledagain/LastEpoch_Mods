using HarmonyLib;
using Il2Cpp;

namespace LastEpoch_Hud.Scripts.Mods.Character
{
    public class Character_Visuals
    {     
        public static int wanted_class = -1;
        public static int backup_class = -1;
        public static readonly string[] CharacterClass = { "Primalist", "Mage", "Sentinel", "Acolyte", "Rogue" };

        public static void Set_WantedClass(string name)
        {            
            if (name == "Primalist") { wanted_class = 0; }
            else if (name == "Mage") { wanted_class = 1; }
            else if (name == "Sentinel") { wanted_class = 2; }
            else if (name == "Acolyte") { wanted_class = 3; }
            else if (name == "Rogue") { wanted_class = 4; }
            else { wanted_class = -1; }
        }

        [HarmonyPatch(typeof(ActorVisuals), "CreateClassVisuals")]
        public class ActorVisuals_CreateClassVisuals
        {
            [HarmonyPrefix]
            static void Prefix(ActorVisuals __instance, ref int __0)
            {
                if (!Save_Manager.instance.data.IsNullOrDestroyed())
                {
                    string str = "";
                    if (__0 == 0) { str = Save_Manager.instance.data.Character.Visuals.Primalist; }
                    else if (__0 == 1) { str = Save_Manager.instance.data.Character.Visuals.Mage; }
                    else if (__0 == 2) { str = Save_Manager.instance.data.Character.Visuals.Sentinel; }
                    else if (__0 == 3) { str = Save_Manager.instance.data.Character.Visuals.Acolyte; }
                    else if (__0 == 4) { str = Save_Manager.instance.data.Character.Visuals.Rogue; }
                    Set_WantedClass(str);
                    if ((wanted_class > -1) && (wanted_class < 5)) { __0 = wanted_class; }
                }              
            }
        }

        [HarmonyPatch(typeof(EquipmentVisualsManager), "EquipGearAsync")]
        public class EquipmentVisualsManager_EquipGearAsync
        {
            [HarmonyPrefix]
            static void Prefix(EquipmentVisualsManager __instance, Il2CppCysharp.Threading.Tasks.UniTask __result, EquipmentType __0, ref int __1, bool __2, ref ushort __3)
            {
                if (__instance.didAwake)
                {
                    if ((wanted_class > -1) && (__instance.name == "v_MainPlayer"))
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

                        //Set character class to wanted_class
                        if (!Refs_Manager.player_data_tracker.IsNullOrDestroyed())
                        {
                            if (!Refs_Manager.player_data_tracker.charData.IsNullOrDestroyed())
                            {
                                backup_class = Refs_Manager.player_data_tracker.charData.CharacterClass;
                                Refs_Manager.player_data_tracker.charData.CharacterClass = wanted_class;
                            }
                            else { Main.logger_instance.Error("player_data_tracker.charData is null"); }
                        }
                        else { Main.logger_instance.Error("player_data_tracker is null"); }
                    }
                }
            }
            [HarmonyPostfix]
            static void Postifx(EquipmentVisualsManager __instance, Il2CppCysharp.Threading.Tasks.UniTask __result, EquipmentType __0, int __1, bool __2, ushort __3)
            {
                if (__instance.didAwake)
                {
                    if ((backup_class > -1) && (__instance.name == "v_MainPlayer"))
                    {
                        //Reset character class
                        if (!Refs_Manager.player_data_tracker.IsNullOrDestroyed())
                        {
                            if (!Refs_Manager.player_data_tracker.charData.IsNullOrDestroyed())
                            {
                                Refs_Manager.player_data_tracker.charData.CharacterClass = backup_class;
                                backup_class = -1;
                            }
                            else { Main.logger_instance.Error("player_data_tracker.charData is null"); }
                        }
                        else { Main.logger_instance.Error("player_data_tracker is null"); }
                    }
                }
            }
        }
    }
}
