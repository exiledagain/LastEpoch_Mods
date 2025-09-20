using HarmonyLib;
using Il2Cpp;

namespace LastEpoch_Hud.Scripts.Mods.Items
{
    public class Items_AutoPickup_AncienBone
    {
        public static bool CanRun()
        {
            if ((Scenes.IsGameScene()) && (!Save_Manager.instance.IsNullOrDestroyed()) &&
                (!Refs_Manager.player_actor.IsNullOrDestroyed()))
            {
                return Save_Manager.instance.data.Items.Pickup.Enable_AutoPickup_Materials;
            }
            else { return false; }
        }

        [HarmonyPatch(typeof(GroundItemManager), "dropAncientBoneForPlayer")]
        public class GroundItemManager_dropAncientBoneForPlayer
        {
            [HarmonyPostfix]
            static void Postfix(ref GroundItemManager __instance, Actor __0, int __1, UnityEngine.Vector3 __2, bool __3, bool __4)
            {
                if (CanRun())
                {
                    System.UInt32 ancien_bone_id = __instance.nextAncientBoneId - 1;
                    foreach (PickupAncientBonesInteraction pick_ancien_bone_interaction in __instance.activeAncientBones)
                    {
                        if (pick_ancien_bone_interaction.id == ancien_bone_id)
                        {
                            __2 = Refs_Manager.player_actor.position();
                            __instance.pickupAncientBone(__0, ancien_bone_id, pick_ancien_bone_interaction);
                            break;
                        }
                    }
                }
            }
        }
    }
}
