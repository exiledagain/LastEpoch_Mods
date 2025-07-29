using HarmonyLib;
using Il2Cpp;
using Il2CppLE.Data;
using Il2CppTMPro;
using MelonLoader;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LastEpoch_Hud.Scripts.Mods.Cosmetics
{
    [RegisterTypeInIl2Cpp]
    public class Skins : MonoBehaviour
    {
        public static Skins instance { get; private set; }
        public Skins(System.IntPtr ptr) : base(ptr) { }

        private string scene_name = "";

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if (Scenes.IsGameScene())
            {
                if (scene_name != Scenes.SceneName) //scene changed
                {
                    scene_name = Scenes.SceneName;
                    Visuals.NeedUpdate = true;
                    Panel.Slots.need_update = true;
                    Panel.initialized = false;
                }
                if (Panel.initialized)
                {
                    if (Panel.Slots.need_update)
                    {
                        //Panel.Slots.need_update = false;
                        Panel.Slots.Update();
                    }
                }
                if (!Refs_Manager.player_visuals.IsNullOrDestroyed())
                {
                    if (Visuals.NeedUpdate)
                    {
                        Visuals.Update();
                    }
                }
            }
            else
            {
                scene_name = "";
                if (!Refs_Manager.game_uibase.IsNullOrDestroyed())
                {
                    if (Refs_Manager.game_uibase.characterSelectOpen)
                    {
                        if (!Refs_Manager.game_uibase.characterSelectPanel.IsNullOrDestroyed())
                        {
                            GameObject char_selection_game_object = Refs_Manager.game_uibase.characterSelectPanel.instance;
                            if (!char_selection_game_object.IsNullOrDestroyed())
                            {
                                CharacterSelect char_select = char_selection_game_object.GetComponent<CharacterSelect>();
                                if (!char_select.IsNullOrDestroyed())
                                {
                                    if (char_select.currentState == CharacterSelect.CharacterSelectState.LoadCharacter)
                                    {
                                        int index = char_select.SelectedCharacterIndex;
                                        if (index > -1)
                                        {
                                            if (index != Save.Data.Character.Character_Index)
                                            {
                                                LocalCharacterSlots local_slots = char_selection_game_object.GetComponent<LocalCharacterSlots>();
                                                if (!local_slots.IsNullOrDestroyed())
                                                {
                                                    if (index < local_slots.characterSlots.Count)
                                                    {
                                                        Save.Data.Character.Character_Index = index;
                                                        Save.Data.Character.Character_Cycle = local_slots.characterSlots[index].Cycle;
                                                        Save.Data.Character.Character_Name = local_slots.characterSlots[index].CharacterName;
                                                        Save.Data.Character.Character_Class = local_slots.characterSlots[index].GetCharacterClass().className;
                                                        Save.Data.Character.Character_Class_Id = local_slots.characterSlots[index].CharacterClass;
                                                        Save.Data.path = Save.Data.base_path + Save.Data.Character.Character_Cycle + @"\";
                                                        Save.Data.Load();
                                                        Visuals.NeedUpdate = true;
                                                    }
                                                    else { Main.logger_instance?.Error("Skins : Error Index (" + index + ") > Character count (" + local_slots.characterSlots.Count + ")"); }
                                                }
                                                else { Main.logger_instance?.Error("Skins : Error local_slots is null"); }
                                            }
                                        }
                                        else { Main.logger_instance?.Error("Skins : Index = " + index); }

                                        if (Visuals.NeedUpdate) { Visuals.Update(); }
                                    }
                                }
                                else { Main.logger_instance?.Error("Skins : Error char_select is null"); }
                            }
                            else { Main.logger_instance?.Error("Skins : Error char_selection_game_object is null"); }
                        }
                        else { Main.logger_instance?.Error("Skins : Error characterSelectPanel is null"); }
                    }
                }
                else { Main.logger_instance?.Error("Skins : Error game_uibase is null"); }
            }            
        }

        public class Tabs
        {
            public static TabUIElement tab_element = null;

            [HarmonyPatch(typeof(InventoryPanelUI), "OnEnable")]
            public class InventoryPanelUI_OnEnable
            {
                [HarmonyPostfix]
                static void Postfix(ref InventoryPanelUI __instance)
                {
                    if (!Refs_Manager.InventoryPanelUI.IsNullOrDestroyed())
                    {
                        if (tab_element.IsNullOrDestroyed())
                        {
                            if (!Refs_Manager.InventoryPanelUI.tabController.IsNullOrDestroyed())
                            {
                                foreach (TabUIElement tab in Refs_Manager.InventoryPanelUI.tabController.tabElements)
                                {
                                    if (tab.gameObject.name == "AppearanceTab") { tab_element = tab; break; }
                                }
                            }
                        }
                        if (!tab_element.IsNullOrDestroyed())
                        {
                            tab_element.isDisabled = false; //Enable Tab
                            Behaviour behavior = tab_element.canvasGroup.TryCast<Behaviour>();
                            if (!behavior.IsNullOrDestroyed())
                            {
                                behavior.enabled = false; //Hide Mask
                            }
                        }
                    }
                }
            }
        }
        public class Panel
        {
            public static bool initialized = false;
            public static bool initializing = false;

            public static string[] title_names = { "Noob", "Dev" };

            public static CosmeticPanelUI instance = null;
            public static GameObject player_title = null;
            public static GameObject player_title_dropdown = null;
            public static GameObject ep = null;
            public static GameObject get_points = null;
            public static GameObject open_shop = null;
            public static GameObject skill_cosmetics = null;
            public static GameObject equippables = null;
            public static GameObject eq_helmet = null;
            public static GameObject eq_chest = null;
            public static GameObject eq_mainhand = null;
            public static GameObject eq_backslot = null;
            public static GameObject eq_gloves = null;
            public static GameObject eq_boots = null;
            public static GameObject eq_offhand = null;
            public static GameObject eq_pet1 = null;
            public static GameObject eq_pet2 = null;
            public static GameObject eq_portal = null;
            public static GameObject eq_profil_image = null;
            public static GameObject eq_profil_frame = null;
                  
            public static Il2CppSystem.Collections.Generic.List<TMP_Dropdown.OptionData> Get_Titles()
            {
                Il2CppSystem.Collections.Generic.List<TMP_Dropdown.OptionData> titles = new Il2CppSystem.Collections.Generic.List<TMP_Dropdown.OptionData>();                
                foreach (string s in title_names)
                {
                    TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                    option.text = s;
                    titles.Add(option);
                }
                
                return titles;
            }

            [HarmonyPatch(typeof(CosmeticPanelUI), "Awake")]
            public class CosmeticPanelUI_Awake
            {
                [HarmonyPrefix]
                static bool Prefix(ref CosmeticPanelUI __instance)
                {
                    return false;
                }
            }

            [HarmonyPatch(typeof(CosmeticPanelUI), "OnEnable")]
            public class CosmeticPanelUI_OnEnable
            {
                [HarmonyPrefix]
                static bool Prefix(ref CosmeticPanelUI __instance)
                {
                    instance = __instance;
                    if (!initialized)
                    {
                        Main.logger_instance?.Msg("Intialize CosmeticPanelUI");
                        if (player_title.IsNullOrDestroyed()) { player_title = Functions.GetChild(__instance.gameObject, "Player Titles"); }
                        if ((player_title_dropdown.IsNullOrDestroyed()) && (!player_title.IsNullOrDestroyed()))
                        {
                            player_title_dropdown = Functions.GetChild(player_title, "Title Dropdown");
                            if (!player_title_dropdown.IsNullOrDestroyed()) // != "NullObject") //not null
                            {
                                TMP_Dropdown dropdown = player_title_dropdown.GetComponent<TMP_Dropdown>();
                                if (!dropdown.IsNullOrDestroyed()) { dropdown.options = Get_Titles(); }
                            }
                        }

                        if (ep.IsNullOrDestroyed()) { ep = Functions.GetChild(__instance.gameObject, "EP"); }
                        if (!ep.IsNullOrDestroyed()) { ep.active = false; } //Hide EP

                        if (get_points.IsNullOrDestroyed()) { get_points = Functions.GetChild(__instance.gameObject, "GetPoints"); }
                        if (!get_points.IsNullOrDestroyed()) { get_points.active = false; } //Hide Get Points Btn

                        if (open_shop.IsNullOrDestroyed()) { open_shop = Functions.GetChild(__instance.gameObject, "OpenShop"); }
                        if (!open_shop.IsNullOrDestroyed()) { open_shop.active = false; } //Hide Open Shop Btn   

                        if (skill_cosmetics.IsNullOrDestroyed()) { skill_cosmetics = Functions.GetChild(__instance.gameObject, "Skill Cosmetics"); }
                        if (!skill_cosmetics.IsNullOrDestroyed()) { skill_cosmetics.active = false; } //Hide Skills

                        if (equippables.IsNullOrDestroyed()) { equippables = Functions.GetChild(__instance.gameObject, "Equippables"); }
                        if (!equippables.IsNullOrDestroyed())
                        {
                            if (eq_helmet.IsNullOrDestroyed()) { eq_helmet = Functions.GetChild(equippables, "Helmet"); }
                            if (!eq_helmet.IsNullOrDestroyed()) { Actions.Set(eq_helmet, Actions.eq_helmet_click); }

                            if (eq_chest.IsNullOrDestroyed()) { eq_chest = Functions.GetChild(equippables, "Chest"); }
                            if (!eq_chest.IsNullOrDestroyed()) { Actions.Set(eq_chest, Actions.eq_chest_click); }

                            if (eq_mainhand.IsNullOrDestroyed()) { eq_mainhand = Functions.GetChild(equippables, "MainHand"); }
                            if (!eq_mainhand.IsNullOrDestroyed()) { Actions.Set(eq_mainhand, Actions.eq_mainhand_click); }

                            if (eq_backslot.IsNullOrDestroyed()) { eq_backslot = Functions.GetChild(equippables, "BackSlot"); }
                            if (!eq_backslot.IsNullOrDestroyed()) { eq_backslot.active = false; } //Hide BackSlot

                            if (eq_gloves.IsNullOrDestroyed()) { eq_gloves = Functions.GetChild(equippables, "Gloves"); }
                            if (!eq_gloves.IsNullOrDestroyed()) { Actions.Set(eq_gloves, Actions.eq_gloves_click); }

                            if (eq_boots.IsNullOrDestroyed()) { eq_boots = Functions.GetChild(equippables, "Boots"); }
                            if (!eq_boots.IsNullOrDestroyed()) { Actions.Set(eq_boots, Actions.eq_boots_click); }

                            if (eq_offhand.IsNullOrDestroyed()) { eq_offhand = Functions.GetChild(equippables, "Offhand"); }
                            if (!eq_offhand.IsNullOrDestroyed()) { Actions.Set(eq_offhand, Actions.eq_offhand_click); }

                            if (eq_pet1.IsNullOrDestroyed()) { eq_pet1 = Functions.GetChild(equippables, "Pet1"); }
                            if (!eq_pet1.IsNullOrDestroyed()) { eq_pet1.active = false; } //Hide Pet1

                            if (eq_pet2.IsNullOrDestroyed()) { eq_pet2 = Functions.GetChild(equippables, "Pet2"); }
                            if (!eq_pet2.IsNullOrDestroyed()) { eq_pet2.active = false; } //Hide Pet2

                            if (eq_portal.IsNullOrDestroyed()) { eq_portal = Functions.GetChild(equippables, "Portal"); }
                            if (!eq_portal.IsNullOrDestroyed()) { eq_portal.active = false; } //Hide Portal

                            if (eq_profil_image.IsNullOrDestroyed()) { eq_profil_image = Functions.GetChild(equippables, "ProfileImage"); }
                            if (!eq_profil_image.IsNullOrDestroyed()) { eq_profil_image.active = false; } //Hide Profil Image

                            if (eq_profil_frame.IsNullOrDestroyed()) { eq_profil_frame = Functions.GetChild(equippables, "ProfileFrame"); }
                            if (!eq_profil_frame.IsNullOrDestroyed()) { eq_profil_frame.active = false; } //Hide Profil Frame 

                            Main.logger_instance?.Msg("Intialized");
                            initialized = true;
                        }
                        else { Main.logger_instance?.Error("Skins :: Error equippables is null"); }
                    }
                    return false;
                }
            }
            
            public class Slots
            {
                public static bool need_update = false;
                public static int[] types = { 0, 1, 3, 4, 50, 99 };

                public static void Update()
                {
                    need_update = false;
                    //Main.logger_instance?.Msg("Skins : Panel.Slots.Update()");
                    System.Collections.Generic.List<Save.Data.Structures.saved_skin> saved_skins = new System.Collections.Generic.List<Save.Data.Structures.saved_skin>
                    {
                        Save.Data.UserData.helmet,
                        Save.Data.UserData.body,
                        Save.Data.UserData.boots,
                        Save.Data.UserData.gloves,
                        Save.Data.UserData.weapon,
                        Save.Data.UserData.offhand
                    };
                    for (int i = 0; i < types.Length; i++)
                    {
                        //Main.logger_instance?.Msg("Skins : " + i);
                        Save.Data.Structures.saved_skin saved_skin = saved_skins[i];
                        if ((saved_skin.type > -1) && (saved_skin.subtype > -1) && (saved_skin.rarity > -1) && (saved_skin.unique_id > -1))
                        {
                            Panel.Slots.AddSkin(types[i], Functions.GetItemIcon(new ItemDataUnpacked
                            {
                                LvlReq = 0,
                                classReq = ItemList.ClassRequirement.Any,
                                itemType = (byte)saved_skin.type,
                                subType = (byte)saved_skin.subtype,
                                rarity = (byte)saved_skin.rarity,
                                sockets = 0,
                                uniqueID = (byte)saved_skin.unique_id,
                            }));
                        }
                        else { Panel.Slots.RemoveSkin(types[i]); }
                    }
                    
                }
                public static void AddSkin(int slot, Sprite s)
                {
                    //Main.logger_instance?.Msg("AddSkin");
                    if ((slot == 0) && (!eq_helmet.IsNullOrDestroyed())) { SetImage(eq_helmet, s); } //Helm
                    else if ((slot == 1) && (!eq_chest.IsNullOrDestroyed())) { SetImage(eq_chest, s); } //Chest
                    else if ((slot == 3) && (!eq_boots.IsNullOrDestroyed())) { SetImage(eq_boots, s); } //Boots
                    else if ((slot == 4) && (!eq_gloves.IsNullOrDestroyed())) { SetImage(eq_gloves, s); } //Gloves
                    else if ((slot == 50) && (!eq_mainhand.IsNullOrDestroyed())) { SetImage(eq_mainhand, s); } //MainHand
                    else if ((slot == 99) && (!eq_offhand.IsNullOrDestroyed())) { SetImage(eq_offhand, s); } //OffHand
                }
                public static void RemoveSkin(int slot)
                {
                    //Main.logger_instance?.Msg("RemoveSkin");
                    AddSkin(slot, null);
                    Save.Data.Update(slot, -1, -1, -1, -1);
                }
                public static void SetImage(GameObject g, Sprite s)
                {
                    //Main.logger_instance?.Msg("Slots.SetImage()");
                    CosmeticItemSlot slot = g.GetComponent<CosmeticItemSlot>();
                    Vector3 position = Vector3.zero;
                    Vector3 size_delta = Vector3.zero;
                    if (!slot.IsNullOrDestroyed())
                    {
                        //Main.logger_instance?.Msg("CosmeticItemSlot found");
                        if (s.IsNullOrDestroyed()) { slot.paperDollImage.gameObject.active = true; }
                        else { slot.paperDollImage.gameObject.active = false; }
                        position = slot.paperDollImage.gameObject.transform.position;
                        size_delta = slot.paperDollImage.gameObject.GetComponent<RectTransform>().sizeDelta;
                    }
                    GameObject new_g = Functions.GetChild(g, "skin");
                    if (new_g.IsNullOrDestroyed()) //.name == "NullObject")
                    {
                        //Main.logger_instance?.Msg("Create Image");
                        GameObject skin_obj = new GameObject("skin");
                        skin_obj.AddComponent<Image>();
                        skin_obj.transform.SetParent(g.transform);
                        skin_obj.transform.position = position;
                        skin_obj.GetComponent<RectTransform>().sizeDelta = size_delta;
                        new_g = skin_obj;
                    }
                    if (!new_g.IsNullOrDestroyed()) // != "NullObject")
                    {
                        //Main.logger_instance?.Msg("Set Image");
                        if (s.IsNullOrDestroyed()) { new_g.active = false; }
                        else
                        {
                            new_g.GetComponent<Image>().sprite = s;
                            new_g.active = true;
                        }
                    }
                }
            }
            public class Actions
            {
                public static void Set(GameObject g, System.Action a)
                {
                    Button b = g.GetComponent<Button>();
                    if (!b.IsNullOrDestroyed())
                    {
                        b.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                        b.onClick.AddListener(a);
                    }
                }
                public static readonly System.Action eq_helmet_click = new System.Action(OnClick_Helmet);
                public static void OnClick_Helmet()
                {
                    Flyout.Close();
                    Flyout.selected_slot = 0;
                    Flyout.Open();
                }
                public static readonly System.Action eq_chest_click = new System.Action(OnClick_Chest);
                public static void OnClick_Chest()
                {
                    Flyout.Close();
                    Flyout.selected_slot = 1;
                    Flyout.Open();
                }
                public static readonly System.Action eq_boots_click = new System.Action(OnClick_Boots);
                public static void OnClick_Boots()
                {
                    Flyout.Close();
                    Flyout.selected_slot = 3;
                    Flyout.Open();
                }
                public static readonly System.Action eq_gloves_click = new System.Action(OnClick_Gloves);
                public static void OnClick_Gloves()
                {
                    Flyout.Close();
                    Flyout.selected_slot = 4;
                    Flyout.Open();
                }
                public static readonly System.Action eq_mainhand_click = new System.Action(OnClick_MainHand);
                public static void OnClick_MainHand()
                {
                    Flyout.Close();
                    Flyout.selected_slot = 50;
                    Flyout.Open();
                }
                public static readonly System.Action eq_offhand_click = new System.Action(OnClick_OffHand);
                public static void OnClick_OffHand()
                {
                    Flyout.Close();
                    Flyout.selected_slot = 99;
                    Flyout.Open();
                }
            }
        }
        public class  Flyout
        {
            public static GameObject flyout_content = null;
            public static bool isOpen = false;
            public static int selected_slot = -1;
            public static int selected_type = -1;
            public static int selected_subtype = -1;
            public static int selected_rarity = -1;
            public static int selected_unique = -1;
            private static readonly string skins_name = "skin_";
            private static readonly int[] mainhand_type = { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 23, 24 };
            private static readonly int[] offhand_type = { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 22, 23, 24 };

            public static void Open()
            {
                //Main.logger_instance?.Msg("Cosmetics_Flyout.Open()");
                if (!Panel.instance.IsNullOrDestroyed())
                {
                    if (!Panel.instance.flyoutWindow.IsNullOrDestroyed())
                    {
                        GameObject go_title = Functions.GetChild(Panel.instance.flyoutWindow, "Title");
                        if (!go_title.IsNullOrDestroyed())
                        {
                            string title = "";
                            if (selected_slot == 0) { title = "Helmet"; }
                            else if (selected_slot == 1) { title = "Chest"; }
                            else if (selected_slot == 3) { title = "Boots"; }
                            else if (selected_slot == 4) { title = "Gloves"; }
                            else if (selected_slot == 50) { title = "MainHand"; }
                            else if (selected_slot == 99) { title = "OffHand"; }
                            title += " APPEARANCE";
                            go_title.GetComponent<Il2CppTMPro.TextMeshProUGUI>().text = title;
                        }
                        Panel.instance.flyoutWindow.active = true;
                    }
                }
            }
            public static void Close()
            {
                //Main.logger_instance?.Msg("Cosmetics_Flyout.Close()");
                if (!Panel.instance.IsNullOrDestroyed())
                {
                    if (!Panel.instance.flyoutWindow.IsNullOrDestroyed())
                    {
                        Panel.instance.flyoutWindow.active = false;
                    }
                }
            }
            public static void ResetContent()
            {
                //Main.logger_instance?.Msg("Cosmetics_Flyout.ResetContent()");
                if (!flyout_content.IsNullOrDestroyed())
                {
                    foreach (GameObject g in Functions.GetAllChild(flyout_content))
                    {
                        if (g.name.Contains(skins_name)) { Object.Destroy(g); }
                    }
                }
            }
            public static void Clear_btn()
            {
                Panel.Slots.RemoveSkin(selected_slot);
                Visuals.RemoveSkin(selected_slot);
                Close();
            }
            private static readonly System.Action ClearAction = new System.Action(Clear_btn);

            [HarmonyPatch(typeof(CosmeticsFlyoutSelectionContentNavigable), "OnEnable")]
            public class CosmeticsFlyoutSelectionContentNavigable_OnEnable
            {
                [HarmonyPostfix]
                static void Postfix(ref CosmeticsFlyoutSelectionContentNavigable __instance)
                {
                    //Main.logger_instance?.Msg("CosmeticsFlyoutSelectionContentNavigable.OnEnable()");
                    isOpen = true;
                    if ((!__instance.scrollContent.IsNullOrDestroyed()) && (flyout_content.IsNullOrDestroyed()))
                    {
                        //Main.logger_instance?.Msg("Set cosmetic Flyout Ref");
                        flyout_content = __instance.scrollContent.gameObject;
                    }
                    if (!flyout_content.IsNullOrDestroyed())
                    {
                        //Clear btn
                        //Main.logger_instance?.Msg("Set cleay button event");
                        GameObject cleat_btn_gameobject = Functions.GetChild(flyout_content, "Remove Button");
                        if (!cleat_btn_gameobject.IsNullOrDestroyed())
                        {
                            Button b = cleat_btn_gameobject.GetComponent<Button>();
                            if (!b.IsNullOrDestroyed())
                            {
                                b.onClick = new Button.ButtonClickedEvent();
                                b.onClick.AddListener(ClearAction);
                            }
                        }
                        //Add skins
                        //Main.logger_instance?.Msg("Add Basic Skins");
                        if ((!Refs_Manager.exp_tracker.IsNullOrDestroyed()) && (!Refs_Manager.player_data.IsNullOrDestroyed()))
                        {
                            foreach (ItemList.BaseEquipmentItem item in ItemList.instance.EquippableItems)
                            {
                                if ((item.baseTypeID == selected_slot) ||
                                    ((selected_slot == 50) && (mainhand_type.Contains(item.baseTypeID))) ||
                                    ((selected_slot == 99) && (offhand_type.Contains(item.baseTypeID))))
                                {
                                    foreach (ItemList.EquipmentItem sub_item in item.subItems)
                                    {
                                        if ((sub_item.levelRequirement <= Refs_Manager.exp_tracker.CurrentLevel) &&
                                            (Functions.CheckClass(Refs_Manager.player_data.CharacterClass, sub_item.classRequirement)))
                                        {
                                            GameObject g = new GameObject(skins_name + item.baseTypeID + "_" + sub_item.subTypeID + "_0_0");
                                            g.AddComponent<Button>();
                                            g.AddComponent<Image>();
                                            g.GetComponent<Image>().sprite = Functions.GetItemIcon(new ItemDataUnpacked
                                            {
                                                LvlReq = 0,
                                                classReq = ItemList.ClassRequirement.Any,
                                                itemType = (byte)item.baseTypeID,
                                                subType = (ushort)sub_item.subTypeID,
                                                rarity = 0,
                                                sockets = 0,
                                                uniqueID = 0
                                            });
                                            g.transform.SetParent(flyout_content.transform);
                                        }
                                    }
                                }
                            }
                            //Main.logger_instance?.Msg("Add Unique Skins");
                            foreach (UniqueList.Entry unique in UniqueList.instance.uniques)
                            {
                                ItemList.EquipmentItem base_item = ItemList.instance.EquippableItems[unique.baseType].subItems[unique.subTypes[0]];
                                if ((base_item.levelRequirement <= Refs_Manager.exp_tracker.CurrentLevel) &&
                                    (Functions.CheckClass(Refs_Manager.player_data.CharacterClass, base_item.classRequirement)))
                                {
                                    if ((unique.baseType == selected_slot) ||
                                        ((selected_slot == 50) && (mainhand_type.Contains(unique.baseType))) ||
                                        ((selected_slot == 99) && (offhand_type.Contains(unique.baseType))))
                                    {
                                        int item_rarity = 7;
                                        if (unique.isSetItem) { item_rarity = 8; }
                                        GameObject g = new GameObject(skins_name + unique.baseType + "_" + unique.subTypes[0] + "_" + item_rarity + "_" + unique.uniqueID);
                                        g.AddComponent<Button>();
                                        g.AddComponent<Image>();
                                        g.GetComponent<Image>().sprite = Functions.GetItemIcon(new ItemDataUnpacked
                                        {
                                            LvlReq = 0,
                                            classReq = ItemList.ClassRequirement.Any,
                                            itemType = unique.baseType,
                                            subType = unique.subTypes[0],
                                            rarity = (byte)item_rarity,
                                            sockets = 0,
                                            uniqueID = unique.uniqueID
                                        });
                                        g.transform.SetParent(flyout_content.transform);
                                    }
                                }
                            }
                        }
                    }
                    //else { Main.logger_instance?.Error("Flyout content is null"); }
                }
            }

            [HarmonyPatch(typeof(CosmeticsFlyoutSelectionContentNavigable), "OnDisable")]
            public class CosmeticsFlyoutSelectionContentNavigable_OnDisable
            {
                [HarmonyPostfix]
                static void Postfix()
                {
                    //Main.logger_instance?.Msg("CosmeticsFlyoutSelectionContentNavigable.OnDisable()");
                    isOpen = false;
                    selected_slot = -1;
                    selected_type = -1;
                    selected_rarity = -1;
                    selected_unique = -1;
                    ResetContent();
                    /*if (!flyout_content.IsNullOrDestroyed())
                    {
                        foreach (GameObject g in Functions.GetAllChild(flyout_content))
                        {
                            if (g.name.Contains(skins_name)) { Object.Destroy(g); }
                        }
                    }*/
                }
            }

            [HarmonyPatch(typeof(Button), "Press")]
            public class Button_Press
            {
                [HarmonyPostfix]
                static void Postfix(ref Button __instance)
                {
                    if (isOpen)
                    {
                        if (__instance.name.Contains(skins_name))
                        {
                            try
                            {
                                selected_type = System.Convert.ToInt32(__instance.name.Split('_')[1]);
                                selected_subtype = System.Convert.ToInt32(__instance.name.Split('_')[2]);
                                selected_rarity = System.Convert.ToInt32(__instance.name.Split('_')[3]);
                                selected_unique = System.Convert.ToInt32(__instance.name.Split('_')[4]);

                                //Save
                                Save.Data.Update(selected_slot, selected_type, selected_subtype, selected_rarity, selected_unique);

                                //Panel Icon
                                Panel.Slots.AddSkin(selected_slot, Functions.GetItemIcon(new ItemDataUnpacked
                                {
                                    LvlReq = 0,
                                    classReq = ItemList.ClassRequirement.Any,
                                    itemType = (byte)selected_type,
                                    subType = (byte)selected_subtype,
                                    rarity = (byte)selected_rarity,
                                    sockets = 0,
                                    uniqueID = (byte)selected_unique,
                                }));
                                
                                //Force Equip
                                Visuals.EquipSkin();

                                //Close Flyout
                                Close();
                            }
                            catch { }
                        }
                    }
                }
            }
        }
        public class Visuals
        {
            public static bool NeedUpdate = false;
            public static void Update()
            {                
                if (Scenes.IsCharacterSelection())
                {
                    //
                    NeedUpdate = false;
                }
                else if (Scenes.IsGameScene())
                {
                    //Main.logger_instance?.Msg("Skins : Visuals.Update()");
                    if (!Refs_Manager.player_visuals.IsNullOrDestroyed())
                    {
                        if (!Save.Data.IsEmpty(Save.Data.UserData.helmet))
                        {
                            Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipGear(EquipmentType.HELMET, 0, false, (ushort)0);
                        }
                        if (!Save.Data.IsEmpty(Save.Data.UserData.body))
                        {
                            Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipGear(EquipmentType.BODY_ARMOR, 0, false, (ushort)0);
                        }
                        if (!Save.Data.IsEmpty(Save.Data.UserData.gloves))
                        {
                            Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipGear(EquipmentType.GLOVES, 0, false, (ushort)0);
                        }
                        if (!Save.Data.IsEmpty(Save.Data.UserData.boots))
                        {
                            Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipGear(EquipmentType.BOOTS, 0, false, (ushort)0);
                        }
                        if (!Save.Data.IsEmpty(Save.Data.UserData.weapon))
                        {
                            Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipWeapon(0, 0, 0, (ushort)0, IMSlotType.MainHand, WeaponEffect.None);
                        }
                        if (!Save.Data.IsEmpty(Save.Data.UserData.offhand))
                        {
                            Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipWeapon(0, 0, 0, (ushort)0, IMSlotType.OffHand, WeaponEffect.None);
                        }
                        NeedUpdate = false;
                    }
                    else { Main.logger_instance?.Error("Refs_Manager.player_visuals is null"); }
                }
            }
            
            /*[HarmonyPatch(typeof(CharacterCreationSelection), "OnEnable")]
            public class CharacterCreationSelection_OnEnable
            {
                [HarmonyPrefix]
                static void Prefix(ref CharacterCreationSelection __instance)
                {
                    //Main.logger_instance?.Msg("CharacterCreationSelection:OnEnable : " + __instance.name);              
                }
            }*/
            /*[HarmonyPatch(typeof(ActorVisuals), "OnEnable")]
            public class ActorVisuals_OnEnable
            {
                [HarmonyPrefix]
                static void Prefix(ref ActorVisuals __instance)
                {
                    //Main.logger_instance?.Msg("ActorVisuals:OnEnable : " + __instance.name);
                }
            }*/

            [HarmonyPatch(typeof(EquipmentVisualsManager), "EquipGear")]
            public class EquipmentVisualsManager_EquipGear
            {
                [HarmonyPrefix]
                static void Prefix(ref EquipmentVisualsManager __instance, ref EquipmentType __0, ref int __1, ref bool __2, ref ushort __3)
                {
                    //Main.logger_instance?.Msg("EquipmentVisualsManager:EquipGear");
                    bool found = false;                    
                    Save.Data.Structures.saved_skin skin = Save.Data.Default.Skin();
                    if (__0 == EquipmentType.HELMET)
                    {
                        skin = Save.Data.UserData.helmet;
                        if (skin.rarity > 6) { skin.subtype = UniqueList.GetVisualSubType((ushort)skin.unique_id, skin.subtype); }
                        found = true;
                    }
                    else if (__0 == EquipmentType.BODY_ARMOR)
                    {
                        skin = Save.Data.UserData.body;
                        if (skin.rarity > 6) { skin.subtype = UniqueList.GetVisualSubType((ushort)skin.unique_id, skin.subtype); }
                        found = true;
                    }
                    else if (__0 == EquipmentType.GLOVES)
                    {
                        skin = Save.Data.UserData.gloves;
                        if (skin.rarity > 6) { skin.subtype = UniqueList.GetVisualSubType((ushort)skin.unique_id, skin.subtype); }
                        found = true;
                    }
                    else if (__0 == EquipmentType.BOOTS)
                    {
                        skin = Save.Data.UserData.boots;
                        if (skin.rarity > 6) { skin.subtype = UniqueList.GetVisualSubType((ushort)skin.unique_id, skin.subtype); }
                        found = true;
                    }

                    if (found)
                    {
                        if ((skin.type > -1) && (skin.subtype > -1) && (skin.unique_id > -1))
                        {
                            bool isUnique = false;
                            if (skin.rarity > 6) {  isUnique = true; }
                            __1 = skin.subtype;
                            __2 = isUnique;
                            __3 = (ushort)skin.unique_id;
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(EquipmentVisualsManager), "EquipWeapon")]
            public class EquipmentVisualsManager_EquipWeapon
            {
                [HarmonyPrefix]
                static void Prefix(ref EquipmentVisualsManager __instance, ref int __0, ref int __1, ref int __2, ref ushort __3, ref IMSlotType __4, ref WeaponEffect __5)
                {
                    //Main.logger_instance?.Msg("EquipmentVisualsManager:EquipWeapon");
                    bool found = false;
                    Save.Data.Structures.saved_skin skin = Save.Data.Default.Skin();
                    if (__4 == IMSlotType.MainHand)
                    {
                        skin = Save.Data.UserData.weapon;
                        if (skin.rarity > 6) { skin.subtype = UniqueList.GetVisualSubType((ushort)skin.unique_id, skin.subtype); }
                        found = true;
                    }
                    else if (__4 == IMSlotType.OffHand)
                    {
                        skin = Save.Data.UserData.offhand;
                        if (skin.rarity > 6) { skin.subtype = UniqueList.GetVisualSubType((ushort)skin.unique_id, skin.subtype); }
                        found = true;
                    }

                    if (found)
                    {
                        if ((skin.type > -1) && (skin.subtype > -1) && (skin.unique_id > -1))
                        {
                            __0 = skin.type;
                            __1 = skin.subtype;
                            __2 = skin.rarity;
                            __3 = (ushort)skin.unique_id;
                        }
                    }
                }
            }

            public static void EquipSkin()
            {
                if (Flyout.selected_slot == 50)
                {
                    PlayerFinder.getPlayerVisuals().GetComponent<EquipmentVisualsManager>().EquipWeapon(Flyout.selected_type, Flyout.selected_subtype, Flyout.selected_rarity, (ushort)Flyout.selected_unique, IMSlotType.MainHand, WeaponEffect.None);
                }
                else if (Flyout.selected_slot == 99)
                {
                    PlayerFinder.getPlayerVisuals().GetComponent<EquipmentVisualsManager>().EquipWeapon(Flyout.selected_type, Flyout.selected_subtype, Flyout.selected_rarity, (ushort)Flyout.selected_unique, IMSlotType.OffHand, WeaponEffect.None);
                }
                else
                {
                    bool isUnique = false;
                    if (Flyout.selected_rarity > 6) { isUnique = true; }
                    PlayerFinder.getPlayerVisuals().GetComponent<EquipmentVisualsManager>().EquipGear((EquipmentType)Flyout.selected_type, Flyout.selected_subtype, isUnique, (ushort)Flyout.selected_unique);
                }
            }
            public static void RemoveSkin(int slot)
            {
                ItemLocationPair item_found = null;
                if (!Refs_Manager.player_data.IsNullOrDestroyed())
                {
                    foreach (ItemLocationPair item in Refs_Manager.player_data.SavedItems)
                    {
                        if (((slot == 0) && (item.ContainerID == 2)) || //helm
                            ((slot == 1) && (item.ContainerID == 3)) || //body
                            ((slot == 50) && (item.ContainerID == 4)) || //weapon
                            ((slot == 99) && (item.ContainerID == 5)) || //offhand
                            ((slot == 4) && (item.ContainerID == 6)) ||//gloves
                            ((slot == 3) && (item.ContainerID == 8))) //boots
                        {
                            item_found = item;
                            break;
                        }
                    }
                }
                if (!item_found.IsNullOrDestroyed())
                {
                    try
                    {
                        Flyout.selected_type = item_found.Data[3];
                        Flyout.selected_subtype = item_found.Data[4];
                        Flyout.selected_rarity = item_found.Data[5];
                        Flyout.selected_unique = (item_found.Data[10] * 255) + item_found.Data[11];
                        EquipSkin();
                    }
                    catch { Main.logger_instance?.Error("Skin.RemoveSkin() Error ItemLocationPair Data"); }
                }
                else
                {
                    PlayerFinder.getPlayerVisuals().GetComponent<EquipmentVisualsManager>().RemoveGear((byte)Flyout.selected_type);
                }
            }
        }
        public class Save
        {
            public class Data
            {
                public static readonly string base_path = Directory.GetCurrentDirectory() + @"\Mods\LastEpoch_Hud\Skins\";
                public static string path = "";
                public static Structures.UserSkin UserData = new Structures.UserSkin();

                public class Structures
                {
                    public struct UserSkin
                    {
                        public saved_skin helmet;
                        public saved_skin body;
                        public saved_skin gloves;
                        public saved_skin boots;
                        public saved_skin weapon;
                        public saved_skin offhand;
                    }
                    public struct saved_skin
                    {
                        public int type;
                        public int subtype;
                        public int rarity;
                        public int unique_id;
                    }
                }
                public class Character
                {
                    public static int Character_Index = -1;
                    public static Il2CppLE.Data.Cycle Character_Cycle = Il2CppLE.Data.Cycle.Beta;
                    public static string Character_Name = "";
                    public static string Character_Class = "";
                    public static int Character_Class_Id = 0;
                }
                public class Default
                {
                    public static void DefaultConfig()
                    {
                        UserData = new Structures.UserSkin
                        {
                            helmet = Skin(),
                            body = Skin(),
                            gloves = Skin(),
                            boots = Skin(),
                            weapon = Skin(),
                            offhand = Skin()
                        };
                        Save();
                    }
                    public static Structures.saved_skin Skin()
                    {
                        Structures.saved_skin default_skin = new Structures.saved_skin()
                        {
                            type = -1,
                            subtype = -1,
                            rarity = -1,
                            unique_id = -1
                        };

                        return default_skin;
                    }
                }
                
                public static bool IsEmpty(Skins.Save.Data.Structures.saved_skin skin)
                {
                    bool result = true;
                    if ((skin.type > -1) && (skin.subtype > -1) && (skin.rarity > -1)) { result = false; }

                    return result;
                }
                public static void Update(int slot, int type, int subtype, int rarity, int unique_id)
                {
                    Save.Data.Structures.saved_skin skin = Default.Skin();
                    bool found = false;
                    if (slot == 0)
                    {
                        UserData.helmet = new Structures.saved_skin
                        {
                            type = type,
                            subtype = subtype,
                            rarity = rarity,
                            unique_id = unique_id
                        };
                        found = true;
                    }
                    else if (slot == 1)
                    {
                        UserData.body = new Structures.saved_skin
                        {
                            type = type,
                            subtype = subtype,
                            rarity = rarity,
                            unique_id = unique_id
                        };
                        found = true;
                    }
                    else if (slot == 3)
                    {
                        UserData.boots = new Structures.saved_skin
                        {
                            type = type,
                            subtype = subtype,
                            rarity = rarity,
                            unique_id = unique_id
                        };
                        found = true;
                    }
                    else if (slot == 4)
                    {
                        UserData.gloves = new Structures.saved_skin
                        {
                            type = type,
                            subtype = subtype,
                            rarity = rarity,
                            unique_id = unique_id
                        };
                        found = true;
                    }
                    else if (slot == 50)
                    {
                        UserData.weapon = new Structures.saved_skin
                        {
                            type = type,
                            subtype = subtype,
                            rarity = rarity,
                            unique_id = unique_id
                        };
                        found = true;
                    }
                    else if (slot == 99)
                    {
                        UserData.offhand = new Structures.saved_skin
                        {
                            type = type,
                            subtype = subtype,
                            rarity = rarity,
                            unique_id = unique_id
                        };
                        found = true;
                    }

                    if (found) { Save(); }
                }
                public static void Load()
                {
                    Main.logger_instance?.Msg("Skins : Try to Load : " + Data.path + Data.Character.Character_Name);
                    bool error = false;
                    if (Data.Character.Character_Name != "")
                    {
                        if (File.Exists(Data.path + Data.Character.Character_Name))
                        {
                            try
                            {
                                Data.UserData = JsonConvert.DeserializeObject<Structures.UserSkin>(File.ReadAllText(Data.path + Data.Character.Character_Name));
                                Main.logger_instance?.Msg("Skins : Loaded");
                            }
                            catch
                            {
                                error = true;
                                Main.logger_instance?.Error("Skins : Error loading file : " + Data.Character.Character_Name);
                            }
                        }
                        if ((error) || (!File.Exists(Data.path + Data.Character.Character_Name)))
                        {
                            Main.logger_instance?.Msg("Skins : Load DefaultConfig");
                            Default.DefaultConfig();
                        }
                    }
                    else { Main.logger_instance?.Error("Skins : Error Character_Name is null"); }
                }
                public static void Save()
                {
                    Main.logger_instance?.Msg("Save : " + Data.path + Data.Character.Character_Name);
                    string jsonString = JsonConvert.SerializeObject(Data.UserData, Newtonsoft.Json.Formatting.Indented);
                    if (!Directory.Exists(Data.path)) { Directory.CreateDirectory(Data.path); }
                    if (File.Exists(Data.path + Data.Character.Character_Name)) { File.Delete(Data.path + Data.Character.Character_Name); }
                    File.WriteAllText(Data.path + Data.Character.Character_Name, jsonString);
                }
            }
        }
    }
}
