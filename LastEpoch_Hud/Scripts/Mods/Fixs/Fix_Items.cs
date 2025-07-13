using HarmonyLib;
using Il2Cpp;
using System;

namespace LastEpoch_Hud.Scripts.Mods.Fixs
{
    public class Fix_Items
    {
        #region Affixes
        public static bool Verify_AffixID(int affix_id)
        {
            bool result = false;
            bool found = false;
            if (!AffixList.instance.IsNullOrDestroyed())
            {
                foreach (AffixList.Affix affix in AffixList.instance.singleAffixes)
                {
                    if (affix.affixId == affix_id)
                    {
                        found = true;
                        result = true;
                        break;
                    }
                }
                if (!found)
                {
                    foreach (AffixList.Affix affix in AffixList.instance.multiAffixes)
                    {
                        if (affix.affixId == affix_id)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                Main.logger_instance.Error("Fix_Items.Verify_AffixID() : AffixList.instance is null, Can't check if affix exist");
                result = true;
            }
            
            return result;
        }

        [HarmonyPatch(typeof(ItemData), "GetNonSealedPrefixAndSuffixCount")]
        public class ItemData_GetNonSealedPrefixAndSuffixCount
        {
            [HarmonyPrefix]
            static bool Prefix(ref ItemData __instance, ref Int32 __0, ref Int32 __1)
            {
                __0 = __instance.GetNonSealedPrefixes().Count;
                __1 = __instance.GetNonSealedSuffixes().Count;
                
                return false;
            }
        }
        [HarmonyPatch(typeof(ItemData), "GetNonSealedPrefixes")]
        public class ItemData_GetNonSealedPrefixes
        {
            [HarmonyPrefix]
            static void Prefix(ref ItemData __instance, Il2CppSystem.Collections.Generic.List<Il2Cpp.ItemAffix> __result)
            {
                int index = 0;
                bool need_fix = false;                
                Il2CppSystem.Collections.Generic.List<ItemAffix> affixlist = new Il2CppSystem.Collections.Generic.List<ItemAffix>();
                foreach (ItemAffix affix in __instance.affixes)
                {
                    bool remove_affix = false;
                    if ((affix.affixType == AffixList.AffixType.PREFIX) && (!affix.isSealedAffix))
                    {
                        if (!Verify_AffixID(affix.affixId))
                        {
                            Main.logger_instance.Error("Fix : AffixId not found, Remove affix from item");
                            remove_affix = true;
                            need_fix = true;
                        }
                        if (affix.affixTier > 6)
                        {
                            Main.logger_instance.Error("Fix : Affix > 7");
                            affix.affixTier = 6;
                            need_fix = true;
                        }                        
                        index++;
                    }
                    if (!remove_affix) { affixlist.Add(affix); }                    
                }
                if (need_fix)
                {
                    __instance.affixes = affixlist;
                    __instance.RefreshIDAndValues();
                }
            }
        }
        [HarmonyPatch(typeof(ItemData), "GetNonSealedSuffixes")]
        public class ItemData_GetNonSealedSuffixes
        {
            [HarmonyPrefix]
            static void Prefix(ref ItemData __instance, Il2CppSystem.Collections.Generic.List<Il2Cpp.ItemAffix> __result)
            {
                int index = 0;
                bool need_fix = false;
                Il2CppSystem.Collections.Generic.List<ItemAffix> affixlist = new Il2CppSystem.Collections.Generic.List<ItemAffix>();
                foreach (ItemAffix affix in __instance.affixes)
                {
                    bool remove_affix = false;
                    if ((affix.affixType == AffixList.AffixType.SUFFIX) && (!affix.isSealedAffix))
                    {
                        if (!Verify_AffixID(affix.affixId))
                        {
                            Main.logger_instance.Error("Fix : AffixId not found, Remove affix from item");
                            remove_affix = true;
                            need_fix = true;
                        }
                        if (affix.affixTier > 6)
                        {
                            Main.logger_instance.Error("Fix : Affix > 7");
                            affix.affixTier = 6;
                            need_fix = true;
                        }                        
                        index++;
                    }
                    if (!remove_affix) { affixlist.Add(affix); }
                }
                if (need_fix)
                {
                    __instance.affixes = affixlist;
                    __instance.RefreshIDAndValues();
                }
            }
        }
        #endregion
        #region Quantity
        [HarmonyPatch(typeof(OneSlotItemContainer), "TryAddItem", new System.Type[] { typeof(ItemData), typeof(int), typeof(Context) })]
        public class OneSlotItemContainer_TryAddItem
        {
            [HarmonyPrefix]
            static void Prefix(OneSlotItemContainer __instance, bool __result, ItemData __0, ref int __1, Context __2)
            {
                if (__1 == 0) //Fix forge slot quantity bug
                {
                    Main.logger_instance.Error("Fix : Item quantity = 0, Set quantity to 1");
                    __1 = 1;
                }
                /*else if (__1 > 1)
                {
                    if (!ItemList.IsStackableItem(__0))
                    {
                        Main.logger_instance.Error("Fix : Item not stackeable, Set quantity to 1");
                        __1 = 1;
                    }
                }*/
            }
        }
        #endregion
    }
}
