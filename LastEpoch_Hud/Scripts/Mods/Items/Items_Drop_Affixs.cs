using HarmonyLib;
using UnityEngine;
using Il2Cpp;

namespace LastEpoch_Hud.Scripts.Mods.Items
{
    public class Items_Drop_Affixs
    {
        private static bool CanRun()
        {
            if ((Hud_Manager.IsPauseOpen()) && (Hud_Manager.Content.OdlForceDrop.enable)) { return false; }
            else if ((Scenes.IsGameScene()) && (!Save_Manager.instance.IsNullOrDestroyed()))
            {
                if ((!Save_Manager.instance.data.IsNullOrDestroyed()) &&
                    ((Save_Manager.instance.data.Items.Drop.Enable_ForceUnique) ||
                    (Save_Manager.instance.data.Items.Drop.Enable_ForceSet) ||
                    (Save_Manager.instance.data.Items.Drop.Enable_ForceLegendary) ||
                    (Save_Manager.instance.data.Items.Drop.AffixCount_Min != 0) ||
                    (Save_Manager.instance.data.Items.Drop.AffixCount_Max != 4) ||
                    (Save_Manager.instance.data.Items.Drop.Enable_AffixTiers) ||
                    (Save_Manager.instance.data.Items.Drop.Enable_AffixValues) ||
                    (Save_Manager.instance.data.Items.Drop.Enable_ForceSeal) ||
                    (Save_Manager.instance.data.Items.Drop.Enable_SealTier) ||
                    (Save_Manager.instance.data.Items.Drop.Enable_SealValue)))
                {
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }
        
        [HarmonyPatch(typeof(GenerateItems), "DropItemAtPoint")]
        public class GenerateItems_DropItemAtPoint
        {
            [HarmonyPrefix]
            static void Prefix(GenerateItems __instance, ref ItemDataUnpacked __0, ref UnityEngine.Vector3 __1, int __2)
            {
                if ((CanRun()) && (__0.itemType < 100))
                {
                    if (__0.itemType < 25)
                    {
                        if ((!__0.isUnique()) && (!__0.isSet())) //Base item and Legendary
                        {
                            if ((Save_Manager.instance.data.Items.Drop.Enable_AffixTiers) ||
                                (Save_Manager.instance.data.Items.Drop.Enable_AffixValues) ||
                                (Save_Manager.instance.data.Items.Drop.Enable_SealTier) ||
                                (Save_Manager.instance.data.Items.Drop.Enable_SealValue))
                            {
                                foreach (ItemAffix aff in __0.affixes)
                                {
                                    if (!aff.IsSealed)
                                    {
                                        if (Save_Manager.instance.data.Items.Drop.Enable_AffixTiers)
                                        {
                                            byte roll = 0;
                                            if (Save_Manager.instance.data.Items.Drop.AffixTiers_Min == Save_Manager.instance.data.Items.Drop.AffixTiers_Max) { roll = (byte)Save_Manager.instance.data.Items.Drop.AffixTiers_Max; }
                                            else { roll = (byte)Random.RandomRange(Save_Manager.instance.data.Items.Drop.AffixTiers_Min, Save_Manager.instance.data.Items.Drop.AffixTiers_Max); }
                                            aff.affixTier = roll;
                                        }
                                        if (Save_Manager.instance.data.Items.Drop.Enable_AffixValues)
                                        {
                                            byte roll = 0;
                                            if (Save_Manager.instance.data.Items.Drop.AffixValues_Min == Save_Manager.instance.data.Items.Drop.AffixValues_Max) { roll = (byte)Save_Manager.instance.data.Items.Drop.AffixValues_Max; }
                                            else { roll = (byte)Random.RandomRange(Save_Manager.instance.data.Items.Drop.AffixValues_Min, Save_Manager.instance.data.Items.Drop.AffixValues_Max); }
                                            aff.affixRoll = roll;
                                        }
                                    }
                                    else
                                    {
                                        if (Save_Manager.instance.data.Items.Drop.Enable_SealTier)
                                        {
                                            byte roll = 0;
                                            if (Save_Manager.instance.data.Items.Drop.SealTier_Min == Save_Manager.instance.data.Items.Drop.SealTier_Max) { roll = (byte)Save_Manager.instance.data.Items.Drop.SealTier_Max; }
                                            else { roll = (byte)Random.RandomRange(Save_Manager.instance.data.Items.Drop.SealTier_Min, Save_Manager.instance.data.Items.Drop.SealTier_Max); }
                                            aff.affixTier = roll;
                                        }
                                        if (Save_Manager.instance.data.Items.Drop.Enable_SealValue)
                                        {
                                            byte roll = 0;
                                            if (Save_Manager.instance.data.Items.Drop.SealValue_Min == Save_Manager.instance.data.Items.Drop.SealValue_Max) { roll = (byte)Save_Manager.instance.data.Items.Drop.SealValue_Max; }
                                            else { roll = (byte)Random.RandomRange(Save_Manager.instance.data.Items.Drop.SealValue_Min, Save_Manager.instance.data.Items.Drop.SealValue_Max); }
                                            aff.affixRoll = roll;
                                        }
                                    }
                                }
                                __0.RefreshIDAndValues();
                            }                            
                        }
                    }
                    else if ((__0.itemType > 24) && (__0.itemType < 34)) //Idols
                    {
                        if (Save_Manager.instance.data.Items.Drop.Enable_AffixValues)
                        {
                            foreach (ItemAffix aff in __0.affixes)
                            {
                                byte roll = 0;
                                if (Save_Manager.instance.data.Items.Drop.AffixValues_Min == Save_Manager.instance.data.Items.Drop.AffixValues_Max) { roll = (byte)Save_Manager.instance.data.Items.Drop.AffixValues_Max; }
                                else { roll = (byte)Random.RandomRange(Save_Manager.instance.data.Items.Drop.AffixValues_Min, Save_Manager.instance.data.Items.Drop.AffixValues_Max); }
                                aff.affixRoll = roll;
                            }
                            __0.RefreshIDAndValues();
                        }
                    }
                    else //New Items (have to check this)
                    {
                        if (Save_Manager.instance.data.Items.Drop.Enable_AffixValues)
                        {
                            foreach (ItemAffix aff in __0.affixes)
                            {
                                byte roll = 0;
                                if (Save_Manager.instance.data.Items.Drop.AffixValues_Min == Save_Manager.instance.data.Items.Drop.AffixValues_Max) { roll = (byte)Save_Manager.instance.data.Items.Drop.AffixValues_Max; }
                                else { roll = (byte)Random.RandomRange(Save_Manager.instance.data.Items.Drop.AffixValues_Min, Save_Manager.instance.data.Items.Drop.AffixValues_Max); }
                                aff.affixRoll = roll;
                            }
                            __0.RefreshIDAndValues();
                        }
                    }
                }
            }
        }
    }
}
