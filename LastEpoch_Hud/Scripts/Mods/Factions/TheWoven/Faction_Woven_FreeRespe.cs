using HarmonyLib;

namespace LastEpoch_Hud.Scripts.Mods.Factions.TheWoven
{
    public class Faction_Woven_FreeRespe
    {
        public static bool CanRun()
        {
            if ((Scenes.IsGameScene()) && (!Save_Manager.instance.IsNullOrDestroyed()))
            {
                return Save_Manager.instance.data.Factions.TheWoven.Enable_FreeRespe;
            }
            else { return false; }
        }

        [HarmonyPatch(typeof(Il2CppLE.Factions.TheWeaver), "GetMemoryAmberRespecCostForWeaverTree")]
        public class TheWeaver_GetMemoryAmberRespecCostForWeaverTree
        {
            [HarmonyPrefix]
            static bool Prefix(ref int __result)
            {
                bool r = true;
                if (CanRun())
                {
                    __result = 0;
                    r = false;
                }

                return r;
            }
        }
    }
}
