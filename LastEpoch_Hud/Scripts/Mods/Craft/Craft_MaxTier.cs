using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Craft
{
    public class Craft_MaxTier
    {
        public static CraftingSlotManager crafting_slot_manager = null;
        public static ItemData item = null;

        public class Get
        {
            public static int Tier(ItemData item_data, int affix_id)
            {
                int result = -1;
                if (!item_data.IsNullOrDestroyed())
                {
                    foreach (ItemAffix affix in item_data.affixes)
                    {
                        if (affix.affixId == affix_id)
                        {
                            result = affix.affixTier;
                            break;
                        }
                    }
                }

                return result;
            }
            public static bool IsIdol(ItemData item)
            {
                bool result = false;
                if ((item.itemType > 24) && (item.itemType < 34))
                {
                    result = true;
                }

                return result;
            }
            public static int MaxForginCost(int tier)
            {
                int forgin_cost = 30; //To T6
                if (tier == 5) { forgin_cost = 36; } //To T7
                else if (tier == 6) { forgin_cost = 42; } //To T8

                return forgin_cost;
            }
        }

        [HarmonyPatch(typeof(CraftingManager), "OnMainItemChange")]
        public class CraftingManager_OnMainItemChange
        {
            [HarmonyPostfix]
            static void Postfix(ref CraftingManager __instance, ref Il2CppSystem.Object __0, ref ItemContainerEntryHandler __1)
            {
                if (!__0.IsNullOrDestroyed())
                {
                    item = null;
                    OneItemContainer item_container = __0.TryCast<OneItemContainer>();
                    if (!item_container.IsNullOrDestroyed())
                    {
                        if (!item_container.content.IsNullOrDestroyed())
                        {
                            item = item_container.content.data;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CraftingManager), "OnMainItemRemoved")]
        public class CraftingManager_OnMainItemRemoved
        {
            [HarmonyPostfix]
            static void Postfix(CraftingManager __instance, Il2CppSystem.Object __0, ItemContainerEntryHandler __1)
            {
                item = null;
            }
        }

        [HarmonyPatch(typeof(CraftingManager), "CheckForgeCapability")]
        public class CheckForgeCapability
        {
            [HarmonyPostfix]
            static void Postfix(ref CraftingManager __instance, ref bool __result, ref System.String __0, ref System.Boolean __1, ref System.Boolean __2, ref System.String __3)
            {
                if ((__0.Contains("This affix has already reached")) && (!item.IsNullOrDestroyed()))
                {
                    int affix_id = __instance.appliedAffixID;
                    int affix_tier = Get.Tier(item, affix_id);
                    int max_forgin_potencial = Get.MaxForginCost(affix_tier);
                    if (!crafting_slot_manager.IsNullOrDestroyed()) { crafting_slot_manager.maxForgingPotentialText.text = "<size=13><color=#FF0000>-" + max_forgin_potencial; }
                    if (item.forgingPotential > max_forgin_potencial)
                    {
                        int max_tier = 6;
                        if (item.IsPrimordialItem()) { max_tier = 7; }
                        if (affix_tier < max_tier)
                        {
                            __0 = "Upgrade Affix";
                            __1 = false;
                            __2 = false;
                            __3 = "";
                            __result = true;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CraftingUpgradeButton), "UpdateButton")]
        public class CraftingUpgradeButton_UpdateButton
        {
            [HarmonyPrefix]
            static void Prefix(ref CraftingUpgradeButton __instance, int __0, ref bool __1)
            {
                if ((Scenes.IsGameScene()) && (!item.IsNullOrDestroyed()) && (__0 > -1))
                {
                    if (Get.IsIdol(item)) { __1 = false; }
                    else
                    {
                        int tier = Get.Tier(item, __0);
                        int max_tier = 6;
                        if (item.IsPrimordialItem()) { max_tier = 7; }
                        if ((tier > -1) && (tier < max_tier) && (item.forgingPotential > Get.MaxForginCost(tier))) { __1 = true; }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CraftingSlotManager), "Awake")]
        public class CraftingSlotManager_Awake
        {
            [HarmonyPostfix]
            static void Postfix(ref CraftingSlotManager __instance)
            {
                crafting_slot_manager = __instance;
            }
        }

        [HarmonyPatch(typeof(CraftingSlotManager), "Forge")]
        public class CraftingSlotManager_Forge
        {
            [HarmonyPrefix]
            static bool Prefix(ref CraftingSlotManager __instance)
            {
                bool result = true;
                if ((Scenes.IsGameScene()) && (!item.IsNullOrDestroyed()))
                {
                    bool glyph_of_hope = false;                             //25% no forgin potencial cost                 
                    bool glyph_of_chaos = false;                            //Change affix
                    bool glyph_of_order = false;                            //don't roll when upgrade
                    bool glyph_of_despair = false;                          //50% chance to seal (don't work well)
                    bool glyph_of_envy = false;                             //Change subtype

                    OneItemContainer glyph_container = __instance.GetSupport();
                    if (!glyph_container.IsNullOrDestroyed())
                    {
                        ItemData glyph_item = glyph_container.getItem();
                        if (!glyph_item.IsNullOrDestroyed())
                        {
                            switch (glyph_item.subType)
                            {
                                case 0: { glyph_of_hope = true; break; }
                                case 1: { glyph_of_chaos = true; break; }
                                case 2: { glyph_of_order = true; break; }
                                case 3: { glyph_of_despair = true; break; }
                                case 5: { glyph_of_envy = true; break; }

                            }
                        }
                    }

                    int affix_id = __instance.appliedAffixID;
                    int affix_tier = Get.Tier(item, affix_id);
                    int max_tier = 6;
                    if (item.IsPrimordialItem()) { max_tier = 7; }
                    if ((affix_tier > 3) && (affix_tier < max_tier))
                    {
                        bool seal = false;
                        if (glyph_of_despair)
                        {
                            //int roll = Random.RandomRangeInt(0, 2); //50% (0-1)
                            //if (roll == 0) { seal = true; }
                        }
                        if (seal)
                        {
                            int index = 0;
                            foreach (ItemAffix affix in item.affixes)
                            {
                                if (affix.affixId == affix_id) { break; }
                                index++;
                            }
                            item.SealAffix(item.affixes[index]);
                        }
                        else
                        {
                            foreach (ItemAffix affix in item.affixes)
                            {
                                if (affix.affixId == affix_id)
                                {
                                    if (glyph_of_chaos)
                                    {
                                        System.Collections.Generic.List<AffixList.SingleAffix> affix_list = new System.Collections.Generic.List<AffixList.SingleAffix>();
                                        foreach (AffixList.SingleAffix aff in AffixList.instance.singleAffixes)
                                        {
                                            if ((aff.type == affix.affixType) && (aff.CanRollOnItemType(item.itemType, item.TryCast<ItemDataUnpacked>().classReq)))
                                            {
                                                affix_list.Add(aff);
                                            }
                                        }
                                        if (affix_list.Count > 0)
                                        {
                                            affix.affixId = (ushort)affix_list[Random.RandomRangeInt(0, affix_list.Count)].affixId;
                                        }
                                        else { Main.logger_instance.Error("No affix found for glyph of chaos"); }
                                    }
                                    if (!glyph_of_order) { affix.affixRoll = (byte)Random.Range(0f, 255f); }
                                    affix.affixTier++;
                                    break;
                                }
                            }
                        }
                        bool no_cost = false;
                        if (glyph_of_hope)
                        {
                            int roll = Random.RandomRangeInt(0, 5); //25% (0-4)
                            if (roll == 0) { no_cost = true; }
                        }
                        if (!no_cost) { item.forgingPotential -= (byte)Random.RandomRangeInt(1, Get.MaxForginCost(affix_tier)); }
                        if (glyph_of_envy)
                        {
                            System.Collections.Generic.List<int> subtype_list = new System.Collections.Generic.List<int>();
                            foreach (ItemList.BaseEquipmentItem base_item in ItemList.instance.EquippableItems)
                            {
                                if (base_item.baseTypeID == item.itemType)
                                {
                                    foreach (ItemList.EquipmentItem sub_item in base_item.subItems)
                                    {
                                        if (item.TryCast<ItemDataUnpacked>().classReq == sub_item.classRequirement)
                                        {
                                            if (sub_item.subTypeID != item.subType)
                                            {
                                                subtype_list.Add(sub_item.subTypeID);
                                            }
                                        }
                                    }
                                }
                            }
                            if (subtype_list.Count > 0)
                            {
                                item.subType = (ushort)subtype_list[Random.RandomRangeInt(0, subtype_list.Count)];
                            }
                            else { Main.logger_instance.Error("No subtype found for glyph of envy"); }
                        }
                        item.RefreshIDAndValues();
                        result = false;
                    }
                }
                
                return result;
            }
        }
    }
}
