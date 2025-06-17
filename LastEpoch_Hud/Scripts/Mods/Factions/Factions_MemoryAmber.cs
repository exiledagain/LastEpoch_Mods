using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Factions
{
    public class Factions_MemoryAmber
    {
        /*[HarmonyPatch(typeof(SilkenCocoonData), "DropMemoryAmberAfterDelay")]
        public class SilkenCocoonData_DropMemoryAmberAfterDelay
        {
            [HarmonyPrefix]
            static void Prefix(ref SilkenCocoonData __instance, Il2CppSystem.Collections.IEnumerator __result, UnityEngine.Vector3 __0, int __1, float __2, Il2CppSystem.Func<int, float, uint> __3, Il2CppSystem.Func<Il2Cpp.Actor, bool> __4, float __5)
            {
                Main.logger_instance.Msg("SilkenCocoonData.DropMemoryAmberAfterDelay(position, corruption, quantityModifier, baseQuantity, dropFor, delay) Prefix");
                //Main.logger_instance.Msg("position = " + __0.ToString());
                Main.logger_instance.Msg("corruption = " + __1.ToString());
                Main.logger_instance.Msg("quantityModifier = " + __2.ToString());
                Main.logger_instance.Msg("baseQuantity = " + __3.ToString() ?? "null");
                //Main.logger_instance.Msg("dropFor = " + __4.ToString() ?? "null");
                //Main.logger_instance.Msg("delay = " + __5.ToString());
            }
        }*/

        /*[HarmonyPatch(typeof(SilkenCocoonData), "DropMemoryAmber", new System.Type[] { typeof(Vector3), typeof(int), typeof(float), typeof(Il2CppSystem.Func<Actor, bool>) })]
        public class SilkenCocoonData_DropMemoryAmber
        {
            [HarmonyPrefix]
            static void Prefix(ref UnityEngine.Vector3 __0, int __1, float __2, Il2CppSystem.Func<Actor, bool> __3)
            {
                Main.logger_instance.Msg("SilkenCocoonData.DropMemoryAmber(position, corruption, quantityModifier, dropFor) Prefix");
                //Main.logger_instance.Msg("position = " + __0.ToString());
                Main.logger_instance.Msg("corruption = " + __1.ToString());
                Main.logger_instance.Msg("quantityModifier = " + __2.ToString());
                //Main.logger_instance.Msg("dropFor = " + __3.ToString() ?? "null");

                if (!Refs_Manager.player_actor.IsNullOrDestroyed())
                {
                    Main.logger_instance.Msg("Set position to Player");
                    __0 = Refs_Manager.player_actor.position();
                }
            }
        }*/

        /*[HarmonyPatch(typeof(SilkenCocoonData), "DropMemoryAmber", new System.Type[] { typeof(UnityEngine.Vector3), typeof(int), typeof(float), typeof(Il2CppSystem.Func<int, float, uint>), typeof(Il2CppSystem.Func<Actor, bool>) })]
        public class SilkenCocoonData_DropMemoryAmber_bq
        {
            [HarmonyPrefix]
            static void Prefix(UnityEngine.Vector3 __0, int __1, float __2, Il2CppSystem.Func<int, float, uint> __3, Il2CppSystem.Func<Actor, bool> __4)
            {
                Main.logger_instance.Msg("SilkenCocoonData.DropMemoryAmber(position, corruption, quantityModifier, baseQuantity, dropFor) Prefix");
                //Main.logger_instance.Msg("position = " + __0.ToString());
                Main.logger_instance.Msg("corruption = " + __1.ToString());
                Main.logger_instance.Msg("quantityModifier = " + __2.ToString());
                Main.logger_instance.Msg("baseQuantity = " + __3.ToString() ?? "null");
                //Main.logger_instance.Msg("dropFor = " + __4.ToString() ?? "null");
            }
        }*/
    }
}
