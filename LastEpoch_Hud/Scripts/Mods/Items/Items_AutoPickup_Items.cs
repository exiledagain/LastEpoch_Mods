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