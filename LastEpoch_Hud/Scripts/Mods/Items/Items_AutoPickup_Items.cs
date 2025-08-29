using HarmonyLib;
using Il2Cpp;
using Il2CppItemFiltering;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Items
{
    [RegisterTypeInIl2Cpp]
    public class Items_AutoPickup_Items : MonoBehaviour
    {
        public Items_AutoPickup_Items(System.IntPtr ptr) : base(ptr) { }
        public static Items_AutoPickup_Items instance { get; private set; }

        public static string SceneName = "";
        public static byte RuneOfShatter_Type = 102;
        public static byte RuneOfShatter_Subtype = 0;
        public static System.Collections.Generic.List<Il2CppSystem.Collections.Generic.List<ItemAffix>> affix_queue = null;
        public static int ShatterIndex = 0;
        public static bool Updating = false;

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if (Scenes.IsGameScene())
            {                
                if (Scenes.SceneName != SceneName) //Scene changed
                {
                    SceneName = Scenes.SceneName;
                    affix_queue = new System.Collections.Generic.List<Il2CppSystem.Collections.Generic.List<ItemAffix>>();
                    ShatterIndex = 0;
                }
                if ((affix_queue.Count > 0) && (ShatterIndex < affix_queue.Count) && (!Updating))
                {
                    Updating = true;
                    Il2CppSystem.Collections.Generic.List<ItemAffix> affixs = affix_queue[ShatterIndex];
                    ShatterIndex++;
                    foreach (ItemAffix affix in affixs)
                    {

                        Drop_Affix(affix.affixId, (affix.affixTier + 1)); }
                    Updating = false;
                }
            }
        }
        
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
                    //Need to save stash
                    //Refs_Manager.player_golbal_data_tracker.SaveMaterials(Refs_Manager.player_golbal_data_tracker.stash);
                    //Refs_Manager.player_golbal_data_tracker.SaveStash();
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
                    item.RefreshIDAndValues();
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
                    //Fix Load items with more than 4 affixs
                    if ((__1.rarity == 5) || (__1.rarity == 6))
                    {
                        __1.rarity = 4;
                        __1.RefreshIDAndValues();
                    }
                    //AutoPickup / AutoShatter / AutoSell                    
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
                            (Save_Manager.instance.data.Items.Pickup.Enable_AutoShatter_FromFilter) ||
                            (Save_Manager.instance.data.Items.Pickup.Enable_AutoSell_FromFilter)))
                        {
                            if (!Refs_Manager.filter_manager.Filter.IsNullOrDestroyed())
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
                                if ((FilterShow) && (Save_Manager.instance.data.Items.Pickup.Enable_AutoPickup_FromFilter))
                                {
                                    bool pickup = ItemContainersManager.Instance.attemptToPickupItem(__1, __0.position());
                                    if (pickup) { result = false; }
                                }
                                else if (!FilterShow)
                                {
                                    if ((Save_Manager.instance.data.Items.Pickup.Enable_AutoShatter_FromFilter) && (Get_RuneOfShatterCount() > 0) && (item.affixes.Count > 0))
                                    {
                                        affix_queue.Add(item.affixes);
                                        Decrease_RuneOfShatter();
                                        result = false;
                                    }
                                    else if (Save_Manager.instance.data.Items.Pickup.Enable_AutoSell_FromFilter)
                                    {
                                        __0.goldTracker.modifyGold(item.VendorSaleValue);
                                        result = false;
                                    }
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