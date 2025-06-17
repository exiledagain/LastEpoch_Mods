// https://github.com/RolandSolymosi/LastEpoch_Mods/blob/master/LastEpoch_Hud/Scripts/Mods/Items/Items_Crafting_Eternity_Anywhere.cs

using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Items
{
    [RegisterTypeInIl2Cpp]
    public class Items_Crafting_Eternity_Anywhere : MonoBehaviour
    {
        public static Items_Crafting_Eternity_Anywhere instance { get; private set; }
        public Items_Crafting_Eternity_Anywhere(System.IntPtr ptr) : base(ptr) { }

        private static bool Enable = false; //Use to disable the whole mod

        private static bool IsOpen = false;
        private static bool IsFuture = false;
        private static bool IsOpenByMod = false;

        void Awake()
        {
            instance = this;
            instance.enabled = Enable;
        }
        void Update()
        {
            if ((Scenes.IsGameScene()) && (instance.enabled))
            {                
                if ((!Refs_Manager.EternityCachePanelUI.IsNullOrDestroyed()) && (IsOpenByMod))
                {
                    if (Refs_Manager.EternityCachePanelUI.pastHolder.active) { IsFuture = false; }
                    else if (Refs_Manager.EternityCachePanelUI.futureHolder.active) { IsFuture = true; }
                }
                if (Input.GetKeyDown(Save_Manager.instance.data.KeyBinds.EternityCache_Past)) { OpenClose(false); }
                //else if (Input.GetKeyDown(Save_Manager.instance.data.KeyBinds.EternityCache_Future)) { OpenClose(true); }
            }
        }

        void OpenClose(bool future)
        {
            if (!Refs_Manager.EternityCachePanelUI.IsNullOrDestroyed())
            {
                //We don't want to hit twice for switching between past and future
                if ((IsOpen) && (IsFuture != future)) { Refs_Manager.EternityCachePanelUI.Close(); } 
                
                if (!IsOpen)
                {
                    Refs_Manager.EternityCachePanelUI.Open();
                    Refs_Manager.EternityCachePanelUI.isFuture = future;
                    Refs_Manager.EternityCachePanelUI.pastHolder.active = !future;                    
                    Refs_Manager.EternityCachePanelUI.futureHolder.active = future;
                    Refs_Manager.EternityCachePanelUI.lockedPastHolder.active = false;
                    Refs_Manager.EternityCachePanelUI.lockedFutureHolder.active = false;
                    IsOpenByMod = true;
                }
                else { Refs_Manager.EternityCachePanelUI.Close(); }
                Refs_Manager.EternityCachePanelUI.gameObject.active = IsOpen;
            }
        }

        [HarmonyPatch(typeof(EternityCachePanelUI), "Open", new System.Type[] { })]
        public class EternityCachePanelUI_Open
        {
            [HarmonyPostfix]
            static void Postfix(ref EternityCachePanelUI __instance)
            {
                IsOpen = true;
            }
        }

        /*[HarmonyPatch(typeof(EternityCachePanelUI), "Open", new System.Type[] { typeof(bool) })]
        public class EternityCachePanelUI_Open2
        {
            [HarmonyPrefix]
            static void Prefix(ref bool __0)
            {
                Main.logger_instance?.Msg("EternityCachePanelUI.Open(bool) Prefix");
                __0 = true; //isFuture
            }
            [HarmonyPostfix]
            static void Postfix(ref EternityCachePanelUI __instance)
            {
                Main.logger_instance?.Msg("EternityCachePanelUI.Open(bool) Postfix");
                IsOpen = true;
            }
        }*/

        [HarmonyPatch(typeof(EternityCachePanelUI), "Close")]
        public class EternityCachePanelUI_Close
        {
            [HarmonyPostfix]
            static void Postfix(ref EternityCachePanelUI __instance)
            {
                IsOpen = false;
                IsOpenByMod = false;
            }
        }

        //We don't need this for Past
        [HarmonyPatch(typeof(EternityCachePanelUI), "updateSelectedAffixLabelText")]
        public class EternityCachePanelUI_updateSelectedAffixLabelText
        {
            [HarmonyPrefix]
            static bool Prefix(ref EternityCachePanelUI __instance, ref ItemData __0)
            {
                bool r = true;
                if (instance.enabled)
                {
                    if (IsOpenByMod) { r = false; }
                }
                return r;
            }
        }

        [HarmonyPatch(typeof(EternityCachePanelUI), "seal")]
        public class EternityCachePanelUI_seal
        {
            [HarmonyPrefix]
            static bool Prefix(ref EternityCachePanelUI __instance)
            {
                bool result = true;
                if ((instance.enabled) && (IsOpenByMod) && (!__instance.beforeMain.IsNullOrDestroyed()) && (!__instance.beforeOther.IsNullOrDestroyed()))
                {
                    if (!__instance.beforeMain.Container.IsNullOrDestroyed() && !__instance.beforeOther.Container.IsNullOrDestroyed())
                    {
                        if (__instance.beforeMain.Container.GetContent().Count > 0 && __instance.beforeOther.Container.GetContent().Count > 0)
                        {
                            ItemData unique = __instance.beforeMain.Container.GetContent()[0].data;
                            ItemData exalted = __instance.beforeOther.Container.GetContent()[0].data;
                            if ((!unique.IsNullOrDestroyed()) && (!exalted.IsNullOrDestroyed()))
                            {
                                if (IsFuture) //Future
                                {
                                    
                                }
                                else //Past
                                {
                                    //Add affixes into unique
                                    unique.affixes = exalted.affixes;
                                    unique.rarity = 9;
                                    unique.RefreshIDAndValues();

                                    //Delete item from exalted slot ?
                                    //__instance.beforeOther.Container.GetContent().Clear();

                                    result = false;
                                }
                            }
                        }
                    }
                }
                return result;
            }
        }
    }
}
