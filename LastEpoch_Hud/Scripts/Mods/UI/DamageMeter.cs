using HarmonyLib;
using Il2Cpp;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace LastEpoch_Hud.Scripts.Mods.UI
{
    [RegisterTypeInIl2Cpp]
    public class DamageMeter : MonoBehaviour
    {
        public static DamageMeter instance { get; private set; }
        public DamageMeter(System.IntPtr ptr) : base(ptr) { }

        public static GameObject DamageMeter_prefab = null;
        public static GameObject DamageMeter_obj = null;
        public static GameObject DamageMeter_content = null;
        public static TextMeshProUGUI MenuText = null;

        public static Button OnOff_btn = null;
        public static bool On = false;
        public static GameObject Reset_obj = null;
        public static Button Reset_btn = null;

        public static Image OnOff_Image = null;
        public static Sprite On_sprite = null;
        public static Sprite Off_sprite = null;

        public static GameObject Skill_prefab = null;
        public static System.Collections.Generic.List<Skill> Skills = new System.Collections.Generic.List<Skill>();
        public static System.Collections.Generic.List<float> Damages = new System.Collections.Generic.List<float>();
        public static System.Collections.Generic.List<bool> Errors = new System.Collections.Generic.List<bool>();
        public static float TotalDamageDeal = 0f;

        void Awake()
        {
            instance = this;
            UI.Reset();
        }
        void Update()
        {
            if (!Assets.Loaded()) { Assets.Load(); }
            else
            {
                if (Scenes.IsGameScene())
                {
                    if (!UI.Initialized) { UI.Init(); }
                    else
                    {
                        if (!MenuText.IsNullOrDestroyed()) { MenuText.text = "Damage Meter"; }
                        if (!DamageMeter_obj.IsNullOrDestroyed())
                        {
                            if (Input.GetKeyDown(KeyCode.U)) { DamageMeter_obj.active = !DamageMeter_obj.active; }
                            if (!DamageMeter_obj.active) { On = false; }
                            if (On) { UI.Update(); }
                        }
                        if (!OnOff_Image.IsNullOrDestroyed())
                        {
                            if (On) { OnOff_Image.sprite = On_sprite; }
                            else { OnOff_Image.sprite = Off_sprite; }
                        }
                        if (!Reset_obj.IsNullOrDestroyed())
                        {
                            if (Skills.Count > 0) { Reset_obj.active = true; }
                            else { Reset_obj.active = false; }
                        }
                    }
                }
                else
                {
                    On = false;
                    if (!DamageMeter_obj.IsNullOrDestroyed())
                    {
                        if (DamageMeter_obj.active) { DamageMeter_obj.active = false; }
                    }
                    UI.Initialized = false;
                }
            }
        }

        public class Assets
        {
            private static bool loading = false;

            public static bool Loaded()
            {
                bool result = false;
                if ((!DamageMeter_prefab.IsNullOrDestroyed()) && (!Skill_prefab.IsNullOrDestroyed())) { result = true; }

                return result;
            }
            public static void Load()
            {
                if ((!Hud_Manager.asset_bundle.IsNullOrDestroyed()) && (!loading))
                {                    
                    loading = true;
                    foreach (string name in Hud_Manager.asset_bundle.GetAllAssetNames())
                    {
                        if (name.Contains("/damagemeter/"))
                        {
                            if (Functions.Check_Prefab(name) && name.Contains("damagemeter.prefab"))
                            {
                                DamageMeter_prefab = Hud_Manager.asset_bundle.LoadAsset(name).TryCast<GameObject>();
                            }
                            else if (Functions.Check_Prefab(name) && name.Contains("skill.prefab"))
                            {
                                Skill_prefab = Hud_Manager.asset_bundle.LoadAsset(name).TryCast<GameObject>();
                            }
                        }
                    }
                    loading = false;
                }
            }
        }
        public class UI
        {
            public static bool Initializing = false;
            public static bool Initialized = false;

            public static void Init()
            {
                if (!Initializing && !Refs_Manager.game_uibase.IsNullOrDestroyed())
                {
                    Initializing = true;
                    DamageMeter_obj = Instantiate(DamageMeter_prefab, Vector3.zero, Quaternion.identity);
                    DontDestroyOnLoad(DamageMeter_obj);
                    DamageMeter_obj.active = false;
                    DamageMeter_obj.transform.SetParent(Refs_Manager.game_uibase.transform);
                    DamageMeter_obj.AddComponent<UIMouseListener>(); //Block mouse
                    if (!DamageMeter_obj.IsNullOrDestroyed())
                    {
                        GameObject images = Functions.GetChild(DamageMeter_obj, "Images");
                        if (!images.IsNullOrDestroyed())
                        {
                            GameObject On_obj = Functions.GetChild(images, "On");
                            if (!On_obj.IsNullOrDestroyed())
                            {
                                Image On_image = On_obj.GetComponent<Image>();
                                if (!On_image.IsNullOrDestroyed()) { On_sprite = On_image.sprite; }                                
                            }
                            GameObject Off_obj = Functions.GetChild(images, "Off");
                            if (!Off_obj.IsNullOrDestroyed())
                            {
                                Image Off_image = Off_obj.GetComponent<Image>();
                                if (!Off_image.IsNullOrDestroyed()) { Off_sprite = Off_image.sprite; }
                            }
                        }
                        GameObject panel = Functions.GetChild(DamageMeter_obj, "Panel");
                        if (!panel.IsNullOrDestroyed())
                        {
                            DamageMeter_content = Functions.GetChild(panel, "Content");
                            GameObject title = Functions.GetChild(panel, "Title");
                            if (!title.IsNullOrDestroyed())
                            {
                                GameObject on_off = Functions.GetChild(title, "OnOff");
                                if (!on_off.IsNullOrDestroyed())
                                {
                                    OnOff_btn = on_off.GetComponent<Button>();
                                    if (!OnOff_btn.IsNullOrDestroyed()) { Events.Set(OnOff_btn, Events.OnOff_OnClick_Action); }
                                    GameObject image = Functions.GetChild(on_off, "Image");
                                    if (!image.IsNullOrDestroyed())
                                    {
                                        OnOff_Image = image.GetComponent<Image>();
                                        if (!OnOff_Image.IsNullOrDestroyed()) { OnOff_Image.sprite = Off_sprite; }
                                    }
                                }

                                Reset_obj = Functions.GetChild(title, "Reset");
                                if (!Reset_obj.IsNullOrDestroyed())
                                {
                                    Reset_btn = Reset_obj.GetComponent<Button>();
                                    if (!Reset_btn.IsNullOrDestroyed()) { Events.Set(Reset_btn, Events.Reset_OnClick_Action); }
                                }
                            }
                        }                        
                    }
                    //menu
                    if (!Refs_Manager.game_uibase.IsNullOrDestroyed())
                    {
                        GameObject go = Refs_Manager.game_uibase.bottomScreenMenu.gameObject; //BottomScreenMenu
                        if (!go.IsNullOrDestroyed())
                        {
                            GameObject panel = Functions.GetChild(go, "BottomScreenMenuPanel");
                            if (!panel.IsNullOrDestroyed())
                            {
                                BottomScreenMenu bottomScreenMenu = panel.GetComponent<BottomScreenMenu>();
                                if (!bottomScreenMenu.IsNullOrDestroyed())
                                {
                                    bottomScreenMenu.buttonsToDisableInOnline = new Il2CppSystem.Collections.Generic.List<Button>();
                                }

                                GameObject shop = Functions.GetChild(panel, "Shop");
                                if (!shop.IsNullOrDestroyed())
                                {
                                    Button shop_btn = shop.GetComponent<Button>();
                                    Events.Set(shop_btn, Events.ToggleVisibility_OnClick_Action);
                                    shop_btn.interactable = true;

                                    CanvasGroup canvasGroup = shop.GetComponent<CanvasGroup>();
                                    if(!canvasGroup.IsNullOrDestroyed()) { Object.Destroy(canvasGroup); }

                                    GameObject text_obj = Functions.GetChild(shop, "TextMeshPro Text");
                                    if (!text_obj.IsNullOrDestroyed())
                                    {
                                        MenuText = text_obj.GetComponent<TextMeshProUGUI>();                                        
                                    }
                                }
                            }
                        }
                    }
                    UI.Reset();
                    Initialized = true;
                    Initializing = false;
                }
            }                        
            public static void AddSkill(string obj_name, string ability_name, float damage)
            {
                if (!Skill_prefab.IsNullOrDestroyed())
                {
                    GameObject skill_obj = Instantiate(Skill_prefab, Vector3.zero, Quaternion.identity);
                    skill_obj.active = false;
                    skill_obj.transform.SetParent(DamageMeter_content.transform);
                    Skills.Add(new Skill { ObjName = obj_name, AbilityName = ability_name, Obj = skill_obj });
                    Damages.Add(damage);
                    Errors.Add(false);
                }
            }
            public static void Update()
            {
                int i = 0;
                foreach (Skill skill in Skills)
                {
                    if (!skill.Obj.IsNullOrDestroyed())
                    {
                        GameObject infos = Functions.GetChild(skill.Obj, "Infos");
                        if ((!skill.Obj.active) && (!Errors[i]))
                        {
                            if ((!infos.IsNullOrDestroyed()))
                            {
                                GameObject icon_obj = Functions.GetChild(infos, "Icon");
                                if ((!icon_obj.IsNullOrDestroyed()))
                                {
                                    GameObject image_obj = Functions.GetChild(icon_obj, "Image");
                                    if ((!image_obj.IsNullOrDestroyed()))
                                    {
                                        Image icon = image_obj.GetComponent<Image>();
                                        if (!icon.IsNullOrDestroyed())
                                        {
                                            icon.sprite = GetIcon(skill.ObjName, skill.AbilityName);
                                            if (icon.sprite.IsNullOrDestroyed()) { Errors[i] = true; }
                                        }                                        
                                    }
                                }
                                GameObject skill_name_obj = Functions.GetChild(infos, "SkillName");
                                if ((!skill_name_obj.IsNullOrDestroyed()))
                                {
                                    GameObject text_obj = Functions.GetChild(skill_name_obj, "Text");
                                    if ((!text_obj.IsNullOrDestroyed()))
                                    {
                                        Text text = text_obj.GetComponent<Text>();
                                        if (!text.IsNullOrDestroyed()) { text.text = skill.AbilityName; }
                                    }
                                }
                            }
                            if (!Errors[i]) { skill.Obj.active = true; }
                            else
                            {
                                TotalDamageDeal -= Damages[i];
                                Damages[i] = 0f;
                            }
                        }
                        if (skill.Obj.active)
                        {
                            float damage = Damages[i];
                            if ((!infos.IsNullOrDestroyed()))
                            {
                                GameObject percent_obj = Functions.GetChild(infos, "Percent");
                                if ((!percent_obj.IsNullOrDestroyed()))
                                {
                                    GameObject text_obj = Functions.GetChild(percent_obj, "Text");
                                    if ((!text_obj.IsNullOrDestroyed()))
                                    {
                                        Text text = text_obj.GetComponent<Text>();
                                        decimal damage_percent_decimal = (decimal)((damage * 100) / TotalDamageDeal);
                                        if (!text.IsNullOrDestroyed()) { text.text = damage_percent_decimal.ToString("0.0") + " %"; }
                                    }
                                }
                            }
                            GameObject skill_bar_obj = Functions.GetChild(skill.Obj, "SkillBar");
                            if (!skill_bar_obj.IsNullOrDestroyed())
                            {
                                Slider slider = skill_bar_obj.GetComponent<Slider>();
                                if (!slider.IsNullOrDestroyed()) { slider.value = damage * 100 / TotalDamageDeal; }
                            }
                        }
                    }
                    i++;
                }
            }
            public static Sprite GetIcon(string obj_name, string ability_name)
            {
                Sprite result = null;
                try
                {
                    foreach (Ability ab in Resources.FindObjectsOfTypeAll<Ability>())
                    {
                        if (ability_name.Contains("Herald")) //Herald
                        {
                            if (ab.abilityName == ability_name)
                            {
                                if (!ab.abilitySprite.IsNullOrDestroyed()) { result = ab.abilitySprite; }
                                break;
                            }
                        }
                        else //Abilities
                        {
                            if ((ab.name == obj_name) && (ab.abilityName == ability_name))
                            {
                                if (!ab.abilitySprite.IsNullOrDestroyed()) { result = ab.abilitySprite; }
                                break;
                            }
                        }
                    }
                    if (result == null) 
                    {
                        foreach (Ability ab in NewItems.Items_Mjolner.Trigger.Abilities) //Mjolner
                        {
                            if (ab.abilityName == ability_name)
                            {
                                result = NewItems.Items_Mjolner.Icon.sprite;
                                break;
                            }
                        }
                    }
                    if (result == null)
                    {
                        foreach (Summoned summoned in Refs_Manager.summon_tracker.summons) //Summon
                        {
                            AbilityList abilitylist = summoned.gameObject.GetComponent<AbilityList>();
                            if (!abilitylist.IsNullOrDestroyed())
                            {
                                foreach (Ability ability in abilitylist.abilities)
                                {
                                    if (ability.name == obj_name)
                                    {
                                        CreationReferences creationReferences = summoned.gameObject.GetComponent<CreationReferences>();
                                        if (!creationReferences.IsNullOrDestroyed())
                                        {
                                            if (!creationReferences.thisAbility.IsNullOrDestroyed())
                                            {
                                                result = creationReferences.thisAbility.abilitySprite;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }

                return result;
            }
            public static void Reset()
            {
                Skills = new System.Collections.Generic.List<Skill>();
                Damages = new System.Collections.Generic.List<float>();
                Errors = new System.Collections.Generic.List<bool>();
                TotalDamageDeal = 0f;
                if (!DamageMeter_content.IsNullOrDestroyed())
                {
                    foreach (GameObject go in Functions.GetAllChild(DamageMeter_content)) { Object.Destroy(go); }
                }
            }
        }
        public class Events
        {
            public static void Set(Button btn, UnityEngine.Events.UnityAction action)
            {
                if (!btn.IsNullOrDestroyed())
                {
                    btn.onClick = new Button.ButtonClickedEvent();
                    btn.onClick.AddListener(action);
                }
            }

            public static readonly System.Action ToggleVisibility_OnClick_Action = new System.Action(ToggleVisibility_Click);
            public static void ToggleVisibility_Click()
            {
                DamageMeter_obj.active = !DamageMeter_obj.active;
            }

            public static readonly System.Action OnOff_OnClick_Action = new System.Action(OnOff_Click);
            public static void OnOff_Click()
            {
                On = !On;                
            }

            public static readonly System.Action Reset_OnClick_Action = new System.Action(Reset_Click);
            public static void Reset_Click()
            {
                UI.Reset();                
            }
        }
        public class DamageDeal
        {
            public static Actor current_target = null;
            public static float current_health = 0;
            public static string current_obj_name = "";
            public static string current_ability_name = "";

            [HarmonyPatch(typeof(DamageStatsHolder), "applyDamage", new System.Type[] { typeof(Actor) })]
            public class DamageStatsHolder_applyDamage
            {
                [HarmonyPrefix]
                static void Prefix(ref DamageStatsHolder __instance, ref Actor __0)
                {
                    if ((Scenes.IsGameScene()) && (On))
                    {
                        current_target = null;
                        current_obj_name = "";
                        current_ability_name = "";
                        current_health = 0;

                        bool target_is_player = false;
                        if (__0 == Refs_Manager.player_actor) { target_is_player = true; }
                        bool target_is_summon = false;
                        foreach (Summoned summoned in Refs_Manager.summon_tracker.summons)
                        {
                            if (summoned.actor.name == __0.name) { target_is_summon = true; break; }
                        }
                        if ((!target_is_player) && (!target_is_summon))
                        {
                            if (!__0.gameObject.GetComponent<UnitHealth>().IsNullOrDestroyed())
                            {
                                float health = __0.gameObject.GetComponent<UnitHealth>().currentHealth;
                                if (health > 0)
                                {                                    
                                    current_target = __0;
                                    current_obj_name = __instance.name.Substring(0, __instance.name.Length - 7);
                                    current_ability_name = __instance.getAbilityName();
                                    current_health = health;
                                }
                            }
                        }
                    }
                }

                [HarmonyPostfix]
                static void Postfix(ref DamageStatsHolder __instance, ref Actor __0)
                {
                    if ((Scenes.IsGameScene()) && (On))
                    {
                        string obj_name = __instance.name.Substring(0, __instance.name.Length - 7);
                        string ability_name = __instance.getAbilityName();
                        if ((current_target == __0) && (current_obj_name == obj_name) && (current_ability_name == ability_name))
                        {
                            if (!__0.gameObject.GetComponent<UnitHealth>().IsNullOrDestroyed())
                            {
                                float health = __0.gameObject.GetComponent<UnitHealth>().currentHealth;
                                float health_diff = (current_health - health);
                                
                                bool found = false;
                                int index = 0;
                                foreach (Skill skill in Skills)
                                {
                                    if ((skill.ObjName == obj_name) && (skill.AbilityName == ability_name))
                                    {
                                        found = true;
                                        break;
                                    }
                                    index++;
                                }
                                if (found)
                                {
                                    if (!Errors[index])
                                    {
                                        TotalDamageDeal += health_diff;
                                        float old_damage = Damages[index];
                                        Damages[index] = old_damage + health_diff;
                                    }
                                }
                                else if (health_diff > 0f)
                                {
                                    TotalDamageDeal += health_diff;
                                    UI.AddSkill(obj_name, ability_name, health_diff);
                                }
                            }
                        }
                    }
                }
            }
        }
        public struct Skill
        {
            public string ObjName;
            public string AbilityName;
            public GameObject Obj;
        }
    }
}
