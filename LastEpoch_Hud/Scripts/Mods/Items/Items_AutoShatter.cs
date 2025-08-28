using HarmonyLib;
using Il2Cpp;
using Il2CppItemFiltering;

namespace LastEpoch_Hud.Scripts.Mods.Items
{
    public class Items_AutoShatter
    {
        public static byte RuneOfShatter_Type = 102;
        public static byte RuneOfShatter_Subtype = 0;

        public static int Get_RuneOfShatterCount()
        {
            int result = 0;
            if (!Refs_Manager.player_golbal_data_tracker.IsNullOrDestroyed())
            {
                if (!Refs_Manager.player_golbal_data_tracker.stash.IsNullOrDestroyed())
                {
                    foreach (Il2CppLE.Data.ItemLocationPair item in Refs_Manager.player_golbal_data_tracker.stash.MaterialsList)
                    {
                        if (item.Data.Count > 4)
                        {
                            if ((item.Data[3] == RuneOfShatter_Type) && (item.Data[4] == RuneOfShatter_Subtype))
                            {
                                result = item.Quantity;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }
        public static void Decrease_RuneOfShatter()
        {
            if (!Refs_Manager.player_golbal_data_tracker.IsNullOrDestroyed())
            {
                if (!Refs_Manager.player_golbal_data_tracker.stash.IsNullOrDestroyed())
                {
                    foreach (Il2CppLE.Data.ItemLocationPair item in Refs_Manager.player_golbal_data_tracker.stash.MaterialsList)
                    {
                        if (item.Data.Count > 4)
                        {
                            if ((item.Data[3] == RuneOfShatter_Type) && (item.Data[4] == RuneOfShatter_Subtype))
                            {
                                item.Quantity--;
                                break;
                            }
                        }
                    }
                }
            }
        }
        public static void Drop_Affix(ushort subtype, int quantity)
        {
            if ((!Refs_Manager.ground_item_manager.IsNullOrDestroyed()) && (!Refs_Manager.player_actor.IsNullOrDestroyed()))
            {
                for (int i = 0; i < quantity; i++)
                {
                    ItemDataUnpacked item = new ItemDataUnpacked
                    {
                        itemType = 101,
                        subType = subtype,
                        rarity = 0
                    };
                    Refs_Manager.ground_item_manager.dropItemForPlayer(Refs_Manager.player_actor, item.TryCast<ItemData>(), Refs_Manager.player_actor.position(), false);
                }
            }
        }

        [HarmonyPatch(typeof(GroundItemManager), "dropItemForPlayer")]
        public class GroundItemManager_dropItemForPlayer
        {
            [HarmonyPrefix]
            static bool Prefix(ref GroundItemManager __instance, ref Actor __0, ref ItemData __1, ref UnityEngine.Vector3 __2, bool __3)
            {
                bool result = true;
                if (Scenes.IsGameScene())
                {
                    ItemDataUnpacked item = __1.TryCast<ItemDataUnpacked>();
                    if ((!Save_Manager.instance.IsNullOrDestroyed()) && (!item.IsNullOrDestroyed()) && (Get_RuneOfShatterCount() > 0) && (!Refs_Manager.filter_manager.IsNullOrDestroyed()))
                    {
                        if ((__1.itemType < 34) && (!Refs_Manager.filter_manager.Filter.IsNullOrDestroyed()) && (Save_Manager.instance.data.Items.Pickup.Enable_AutoShatter_FromFilter))
                        {
                            bool FilterShow = false;
                            foreach (Rule rule in Refs_Manager.filter_manager.Filter.rules)
                            {
                                if ((rule.isEnabled) && (rule.Match(item, Refs_Manager.player_actor.stats.level)))
                                {
                                    if (rule.type == Rule.RuleOutcome.SHOW)
                                    {
                                        FilterShow = true;
                                        break;
                                    }
                                }
                            }
                            if (!FilterShow)
                            {
                                foreach (ItemAffix affix in __1.affixes) { Drop_Affix(affix.affixId, (affix.affixTier + 1)); }
                                Decrease_RuneOfShatter();
                                result = false; //Don't drop
                            }
                        }
                    }
                }

                return result;
            }
        }
    }
}
