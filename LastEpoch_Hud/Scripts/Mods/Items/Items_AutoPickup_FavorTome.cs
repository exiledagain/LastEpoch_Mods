using HarmonyLib;
using Il2Cpp;

namespace LastEpoch_Hud.Scripts.Mods.Items
{
    public class Items_AutoPickup_FavorTome
    {
        public static bool CanRun()
        {
            if ((Scenes.IsGameScene()) && (!Save_Manager.instance.IsNullOrDestroyed()) &&
                (!Refs_Manager.player_actor.IsNullOrDestroyed()))
            {
                return Save_Manager.instance.data.Items.Pickup.Enable_AutoPickup_FavorTome;
            }
            else { return false; }
        }
        [HarmonyPatch(typeof(GroundItemManager), "dropFavorTomeForPlayer")]
        public class GroundItemManager_dropFavorTomeForPlayer
        {
            [HarmonyPostfix]
            static void Postfix(ref GroundItemManager __instance, Actor __0, int __1, ref UnityEngine.Vector3 __2, bool __3, bool __4)
            {
                if (CanRun())
                {
                    System.UInt32 tome_id = __instance.nextFavorTomeId - 1;
                    foreach (PickupFavorTomeInteraction pick_favor_tome_interaction in __instance.activeFavorTomes)
                    {
                        if (pick_favor_tome_interaction.id == tome_id)
                        {
                            __2 = Refs_Manager.player_actor.position();
                            __instance.pickupFavorTome(__0, tome_id, pick_favor_tome_interaction);
                            break;
                        }
                    }
                }
            }
        }
    }
}
