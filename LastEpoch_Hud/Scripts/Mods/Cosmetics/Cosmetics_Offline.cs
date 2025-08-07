using HarmonyLib;
using Il2Cpp;
using Il2CppLE.Networking.Cosmetics;
using Il2CppLE.Services.Cosmetics;
using Il2CppLE.Services.Visuals;
using Il2CppLE.Services.Visuals.Pets.Data;
using Il2CppLE.Services.Visuals.Portals.Data;
using Il2CppSystem.Threading;
using MelonLoader;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Cosmetics
{
    [RegisterTypeInIl2Cpp]
    public class Cosmetics_Offline : MonoBehaviour
    {
        public Cosmetics_Offline(System.IntPtr ptr) : base(ptr) { }
        public static Cosmetics_Offline instance { get; private set; }

        //We need to edit CharacterPreviewManager.Awake(); to see cosmetic on character selection
        //We need to edit ClientVisualsService.GetPetVisuals(); to see Pets

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if (Scenes.IsGameScene())
            {
                if ((CosmeticTab.cosmetic_panel_ui.IsNullOrDestroyed()) && (!Refs_Manager.InventoryPanelUI.IsNullOrDestroyed()))
                {
                    CosmeticTab.cosmetic_panel_ui = Refs_Manager.InventoryPanelUI.cosmeticPanel;
                }
                if (Data.cosmetics.IsNullOrDestroyed())
                {
                    Data.cosmetics = new System.Collections.Generic.List<Cosmetic>();
                    foreach (Cosmetic cosmetic in Resources.FindObjectsOfTypeAll<Cosmetic>()) { Data.cosmetics.Add(cosmetic); }
                }
                if ((Character.Containers.weapon_container.IsNullOrDestroyed()) ||
                    (Character.Containers.offhand_container.IsNullOrDestroyed()) ||
                    (Character.Containers.helmet_container.IsNullOrDestroyed()) ||
                    (Character.Containers.armor_container.IsNullOrDestroyed()) ||
                    (Character.Containers.gloves_container.IsNullOrDestroyed()) ||
                    (Character.Containers.boots_container.IsNullOrDestroyed()))
                {
                    if (!Refs_Manager.InventoryPanelUI.IsNullOrDestroyed())
                    {
                        if (!Refs_Manager.InventoryPanelUI.inventoryPanel.IsNullOrDestroyed())
                        {
                            GameObject equipped_slots = Functions.GetChild(Refs_Manager.InventoryPanelUI.inventoryPanel, "Equipped Item Slots (Paper Doll)");
                            if (!equipped_slots.IsNullOrDestroyed())
                            {
                                GameObject weapon_slot = Functions.GetChild(equipped_slots, "weapon");
                                if (!weapon_slot.IsNullOrDestroyed())
                                {
                                    ItemContainerUIWithBorder item_container = weapon_slot.GetComponent<ItemContainerUIWithBorder>();
                                    Character.Containers.weapon_container = item_container.Container.TryCast<OneSlotItemContainer>();
                                }
                                GameObject offhand_slot = Functions.GetChild(equipped_slots, "weapon OH");
                                if (!offhand_slot.IsNullOrDestroyed())
                                {
                                    ItemContainerUIWithBorder item_container = offhand_slot.GetComponent<OffhandItemContainerUI>();
                                    Character.Containers.offhand_container = item_container.Container.TryCast<OneSlotItemContainer>();
                                }
                                GameObject helmet_slot = Functions.GetChild(equipped_slots, "helmet");
                                if (!helmet_slot.IsNullOrDestroyed())
                                {
                                    ItemContainerUIWithBorder item_container = helmet_slot.GetComponent<ItemContainerUIWithBorder>();
                                    Character.Containers.helmet_container = item_container.Container.TryCast<OneSlotItemContainer>();
                                }
                                GameObject armor_slot = Functions.GetChild(equipped_slots, "armor");
                                if (!armor_slot.IsNullOrDestroyed())
                                {
                                    ItemContainerUIWithBorder item_container = armor_slot.GetComponent<ItemContainerUIWithBorder>();
                                    Character.Containers.armor_container = item_container.Container.TryCast<OneSlotItemContainer>();
                                }
                                GameObject gloves_slot = Functions.GetChild(equipped_slots, "gloves");
                                if (!gloves_slot.IsNullOrDestroyed())
                                {
                                    ItemContainerUIWithBorder item_container = gloves_slot.GetComponent<ItemContainerUIWithBorder>();
                                    Character.Containers.gloves_container = item_container.Container.TryCast<OneSlotItemContainer>();
                                }
                                GameObject boots_slot = Functions.GetChild(equipped_slots, "boots");
                                if (!boots_slot.IsNullOrDestroyed())
                                {
                                    ItemContainerUIWithBorder item_container = boots_slot.GetComponent<ItemContainerUIWithBorder>();
                                    Character.Containers.boots_container = item_container.Container.TryCast<OneSlotItemContainer>();
                                }
                            }
                        }
                    }
                }
                if (Data.scene_name != Scenes.SceneName) //scene changed
                {
                    Data.scene_name = Scenes.SceneName;
                    if ((Character.NewCharacter) && (!Refs_Manager.player_data.IsNullOrDestroyed()))
                    {
                        Character.NewCharacter = false;
                        Character.Character_Cycle = Refs_Manager.player_data.Cycle;
                        Character.Character_Name = Refs_Manager.player_data.CharacterName;
                        Character.Character_Class = Refs_Manager.player_data.GetCharacterClass().className;
                        Character.Character_Class_Id = Refs_Manager.player_data.CharacterClass;
                        Data.path = Data.base_path + Character.Character_Cycle + @"\";
                        Data.Load();
                    }
                    Character.SetCosmetics();
                    Visuals.Update();
                }
            }
            else
            {
                Data.scene_name = "";
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
                                            if (index != Character.Character_Index)
                                            {
                                                LocalCharacterSlots local_slots = char_selection_game_object.GetComponent<LocalCharacterSlots>();
                                                if (!local_slots.IsNullOrDestroyed())
                                                {
                                                    if (index < local_slots.characterSlots.Count)
                                                    {
                                                        Character.Character_Index = index;
                                                        Character.Character_Cycle = local_slots.characterSlots[index].Cycle;
                                                        Character.Character_Name = local_slots.characterSlots[index].CharacterName;
                                                        Character.Character_Class = local_slots.characterSlots[index].GetCharacterClass().className;
                                                        Character.Character_Class_Id = local_slots.characterSlots[index].CharacterClass;
                                                        Data.path = Data.base_path + Character.Character_Cycle + @"\";
                                                        Data.Load();
                                                    }
                                                    else { Main.logger_instance.Error("Cosmetics : Error Index (" + index + ") > Character count (" + local_slots.characterSlots.Count + ")"); }
                                                }
                                                else { Main.logger_instance.Error("Cosmetics : Error local_slots is null"); }
                                            }
                                        }
                                        else { Main.logger_instance.Error("Cosmetics : Index = " + index); }
                                    }
                                }
                                else { Main.logger_instance.Error("Cosmetics : Error char_select is null"); }
                            }
                            else { Main.logger_instance.Error("Cosmetics : Error char_selection_game_object is null"); }
                        }
                        else { Main.logger_instance.Error("Cosmetics : Error characterSelectPanel is null"); }
                    }
                }
                else { Main.logger_instance.Error("Cosmetics : Error game_uibase is null"); }
            }
        }

        public class Character
        {            
            public static bool NewCharacter = false;
            public static int Character_Index = -1;
            public static Il2CppLE.Data.Cycle Character_Cycle = Il2CppLE.Data.Cycle.Beta;
            public static string Character_Name = "";
            public static string Character_Class = "";
            public static int Character_Class_Id = 0;

            public static void SetCosmetics()
            {
                if (!CosmeticTab.cosmetic_panel_ui.IsNullOrDestroyed())
                {
                    foreach (CosmeticItemSlot cosmetic_item_slot in CosmeticTab.cosmetic_panel_ui.equipSlots)
                    {
                        switch (cosmetic_item_slot.cosmeticSlot)
                        {
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Helm:
                                {
                                    if (Data.UserData.helmet != "") { SetCosmetic(cosmetic_item_slot, Data.GetCometicFromList(Data.UserData.helmet)); }
                                    break;
                                }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Chest:
                                {
                                    if (Data.UserData.body != "") { SetCosmetic(cosmetic_item_slot, Data.GetCometicFromList(Data.UserData.body)); }
                                    break;
                                }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Boots:
                                {
                                    if (Data.UserData.boots != "") { SetCosmetic(cosmetic_item_slot, Data.GetCometicFromList(Data.UserData.boots)); }
                                    break;
                                }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Hands:
                                {
                                    if (Data.UserData.gloves != "") { SetCosmetic(cosmetic_item_slot, Data.GetCometicFromList(Data.UserData.gloves)); }
                                    break;
                                }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand:
                                {
                                    if (Data.UserData.weapon != "") { SetCosmetic(cosmetic_item_slot, Data.GetCometicFromList(Data.UserData.weapon)); }
                                    break;
                                }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Offhand:
                                {
                                    if (Data.UserData.offhand != "") { SetCosmetic(cosmetic_item_slot, Data.GetCometicFromList(Data.UserData.offhand)); }
                                    break;
                                }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Portals:
                                {
                                    if (Data.UserData.portal != "") { SetCosmetic(cosmetic_item_slot, Data.GetCometicFromList(Data.UserData.portal)); }
                                    break;
                                }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Back:
                                {
                                    if (Data.UserData.back != "") { SetCosmetic(cosmetic_item_slot, Data.GetCometicFromList(Data.UserData.back)); }
                                    break;
                                }
                        }
                    }
                    foreach (CosmeticItemSlot cosmetic_item_slot in CosmeticTab.cosmetic_panel_ui.petSlots)
                    {
                        switch (cosmetic_item_slot.cosmeticSlot)
                        {
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Pet1:
                                {
                                    if (Data.UserData.pet1 != "") { SetCosmetic(cosmetic_item_slot, Data.GetCometicFromList(Data.UserData.pet1)); }
                                    break;
                                }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Pet2:
                                {
                                    if (Data.UserData.pet1 != "") { SetCosmetic(cosmetic_item_slot, Data.GetCometicFromList(Data.UserData.pet2)); }
                                    break;
                                }
                        }
                    }
                }
            }
            public static void SetCosmetic(CosmeticItemSlot slot, Cosmetic cosmetic)
            {
                if (!cosmetic.IsNullOrDestroyed())
                {
                    slot.SetCosmetic(cosmetic);
                    slot.SetSprite(cosmetic.StoreSprite);
                    GameObject item_image = Functions.GetChild(slot.gameObject, "CosmeticItemImage");
                    if (!item_image.IsNullOrDestroyed()) { item_image.SetActive(true); }
                }
            }
            public static void RemoveCosmetic(CosmeticItemSlot slot)
            {
                slot._storedCosmetic = null;
                GameObject item_image = Functions.GetChild(slot.gameObject, "CosmeticItemImage");
                if (!item_image.IsNullOrDestroyed()) { item_image.SetActive(false); }
            }

            public class Selection
            {
                /*public static CharacterPreviewManager character_preview_manager = null;

                [HarmonyPatch(typeof(CharacterPreviewManager), "Awake")]
                public class CharacterPreviewManager_Awake
                {
                    [HarmonyPostfix]
                    static void Postfix(ref CharacterPreviewManager __instance)
                    {
                        Main.logger_instance.Msg("CharacterPreviewManager.Awake();");
                        character_preview_manager = __instance;
                    }
                }*/
                
                [HarmonyPatch(typeof(LocalCharacterSlots), "CreateCharacter")]
                public class LocalCharacterSlots_CreateCharacter
                {
                    [HarmonyPostfix]
                    static void Postfix()
                    {
                        NewCharacter = true;
                    }
                }
            }
            public class Containers
            {
                public static OneSlotItemContainer weapon_container = null;
                public static OneSlotItemContainer offhand_container = null;
                public static OneSlotItemContainer helmet_container = null;
                public static OneSlotItemContainer armor_container = null;
                public static OneSlotItemContainer gloves_container = null;
                public static OneSlotItemContainer boots_container = null;
            }

            [HarmonyPatch(typeof(CosmeticsManager), "GetOwnedCosmetics")]
            public class CosmeticsManager_GetOwnedCosmetics
            {
                [HarmonyPostfix]
                static void Postfix(ref CosmeticsManager __instance, ref Il2CppCysharp.Threading.Tasks.UniTask<Il2CppSystem.Collections.Generic.List<string>> __result)
                {
                    if (!Refs_Manager.player_actor.IsNullOrDestroyed()) { __instance.player = Refs_Manager.player_actor.gameObject; }
                    Il2CppSystem.Collections.Generic.List<string> list = new Il2CppSystem.Collections.Generic.List<string>();
                    foreach (Cosmetic cosmetic in Data.cosmetics) { list.Add(cosmetic.BackendID); }
                    __result = new Il2CppCysharp.Threading.Tasks.UniTask<Il2CppSystem.Collections.Generic.List<string>>(list);
                }
            }
        }
        public class CosmeticFlyout
        {
            //public static CosmeticItemObject cosmetic_item_objet = null; //Used for debug

            //Select cosmetic
            [HarmonyPatch(typeof(CosmeticsManager), "SendSelectCosmetic", new System.Type[] { typeof(CosmeticItemObject), typeof(Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot) })]
            public class CosmeticsManager_SendSelectCosmetic
            {
                [HarmonyPostfix]
                static void Postfix(ref CosmeticsManager __instance, ref CosmeticItemObject __0, Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot __1)
                {
                    //cosmetic_item_objet = __0; //Used for debug
                    if (!CosmeticTab.cosmetic_panel_ui.IsNullOrDestroyed())
                    {
                        Character.SetCosmetic(CosmeticTab.cosmetic_panel_ui.selectedSlot, __0.storedCosmetic);
                        Data.Update();
                        Visuals.Update();
                    }
                }
            }

            //Remove cosmetic
            [HarmonyPatch(typeof(CosmeticPanelUI), "RemoveCosmetic")]
            public class CosmeticPanelUI_RemoveCosmetic
            {
                [HarmonyPostfix]
                static void Postfix(ref CosmeticPanelUI __instance)
                {
                    Character.RemoveCosmetic(CosmeticTab.cosmetic_panel_ui.selectedSlot);
                    Data.Update();
                    Visuals.Update();
                }
            }
        }
        public class CosmeticTab
        {
            public static CosmeticPanelUI cosmetic_panel_ui = null;
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
                            tab_element.isDisabled = false;
                            Behaviour behavior = tab_element.canvasGroup.TryCast<Behaviour>();
                            if (!behavior.IsNullOrDestroyed()) { behavior.enabled = false; }
                        }
                    }
                }
            }
        }
        public class Data
        {
            public static System.Collections.Generic.List<Cosmetic> cosmetics = null; //List of all cosmetics
            public static readonly string base_path = Directory.GetCurrentDirectory() + @"\Mods\LastEpoch_Hud\Cosmetics\";
            public static string path = "";
            public static string scene_name = "";
            public static UserCosmetics UserData = new UserCosmetics();

            public struct UserCosmetics
            {
                public string helmet;
                public string body;
                public string gloves;
                public string boots;
                public string weapon;
                public string offhand;
                public string portal;
                public string back;
                public string pet1;
                public string pet2;
            }

            public static string GetCosmeticId(CosmeticItemSlot cosmetic_item_slot)
            {
                string result = "";
                if (!cosmetic_item_slot._storedCosmetic.IsNullOrDestroyed())
                {
                    result = cosmetic_item_slot._storedCosmetic.BackendID;
                }

                return result;
            }
            public static Cosmetic GetCometicFromList(string id)
            {
                Cosmetic result = null;
                foreach (Cosmetic cosmetic in Data.cosmetics)
                {
                    if (cosmetic.BackendID == id) { result = cosmetic; break; }
                }

                return result;
            }
            public static Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot GetCosmeticSlot(EquipmentType type)
            {
                Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Unknown;
                switch (type)
                {
                    case EquipmentType.HELMET: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Helm; break; }
                    case EquipmentType.BODY_ARMOR: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Chest; break; }
                    case EquipmentType.BOOTS: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Boots; break; }
                    case EquipmentType.GLOVES: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Hands; break; }
                    case EquipmentType.ONE_HANDED_AXE: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.ONE_HANDED_DAGGER: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.ONE_HANDED_FIST: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.ONE_HANDED_MACES: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.ONE_HANDED_SCEPTRE: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.ONE_HANDED_SWORD: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.TWO_HANDED_AXE: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.TWO_HANDED_MACE: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.TWO_HANDED_SPEAR: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.TWO_HANDED_STAFF: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.TWO_HANDED_SWORD: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.WAND: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.BOW: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.CROSSBOW: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand; break; }
                    case EquipmentType.QUIVER: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Offhand; break; }
                    case EquipmentType.SHIELD: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Offhand; break; }
                    case EquipmentType.CATALYST: { slot = Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Offhand; break; }
                }

                return slot;
            }
            public static CosmeticItem GetStoredCosmetic(Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot slot)
            {
                CosmeticItem cosmetic_item = null;
                if ((!CosmeticTab.cosmetic_panel_ui.IsNullOrDestroyed()) && (slot != Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Unknown))
                {
                    foreach (CosmeticItemSlot cosmetic_item_slot in CosmeticTab.cosmetic_panel_ui.equipSlots)
                    {
                        if (cosmetic_item_slot.cosmeticSlot == slot)
                        {
                            Cosmetic cosmetic = cosmetic_item_slot._storedCosmetic;
                            if (!cosmetic.IsNullOrDestroyed())
                            {
                                cosmetic_item = cosmetic.TryCast<CosmeticItem>();
                            }
                            break;
                        }
                    }
                }

                return cosmetic_item;
            }
            public static void DefaultConfig()
            {
                UserData = new UserCosmetics
                {
                    helmet = "",
                    body = "",
                    gloves = "",
                    boots = "",
                    weapon = "",
                    offhand = "",
                    back = "",
                    portal = "",
                    pet1 = "",
                    pet2 = ""
                };
            }
            public static void Update()
            {
                if (!CosmeticTab.cosmetic_panel_ui.IsNullOrDestroyed())
                {
                    foreach (CosmeticItemSlot cosmetic_item_slot in CosmeticTab.cosmetic_panel_ui.equipSlots)
                    {
                        switch (cosmetic_item_slot.cosmeticSlot)
                        {
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Helm: { UserData.helmet = GetCosmeticId(cosmetic_item_slot); break; }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Chest: { UserData.body = GetCosmeticId(cosmetic_item_slot); break; }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Boots: { UserData.boots = GetCosmeticId(cosmetic_item_slot); break; }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Hands: { UserData.gloves = GetCosmeticId(cosmetic_item_slot); break; }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Mainhand: { UserData.weapon = GetCosmeticId(cosmetic_item_slot); break; }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Offhand: { UserData.offhand = GetCosmeticId(cosmetic_item_slot); break; }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Back: { UserData.back = GetCosmeticId(cosmetic_item_slot); break; }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Portals: { UserData.portal = GetCosmeticId(cosmetic_item_slot); break; }
                        }
                    }
                    foreach (CosmeticItemSlot cosmetic_item_slot in CosmeticTab.cosmetic_panel_ui.petSlots)
                    {
                        switch (cosmetic_item_slot.cosmeticSlot)
                        {
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Pet1: { UserData.pet1 = GetCosmeticId(cosmetic_item_slot); break; }
                            case Il2CppLE.Networking.Cosmetics.CosmeticEquipSlot.Pet2: { UserData.pet2 = GetCosmeticId(cosmetic_item_slot); break; }
                        }
                    }
                }

                Save();
            }
            public static void Load()
            {
                Main.logger_instance?.Msg("Cosmetics : Try to Load : " + Data.path + Character.Character_Name);
                bool error = false;
                if (Character.Character_Name != "")
                {
                    if (File.Exists(Data.path + Character.Character_Name))
                    {
                        try
                        {
                            Data.UserData = JsonConvert.DeserializeObject<UserCosmetics>(File.ReadAllText(Data.path + Character.Character_Name));
                            Main.logger_instance?.Msg("Cosmetics : Loaded");
                        }
                        catch
                        {
                            error = true;
                            Main.logger_instance?.Error("Cosmetics : Error loading file : " + Character.Character_Name);
                        }
                    }
                    if ((error) || (!File.Exists(Data.path + Character.Character_Name)))
                    {
                        Main.logger_instance?.Msg("Cosmetics : Load DefaultConfig");
                        DefaultConfig();
                        Save();
                    }
                }
                else { Main.logger_instance?.Error("Cosmetics : Error Character_Name is null"); }
            }
            public static void Save()
            {
                //Main.logger_instance.Msg("Save : " + Data.path + Data.Character.Character_Name);
                string jsonString = JsonConvert.SerializeObject(Data.UserData, Newtonsoft.Json.Formatting.Indented);
                if (!Directory.Exists(Data.path)) { Directory.CreateDirectory(Data.path); }
                if (File.Exists(Data.path + Character.Character_Name)) { File.Delete(Data.path + Character.Character_Name); }
                File.WriteAllText(Data.path + Character.Character_Name, jsonString);
            }
        }
        public class Visuals
        {
            public static void Update()
            {
                if (Scenes.IsCharacterSelection())
                {
                    
                }
                else if (Scenes.IsGameScene())
                {
                    if (!Refs_Manager.player_visuals.IsNullOrDestroyed())
                    {
                        if (!Character.Containers.helmet_container.IsNullOrDestroyed())
                        {
                            if (!Character.Containers.helmet_container.content.IsNullOrDestroyed())
                            {
                                bool isUnique = false;
                                if (Character.Containers.helmet_container.content.data.rarity > 6) { isUnique = true; }
                                Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipGear(EquipmentType.HELMET, Character.Containers.helmet_container.content.data.subType, isUnique, Character.Containers.helmet_container.content.data.uniqueID);
                            }
                        }
                        if (!Character.Containers.armor_container.IsNullOrDestroyed())
                        {
                            if (!Character.Containers.armor_container.content.IsNullOrDestroyed())
                            {
                                bool isUnique = false;
                                if (Character.Containers.armor_container.content.data.rarity > 6) { isUnique = true; }
                                Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipGear(EquipmentType.BODY_ARMOR, Character.Containers.armor_container.content.data.subType, isUnique, Character.Containers.armor_container.content.data.uniqueID);
                            }
                        }
                        if (!Character.Containers.gloves_container.IsNullOrDestroyed())
                        {
                            if (!Character.Containers.gloves_container.content.IsNullOrDestroyed())
                            {
                                bool isUnique = false;
                                if (Character.Containers.gloves_container.content.data.rarity > 6) { isUnique = true; }
                                Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipGear(EquipmentType.GLOVES, Character.Containers.gloves_container.content.data.subType, isUnique, Character.Containers.gloves_container.content.data.uniqueID);
                            }
                        }
                        if (!Character.Containers.boots_container.IsNullOrDestroyed())
                        {
                            if (!Character.Containers.boots_container.content.IsNullOrDestroyed())
                            {
                                bool isUnique = false;
                                if (Character.Containers.boots_container.content.data.rarity > 6) { isUnique = true; }
                                Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipGear(EquipmentType.BOOTS, Character.Containers.boots_container.content.data.subType, isUnique, Character.Containers.boots_container.content.data.uniqueID);
                            }
                        }
                        if (!Character.Containers.weapon_container.IsNullOrDestroyed())
                        {
                            if (!Character.Containers.weapon_container.content.IsNullOrDestroyed())
                            {
                                Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().RemoveWeapon(false);
                                Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipWeapon(Character.Containers.weapon_container.content.data.itemType, Character.Containers.weapon_container.content.data.subType, Character.Containers.weapon_container.content.data.rarity, Character.Containers.weapon_container.content.data.uniqueID, IMSlotType.MainHand, WeaponEffect.None);
                            }
                        }
                        if (!Character.Containers.offhand_container.IsNullOrDestroyed())
                        {
                            if (!Character.Containers.offhand_container.content.IsNullOrDestroyed())
                            {
                                Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().RemoveWeapon(true);
                                Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipWeapon(Character.Containers.offhand_container.content.data.itemType, Character.Containers.offhand_container.content.data.subType, Character.Containers.offhand_container.content.data.rarity, Character.Containers.offhand_container.content.data.uniqueID, IMSlotType.OffHand, WeaponEffect.None);
                            }
                        }
                        if (Data.UserData.back != "") 
                        {
                            Refs_Manager.player_visuals.GetComponent<EquipmentVisualsManager>().EquipBackSlotAsync(Data.UserData.back, new CancellationToken());
                        }
                        if (Data.UserData.pet1 != "")
                        {

                        }
                        if (Data.UserData.pet2 != "")
                        {

                        }
                    }
                    else { Main.logger_instance?.Error("Refs_Manager.player_visuals is null"); }
                }
            }

            [HarmonyPatch(typeof(ClientVisualsService), "GetPortalVisual")]
            public class ClientVisualsService_GetPortalVisual
            {
                [HarmonyPrefix]
                static void Prefix(ClientVisualsService __instance, ref PortalVisualKey __0)
                {
                    if (!CosmeticTab.cosmetic_panel_ui.IsNullOrDestroyed())
                    {
                        if (!CosmeticTab.cosmetic_panel_ui.portalSlot._storedCosmetic.IsNullOrDestroyed())
                        {
                            __0 = CosmeticTab.cosmetic_panel_ui.portalSlot._storedCosmetic.TryCast<CosmeticPortal>().Key;
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(ClientVisualsService), "GetItemVisual")]
            public class ClientVisualsService_GetItemVisual
            {
                [HarmonyPrefix]
                static void Prefix(ClientVisualsService __instance, ref Il2CppLE.Services.Models.Items.ItemVisualKey __0)
                {
                    CosmeticItem cosmetic_item = Data.GetStoredCosmetic(Data.GetCosmeticSlot(__0.EquipmentType));
                    if (!cosmetic_item.IsNullOrDestroyed()) { __0 = cosmetic_item.Override.Replacement; }
                }
            }

            [HarmonyPatch(typeof(ClientVisualsService), "GetPetVisuals")]
            public class ClientVisualsService_GetPetVisuals
            {
                [HarmonyPrefix]
                static void Prefix(ClientVisualsService __instance, PetVisualKey __0)
                {
                    Main.logger_instance.Msg("ClientVisualsService.GetPetVisuals()");
                    //0 'visualKey

                    /*if (!cosmetic_panel_ui.IsNullOrDestroyed())
                    {
                        //Use visualKey to see if you want pet1 or pet2
                        int index = 0; //Pet1, so to 1 for Pet2
                        int i = 0;
                        foreach (CosmeticItemSlot cosmetic_item_slot in cosmetic_panel_ui.petSlots)
                        {
                            if (i ==index)
                            {
                                __0 = cosmetic_item_slot._storedCosmetic.TryCast<CosmeticPet>().Key;
                                break;
                            }
                            i++;
                        }
                    }*/
                }
            }
        }
    }
}
