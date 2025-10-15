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
        public static Cosmetics_Offline instance { get; private set; }
        public Cosmetics_Offline(System.IntPtr ptr) : base(ptr) { }

        public static bool Initialized = false;
        public static Il2CppSystem.Collections.Generic.List<string> list_id = null; //List of all cosmetics ids

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if (Scenes.IsGameScene())
            {
                if (!Initialized) { Init(); }
            }
            else { Initialized = false; }
        }
        void Init()
        {
            if (!Refs_Manager.game_uibase.IsNullOrDestroyed())
            {
                GameObject go = Refs_Manager.game_uibase.bottomScreenMenu.gameObject;
                if (!go.IsNullOrDestroyed())
                {
                    GameObject panel = Functions.GetChild(go, "BottomScreenMenuPanel");
                    if (!panel.IsNullOrDestroyed())
                    {
                        GameObject cosmetics = Functions.GetChild(panel, "Cosmetics");
                        if (!cosmetics.IsNullOrDestroyed())
                        {
                            cosmetics.active = false;
                            Initialized = true;
                        }
                    }
                }
            }
        }

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
