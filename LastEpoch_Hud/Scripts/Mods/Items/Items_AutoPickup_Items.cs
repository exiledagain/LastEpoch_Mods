using HarmonyLib;
using Il2Cpp;
using Il2CppItemFiltering;

namespace LastEpoch_Hud.Scripts.Mods.Items
{
    public class Items_AutoPickup_Items
    {
        [HarmonyPatch(typeof(GroundItemManager), "dropItemForPlayer")]
        public class GroundItemManager_dropItemForPlayer
        {
            [HarmonyPrefix]
            static bool Prefix(ref GroundItemManager __instance, ref Actor __0, ref ItemData __1, ref UnityEngine.Vector3 __2, bool __3)
            {
                bool result = true;
                if (Scenes.IsGameScene())
                {
                    /*//Fix unique subtype when Force Legendary
                    if ((!Save_Manager.instance.IsNullOrDestroyed()) && (__1.rarity == 9))
                    {
                        if (Save_Manager.instance.data.Items.Drop.Enable_ForceLegendary)
                        {
                            UniqueList.Entry unique = UniqueList.getUnique(__1.uniqueID);
                            if (!unique.subTypes.Contains((byte)__1.subType))
                            {
                                ushort backup = __1.subType;
                                if (unique.subTypes.Count > 0)
                                {
                                    int rand = UnityEngine.Random.RandomRangeInt(0, unique.subTypes.Count);
                                    __1.subType = unique.subTypes[rand];
                                    __1.id[2] = unique.subTypes[rand]; //subtype
                                    __1.RefreshIDAndValues();
                                }
                            }

                            //Get eligibles affixes
                            Il2CppSystem.ValueTuple<Il2CppSystem.Collections.Generic.List<AffixList.Affix>, Il2CppSystem.Collections.Generic.List<AffixList.Affix>> eligibles_affixes = __1.GetEligibleAffixes(new Il2CppSystem.Collections.Generic.List<int>(), AffixList.SpecialAffixType.Standard);
                            //Add affixes

                        }
                    }*/

                    //Fix Load items with more than 4 affixs
                    if ((__1.rarity == 5) || (__1.rarity == 6))
                    {
                        __1.rarity = 4;
                        __1.RefreshIDAndValues();
                    }
                    //AutoPickup / AutoSell                    
                    ItemDataUnpacked item = __1.TryCast<ItemDataUnpacked>();
                    if ((!Save_Manager.instance.IsNullOrDestroyed()) && (!item.IsNullOrDestroyed()))
                    {
                        if (((Save_Manager.instance.data.Items.Pickup.Enable_AutoPickup_Keys) && (Item.isKey(__1.itemType))) ||
                            ((Save_Manager.instance.data.Items.Pickup.Enable_AutoPickup_WovenEchoes) && (__1.itemType == 107)) ||
                            ((Save_Manager.instance.data.Items.Pickup.Enable_AutoPickup_Materials) && (ItemList.isCraftingItem(__1.itemType))))
                        {
                            bool pickup = ItemContainersManager.Instance.attemptToPickupItem(__1, __0.position());
                            if (pickup) { result = false; }
                        }
                        else if ((__1.itemType < 34) &&
                            (!Refs_Manager.filter_manager.IsNullOrDestroyed()) &&
                            ((Save_Manager.instance.data.Items.Pickup.Enable_AutoPickup_FromFilter) ||
                            (Save_Manager.instance.data.Items.Pickup.Enable_AutoSell_FromFilter)))
                        {
                            if (!Refs_Manager.filter_manager.Filter.IsNullOrDestroyed())
                            {
                                bool FilterShow = false;
                                foreach (Rule rule in Refs_Manager.filter_manager.Filter.rules)
                                {
                                    if ((rule.isEnabled) && (rule.Match(item)) &&
                                        (((rule.levelDependent) && (rule.LevelInBounds(__0.stats.level))) ||
                                        (!rule.levelDependent)))
                                    {
                                        if ((rule.type == Rule.RuleOutcome.SHOW) || (rule.type == Rule.RuleOutcome.HIGHLIGHT))
                                        {
                                            FilterShow = true;
                                            break;
                                        }
                                    }
                                }
                                if ((FilterShow) && (Save_Manager.instance.data.Items.Pickup.Enable_AutoPickup_FromFilter))
                                {
                                    bool pickup = ItemContainersManager.Instance.attemptToPickupItem(__1, __0.position());
                                    if (pickup) { result = false; }
                                }
                                else if ((!FilterShow) && (Save_Manager.instance.data.Items.Pickup.Enable_AutoSell_FromFilter))
                                {
                                    __0.goldTracker.modifyGold(item.VendorSaleValue);
                                    result = false;
                                }
                            }
                        }
                    }
                }

                return result;
            }
        }
    }
}