using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Chat
{
    [RegisterTypeInIl2Cpp]
    public class Chat_Remove : MonoBehaviour
    {
        public static Chat_Remove instance { get; private set; }
        public Chat_Remove(System.IntPtr ptr) : base(ptr) { }

        public static bool Initialized = false;
        public static GameObject Chat_obj = null;

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if (Scenes.IsGameScene())
            {
                if (!Initialized) { Init(); }
                else if (!Chat_obj.IsNullOrDestroyed())
                {
                    if (Chat_obj.active) { Chat_obj.active = false; }
                }
            }
            else { Initialized = false; }
        }
        void Init()
        {
            if (!Refs_Manager.game_uibase.IsNullOrDestroyed())
            {
                Chat_obj = Functions.GetChild(Refs_Manager.game_uibase.gameObject, "Canvas (chat)");
                Initialized = true;
            }            
        }

        [HarmonyPatch(typeof(UIBase), "ChatKeyDown")]
        public class UIBase_ChatKeyDown
        {
            [HarmonyPrefix]
            static bool Prefix()
            {
                return false;
            }
        }
    }
}
