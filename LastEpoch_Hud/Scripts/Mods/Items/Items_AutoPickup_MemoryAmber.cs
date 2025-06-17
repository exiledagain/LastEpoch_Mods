using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Items
{
    public class Items_AutoPickup_MemoryAmber
    {
        public static bool CanRun()
        {
            if ((Scenes.IsGameScene()) && (!Save_Manager.instance.IsNullOrDestroyed()) &&
                (!Refs_Manager.player_actor.IsNullOrDestroyed()))
            {
                return Save_Manager.instance.data.Items.Pickup.Enable_AutoPickup_MemoryAmber;
            }
            else { return false; }
        }
        [HarmonyPatch(typeof(SilkenCocoonData), "DropMemoryAmber", new System.Type[] { typeof(Vector3), typeof(int), typeof(float), typeof(Il2CppSystem.Func<Actor, bool>) })]
        public class SilkenCocoonData_DropMemoryAmber
        {
            [HarmonyPrefix]
            static void Prefix(ref UnityEngine.Vector3 __0, int __1, float __2, Il2CppSystem.Func<Actor, bool> __3)
            {
                if (CanRun())
                {
                    __0 = Refs_Manager.player_actor.position();
                }
            }
        }
    }
}
