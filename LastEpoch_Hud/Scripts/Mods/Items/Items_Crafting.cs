using HarmonyLib;
using Il2Cpp;
using Il2CppLE.Data;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace LastEpoch_Hud.Scripts.Mods.Items
{
    [RegisterTypeInIl2Cpp]
    public class Items_Crafting : MonoBehaviour
    {
        public static Items_Crafting instance { get; private set; }
        public Items_Crafting(System.IntPtr ptr) : base(ptr) { }

        private static bool Enable = true; //Use to disable the whole mod

        public static bool ShowDebug = false;//Main.debug; //Debug log
        public static bool CanCraftToT7 = true;
        public static bool CanCraftIdols = true;
        public static bool CanCraftUniqueSet = true;
        public static bool CanCraftWithoutShard = true; //Enable UpgradeBtn, Show shards in Material

        public static CraftingManager Crafting_Manager_instance = null; //Used to Show value in game

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if (Enable)
            {
                if (Scenes.IsGameScene()) { if (Ui.initialized) { Ui.Update(); } }
                else
                {
                    Ui.initialized = false;
                    Crafting_Materials_Panel_UI.Initialized = false;
                }
            }
        }
        public static void Debug(bool error, string msg)
        {
            if (ShowDebug)
            {
                string mod = "Forge : ";
                if (error) { Main.logger_instance?.Error(mod + msg); }
                else { Main.logger_instance?.Msg(mod + msg); }
            }
        }
        
        public class Current
        {    
            public static OneItemContainer item_container = null;
            public static ItemData item = null;
            public static AffixSlotForge slot = null;
            public static CraftingUpgradeButton btn = null;
        }
        public class Get
        {
            public static int Tier(ItemData item_data, int affix_id)
            {
                int result = -1;
                if (!item_data.IsNullOrDestroyed())
                {
                    foreach (ItemAffix affix in item_data.affixes)
                    {
                        if (affix.affixId == affix_id)
                        {
                            result = affix.affixTier;
                            break;
                        }
                    }
                }
                //Debug(false, "Get.Tier(), result = " + result);

                return result;
            }
            public static bool IsIdol(ItemData item)
            {
                bool result = false;
                if ((item.itemType > 24) && (item.itemType < 34)) { result = true; }
                //Debug(false, "Get.IsIdol(), result = " + result);

                return result;
            }
            public static bool IsFourSocketOrMore(ItemData item)
            {
                bool result = false;
                int count = 0;
                foreach (ItemAffix affix in item.affixes)
                {
                    if (!affix.isSealedAffix) { count++; }
                }
                if (count > 3) { result = true; }
                //Debug(false, "Get.IsFourSocketOrMore(), result = " + result);

                return result;
            }
            public static bool IsCraftable(ItemData item)
            {
                bool result = false;
                if ((item.itemType < 34) && (CanCraftIdols)) { result = true; }
                else if ((item.rarity > 6) && (CanCraftUniqueSet)) { result = true; }
                else if (item.itemType < 25) { result = true; }
                //Debug(false, "Get.IsCraftable(), result = " + result);

                return result;
            }
            public static bool IsAffixFull(ItemData item)
            {
                bool result = false;
                int nb_max = item.affixes.Count;
                bool idol = IsIdol(item);
                int max = 6; // Save_Manager.instance.data.modsNotInHud.Craft_Items_Nb_Prefixs + Save_Manager.instance.data.modsNotInHud.Craft_Items_Nb_Suffixs;
                /*if (idol)
                {
                    if (CanCraftIdols) { max = Save_Manager.instance.data.modsNotInHud.Craft_Idols_Nb_Prefixs + Save_Manager.instance.data.modsNotInHud.Craft_Idols_Nb_Suffixs; }
                    else { max = 0; }
                }*/
                if (max > 6) { max = 6; }
                if (nb_max > (max - 1)) { result = true; }
                //Debug(false, "Get.IsAffixFull(), result = " + result);

                return result;
            }
            public static bool IsPrefixFull(ItemData item)
            {
                bool result = false;
                int count = 0;
                int max_prefix = 3;
                bool idol = IsIdol(item);
                /*if (!Save_Manager.instance.IsNullOrDestroyed())
                {
                    max_prefix = Save_Manager.instance.data.modsNotInHud.Craft_Items_Nb_Prefixs;
                    if (idol)
                    {
                        if (CanCraftIdols) { max_prefix = Save_Manager.instance.data.modsNotInHud.Craft_Idols_Nb_Prefixs; }
                        else { max_prefix = 0; }
                    }
                }*/
                    foreach (ItemAffix affix in item.affixes)
                    {
                        if ((affix.affixType == AffixList.AffixType.PREFIX) && ((idol)) ||
                            ((!idol) && (affix.affixTier > 5) && (CanCraftToT7)) ||    //6 = T7
                            ((!idol) && (affix.affixTier > 3)))
                        {
                            count++;
                        }
                    }
                    if (count > (max_prefix - 1)) { result = true; }
                
                //Debug(false, "Get.IsPrefixFull(), result = " + result);

                return result;
            }
            public static bool IsSuffixFull(ItemData item)
            {
                bool result = false;
                int nb_max = 0;
                bool idol = IsIdol(item);
                int max_prefix = 3; // Save_Manager.instance.data.modsNotInHud.Craft_Items_Nb_Suffixs;
                /*if (idol)
                {
                    if (CanCraftIdols) { max_prefix = Save_Manager.instance.data.modsNotInHud.Craft_Idols_Nb_Suffixs; }
                    else { max_prefix = 0; }
                }*/
                foreach (ItemAffix affix in item.affixes)
                {
                    if ((affix.affixType == AffixList.AffixType.SUFFIX) && ((idol)) ||
                        ((!idol) && (affix.affixTier > 5) && (CanCraftToT7)) ||
                        ((!idol) && (affix.affixTier > 3)))
                    {
                        nb_max++;
                    }
                }
                if (nb_max > (max_prefix - 1)) { result = true; }
                //Debug(false, "Get.IsSuffixFull(), result = " + result);

                return result;
            }
        }
        public class Create
        {
            public static ShardAffixListElement AffixElement(GameObject shardAffixPrefab, AffixList.Affix affix, BaseStats.ModType affix_modifier)
            {
                //Debug(false, "Create.ShardAffixListElement(), Name = " + affix.affixName );
                GameObject obj = Object.Instantiate(shardAffixPrefab, Vector3.zero, Quaternion.identity);
                obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                ShardAffixListElement element = obj.GetComponent<ShardAffixListElement>();
                if (!element.IsNullOrDestroyed())
                {
                    element.affix = affix;
                    element.affixTitle = affix.affixTitle;
                    if (affix.type == AffixList.AffixType.SUFFIX)
                    {
                        element.affixType = "Suffix";
                        element.prefixArrow.active = false;
                        element.suffixArrow.active = true;
                    }
                    else if (affix.type == AffixList.AffixType.PREFIX)
                    {
                        element.affixType = "Prefix";
                        element.prefixArrow.active = true;
                        element.suffixArrow.active = false;
                    }
                    else if (affix.type == AffixList.AffixType.SPECIAL)
                    {
                        Main.logger_instance?.Msg(affix.affixName + " is Special");
                        element.affixType = "Special";
                        element.prefixArrow.active = false;
                        element.suffixArrow.active = false;
                    }
                    element.shardItemName = affix.affixName;
                    element.shardName = affix.affixDisplayName;
                    element.shardType = affix.affixId;
                    element.improvementTMP.text = affix_modifier.ToString();
                    element.init = true;                    
                }
                else { Debug(true, "element is null"); }

                return element;
            }
        }
        public class Locales
        {
            public static string no_space_affix_key = "Crafting_ForgeButton_Title_NoSpaceAffix";
            public static string no_space_affix = "force_affix";
            public static string no_space_prefix_key = "Crafting_ForgeButton_Title_NoSpacePrefix";
            public static string no_space_prefix = "force_prefix";
            public static string no_space_suffix_key = "Crafting_ForgeButton_Title_NoSpaceSuffix";
            public static string no_space_suffix = "force_suffix";
            public static string affix_is_maxed_key = "Crafting_ForgeButton_Title_AffixMaxed";
            public static string affix_is_maxed = "maxed_craft";
            public static string cant_craft_unique_key = "Crafting_ForgeButton_Title_Uniques";
            public static string cant_craft_unique = "unique_craft";
            public static string cant_craft_idol_key = "Crafting_ForgeButton_Title_Idols";
            public static string cant_craft_idol = "idol_craft";
            public static string no_forgin_potencial_key = "Crafting_ForgeButton_Title_NoPotential";
            public static string no_forgin_potencial = "no_forgin_potencial_craft";
            //public static string cannot_add_affix_key = "";
            //public static string cannot_add_affix = "cannot_add_affix";
            public static string add_affix_key = "Crafting_ForgeButton_Title_AddAffix";
            public static string add_affix = "add_affix";
            public static string upgrade_affix_key = "Crafting_ForgeButton_Title_UpgradeAffix";
            public static string upgrade_affix = "upgrade_affix";

            [HarmonyPatch(typeof(Localization), "TryGetText")]
            public class Localization_TryGetText
            {
                [HarmonyPrefix]
                static bool Prefix(ref bool __result, string __0) //, Il2CppSystem.String __1)
                {
                    //Main.logger_instance?.Msg("Localization:TryGetText key = " + __0);
                    bool result = true;
                    if (Enable)
                    {
                        if ((__0 == affix_is_maxed_key) || (__0 == cant_craft_unique_key) || (__0 == cant_craft_idol_key) ||
                            (__0 == no_forgin_potencial_key) || (__0 == no_space_prefix_key) ||
                            (__0 == no_space_suffix_key) || (__0 == no_space_affix_key) ||
                            /*(__0 == cannot_add_affix_key) ||*/ (__0 == add_affix_key) || (__0 == upgrade_affix_key))
                        {
                            __result = true;
                            result = false;
                        }
                    }

                    return result;
                }
            }

            [HarmonyPatch(typeof(Localization), "GetText")]
            public class Localization_GetText
            {
                [HarmonyPrefix]
                static bool Prefix(ref string __result, string __0)
                {
                    //Main.logger_instance?.Msg("Localization:GetText key = " + __0);
                    bool result = true;
                    if (Enable)
                    {
                        if (__0 == affix_is_maxed_key)
                        {
                            __result = affix_is_maxed;
                            result = false;
                        }
                        else if (__0 == no_space_affix_key)
                        {
                            __result = no_space_affix;
                            result = false;
                        }
                        else if (__0 == no_space_prefix_key)
                        {
                            __result = no_space_prefix;
                            result = false;
                        }
                        else if (__0 == no_space_suffix_key)
                        {
                            __result = no_space_suffix;
                            result = false;
                        }
                        else if (__0 == cant_craft_unique_key)
                        {
                            __result = cant_craft_unique;
                            result = false;
                        }
                        else if (__0 == cant_craft_idol_key)
                        {
                            __result = cant_craft_idol;
                            result = false;
                        }
                        else if (__0 == no_forgin_potencial_key)
                        {
                            __result = no_forgin_potencial;
                            result = false;
                        }
                        /*else if (__0 == cannot_add_affix_key)
                        {
                            __result = cannot_add_affix;
                            result = false;
                        }*/
                        else if (__0 == add_affix_key)
                        {
                            __result = add_affix;
                            result = false;
                        }
                        else if (__0 == upgrade_affix_key)
                        {
                            __result = upgrade_affix;
                            result = false;
                        }
                    }

                    return result;
                }
            }
        }
        public class Ui
        {
            public struct Slot_refs
            {
                public GameObject Slot_obj;
                public GameObject shards;
                public GameObject add_shard_btn;
                public GameObject shard_icon;
                public GameObject glass_lense;
                public GameObject available_shards;
                public GameObject available_shards_count;
                public GameObject active_pathing;
                public GameObject upgrade_available;
                public GameObject upgrade_available_indicator;
                public GameObject affix_desc_holder;
                public GameObject drop_shadow;
                public GameObject tier_level;
                public GameObject separator;
                public GameObject affix_name;
            }
            public static Slot_refs slot_0_refs;    //prefix_0
            public static Slot_refs slot_1_refs;    //prefix_1
            public static Slot_refs slot_2_refs;    //suffix_0
            public static Slot_refs slot_3_refs;    //suffix_1
            public static Slot_refs slot_4_refs;    //prefix_2
            public static Slot_refs slot_5_refs;    //suffix_2
            public static bool initialized = false;
            public static Il2CppSystem.Collections.Generic.List<ItemAffix> backup_affixs = null;
            public static bool force_update_slots = false;
            public static bool updating = false;
            public static bool already_reset = false;
            public static string shards_text_filter = "";

            public static void Init()
            {
                Debug(false, "Ui.Init()");
                if ((!Refs_Manager.crafting_panel_ui.IsNullOrDestroyed()) && (!Refs_Manager.craft_slot_manager.IsNullOrDestroyed()))
                {
                    GameObject __instance = Functions.GetChild(Refs_Manager.crafting_panel_ui.gameObject, "MainContent");
                    if (!__instance.IsNullOrDestroyed())
                    {
                        slot_0_refs = Get_SlotRefs(0);    //prefix_0
                        slot_1_refs = Get_SlotRefs(1);    //prefix_1
                        GameObject slot_4 = Object.Instantiate(slot_1_refs.Slot_obj, Vector3.zero, Quaternion.identity);
                        slot_4.name = "ModSlot (4)";
                        slot_4.transform.SetParent(__instance.transform);
                        slot_4_refs = Get_SlotRefs(4);
                        Set_SlotPositions(4);                        
                        slot_2_refs = Get_SlotRefs(2);    //suffix_0
                        slot_3_refs = Get_SlotRefs(3);    //suffix_1
                        GameObject slot_5 = Object.Instantiate(slot_3_refs.Slot_obj, Vector3.zero, Quaternion.identity);
                        slot_5.name = "ModSlot (5)";
                        slot_5.transform.SetParent(__instance.transform);
                        slot_5_refs = Get_SlotRefs(5);
                        Set_SlotPositions(5);
                        if (!Refs_Manager.craft_slot_manager.affixSlots.IsNullOrDestroyed())
                        {
                            Refs_Manager.craft_slot_manager.affixSlots.Add(slot_4_refs.Slot_obj.GetComponent<AffixSlotForge>());
                            Refs_Manager.craft_slot_manager.affixSlots.Add(slot_5_refs.Slot_obj.GetComponent<AffixSlotForge>());
                        }

                        //Move seal
                        Vector3 seal_position = Refs_Manager.craft_slot_manager.sealedAffixHolder.transform.position;
                        Refs_Manager.craft_slot_manager.sealedAffixHolder.transform.position = new Vector3(seal_position.x, seal_position.y - 200, seal_position.z);

                        //Add Clear slot button (fix item stuck in forge)
                        GameObject craftingMatButton = Functions.GetChild(__instance, "ViewCraftingMaterialsButton");
                        float craftingMatButton_H = craftingMatButton.GetComponent<RectTransform>().rect.height;
                        float margin = 10;
                        GameObject FixStuckButton_obj = Object.Instantiate(craftingMatButton, new Vector3(craftingMatButton.transform.position.x, (craftingMatButton.transform.position.y - craftingMatButton_H - margin), craftingMatButton.transform.position.z), Quaternion.identity);
                        //GameObject FixStuckButton_obj = Object.Instantiate(craftingMatButton, new Vector3((craftingMatButton.transform.position.x + craftingMatButton_W + margin), craftingMatButton.transform.position.y, craftingMatButton.transform.position.z), Quaternion.identity);
                        FixStuckButton_obj.name = "UnstuckButton";
                        FixStuckButton_obj.transform.SetParent(__instance.transform);
                        FixStuckButton_obj.transform.localScale = craftingMatButton.transform.localScale;
                        Button FixStuckButton = FixStuckButton_obj.GetComponent<Button>();
                        FixStuckButton.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                        FixStuckButton.onClick.AddListener(Events.ClearSlot_OnClick_Action);
                        GameObject FixStuckButtonText_obj = Functions.GetChild(FixStuckButton_obj, "text");
                        TextMeshProUGUI FixStuckButtonText = FixStuckButtonText_obj.GetComponent<TextMeshProUGUI>();
                        FixStuckButtonText.text = "Fix : Clear Slot";

                        initialized = true;
                    }
                }
            }
            public static Slot_refs Get_SlotRefs(int slot)
            {
                Debug(false, "Ui.Get_SlotRefs(" + slot + ")");
                GameObject Slot_obj = null;
                GameObject shards = null;
                GameObject add_shard_btn = null;
                GameObject shard_icon = null;
                GameObject glass_lense = null;
                GameObject available_shards = null;
                GameObject available_shards_count = null;
                GameObject active_pathing = null;
                GameObject upgrade_available = null;
                GameObject upgrade_available_indicator = null;
                GameObject affix_desc_holder = null;
                GameObject drop_shadow = null;
                GameObject tier_level = null;
                GameObject separator = null;
                GameObject affix_name = null;

                string slot_name = "ModSlot";
                if (slot == 1) { slot_name = "ModSlot (1)"; }  //prefix_1
                else if (slot == 2) { slot_name = "ModSlot (2)"; }  //suffix_0
                else if (slot == 3) { slot_name = "ModSlot (3)"; }  //suffix_1
                else if (slot == 4) { slot_name = "ModSlot (4)"; }  //prefix_2
                else if (slot == 5) { slot_name = "ModSlot (5)"; }  //suffix_2

                if (!Refs_Manager.crafting_panel_ui.IsNullOrDestroyed())
                {
                    GameObject __instance = Functions.GetChild(Refs_Manager.crafting_panel_ui.gameObject, "MainContent");
                    if (!__instance.IsNullOrDestroyed())
                    {
                        Slot_obj = Functions.GetChild(__instance, slot_name);
                        if (!Slot_obj.IsNullOrDestroyed())
                        {
                            shards = Functions.GetChild(Slot_obj, "Shard");
                            if (!shards.IsNullOrDestroyed())
                            {
                                add_shard_btn = Functions.GetChild(shards, "addShardButton");
                                shard_icon = Functions.GetChild(shards, "ShardIcon");
                                glass_lense = Functions.GetChild(shards, "GlassLense");
                            }
                            available_shards = Functions.GetChild(Slot_obj, "AvailableShardsofSlottedType");
                            if (!available_shards.IsNullOrDestroyed())
                            {
                                available_shards_count = Functions.GetChild(available_shards, "Available Shard Count TMP");
                            }
                            active_pathing = Functions.GetChild(Slot_obj, "activePathing");
                            upgrade_available = Functions.GetChild(Slot_obj, "upgradeAvailable");
                            if (!upgrade_available.IsNullOrDestroyed())
                            {
                                upgrade_available_indicator = Functions.GetChild(upgrade_available, "Upgrade Available Indicator");
                            }
                            affix_desc_holder = Functions.GetChild(Slot_obj, "affixDescHolder");
                            if (!affix_desc_holder.IsNullOrDestroyed())
                            {
                                drop_shadow = Functions.GetChild(affix_desc_holder, "dropshadow");
                                tier_level = Functions.GetChild(affix_desc_holder, "tierLevel");
                                separator = Functions.GetChild(affix_desc_holder, "separator");
                                affix_name = Functions.GetChild(affix_desc_holder, "AffixName");
                            }
                        }
                    }
                }
                Slot_refs refs = new Slot_refs
                {
                    Slot_obj = Slot_obj,
                    shards = shards,
                    add_shard_btn = add_shard_btn,
                    shard_icon = shard_icon,
                    glass_lense = glass_lense,
                    available_shards = available_shards,
                    available_shards_count = available_shards_count,
                    active_pathing = active_pathing,
                    upgrade_available = upgrade_available,
                    upgrade_available_indicator = upgrade_available_indicator,
                    affix_desc_holder = affix_desc_holder,
                    drop_shadow = drop_shadow,
                    tier_level = tier_level,
                    separator = separator,
                    affix_name = affix_name
                };

                return refs;
            }            
            public static int Get_PlayerShardCount(int affix_id)
            {
                int count = 0;
                if (!Refs_Manager.player_golbal_data_tracker.IsNullOrDestroyed())
                {
                    foreach (SavedShard shard in Refs_Manager.player_golbal_data_tracker.stash.SavedShards)
                    {
                        if (shard.ShardType == affix_id) { count = shard.Quantity; break; }
                    }
                }
                Debug(false, "Ui.Get_PlayerShardCount(" + affix_id + ") = " + count);

                return count;
            }
            public static void Set_SlotPositions(int slot)
            {
                Debug(false, "Ui.Set_SlotPositions()" );
                if ((!Refs_Manager.crafting_panel_ui.IsNullOrDestroyed()) && ((slot == 4) || (slot == 5)))
                {
                    string slot_name = "ModSlot (4)";
                    Slot_refs refs_0 = slot_0_refs;
                    Slot_refs refs_1 = slot_1_refs;
                    if (slot == 5) { slot_name = "ModSlot (5)"; refs_0 = slot_2_refs; refs_1 = slot_3_refs; }
                    GameObject __instance = Functions.GetChild(Refs_Manager.crafting_panel_ui.gameObject, "MainContent");
                    if (!__instance.IsNullOrDestroyed())
                    {
                        GameObject Slot_obj = Functions.GetChild(__instance, slot_name);
                        if (!Slot_obj.IsNullOrDestroyed())
                        {
                            Slot_obj.transform.position = refs_1.Slot_obj.transform.position - (refs_0.Slot_obj.transform.position - refs_1.Slot_obj.transform.position);
                            GameObject shards = Functions.GetChild(Slot_obj, "Shard");
                            if (!shards.IsNullOrDestroyed())
                            {
                                shards.transform.position = refs_1.shards.transform.position - (refs_0.shards.transform.position - refs_1.shards.transform.position);
                                GameObject add_shard_btn = Functions.GetChild(shards, "addShardButton");
                                if ((!add_shard_btn.IsNullOrDestroyed()) && (!refs_0.add_shard_btn.IsNullOrDestroyed()) && (!refs_1.add_shard_btn.IsNullOrDestroyed()))
                                {
                                    add_shard_btn.transform.position = refs_1.add_shard_btn.transform.position - (refs_0.add_shard_btn.transform.position - refs_1.add_shard_btn.transform.position);
                                }
                                else { Debug(true, "Error : AddShard"); }
                                GameObject shard_icon = Functions.GetChild(shards, "ShardIcon");
                                if ((!shard_icon.IsNullOrDestroyed()) && (!refs_0.shard_icon.IsNullOrDestroyed()) && (!refs_1.shard_icon.IsNullOrDestroyed()))
                                {
                                    shard_icon.transform.position = refs_1.shard_icon.transform.position - (refs_0.shard_icon.transform.position - refs_1.shard_icon.transform.position);
                                }
                                else { Debug(true, "Error : ShardIcon"); }
                                GameObject glass_lense = Functions.GetChild(shards, "GlassLense");
                                if ((!glass_lense.IsNullOrDestroyed()) && (!refs_0.glass_lense.IsNullOrDestroyed()) && (!refs_1.glass_lense.IsNullOrDestroyed()))
                                {
                                    glass_lense.transform.position = refs_1.glass_lense.transform.position - (refs_0.glass_lense.transform.position - refs_1.glass_lense.transform.position);
                                }
                                else { Debug(true, "Error : glass_lense"); }
                            }
                            else { Debug(true, "Error : Shard"); }
                            GameObject available_shard = Functions.GetChild(Slot_obj, "AvailableShardsofSlottedType");
                            if ((!available_shard.IsNullOrDestroyed()) && (!refs_0.available_shards.IsNullOrDestroyed()) && (!refs_1.available_shards.IsNullOrDestroyed()))
                            {
                                available_shard.transform.position = refs_1.available_shards.transform.position - (refs_0.available_shards.transform.position - refs_1.available_shards.transform.position);
                                GameObject available_shard_count = Functions.GetChild(available_shard, "Available Shard Count TMP");
                                if ((!available_shard_count.IsNullOrDestroyed()) && (!refs_0.available_shards_count.IsNullOrDestroyed()) && (!refs_1.available_shards_count.IsNullOrDestroyed()))
                                {
                                    available_shard_count.transform.position = refs_1.available_shards_count.transform.position - (refs_0.available_shards_count.transform.position - refs_1.available_shards_count.transform.position);
                                }
                                else { Debug(true, "Error : available_shard_count"); }
                            }
                            else { Debug(true, "Error : available_shard"); }
                            GameObject active_pathing = Functions.GetChild(Slot_obj, "activePathing");
                            if ((!active_pathing.IsNullOrDestroyed()) && (!refs_0.active_pathing.IsNullOrDestroyed()) && (!refs_1.active_pathing.IsNullOrDestroyed()))
                            {
                                active_pathing.transform.position = refs_1.active_pathing.transform.position - (refs_0.active_pathing.transform.position - refs_1.active_pathing.transform.position) - new Vector3(0, 68, 0); 
                                
                            }
                            else { Debug(true, "Error : active_pathing"); }
                            GameObject upgrade_available = Functions.GetChild(Slot_obj, "upgradeAvailable");
                            if ((!upgrade_available.IsNullOrDestroyed()) && (!refs_0.upgrade_available.IsNullOrDestroyed()) && (!refs_1.upgrade_available.IsNullOrDestroyed()))
                            {
                                upgrade_available.transform.position = refs_1.upgrade_available.transform.position - (refs_0.upgrade_available.transform.position - refs_1.upgrade_available.transform.position);
                                GameObject upgrade_available_indicator = Functions.GetChild(upgrade_available, "Upgrade Available Indicator");
                                if ((!upgrade_available_indicator.IsNullOrDestroyed()) && (!refs_0.upgrade_available_indicator.IsNullOrDestroyed()) && (!refs_1.upgrade_available_indicator.IsNullOrDestroyed()))
                                {
                                    upgrade_available_indicator.transform.position = refs_1.upgrade_available_indicator.transform.position - (refs_0.upgrade_available_indicator.transform.position - refs_1.upgrade_available_indicator.transform.position);
                                }
                                else { Debug(true, "Error : upgrade_available_indicator"); }

                                if (upgrade_available_indicator.IsNullOrDestroyed()) { Debug(true, "Error : upgrade_available_indicator"); }
                                if (refs_0.upgrade_available_indicator.IsNullOrDestroyed()) { Debug(true, "Error : refs_0.upgrade_available_indicator"); }
                                if (refs_1.upgrade_available_indicator.IsNullOrDestroyed()) { Debug(true, "Error : refs_1.upgrade_available_indicator"); }
                            }
                            else { Debug(true, "Error : upgradeAvailable"); }
                            GameObject affix_desc_holder = Functions.GetChild(Slot_obj, "affixDescHolder");
                            if ((!affix_desc_holder.IsNullOrDestroyed()) && (!refs_0.affix_desc_holder.IsNullOrDestroyed()) && (!refs_1.affix_desc_holder.IsNullOrDestroyed()))
                            {
                                affix_desc_holder.transform.position = refs_1.affix_desc_holder.transform.position - (refs_0.affix_desc_holder.transform.position - refs_1.affix_desc_holder.transform.position);
                                GameObject drop_shadow = Functions.GetChild(affix_desc_holder, "dropshadow");
                                if ((!drop_shadow.IsNullOrDestroyed()) && (!refs_0.drop_shadow.IsNullOrDestroyed()) && (!refs_1.drop_shadow.IsNullOrDestroyed()))
                                {
                                    drop_shadow.transform.position = refs_1.drop_shadow.transform.position - (refs_0.drop_shadow.transform.position - refs_1.drop_shadow.transform.position);
                                }
                                else { Debug(true, "Error : dropshadow"); }
                                GameObject tier_level = Functions.GetChild(affix_desc_holder, "tierLevel");
                                if ((!tier_level.IsNullOrDestroyed()) && (!refs_0.tier_level.IsNullOrDestroyed()) && (!refs_1.tier_level.IsNullOrDestroyed()))
                                {
                                    tier_level.transform.position = refs_1.tier_level.transform.position - (refs_0.tier_level.transform.position - refs_1.tier_level.transform.position);
                                }
                                else { Debug(true, "Error : tierLevel"); }
                                GameObject separator = Functions.GetChild(affix_desc_holder, "separator");
                                if ((!separator.IsNullOrDestroyed()) && (!refs_0.separator.IsNullOrDestroyed()) && (!refs_1.separator.IsNullOrDestroyed()))
                                {
                                    separator.transform.position = refs_1.separator.transform.position - (refs_0.separator.transform.position - refs_1.separator.transform.position);
                                }
                                else { Debug(true, "Error : separator"); }
                                GameObject affix_name = Functions.GetChild(affix_desc_holder, "AffixName");
                                if ((!affix_name.IsNullOrDestroyed()) && (!refs_0.affix_name.IsNullOrDestroyed()) && (!refs_1.affix_name.IsNullOrDestroyed()))
                                {
                                    affix_name.transform.position = refs_1.affix_name.transform.position - (refs_0.affix_name.transform.position - refs_1.affix_name.transform.position);
                                }
                                else { Debug(true, "Error : AffixName"); }
                            }
                            else { Debug(true, "Error : affixDescHolder"); }
                        }
                    }
                }
            }
            public static void Set_SlotValues(Slot_refs slot_refs, ushort id, byte tier)
            {
                Debug(false, "Ui.Set_SlotValues(" + slot_refs.Slot_obj.name + "), id = " + id + ", tier = " + tier);
                bool idol = Get.IsIdol(Current.item);
                bool unique = Current.item.isUniqueSetOrLegendary();
                int applied_Id = -1;
                if (!Crafting_Manager_instance.IsNullOrDestroyed())  { applied_Id = Crafting_Manager_instance.appliedAffixID; }
                int shard_count = Get_PlayerShardCount(id);
                slot_refs.Slot_obj.active = true;
                slot_refs.Slot_obj.GetComponent<AffixSlotForge>().affixID = id;
                slot_refs.shards.active = true;
                slot_refs.add_shard_btn.active = false;
                slot_refs.shard_icon.active = true;
                slot_refs.shard_icon.GetComponent<UnityEngine.UI.Image>().sprite = AffixList.instance.GetAffix(id).GetShardSprite();
                slot_refs.glass_lense.active = true;
                slot_refs.available_shards.active = true;
                slot_refs.affix_desc_holder.active = true;
                slot_refs.drop_shadow.active = true;
                slot_refs.tier_level.active = true;
                slot_refs.tier_level.GetComponent<TextMeshProUGUI>().text = "T" + (tier + 1);
                slot_refs.separator.active = true;
                slot_refs.affix_name.active = true;
                slot_refs.affix_name.GetComponent<TextMeshProUGUI>().text = AffixList.instance.GetAffix(id).affixName;
                slot_refs.active_pathing.GetComponent<UnityEngine.UI.Image>().maskable = true;

                if (idol)
                {
                    slot_refs.available_shards_count.active = false;
                    slot_refs.upgrade_available.active = false;
                    slot_refs.upgrade_available_indicator.active = false;
                }
                else
                {
                    slot_refs.available_shards_count.active = true;
                    slot_refs.available_shards_count.GetComponent<TextMeshProUGUI>().text = shard_count.ToString();
                    if ((tier < 4) || ((tier < 6) && (CanCraftToT7)))
                    {
                        if ((unique) || ((!unique) && ((CanCraftWithoutShard) || (shard_count > 0))))
                        {
                            slot_refs.upgrade_available.active = true;
                            slot_refs.upgrade_available_indicator.active = true;
                            if (id == applied_Id)
                            {
                                slot_refs.active_pathing.GetComponent<UnityEngine.UI.Image>().fillAmount = 1;
                                slot_refs.active_pathing.active = true;
                            }
                            else
                            {
                                slot_refs.active_pathing.GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
                                slot_refs.active_pathing.active = false;
                            }
                        }
                        else
                        {
                            slot_refs.upgrade_available.active = false;
                            slot_refs.upgrade_available_indicator.active = false;
                            slot_refs.active_pathing.GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
                            slot_refs.active_pathing.active = false;
                            //Disable Forge btn
                        }
                    }
                    else
                    {
                        slot_refs.upgrade_available.active = false;
                        slot_refs.upgrade_available_indicator.active = false;
                        slot_refs.active_pathing.GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
                        slot_refs.active_pathing.active = false;
                    }
                }
                
                //Events
                UnityEngine.UI.Button upgrade_available_btn = slot_refs.upgrade_available.GetComponent<UnityEngine.UI.Button>();
                upgrade_available_btn.interactable = true;

                if (slot_refs.Slot_obj.name == slot_0_refs.Slot_obj.name)
                {
                    upgrade_available_btn.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                    upgrade_available_btn.onClick.AddListener(Events.Slot_0_UpgradeAvailable_OnClick_Action);
                }
                else if (slot_refs.Slot_obj.name == slot_1_refs.Slot_obj.name)
                {
                    upgrade_available_btn.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                    upgrade_available_btn.onClick.AddListener(Events.Slot_1_UpgradeAvailable_OnClick_Action);
                }
                else if (slot_refs.Slot_obj.name == slot_2_refs.Slot_obj.name)
                {
                    upgrade_available_btn.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                    upgrade_available_btn.onClick.AddListener(Events.Slot_2_UpgradeAvailable_OnClick_Action);
                }
                else if (slot_refs.Slot_obj.name == slot_3_refs.Slot_obj.name)
                {
                    upgrade_available_btn.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                    upgrade_available_btn.onClick.AddListener(Events.Slot_3_UpgradeAvailable_OnClick_Action);
                }
                else if (slot_refs.Slot_obj.name == slot_4_refs.Slot_obj.name)
                {
                    upgrade_available_btn.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                    upgrade_available_btn.onClick.AddListener(Events.Slot_4_UpgradeAvailable_OnClick_Action);
                }
                else if (slot_refs.Slot_obj.name == slot_5_refs.Slot_obj.name)
                {
                    upgrade_available_btn.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                    upgrade_available_btn.onClick.AddListener(Events.Slot_5_UpgradeAvailable_OnClick_Action);
                }
            }
            public static void Set_SlotNoAffix(Slot_refs slot_refs, bool enable)
            {
                Debug(false, "Ui.Set_SlotNoAffix(" + slot_refs.Slot_obj.name + ")");
                if (enable)
                {
                    slot_refs.Slot_obj.active = true;
                    slot_refs.shards.active = true;
                    slot_refs.add_shard_btn.active = true;
                    slot_refs.add_shard_btn.GetComponent<UnityEngine.UI.Button>().interactable = true;
                    slot_refs.shard_icon.active = false;
                    slot_refs.glass_lense.active = false;
                    slot_refs.available_shards.active = false;
                    slot_refs.active_pathing.active = false;
                    slot_refs.upgrade_available.active = false;
                    slot_refs.affix_desc_holder.active = false;
                }
                else
                {
                    slot_refs.Slot_obj.active = false;
                    slot_refs.shards.active = false;
                    slot_refs.available_shards.active = false;
                    slot_refs.active_pathing.active = false;
                    slot_refs.upgrade_available.active = false;
                    slot_refs.affix_desc_holder.active = false;
                }
            }
            public static void Set_SlotNewAffixs(Slot_refs slot_refs, int id)
            {
                Debug(false, "Ui.Set_SlotNewAffixs(" + slot_refs.Slot_obj.name + "), id = " + id);
                bool idol = Get.IsIdol(Current.item);
                int shard_count = Get_PlayerShardCount(id);
                slot_refs.Slot_obj.active = true;
                slot_refs.Slot_obj.GetComponent<AffixSlotForge>().affixID = id;
                slot_refs.shards.active = true;
                slot_refs.add_shard_btn.active = false;
                slot_refs.shard_icon.active = true;
                slot_refs.shard_icon.GetComponent<UnityEngine.UI.Image>().sprite = AffixList.instance.GetAffix(id).GetShardSprite();
                slot_refs.glass_lense.active = false;
                if (idol)
                {
                    slot_refs.available_shards.active = false;
                    slot_refs.available_shards_count.active = false;
                }
                else
                {
                    slot_refs.available_shards.active = true;
                    slot_refs.available_shards_count.active = true;
                    slot_refs.available_shards_count.GetComponent<TextMeshProUGUI>().text = shard_count.ToString();
                }

                slot_refs.active_pathing.GetComponent<UnityEngine.UI.Image>().maskable = true;
                slot_refs.active_pathing.GetComponent<UnityEngine.UI.Image>().fillAmount = 1;
                slot_refs.active_pathing.active = true;
                slot_refs.upgrade_available.active = false;
                slot_refs.affix_desc_holder.active = true;
                slot_refs.drop_shadow.active = true;
                slot_refs.tier_level.active = true;
                slot_refs.tier_level.GetComponent<TextMeshProUGUI>().text = "T1";
                slot_refs.separator.active = true;
                slot_refs.affix_name.active = true;
                slot_refs.affix_name.GetComponent<TextMeshProUGUI>().text = AffixList.instance.GetAffix(id).affixName;
            }
            public static void ShowHide_MainPath(bool show)
            {
                //Debug(false, "Ui.ShowHide_MainPath(" + show + ")");
                if (!Refs_Manager.craft_slot_manager.IsNullOrDestroyed())
                {
                    Refs_Manager.craft_slot_manager.modifierPathing.gameObject.active = show;
                    Refs_Manager.craft_slot_manager.itemPathing.gameObject.active = show;
                    Refs_Manager.craft_slot_manager.itemPathingExtraGlow.gameObject.active = show;
                }
            }
            public static bool Invoke_UpgradeAvailable(CraftingSlotManager __instance, int affix_id )
            {
                bool already = false;
                if (!Current.item.IsNullOrDestroyed())
                {
                    Debug(false, "Ui.Invoke_UpgradeAvailable(), Id = " + affix_id);                    
                    bool idol = Get.IsIdol(Current.item);
                    if (!idol)
                    {
                        foreach (AffixSlotForge affix_slot in __instance.affixSlots)
                        {
                            if (affix_slot.affixID == affix_id)
                            {
                                GameObject go = Functions.GetChild(affix_slot.gameObject, "upgradeAvailable");
                                if (!go.IsNullOrDestroyed())
                                {
                                    if (go.active)
                                    {
                                        go.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                                        already = true;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }

                return already;
            }
            public static void Invoke_ShardElementBtn()
            {
                Debug(false, "Ui.Invoke_ShardElementBtn()");
                if ((!Crafting_Materials_Panel_UI.instance.IsNullOrDestroyed()) && (!Crafting_Manager_instance.IsNullOrDestroyed()))
                {
                    foreach (ShardAffixListElement affix_element in Crafting_Materials_Panel_UI.instance.shardAffixList)
                    {
                        if (affix_element.affix.affixId == Crafting_Manager_instance.appliedAffixID)
                        {
                            GameObject go = Functions.GetChild(affix_element.gameObject, "Button");
                            if (!go.IsNullOrDestroyed()) { go.GetComponent<UnityEngine.UI.Button>().onClick.Invoke(); }
                            break;
                        }
                    }
                }
            }

            public static void Update()
            {
                if (Enable)
                {
                    if (!updating)
                    {
                        updating = true;

                        //Forge Panel Update
                        if (Crafting_Main_Ui.IsOpen)
                        {
                            Slot_refs[] prefixs = { slot_0_refs, slot_1_refs, slot_4_refs };
                            bool[] contain_prefix = { false, false, false };
                            Slot_refs[] suffixs = { slot_2_refs, slot_3_refs, slot_5_refs };
                            bool[] contain_suffix = { false, false, false };
                            int id = -1;
                            if (!Crafting_Manager_instance.IsNullOrDestroyed()) { id = Crafting_Manager_instance.appliedAffixID; }
                            if (id > -1) { ShowHide_MainPath(true); }
                            else { ShowHide_MainPath(false); }
                            if (!Current.item.IsNullOrDestroyed()) //Update slot values
                            {
                                if ((Current.item.affixes != backup_affixs) || (force_update_slots))
                                {
                                    if (force_update_slots) { Debug(false, "Force Update Slots, id = " + id); }
                                    else { Debug(false, "Updated Item.Affixes, id = " + id); }

                                    force_update_slots = false;

                                    backup_affixs = Current.item.affixes;
                                    already_reset = false;
                                    int p = 0;
                                    int s = 0;
                                    //int i = 0;
                                    Il2CppSystem.Collections.Generic.List<int> affix_list = new Il2CppSystem.Collections.Generic.List<int>();
                                    foreach (ItemAffix affix in Current.item.affixes)
                                    {
                                        if ((affix.affixType == AffixList.AffixType.PREFIX) && (!affix.isSealedAffix))
                                        {
                                            Debug(false, "Prefix " + p + ", id = " + affix.affixId + ", tier = " + affix.affixTier);
                                            if (p < prefixs.Length) //3
                                            {
                                                Set_SlotValues(prefixs[p], affix.affixId, affix.affixTier);
                                                contain_prefix[p] = true;
                                                affix_list.Add(affix.affixId);
                                            }
                                            p++;
                                        }
                                        else if ((affix.affixType == AffixList.AffixType.SUFFIX) && (!affix.isSealedAffix))
                                        {
                                            Debug(false, "Suffix " + s + ", id = " + affix.affixId + ", tier = " + affix.affixTier);
                                            if (s < suffixs.Length) //3
                                            {
                                                Set_SlotValues(suffixs[s], affix.affixId, affix.affixTier);
                                                contain_suffix[s] = true;
                                                affix_list.Add(affix.affixId);
                                            }
                                            s++;
                                        }
                                        else if (affix.isSealedAffix)
                                        {
                                            //
                                        }
                                    }

                                    bool first = true;
                                    for (int j = 0; j < contain_prefix.Length; j++)
                                    {
                                        if (contain_prefix[j] == false)
                                        {
                                            if (first)
                                            {
                                                if (id > -1)
                                                {
                                                    AffixList.AffixType affix_type = AffixList.instance.GetAffix(id).type;
                                                    if ((!affix_list.Contains(id)) && (affix_type == AffixList.AffixType.PREFIX))
                                                    {
                                                        Debug(false, "prefix " + j + ", NewAffix(" + id + ")");
                                                        Set_SlotNewAffixs(prefixs[j], id);
                                                        first = false;
                                                    }
                                                    else
                                                    {
                                                        Debug(false, "prefix " + j + ", NoAffix() Craftable");
                                                        Set_SlotNoAffix(prefixs[j], first);
                                                        first = false;
                                                    }
                                                }
                                                else
                                                {
                                                    Debug(false, "prefix " + j + ", NoAffix() Craftable");
                                                    Set_SlotNoAffix(prefixs[j], first);
                                                    first = false;
                                                }
                                            }
                                            else
                                            {
                                                Debug(false, "prefix " + j + ", NoAffix()");
                                                Set_SlotNoAffix(prefixs[j], first);
                                            }
                                        }
                                    }

                                    first = true;
                                    for (int j = 0; j < contain_suffix.Length; j++)
                                    {
                                        if ((contain_suffix[j] == false) && (j < suffixs.Length))
                                        {
                                            if (first)
                                            {
                                                if (id > -1)
                                                {
                                                    AffixList.AffixType affix_type = AffixList.instance.GetAffix(id).type;
                                                    if ((!affix_list.Contains(id)) && (affix_type == AffixList.AffixType.SUFFIX))
                                                    {
                                                        Debug(false, "suffix " + j + ", NewAffix(" + id + ")");
                                                        Set_SlotNewAffixs(suffixs[j], id);
                                                        first = false;
                                                    }
                                                    else
                                                    {
                                                        Debug(false, "suffix " + j + ", NoAffix() Craftable");
                                                        Set_SlotNoAffix(suffixs[j], first);
                                                        first = false;
                                                    }
                                                }
                                                else
                                                {
                                                    Debug(false, "suffix " + j + ", NoAffix() Craftable");
                                                    Set_SlotNoAffix(suffixs[j], first);
                                                    first = false;
                                                }
                                            }
                                            else
                                            {
                                                Debug(false, "suffix " + j + ", NoAffix()");
                                                Set_SlotNoAffix(suffixs[j], first);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (!already_reset)
                            {
                                Debug(false, "Reset Slots");
                                already_reset = true;
                                backup_affixs = null;
                                int i = 0;
                                bool first = true;
                                foreach (bool r in contain_prefix)
                                {
                                    if ((r == false) && (i < prefixs.Length))
                                    {
                                        Debug(false, "prefix " + i + ", NewAffix");
                                        Set_SlotNoAffix(prefixs[i], first);
                                        first = false;
                                    }
                                    i++;
                                }
                                i = 0;
                                first = true;
                                foreach (bool r in contain_suffix)
                                {
                                    if ((r == false) && (i < suffixs.Length))
                                    {
                                        Debug(false, "suffix " + i + ", NewAffix");
                                        Set_SlotNoAffix(suffixs[i], first);
                                        first = false;
                                    }
                                    i++;
                                }
                                ShowHide_MainPath(false);
                            }
                        }

                        //Materials Panel Update
                        if (Crafting_Materials_Panel_UI.IsOpen)
                        {
                            if (!Crafting_Materials_Panel_UI.instance.IsNullOrDestroyed()) //Update text filter
                            {
                                if (shards_text_filter != Crafting_Materials_Panel_UI.instance.searchText)
                                {
                                    Debug(false, "Updated Text filter");
                                    shards_text_filter = Crafting_Materials_Panel_UI.instance.searchText;
                                    Crafting_Materials_Panel_UI.instance.RefreshAffixList();
                                }
                            }
                        }

                        updating = false;
                    }
                }
            }
        }
        public class Events
        {
            public static readonly System.Action Slot_0_UpgradeAvailable_OnClick_Action = new System.Action(UpgradeAvailable_0_Click);
            private static void UpgradeAvailable_0_Click()
            {
                Debug(false, "Prefix 1, slot 0 Click()");
                if (!Crafting_Manager_instance.IsNullOrDestroyed())
                {
                    Crafting_Manager_instance.appliedAffixID = Ui.slot_0_refs.Slot_obj.GetComponent<AffixSlotForge>().affixID;
                    Ui.Invoke_ShardElementBtn();
                    //Ui.force_update_slots = true;
                }
            }
            public static readonly System.Action Slot_1_UpgradeAvailable_OnClick_Action = new System.Action(UpgradeAvailable_1_Click);
            private static void UpgradeAvailable_1_Click()
            {
                Debug(false, "Prefix 2, slot 1 Click()");
                if (!Crafting_Manager_instance.IsNullOrDestroyed())
                {
                    Crafting_Manager_instance.appliedAffixID = Ui.slot_1_refs.Slot_obj.GetComponent<AffixSlotForge>().affixID;
                    Ui.Invoke_ShardElementBtn();
                }
            }
            public static readonly System.Action Slot_2_UpgradeAvailable_OnClick_Action = new System.Action(UpgradeAvailable_2_Click);
            private static void UpgradeAvailable_2_Click()
            {
                Debug(false, "Suffix 1, slot 2 Click()");
                if (!Crafting_Manager_instance.IsNullOrDestroyed())
                {
                    Crafting_Manager_instance.appliedAffixID = Ui.slot_2_refs.Slot_obj.GetComponent<AffixSlotForge>().affixID;
                    Ui.Invoke_ShardElementBtn();
                }
            }
            public static readonly System.Action Slot_3_UpgradeAvailable_OnClick_Action = new System.Action(UpgradeAvailable_3_Click);
            private static void UpgradeAvailable_3_Click()
            {
                Debug(false, "Suffix 2, slot 3 Click()");
                if (!Crafting_Manager_instance.IsNullOrDestroyed())
                {
                    Crafting_Manager_instance.appliedAffixID = Ui.slot_3_refs.Slot_obj.GetComponent<AffixSlotForge>().affixID;
                    Ui.Invoke_ShardElementBtn();
                }
            }
            public static readonly System.Action Slot_4_UpgradeAvailable_OnClick_Action = new System.Action(UpgradeAvailable_4_Click);
            private static void UpgradeAvailable_4_Click()
            {
                
                if (!Crafting_Manager_instance.IsNullOrDestroyed())
                {
                    Debug(false, "prefix 3, slot 4 Click(), id = " + Ui.slot_4_refs.Slot_obj.GetComponent<AffixSlotForge>().affixID);
                    Crafting_Manager_instance.appliedAffixID = Ui.slot_4_refs.Slot_obj.GetComponent<AffixSlotForge>().affixID;
                    Ui.Invoke_ShardElementBtn();
                }
            }
            public static readonly System.Action Slot_5_UpgradeAvailable_OnClick_Action = new System.Action(UpgradeAvailable_5_Click);
            private static void UpgradeAvailable_5_Click()
            {                
                if (!Crafting_Manager_instance.IsNullOrDestroyed())
                {
                    Debug(false, "suffix 3, slot 5 Click(), id = " + Ui.slot_5_refs.Slot_obj.GetComponent<AffixSlotForge>().affixID);
                    Crafting_Manager_instance.appliedAffixID = Ui.slot_5_refs.Slot_obj.GetComponent<AffixSlotForge>().affixID;
                    Ui.Invoke_ShardElementBtn();
                }
            }
            public static readonly System.Action ClearSlot_OnClick_Action = new System.Action(ClearSlot_Click);
            private static void ClearSlot_Click() //Fix Clear Slot
            {
                if (!Current.item_container.IsNullOrDestroyed())
                {
                    if (!Current.item_container.content.IsNullOrDestroyed()) { Current.item_container.Clear(); }
                }
            }
        }
        public class Ui_Base
        {
            [HarmonyPatch(typeof(UIBase), "openCraftingPanel")]
            public class UIBase_openCraftingPanel
            {
                [HarmonyPostfix]
                static void Postifx()
                {
                    Debug(false, "UIBase.openCraftingPanel()");
                    if (Enable)
                    {
                        Crafting_Main_Ui.IsOpen = true;
                        if (!Ui.initialized) { Ui.Init(); }
                        Ui.force_update_slots = true; //Force Update
                    }
                    else { Debug(false, "Disable"); }
                }
            }
            
            [HarmonyPatch(typeof(UIBase), "closeCraftingPanel")]
            public class UIBase_closeCraftingPanel
            {
                [HarmonyPostfix]
                static void Postifx()
                {
                    Debug(false, "UIBase.closeCraftingPanel()");
                    if (Enable) { Crafting_Main_Ui.IsOpen = false; }
                    else { Debug(false, "Disable"); }
                }
            }
        }
        public class Crafting_Manager
        {
            public static string forge_string = "Forge Mod";
            public static string rune_of_discovery_string = "Discovery";
            public static string latest_string = "";

            //Select Item //CraftingSlot
            [HarmonyPatch(typeof(CraftingManager), "OnMainItemChange")]
            public class CraftingManager_OnMainItemChange
            {
                [HarmonyPostfix]
                static void Postfix(ref CraftingManager __instance, ref Il2CppSystem.Object __0, ref ItemContainerEntryHandler __1)
                {
                    Debug(false, "CraftingManager.OnMainItemChange()");
                    if (Enable)
                    {                        
                        if (Crafting_Manager_instance.IsNullOrDestroyed()) { Crafting_Manager_instance = __instance; }
                        ;
                        if (!__0.IsNullOrDestroyed())
                        {
                            Current.item = null;                            
                            OneItemContainer item_container = __0.TryCast<OneItemContainer>();
                            if (!item_container.IsNullOrDestroyed())
                            {
                                Current.item_container = item_container;
                                if (!item_container.content.IsNullOrDestroyed()) { Current.item = item_container.content.data; }
                            }
                            else { Current.item_container = null; }
                        }
                    }
                    else { Debug(false, "Disable"); }
                }
            }

            [HarmonyPatch(typeof(CraftingManager), "OnMainItemRemoved")]
            public class CraftingManager_OnMainItemRemoved
            {
                [HarmonyPostfix]
                static void Postfix(CraftingManager __instance, Il2CppSystem.Object __0, ItemContainerEntryHandler __1)
                {
                    Debug(false, "CraftingManager.OnMainItemRemoved()");
                    if (Enable)
                    {                        
                        if (Crafting_Manager_instance.IsNullOrDestroyed()) { Crafting_Manager_instance = __instance; }
                        Current.item = null;
                        Current.slot = null;
                        Current.btn = null;
                        Current.item_container = null;
                    }
                    else { Debug(false, "Disable"); }
                }
            }

            //Unlock Craft //BypassReq
            [HarmonyPatch(typeof(CraftingManager), "CheckForgeCapability")]
            public class CheckForgeCapability
            {
                [HarmonyPostfix]
                static void Postfix(ref CraftingManager __instance, ref bool __result, ref System.String __0, ref System.Boolean __1)
                {
                    string before = __0;
                    if (Enable)
                    {                        
                        if (Crafting_Manager_instance.IsNullOrDestroyed()) { Crafting_Manager_instance = __instance; }
                        ;
                        if ((!Refs_Manager.craft_slot_manager.IsNullOrDestroyed()) && (!Current.item.IsNullOrDestroyed()))
                        {
                            int affix_id = __instance.appliedAffixID;
                            int affix_tier = Get.Tier(Current.item, affix_id);
                            if ((__0 == Locales.cant_craft_unique) && (CanCraftUniqueSet))
                            {
                                if ((affix_id > -1) && (affix_tier < 6))
                                {
                                    Refs_Manager.craft_slot_manager.prefixFullOfMax = Get.IsPrefixFull(Current.item);
                                    Refs_Manager.craft_slot_manager.suffixFullOfMax = Get.IsSuffixFull(Current.item);
                                    Refs_Manager.craft_slot_manager.canForge = true;
                                    __0 = forge_string; //string
                                    __1 = false; //warning
                                    __result = true;
                                }
                                else if (affix_id == -1) { __0 = "Select an affix"; }
                                else if (affix_tier > 5) { __0 = "Maxed"; } //Tier = 0-6
                            }
                            else if ((__0 == Locales.cant_craft_idol) && (CanCraftIdols))
                            {
                                if ((affix_id > -1) && (affix_tier < 1)) //Tier max = 1 for idols
                                {
                                    Refs_Manager.craft_slot_manager.prefixFullOfMax = Get.IsPrefixFull(Current.item);
                                    Refs_Manager.craft_slot_manager.suffixFullOfMax = Get.IsSuffixFull(Current.item);
                                    Refs_Manager.craft_slot_manager.canForge = true;
                                    __0 = forge_string;
                                    __1 = false;
                                    __result = true;
                                }
                                else if (affix_id == -1) { __0 = "Select an affix"; }
                                else if (affix_tier > 0) { __0 = "Maxed"; }
                            }
                            else if (__0 == Locales.no_space_affix)
                            {
                                bool full = Get.IsAffixFull(Current.item);
                                Refs_Manager.craft_slot_manager.suffixFullOfMax = full;
                                if (!full)
                                {
                                    __0 = forge_string;
                                    __1 = false;
                                    __result = true;
                                }
                            }
                            else if (__0 == Locales.no_space_prefix)
                            {
                                bool full = Get.IsPrefixFull(Current.item);
                                Refs_Manager.craft_slot_manager.suffixFullOfMax = full;
                                if (!full)
                                {
                                    __0 = forge_string;
                                    __1 = false;
                                    __result = true;
                                }
                            }
                            else if (__0 == Locales.no_space_suffix)
                            {
                                bool full = Get.IsSuffixFull(Current.item);
                                Refs_Manager.craft_slot_manager.suffixFullOfMax = full;
                                if (!full)
                                {
                                    __0 = forge_string;
                                    __1 = false;
                                    __result = true;
                                }
                            }
                            else if (__0 == Locales.affix_is_maxed)
                            {
                                if ((affix_tier > 3) && (affix_tier < 6) && (CanCraftToT7))
                                {
                                    Refs_Manager.craft_slot_manager.prefixFullOfMax = Get.IsPrefixFull(Current.item);
                                    Refs_Manager.craft_slot_manager.suffixFullOfMax = Get.IsSuffixFull(Current.item);
                                    __0 = forge_string;
                                    __1 = false;
                                    __result = true;
                                }
                                // if ((affix_tier > 5) && (CanCraftToT7)) { __0 = "Maxed"; }
                            }
                        }                        
                        latest_string = __0;
                    }
                    Debug(false, "CraftingManager.CheckForgeCapability() before = " + before + ", after = " + __0 + ", result = " + __result);
                }
            }
        }
        public class Crafting_Main_Ui
        {
            public static bool IsOpen = false;

            [HarmonyPatch(typeof(CraftingMainUI), "Initialize")]
            public class CraftingMainUI_Initialize
            {
                [HarmonyPostfix]
                static void Postifx(ref CraftingMainUI __instance, ref bool __result)
                {
                    Debug(false, "CraftingMainUI.Initialize()");
                    if (Enable)
                    {
                        Crafting_Main_Item_Container.rect_transform = __instance.gameObject.GetComponent<RectTransform>();
                    }
                    else { Debug(false, "Disable"); }
                }
            }
        }
        public class Crafting_Main_Item_Container
        {
            public static CraftingMainItemContainer main_item_container = null;
            public static RectTransform rect_transform = null;
            public static Vector2Int default_size = Vector2Int.zero;
            public static Vector2 default_sizedelta = Vector2.zero;
            public static Vector2 default_localscale = Vector2.zero;
            public static bool backup_initialized = false;

            //Fix slot size
            [HarmonyPatch(typeof(OneSlotItemContainer), "TryAddItem", new System.Type[] { typeof(ItemData), typeof(int), typeof(Context) })]
            public class OneSlotItemContainer_TryAddItem
            {
                [HarmonyPrefix]
                static void Prefix(ref OneSlotItemContainer __instance, bool __result, ItemData __0)
                {
                    Debug(false, "OneSlotItemContainer.TryAddItem()");
                    if (Enable)
                    {
                        if ((Scenes.IsGameScene()) && (!__0.IsNullOrDestroyed()))
                        {
                            if ((__instance.ToString() == "CraftingMainItemContainer") && (!rect_transform.IsNullOrDestroyed()))
                            {
                                if (!backup_initialized)
                                {
                                    default_size = __instance.size;
                                    default_sizedelta = rect_transform.sizeDelta;
                                    default_localscale = rect_transform.localScale;
                                    backup_initialized = true;
                                }
                                if (backup_initialized)
                                {
                                    if (((__0.itemType == 29) || (__0.itemType == 31)) && (CanCraftIdols))
                                    {
                                        __instance.size = new Vector2Int((2 * default_size.x), default_size.y);
                                        rect_transform.sizeDelta = new Vector2((2 * default_sizedelta.x), default_sizedelta.y);
                                        rect_transform.localScale = new Vector3((default_localscale.x / 2), default_localscale.y);
                                    }
                                    else
                                    {
                                        __instance.size = default_size;
                                        rect_transform.sizeDelta = default_sizedelta;
                                        rect_transform.localScale = default_localscale;
                                    }
                                }
                            }
                        }
                    }
                    else { Debug(false, "Disable"); }
                }
            }

            //Idols can be added to slot
            [HarmonyPatch(typeof(CraftingMainItemContainer), "CanReceiveItem")]
            public class CraftingMainItemContainer_CanReceiveItem
            {
                [HarmonyPostfix]
                static void Postifx(ref CraftingMainItemContainer __instance, ref bool __result, ItemData __0, int __1)
                {                    
                    Debug(false, "CraftingMainItemContainer.CanReceiveItem()");
                    if (Enable)
                    {                        
                        main_item_container = __instance;
                        if (Get.IsCraftable(__0)) { __result = true; }
                    }
                    else { Debug(false, "Disable"); }
                }
            }
        }
        public class Crafting_Upgrade_Button
        {
            //Unlock Craft Button when tier < T7 //CanCraftToT7
            [HarmonyPatch(typeof(CraftingUpgradeButton), "UpdateButton")]
            public class CraftingUpgradeButton_UpdateButton
            {
                [HarmonyPrefix]
                static void Prefix(ref CraftingUpgradeButton __instance, int __0, ref bool __1)
                {
                    Debug(false, "CraftingUpgradeButton.UpdateButton()");
                    if (Enable)
                    {
                        //Debug(false, "CraftingUpgradeButton.UpdateButton()");
                        if ((Scenes.IsGameScene()) && (!Current.item.IsNullOrDestroyed()) && (!Save_Manager.instance.IsNullOrDestroyed()))
                        {
                            if (CanCraftToT7)
                            {
                                AffixSlotForge temp = __instance.gameObject.GetComponentInParent<AffixSlotForge>();
                                if (!temp.IsNullOrDestroyed())
                                {
                                    int affix_id = temp.affixID;
                                    int tier = Get.Tier(Current.item, affix_id);

                                    if (Get.IsIdol(Current.item)) { __1 = false; }
                                    else if ((tier > -1) && (tier < 6)) { __1 = true; }
                                }
                            }
                        }
                    }
                    else { Debug(false, "Disable"); }
                }
            }

            //Update slot and button ref
            [HarmonyPatch(typeof(CraftingUpgradeButton), "UpgradeButtonClicked")]
            public class CraftingUpgradeButton_UpgradeButtonClicked
            {
                [HarmonyPrefix]
                static void Prefix(ref CraftingUpgradeButton __instance)
                {
                    Debug(false, "CraftingUpgradeButton.UpgradeButtonClicked()");
                    if (Enable)
                    {
                        if (Scenes.IsGameScene())
                        {
                            Current.btn = __instance;
                            Current.slot = __instance.gameObject.GetComponentInParent<AffixSlotForge>();
                        }
                    }
                    else { Debug(false, "Disable"); }
                }
            }
        }
        public class Affix_Slot_Forge
        {
            //Update slot and button ref
            [HarmonyPatch(typeof(AffixSlotForge), "SlotClicked")]
            public class AffixSlotForge_SlotClicked
            {
                [HarmonyPostfix]
                static void Postfix(ref AffixSlotForge __instance)
                {
                    Debug(false, "AffixSlotForge.SlotClicked()");
                    if (Enable)
                    {
                        if (Scenes.IsGameScene())
                        {
                            Current.slot = __instance;
                            GameObject upgrade = Functions.GetChild(__instance.gameObject, "upgradeAvailable");
                            if (!upgrade.IsNullOrDestroyed())
                            {
                                Current.btn = upgrade.GetComponent<CraftingUpgradeButton>();
                            }
                            //Ui.force_update_slots = true;
                        }
                    }
                    else { Debug(false, "Disable"); }
                }
            }
        }        
        public class Crafting_Materials_Panel_UI
        {
            public static CraftingMaterialsPanelUI instance = null;
            public static bool IsOpen = false;
            public static bool Initialized = false;
            public static void InitializeAffixs(CraftingMaterialsPanelUI __instance)
            {
                if (!Initialized)
                {
                    Debug(false, "Crafting_Materials_Panel_UI:InitializeAffixs()");
                    System.Collections.Generic.List<int> already = new System.Collections.Generic.List<int>();
                    foreach (ShardAffixListElement affix_element in __instance.shardAffixList) { already.Add(affix_element.affix.affixId); }
                    GameObject unused_holder = __instance.unusedAffixesHolder.gameObject;
                    SimpleLayoutGroup unused_layout = unused_holder.GetComponent<SimpleLayoutGroup>();

                    if (!Refs_Manager.item_list.IsNullOrDestroyed())
                    {
                        if (!Refs_Manager.item_list.affixList.IsNullOrDestroyed() && (!__instance.shardAffixPrefab.IsNullOrDestroyed()))
                        {
                            foreach (AffixList.SingleAffix affix in Refs_Manager.item_list.affixList.singleAffixes)
                            {
                                if (!already.Contains(affix.affixId))
                                {
                                    ShardAffixListElement affix_element = Create.AffixElement(__instance.shardAffixPrefab, affix, affix.modifierType);
                                    __instance.shardAffixList.Add(affix_element);
                                    affix_element.transform.SetParent(unused_holder.transform);
                                    unused_layout.AddElement(affix_element.gameObject);
                                }
                            }
                            foreach (AffixList.MultiAffix affix in Refs_Manager.item_list.affixList.multiAffixes)
                            {
                                if (!already.Contains(affix.affixId))
                                {
                                    ShardAffixListElement affix_element = Create.AffixElement(__instance.shardAffixPrefab, affix, affix.affixProperties[0].modifierType);
                                    __instance.shardAffixList.Add(affix_element);
                                    affix_element.transform.SetParent(unused_holder.transform);
                                    unused_layout.AddElement(affix_element.gameObject);
                                }
                            }
                            Initialized = true;
                        }
                    }
                }
            }
            public static Il2CppSystem.Collections.Generic.List<AffixList.Affix> Get_AllAffixs_List()
            {
                Debug(false, "Crafting_Materials_Panel_UI.Get_AllAffixs_List()");
                Il2CppSystem.Collections.Generic.List<AffixList.Affix> affix_list = new Il2CppSystem.Collections.Generic.List<AffixList.Affix>();
                if (!Refs_Manager.item_list.IsNullOrDestroyed())
                {
                    foreach (AffixList.SingleAffix single_affix in Refs_Manager.item_list.affixList.singleAffixes)
                    {
                        AffixList.Affix affix = single_affix.TryCast<AffixList.Affix>();
                        affix_list.Add(affix);
                    }
                    foreach (AffixList.MultiAffix multi_affix in Refs_Manager.item_list.affixList.multiAffixes)
                    {
                        AffixList.Affix affix = multi_affix.TryCast<AffixList.Affix>();
                        affix_list.Add(affix);
                    }
                }
                return affix_list;
            }
            public static Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<AffixList.Affix> Get_AllAffixs_Array()
            {
                Debug(false, "Crafting_Materials_Panel_UI.Get_AllAffixs_Array()");
                Il2CppSystem.Collections.Generic.List<AffixList.Affix> affix_list = Get_AllAffixs_List();
                Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<AffixList.Affix> new_list = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<AffixList.Affix>(affix_list.Count);
                int i = 0;
                foreach (AffixList.Affix affix in affix_list) { new_list[i] = affix; i++; }

                return new_list;
            }
            public static bool IsPrefixFull(int nb_prefix, bool idol)
            {
                bool full = false;
                if (((idol) && (nb_prefix >= 3)) || //Save_Manager.instance.data.modsNotInHud.Craft_Idols_Nb_Prefixs)) ||
                    ((!idol) && (nb_prefix >= 3)))//Save_Manager.instance.data.modsNotInHud.Craft_Items_Nb_Prefixs)))
                { full = true; }

                Debug(false, "Crafting_Materials_Panel_UI.IsPrefixFull(), Result = " + full);
                return full;
            }
            public static bool IsSuffixFull(int nb_prefix, bool idol)
            {
                bool full = false;
                if (((idol) && (nb_prefix >= 3)) || //Save_Manager.instance.data.modsNotInHud.Craft_Idols_Nb_Suffixs)) ||
                    ((!idol) && (nb_prefix >= 3))) //Save_Manager.instance.data.modsNotInHud.Craft_Items_Nb_Suffixs)))
                { full = true; }

                Debug(false, "Crafting_Materials_Panel_UI.IsSuffixFull(), Result = " + full);
                return full;
            }
            public static bool IsFullT7(bool full, Il2CppSystem.Collections.Generic.List<int> tiers)
            {
                bool full_t7 = false;
                if ((full) && (tiers.Count >= 3))
                {
                    full_t7 = true;
                    for (int i = 0; i < tiers.Count; i++)
                    {
                        if (tiers[i] < 6) { full_t7 = false; break; }
                    }
                }

                Debug(false, "Crafting_Materials_Panel_UI.IsFullT7(), Result = " + full_t7);
                return full_t7;
            }            
            public static void Move_ElementsToHidden(SimpleLayoutGroup content, GameObject holder)
            {
                if ((Scenes.IsGameScene()) && (!instance.IsNullOrDestroyed()))
                {
                    Debug(false, "Crafting_Materials_Panel_UI.Move_ElementsToHidden()");
                    Il2CppSystem.Collections.Generic.List<SimpleLayoutGroup.SimpleLayoutElement> elements = new Il2CppSystem.Collections.Generic.List<SimpleLayoutGroup.SimpleLayoutElement>();
                    foreach (SimpleLayoutGroup.SimpleLayoutElement element in content.Elements) { elements.Add(element); }
                    foreach (SimpleLayoutGroup.SimpleLayoutElement element in elements) { content.RemoveElement(element); }
                    foreach (GameObject child in Functions.GetAllChild(holder)) { child.transform.SetParent(instance.hiddenAffixHolder.gameObject.transform); }
                    elements.Clear();
                }
            }
            public static void Refresh_Elements(CraftingMaterialsPanelUI __instance)
            {
                if ((Scenes.IsGameScene()) && (!Current.item.IsNullOrDestroyed()))
                {
                    Debug(false, "Crafting_Materials_Panel_UI.Refresh_Elements()");

                    CraftingMaterialsPanelUI.AffixFilterType filter_type = __instance.affixFilterType;
                    string filter = __instance.searchText;
                    bool idol = Get.IsIdol(Current.item);
                    ItemList.ClassRequirement class_req = ItemList.ClassRequirement.None;
                    ItemList.SubClassRequirement subclass_req = ItemList.SubClassRequirement.None;
                    Current.item.CalculateLevelAndClassRequirement(out class_req, out subclass_req);
                    bool unique = Current.item.isUniqueSetOrLegendary();
                    int p = 0;
                    int s = 0;
                    Il2CppSystem.Collections.Generic.List<int> p_tiers = new Il2CppSystem.Collections.Generic.List<int>();
                    Il2CppSystem.Collections.Generic.List<int> s_tiers = new Il2CppSystem.Collections.Generic.List<int>();
                    foreach (ItemAffix affix in Current.item.affixes)
                    {
                        if (affix.affixType == AffixList.AffixType.PREFIX) { p++; p_tiers.Add(affix.affixTier); }
                        else if (affix.affixType == AffixList.AffixType.SUFFIX) { s++; s_tiers.Add(affix.affixTier); }
                    }
                    bool prefix_full = IsPrefixFull(p, idol);
                    bool prefix_full_t7 = IsFullT7(prefix_full, p_tiers);
                    bool suffix_full = IsSuffixFull(s, idol);
                    bool suffix_full_t7 = IsFullT7(suffix_full, s_tiers);

                    //Refs
                    SimpleLayoutGroup main_content = __instance.commonLayoutGroup;
                    main_content._updateMode = SimpleLayoutGroup.UpdateMode.Manual;

                    GameObject applied_header = __instance.appliedAffixesHeader.gameObject;
                    GameObject applied_holder = __instance.appliedAffixesHolder.gameObject;
                    SimpleLayoutGroup applied_content = applied_holder.GetComponent<SimpleLayoutGroup>();
                    applied_content._updateMode = SimpleLayoutGroup.UpdateMode.Manual;

                    GameObject incompatible_header = __instance.incompatibleAffixesHeader.gameObject;
                    GameObject incompatible_holder = __instance.incompatibleAffixesHolder.gameObject;
                    SimpleLayoutGroup incompatible_content = incompatible_holder.GetComponent<SimpleLayoutGroup>();
                    incompatible_content._updateMode = SimpleLayoutGroup.UpdateMode.Manual;

                    GameObject unused_header = __instance.unusedAffixesHeader.gameObject;
                    GameObject unused_holder = __instance.unusedAffixesHolder.gameObject;
                    SimpleLayoutGroup unused_content = unused_holder.GetComponent<SimpleLayoutGroup>();
                    unused_content._updateMode = SimpleLayoutGroup.UpdateMode.Manual;

                    //Move to hidden
                    Move_ElementsToHidden(applied_content, applied_holder);
                    Move_ElementsToHidden(unused_content, unused_holder);
                    Move_ElementsToHidden(incompatible_content, incompatible_holder);
                    
                    //Move
                    Il2CppSystem.Collections.Generic.List<int> duplicated_affixs = new Il2CppSystem.Collections.Generic.List<int>();
                    foreach (ShardAffixListElement shard_affix_element in __instance.shardAffixList)
                    {
                        AffixList.Affix aff = shard_affix_element.affix;
                        if (!duplicated_affixs.Contains(aff.affixId))
                        {
                            duplicated_affixs.Add(aff.affixId);
                            if (((aff.type == AffixList.AffixType.PREFIX) && ((!prefix_full) || (!prefix_full_t7)) && (filter_type == CraftingMaterialsPanelUI.AffixFilterType.PREFIX)) ||
                            ((aff.type == AffixList.AffixType.SUFFIX) && ((!suffix_full) || (!suffix_full_t7)) && (filter_type == CraftingMaterialsPanelUI.AffixFilterType.SUFFIX)) ||
                            (filter_type == CraftingMaterialsPanelUI.AffixFilterType.ANY) &&
                            (((aff.type == AffixList.AffixType.PREFIX) && ((!prefix_full)) || (!prefix_full_t7)) ||
                            ((aff.type == AffixList.AffixType.SUFFIX) && ((!suffix_full) || (!suffix_full_t7)))))
                            {
                                if (aff.affixName.ToLower().Contains(filter.ToLower()))
                                {
                                    bool already = false;
                                    foreach (ItemAffix item_affix in Current.item.affixes)
                                    {
                                        if (item_affix.affixId == aff.affixId)
                                        {
                                            applied_content.AddElement(shard_affix_element.gameObject);
                                            shard_affix_element.gameObject.transform.SetParent(applied_holder.transform);
                                            already = true;
                                            break;
                                        }
                                    }
                                    if ((!already) && (((aff.type == AffixList.AffixType.PREFIX) && (!prefix_full)) ||
                                        ((aff.type == AffixList.AffixType.SUFFIX) && (!suffix_full))))
                                    {                                        
                                        //int count = Ui.Get_PlayerShardCount(aff.affixId); //Slow
                                        int count = shard_affix_element.quantity; //Not working fine, return 1 whithout shard
                                        bool can_craft = false;
                                        if ((idol) || (unique)) { can_craft = true; }
                                        else if ((count > 0) || (CanCraftWithoutShard)) { can_craft = true; }

                                        if ((can_craft) && ((aff.CanRollOnItemType(Current.item.itemType, ItemList.ClassRequirement.None)) ||
                                            (aff.CanRollOnItemType(Current.item.itemType, class_req))))
                                        {
                                            unused_content.AddElement(shard_affix_element.gameObject);
                                            shard_affix_element.gameObject.transform.SetParent(unused_holder.transform);
                                        }
                                        else
                                        {
                                            incompatible_content.AddElement(shard_affix_element.gameObject);
                                            shard_affix_element.gameObject.transform.SetParent(incompatible_holder.transform);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (SimpleLayoutGroup.SimpleLayoutElement element in applied_content.Elements) { element.Ignored = false; }
                    foreach (SimpleLayoutGroup.SimpleLayoutElement element in unused_content.Elements) { element.Ignored = false; }
                    foreach (SimpleLayoutGroup.SimpleLayoutElement element in incompatible_content.Elements) { element.Ignored = true; }
                    if (applied_holder.transform.childCount > 0) { applied_header.active = true; }
                    else { applied_header.active = false; }
                    
                    for (int i = 0; i < applied_holder.transform.childCount; i++)
                    {
                        if ((idol) || (unique)) { Functions.GetChild(applied_holder.transform.GetChild(i).gameObject, "Button").active = false; }
                        else
                        {
                            if ((Ui.Get_PlayerShardCount(applied_holder.transform.GetChild(i).gameObject.GetComponent<ShardAffixListElement>().affix.affixId) > 0) ||
                                (CanCraftWithoutShard))
                            { Functions.GetChild(applied_holder.transform.GetChild(i).gameObject, "Button").active = true; }
                            else { Functions.GetChild(applied_holder.transform.GetChild(i).gameObject, "Button").active = false; }
                        }
                        applied_holder.transform.GetChild(i).gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); //Fix Scale
                    }

                    if (unused_holder.transform.childCount > 0) { unused_header.active = true; }
                    else { unused_header.active = false; }
                    for (int i = 0; i < unused_holder.transform.childCount; i++)
                    {
                        Functions.GetChild(unused_holder.transform.GetChild(i).gameObject, "Button").active = true;
                        unused_holder.transform.GetChild(i).gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); //Fix Scale
                    }

                    if (incompatible_holder.transform.childCount > 0) { incompatible_header.active = true; }
                    else { incompatible_header.active = false; }
                    for (int i = 0; i < incompatible_holder.transform.childCount; i++)
                    {
                        incompatible_holder.transform.GetChild(i).gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); //Fix Scale
                    }
                    applied_content.UpdateLayout();
                    unused_content.UpdateLayout();
                    incompatible_content.UpdateLayout();
                    foreach (SimpleLayoutGroup.SimpleLayoutElement element in main_content.Elements) {  element.Ignored = !element.GameObject.active; }
                    __instance.commonLayoutGroup.UpdateLayout();
                }
            }

            //Add missing ShardAffixListElement (idol affix)
            [HarmonyPatch(typeof(CraftingMaterialsPanelUI), "OnOpen")]
            public class CraftingMaterialsPanelUI_OnOpen
            {
                [HarmonyPrefix]
                static void Prefix(ref CraftingMaterialsPanelUI __instance)
                {
                    Debug(false, "CraftingMaterialsPanelUI.OnOpen() Prefix");
                    if (Enable)
                    {                        
                        if (instance.IsNullOrDestroyed()) { instance = __instance; }
                        InitializeAffixs(__instance);
                        //__instance.affixFilterType = CraftingMaterialsPanelUI.AffixFilterType.ANY;
                    }
                    else { Debug(false, "Disable"); }
                }
                [HarmonyPostfix]
                static void Postfix()
                {
                    Debug(false, "CraftingMaterialsPanelUI.OnOpen() Postfix");
                    if (Enable) { IsOpen = true; }
                    else { Debug(false, "Disable"); }
                }
            }

            [HarmonyPatch(typeof(CraftingMaterialsPanelUI), "OnClose")]
            public class CraftingMaterialsPanelUI_OnClose
            {
                [HarmonyPostfix]
                static void Postfix()
                {
                    Debug(false, "CraftingMaterialsPanelUI.OnClose()");
                    if (Enable) { IsOpen = false; }
                    else { Debug(false, "Disable"); }
                }
            }
            
            //Add all affixs in list
            [HarmonyPatch(typeof(CraftingMaterialsPanelUI), "AddShardsFromList")]
            public class CraftingMaterialsPanelUI_AddShardsFromList
            {
                [HarmonyPrefix] //Add all affixs in list
                static void Prefix(ref CraftingMaterialsPanelUI __instance, ref Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<AffixList.Affix> __0)
                {
                    Debug(false, "CraftingMaterialsPanelUI.AddShardsFromList() Prefix");
                    if (instance.IsNullOrDestroyed()) { instance = __instance; }
                    if (Enable) { __0 = Get_AllAffixs_Array(); }
                    else { Debug(false, "Disable"); }
                }
            }

            //Fix shards for Unique Set Legendary and Idols
            [HarmonyPatch(typeof(CraftingMaterialsPanelUI), "RefreshAffixList")]
            public class CraftingMaterialsPanelUI_RefreshAffixList
            {
                [HarmonyPostfix]
                static void Postfix(ref CraftingMaterialsPanelUI __instance)
                {
                    Debug(false, "CraftingMaterialsPanelUI.RefreshAffixList()");
                    if (instance.IsNullOrDestroyed()) { instance = __instance; }
                    if (Enable) { Refresh_Elements(__instance); }
                    else { Debug(false, "Disable"); }
                }
            }
        }
        public class Shard_Affix_List_Element
        {
            //Craft without shards for unique set legendary and idol
            [HarmonyPatch(typeof(ShardAffixListElement), "setQuantityAndUpdateText")]
            public class ShardAffixListElement_setQuantityAndUpdateText
            {
                [HarmonyPrefix]
                static void Prefix(ref int __0)
                {
                    if (Enable)
                    {
                        if ((Current.item != null) && (__0 == 0))
                        {
                            if (((Get.IsIdol(Current.item)) && (CanCraftIdols)) || ((Current.item.isUniqueSetOrLegendary()) && (CanCraftUniqueSet)))
                            {
                                Debug(false, "ShardAffixListElement.setQuantityAndUpdateText(), result from 0 to 1");
                                __0 = 1;
                            }
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(ShardAffixListElement), "Click")]
            public class ShardAffixListElement_Click
            {
                [HarmonyPostfix]
                static void postfix(ref Il2Cpp.ShardAffixListElement __instance)
                {
                    Debug(false, "ShardAffixListElement.Click()");
                    if (Enable) { Ui.force_update_slots = true; }
                    else { Debug(false, "Disable"); }
                }
            }

        }
        public class Crafting_Slot_Manager
        {
            public static bool forgin = false;
            public static int affix_id = -1;
            public static int affix_tier = -1;

            public static void Update_Slot(CraftingSlotManager __instance, float affix_id)
            {
                //bool found = false;
                foreach (AffixSlotForge slot in __instance.affixSlots)
                {
                    if (slot.AffixID == affix_id)
                    {
                        //found = true;
                        Current.slot = slot;
                        break;
                    }
                }
            }
            public static void Update_UpgradeBtn()
            {
                if (!Current.slot.IsNullOrDestroyed())
                {
                    GameObject upgrade = Functions.GetChild(Current.slot.gameObject, "upgradeAvailable");
                    if (!upgrade.IsNullOrDestroyed())
                    {
                        Current.btn = upgrade.GetComponent<CraftingUpgradeButton>();
                    }
                }
            }
            public static void Unselected_Affix()
            {
                Debug(false, "Crafting_Slot_Manager.Unselected_Affix()");
                if (!Crafting_Manager_instance.IsNullOrDestroyed())
                {
                    Crafting_Manager_instance.appliedAffixID = -1;
                }
                if (!Refs_Manager.craft_slot_manager.IsNullOrDestroyed())
                {
                    //Refs_Manager.craft_slot_manager.appliedAffixID = -1;
                    Refs_Manager.craft_slot_manager.forgeButton.interactable = false;
                    Refs_Manager.craft_slot_manager.forgeButtonText.text = "SELECT AN AFFIX";
                }
            }

            [HarmonyPatch(typeof(CraftingSlotManager), "Forge")]
            public class CraftingSlotManager_Forge
            {
                [HarmonyPrefix]
                static bool Prefix(ref CraftingSlotManager __instance)
                {
                    Debug(false, "CraftingSlotManager.Forge() Prefix");
                    bool result = true;
                    if (Enable)
                    {
                        forgin = true;
                        __instance.forgeButton.gameObject.active = false;
                        affix_id = -1;
                        affix_tier = -1;
                        if (!Crafting_Manager_instance.IsNullOrDestroyed()) { affix_id = Crafting_Manager_instance.appliedAffixID; }

                        //glyphs
                        bool glyph_of_hope = false; //25% no forgin potencial cost
                        bool glyph_of_chaos = false; //Change affix //Not Added
                        bool glyph_of_order = false; //don't roll when upgrade
                        bool glyph_of_despair = false; //chance to seal
                        bool glyph_of_insight = false; //prefix to experimental //Not Added
                        bool glyph_of_envy = false; //increase monolith stability //Not Added

                        OneItemContainer glyph_container = __instance.GetSupport();
                        if (!glyph_container.IsNullOrDestroyed())
                        {
                            if (!glyph_container.content.IsNullOrDestroyed())
                            {
                                if (glyph_container.content.data.itemType == 103)
                                {
                                    ushort sub_type = glyph_container.content.data.subType;
                                    switch (sub_type)
                                    {
                                        case 0: { glyph_of_hope = true; break; }
                                        case 1: { glyph_of_chaos = true; break; }
                                        case 2: { glyph_of_order = true; break; }
                                        case 3: { glyph_of_despair = true; break; }
                                        case 4: { glyph_of_insight = true; break; }
                                        case 5: { glyph_of_envy = true; break; }

                                    }
                                }
                            }
                        }

                        bool forgin_no_cost = Save_Manager.instance.data.modsNotInHud.Craft_No_Forgin_Potencial_Cost;
                        int glyph_of_hope_result = UnityEngine.Random.Range(0, 4); //25% chance no forgin potencial cost when Glyph of Hope
                        if ((glyph_of_hope) && (glyph_of_hope_result == 0)) { forgin_no_cost = false; }

                        //Idols/Unique/Set/T6/T7
                        if (Crafting_Manager.latest_string == Crafting_Manager.forge_string) //Forge Mod
                        {
                            if ((Scenes.IsGameScene()) && (!Current.item.IsNullOrDestroyed()))
                            {
                                affix_tier = Get.Tier(Current.item, affix_id);
                                if (Current.slot.IsNullOrDestroyed()) { Update_Slot(__instance, affix_id); }
                                Update_UpgradeBtn();

                                bool legendary = Current.item.isUniqueSetOrLegendary();
                                bool idol = Get.IsIdol(Current.item);

                                if (((idol) && (CanCraftIdols)) ||
                                    ((legendary) && (CanCraftUniqueSet)) ||
                                    (CanCraftToT7))
                                {
                                    AffixList.AffixType affix_type = AffixList.AffixType.SPECIAL;
                                    if (legendary) { Current.item.rarity = 9; }
                                    if (affix_tier > -1) //update affix
                                    {
                                        if (!idol)
                                        {
                                            bool force_upgrade = false;
                                            ItemAffix seal_affix = null;
                                            foreach (ItemAffix affix in Current.item.affixes)
                                            {
                                                if (affix.affixId == affix_id)
                                                {
                                                    affix_type = affix.affixType;
                                                    if ((affix_tier == affix.affixTier) && (affix_tier < 6))
                                                    {
                                                        bool error = false;
                                                        if ((!legendary) && (!forgin_no_cost))
                                                        {
                                                            int min = 0;
                                                            int max = 0;
                                                            if (affix_tier == 4) { min = 1; max = 23; }
                                                            else if (affix_tier == 5) { min = 1; max = 27; }
                                                            if (Current.item.forgingPotential >= (max - 1)) { Current.item.forgingPotential -= (byte)Random.RandomRangeInt(min, max); }
                                                            else
                                                            {
                                                                error = true; //Don't increment affix
                                                                Main.logger_instance?.Error("You need " + (max - 1) + " forgin potencial on this item to craft T" + (affix_tier + 2));
                                                            }
                                                        }
                                                        if (!error)
                                                        {
                                                            force_upgrade = true;
                                                            affix.affixTier++;
                                                            affix_tier = (int)affix.affixTier;
                                                            if (!glyph_of_order) { affix.affixRoll = (byte)Random.Range(0f, 255f); }
                                                            if (glyph_of_despair) { seal_affix = affix; }
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                            if (glyph_of_hope)
                                            {
                                                //remove one glyph of hope from character items
                                            }
                                            if (glyph_of_order)
                                            {
                                                //remove one glyph of order from character items
                                            }
                                            if (glyph_of_despair)
                                            //if (force_seal) //glyph of despair
                                            {
                                                seal_affix.affixTier = Save_Manager.instance.data.modsNotInHud.Craft_Seal_Tier;
                                                Current.item.SealAffix(seal_affix);
                                                //remove one glyph of despair from character items
                                            }
                                            if (force_upgrade)
                                            {
                                                Current.item.RefreshIDAndValues();
                                                result = false;
                                            }
                                            else { Main.logger_instance?.Error("Error when upgrading item"); }
                                        }
                                    }
                                    else //Add Affix
                                    {
                                        int nb_prefix = 0;
                                        int nb_suffix = 0;
                                        bool already_contain_affix = false;
                                        bool already_contain_seal = false;
                                        foreach (ItemAffix item_affix in Current.item.affixes)
                                        {
                                            if (item_affix.affixId == affix_id) { already_contain_affix = true; break; }
                                            if ((item_affix.affixType == AffixList.AffixType.PREFIX) && (!item_affix.isSealedAffix)) { nb_prefix++; }
                                            else if ((item_affix.affixType == AffixList.AffixType.SUFFIX) && (!item_affix.isSealedAffix)) { nb_suffix++; }
                                            else if (item_affix.isSealedAffix) { already_contain_seal = true; }
                                        }
                                        if (!already_contain_affix)
                                        {
                                            AffixList.AffixType new_affix_type = AffixList.instance.GetAffixType(affix_id);

                                            if (((idol) && ((((new_affix_type == AffixList.AffixType.PREFIX) && (nb_prefix < 3)) ||
                                                ((new_affix_type == AffixList.AffixType.SUFFIX) && (nb_suffix < 3))))) ||
                                                ((!idol) && ((((new_affix_type == AffixList.AffixType.PREFIX) && (nb_prefix < 3)) ||
                                                ((new_affix_type == AffixList.AffixType.SUFFIX) && (nb_suffix < 3))))))
                                            {

                                                /*}

                                                if (((new_affix_type == AffixList.AffixType.PREFIX) && (nb_prefix < Save_Manager.instance.data.modsNotInHud.Craft_Items_Nb_Prefixs)) ||
                                                    ((new_affix_type == AffixList.AffixType.SUFFIX) && (nb_suffix < Save_Manager.instance.data.modsNotInHud.Craft_Items_Nb_Suffixs)))
                                                {*/
                                                ItemAffix affix = new ItemAffix
                                                {
                                                    affixId = (ushort)affix_id,
                                                    affixTier = (byte)0,
                                                    affixRoll = (byte)Random.Range(0f, 255f),
                                                    affixType = new_affix_type
                                                };
                                                Current.item.affixes.Add(affix);

                                                if ((!already_contain_seal) && (glyph_of_despair))
                                                {
                                                    affix.affixTier = Save_Manager.instance.data.modsNotInHud.Craft_Seal_Tier;
                                                    Current.item.SealAffix(affix);
                                                    //remove one glyph of despair from character items
                                                }
                                                if (!legendary)
                                                {
                                                    int count = Current.item.affixes.Count;
                                                    if (count > 6) { count = 6; }
                                                    Current.item.rarity = (byte)count;
                                                }
                                                Current.item.RefreshIDAndValues();
                                                //Refresh slot for idol
                                                /*if (idol)
                                                {
                                                    //ItemContainerEntry container_entry = Crafting_Manager.item_container.content;
                                                    ItemData item_data = Crafting_Manager.item_container.content.data;
                                                    Crafting_Manager.item_container.Clear();
                                                    Crafting_Manager.item_container.TryAddItem(item_data, 1, Context.DEFAULT);

                                                    //Crafting_Manager.item_container.MoveItemTo(container_entry, Crafting_Manager.item_container.content, null, Context.DEFAULT);
                                                */
                                                result = false;
                                            }
                                            else { Main.logger_instance?.Error("No space for affix"); }
                                        }
                                    }
                                }
                            }
                        }
                        /*else if (Crafting_Manager.latest_string == Crafting_Manager.seal_string) //Should be use to force seal
                        {
                            ItemAffix affix = null;
                            foreach (ItemAffix item_affix in Current.item.affixes)
                            {
                                if (item_affix.affixId == affix_id) { affix = item_affix; break; }
                            }
                            if (!affix.IsNullOrDestroyed()) { Current.item.SealAffix(affix); }                        
                        }*/
                        else if (Crafting_Manager.latest_string == Crafting_Manager.rune_of_discovery_string) //shoud be use for rune of discovery
                        {
                            //Default = Add 4 affix maximum                        
                        }
                        __instance.forgeButton.gameObject.active = true;
                        forgin = false;

                        //}
                        //Debug(false, "Use Default forge = " + result);
                    }
                    else { Debug(false, "Disable"); }

                    return result;
                }
                
                [HarmonyPostfix]
                static void Postfix(ref CraftingSlotManager __instance)
                {
                    Debug(false, "CraftingSlotManager.Forge() Postfix");
                    if (Enable)
                    {                        
                        if (!Ui.Invoke_UpgradeAvailable(__instance, affix_id))
                        {
                            Unselected_Affix();
                            Ui.ShowHide_MainPath(false);
                        }
                        //if (Current.item.IsNullOrDestroyed()) { Current.item.RefreshIDAndValues(); }
                        Ui.force_update_slots = true;
                    }
                    else { Debug(false, "Disable"); }
                }
            }
        }
    }
}
