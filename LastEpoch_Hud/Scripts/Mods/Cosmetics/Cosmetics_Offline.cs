using HarmonyLib;
using Il2Cpp;
using Il2CppLE.Services.Cosmetics;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Cosmetics
{
    [RegisterTypeInIl2Cpp]
    public class Cosmetics_Offline : MonoBehaviour
    {
        public Cosmetics_Offline(System.IntPtr ptr) : base(ptr) { }
        public static Cosmetics_Offline instance { get; private set; }

        public static bool updating = false;

        public static System.Collections.Generic.List<Cosmetic> cosmetics = null; //List of all cosmetics

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if (!updating)
            {
                updating = true;
                if (cosmetics.IsNullOrDestroyed())
                {
                    cosmetics = new System.Collections.Generic.List<Cosmetic>();
                    foreach (Cosmetic cosmetic in Resources.FindObjectsOfTypeAll<Cosmetic>()) { cosmetics.Add(cosmetic); }
                }
                updating = false;
            }
        }

        [HarmonyPatch(typeof(CosmeticsManager), "GetOwnedCosmetics")]
        public class CosmeticsManager_GetOwnedCosmetics
        {
            [HarmonyPostfix]
            static void Postfix(ref CosmeticsManager __instance, ref Il2CppCysharp.Threading.Tasks.UniTask<Il2CppSystem.Collections.Generic.List<string>> __result)
            {
                if (!Refs_Manager.player_actor.IsNullOrDestroyed()) { __instance.player = Refs_Manager.player_actor.gameObject; }
                Il2CppSystem.Collections.Generic.List<string> list = new Il2CppSystem.Collections.Generic.List<string>();
                foreach (Cosmetic cosmetic in cosmetics) { list.Add(cosmetic.BackendID); }
                __result = new Il2CppCysharp.Threading.Tasks.UniTask<Il2CppSystem.Collections.Generic.List<string>>(list);
            }
        }
    }
}
