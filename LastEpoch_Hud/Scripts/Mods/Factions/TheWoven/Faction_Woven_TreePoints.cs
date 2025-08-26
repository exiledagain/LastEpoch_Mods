using HarmonyLib;
using Il2Cpp;

namespace LastEpoch_Hud.Scripts.Mods.Factions.TheWoven
{
    public class Faction_Woven_TreePoints
    {
        public static bool CanRun()
        {
            if ((Scenes.IsGameScene()) && (!Save_Manager.instance.IsNullOrDestroyed()))
            {
                return Save_Manager.instance.data.Factions.TheWoven.Enable_TreePoints;
            }
            else { return false; }
        }

        [HarmonyPatch(typeof(LocalTreeData.WeaverTreeData), "getUnspentPoints")]
        public class WeaverTreeData_getUnspentPoints
        {
            [HarmonyPrefix]
            static void Prefix(ref LocalTreeData.WeaverTreeData __instance)
            {
                if (CanRun())
                {
                    __instance.EarnedWeaverPoints = (ushort)Save_Manager.instance.data.Factions.TheWoven.TreePoints;
                }
            }
        }
    }
}
