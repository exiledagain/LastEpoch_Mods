using HarmonyLib;
using Il2CppLE.Factions;

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

        [HarmonyPatch(typeof(Il2CppLE.Factions.FactionRankPanel), "OnEnable")]
        public class FactionRankPanel_OnEnable
        {
            [HarmonyPrefix]
            static void Prefix(ref Il2CppLE.Factions.FactionRankPanel __instance)
            {
                if (CanRun())
                {
                    try
                    {
                        FactionRankPanelWeaver fcpw = __instance.TryCast<FactionRankPanelWeaver>();
                        if (!fcpw.IsNullOrDestroyed())
                        {
                            fcpw.weaverTree.unspentPoints = Save_Manager.instance.data.Factions.TheWoven.TreePoints;
                        }
                    }
                    catch { }
                }
            }
        }
    }
}
