using HarmonyLib;
using Il2Cpp;

namespace LastEpoch_Hud.Scripts.Mods.Mobs
{
    public class Mobs_Density
    {
        public static bool CanRun()
        {
            if ((Scenes.IsGameScene()) && (!Save_Manager.instance.IsNullOrDestroyed()))
            {
                if (!Save_Manager.instance.data.IsNullOrDestroyed())
                {
                    return Save_Manager.instance.data.Character.Cheats.Enable_DensityMultiplier;
                }
                else { return false; }
            }
            else { return false; }
        }
        
        [HarmonyPatch(typeof(SpawnerPlacementManager), "RollSpawners", new System.Type[] { typeof(SpawnerPlacementRoom.SpawnerRuntimeConfig) })] //LastEpocj 1.3.2
        public class SpawnerPlacementManager_RollSpawners
        {
            [HarmonyPrefix]
            public static void Prefix(ref SpawnerPlacementManager __instance)
            {
                Main.logger_instance.Msg("SpawnerPlacementManager.RollSpawners(SpawnerPlacementRoom.SpawnerRuntimeConfig);");
                if (CanRun())
                {
                    __instance.defaultSpawnerDensity = Save_Manager.instance.data.Character.Cheats.DensityMultiplier;
                    __instance.alwaysRollSpawnerDensity = false;
                }
            }
        }
    }
}
