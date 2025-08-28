using HarmonyLib;
using Il2Cpp;
using Il2CppLE.Services.Cosmetics;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Cosmetics
{
    public class Cosmetics_Offline
    {
        public static Il2CppSystem.Collections.Generic.List<string> list_id = null; //List of all cosmetics ids

        [HarmonyPatch(typeof(CosmeticsManager), "GetOwnedCosmetics")]
        public class CosmeticsManager_GetOwnedCosmetics
        {
            [HarmonyPrefix]
            static bool Prefix(ref CosmeticsManager __instance, ref Il2CppCysharp.Threading.Tasks.UniTask<Il2CppSystem.Collections.Generic.List<string>> __result)
            {
                if (list_id.IsNullOrDestroyed())
                {
                    System.Collections.Generic.List<Cosmetic> cosmetics = new System.Collections.Generic.List<Cosmetic>();
                    foreach (Cosmetic cosmetic in Resources.FindObjectsOfTypeAll<Cosmetic>()) { cosmetics.Add(cosmetic); }
                    if (!cosmetics.IsNullOrDestroyed())
                    {
                        list_id = new Il2CppSystem.Collections.Generic.List<string>();
                        foreach (Cosmetic cosmetic in cosmetics)
                        {
                            if (!list_id.Contains(cosmetic.BackendID)) { list_id.Add(cosmetic.BackendID); }
                        }
                    }
                }
                
                __result = new Il2CppCysharp.Threading.Tasks.UniTask<Il2CppSystem.Collections.Generic.List<string>>(list_id);

                return false;
            }
        }
    }
}
