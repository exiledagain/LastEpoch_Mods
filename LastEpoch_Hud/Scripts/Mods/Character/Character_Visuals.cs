using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Character
{
    [RegisterTypeInIl2Cpp]
    public class Character_Visuals : MonoBehaviour
    {
        public static Character_Visuals instance { get; private set; }
        public Character_Visuals(System.IntPtr ptr) : base(ptr) { }

        //Primalist = 0, Mage = 1, Sentinel = 2, Acolyte = 3, Rogue = 4        
        public static int wanted_class = -1;
        public static int backup_class = -1;

        /*public static bool Initializing = false;
        public static bool Initialized = false;
        public static EquipmentVisualsManager equipment_manager = null;
        public static System.Collections.Generic.List<SkinnedMeshRenderer> primalist_renderer = null;
        public static System.Collections.Generic.List<SkinnedMeshRenderer> mage_renderer = null;
        public static System.Collections.Generic.List<SkinnedMeshRenderer> sentinel_renderer = null;
        public static System.Collections.Generic.List<SkinnedMeshRenderer> acolyte_renderer = null;
        public static System.Collections.Generic.List<SkinnedMeshRenderer> rogue_renderer = null;
        public static System.Collections.Generic.List<SkinnedMeshRenderer>[] renderers = null;
        public static readonly string[] CharacterClass = { "Primalist", "Mage", "Sentinel", "Acolyte", "Rogue" };*/

        void Awake()
        {
            instance = this;
        }
        /*void Update()
        {
            if ((!Initialized) && (!Initializing)) { Init(); }
        }*/

        /*public static void Init()
        {
            Initializing = true;
            primalist_renderer = new System.Collections.Generic.List<SkinnedMeshRenderer>();
            mage_renderer = new System.Collections.Generic.List<SkinnedMeshRenderer>();
            sentinel_renderer = new System.Collections.Generic.List<SkinnedMeshRenderer>();
            acolyte_renderer = new System.Collections.Generic.List<SkinnedMeshRenderer>();
            rogue_renderer = new System.Collections.Generic.List<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer skin in Resources.FindObjectsOfTypeAll<SkinnedMeshRenderer>())
            {
                if (skin.name.Contains(CharacterClass[0])) { primalist_renderer.Add(skin); }
                else if (skin.name.Contains(CharacterClass[1])) { mage_renderer.Add(skin); }
                else if (skin.name.Contains(CharacterClass[2])) { sentinel_renderer.Add(skin); }
                else if (skin.name.Contains(CharacterClass[3])) { acolyte_renderer.Add(skin); }
                else if (skin.name.Contains(CharacterClass[4])) { rogue_renderer.Add(skin); }
            }
            renderers = new System.Collections.Generic.List<SkinnedMeshRenderer>[5];
            renderers[0] = primalist_renderer;
            renderers[1] = mage_renderer;
            renderers[2] = sentinel_renderer;
            renderers[3] = acolyte_renderer;
            renderers[4] = rogue_renderer;

            Initialized = true;
            Initializing = false;
        }*/        
        public static void Set_WantedClass(string name)
        {            
            if (name == "Primalist") { wanted_class = 0; }
            else if (name == "Mage") { wanted_class = 1; }
            else if (name == "Sentinel") { wanted_class = 2; }
            else if (name == "Acolyte") { wanted_class = 3; }
            else if (name == "Rogue") { wanted_class = 4; }
            else { wanted_class = -1; }
        }

        [HarmonyPatch(typeof(ActorVisuals), "CreateClassVisuals")]
        public class ActorVisuals_CreateClassVisuals
        {
            [HarmonyPrefix]
            static void Prefix(ActorVisuals __instance, ref int __0)
            {
                if (!Save_Manager.instance.data.IsNullOrDestroyed())
                {
                    string str = "";
                    if (__0 == 0) { str = Save_Manager.instance.data.Character.Visuals.Primalist; }
                    else if (__0 == 1) { str = Save_Manager.instance.data.Character.Visuals.Mage; }
                    else if (__0 == 2) { str = Save_Manager.instance.data.Character.Visuals.Sentinel; }
                    else if (__0 == 3) { str = Save_Manager.instance.data.Character.Visuals.Acolyte; }
                    else if (__0 == 4) { str = Save_Manager.instance.data.Character.Visuals.Rogue; }
                    Set_WantedClass(str);
                    if ((wanted_class > -1) && (wanted_class < 5)) { __0 = wanted_class; }
                }              
            }
        }

        [HarmonyPatch(typeof(EquipmentVisualsManager), "EquipGearAsync")]
        public class EquipmentVisualsManager_EquipGearAsync
        {
            [HarmonyPrefix]
            static void Prefix(EquipmentVisualsManager __instance, Il2CppCysharp.Threading.Tasks.UniTask __result, EquipmentType __0, int __1, bool __2, ushort __3)
            {
                if ((wanted_class > -1) && (!Refs_Manager.player_data.IsNullOrDestroyed()))
                {
                    backup_class = Refs_Manager.player_data.CharacterClass;
                    Refs_Manager.player_data.CharacterClass = wanted_class;
                }
            }
            [HarmonyPostfix]
            static void Postifx(EquipmentVisualsManager __instance, Il2CppCysharp.Threading.Tasks.UniTask __result, EquipmentType __0, int __1, bool __2, ushort __3)
            {
                if ((wanted_class > -1) && (!Refs_Manager.player_data.IsNullOrDestroyed()))
                {
                    Refs_Manager.player_data.CharacterClass = backup_class;
                }

                /*if ((__0 == EquipmentType.BODY_ARMOR) && (!equipment_manager.IsNullOrDestroyed()))
                {
                    RendererManager renderer_manager = equipment_manager.gameObject.GetComponent<RendererManager>();
                    if (!renderer_manager.IsNullOrDestroyed())
                    {
                        foreach (Renderer renderer in renderer_manager.renderers)
                        {
                            if ((renderer.name.Contains(CharacterClass[backup_class])) && ((renderer.name.Contains("Chest")) || (renderer.name.Contains("Cloth"))))
                            {
                                Object.Destroy(renderer);
                            }
                        }

                        foreach (SkinnedMeshRenderer skin in renderers[wanted_class])
                        {
                            if (!skin.IsNullOrDestroyed())
                            {
                                if ((skin.name.Contains("Chest")) || (skin.name.Contains("Cloth")))
                                {
                                    renderer_manager.renderers.Add(skin);
                                }
                            }
                        }
                    }
                }*/
            }
        }
    }
}
