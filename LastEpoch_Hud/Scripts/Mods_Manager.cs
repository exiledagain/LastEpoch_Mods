﻿using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts
{
    [RegisterTypeInIl2Cpp]
    public class Mods_Manager : MonoBehaviour
    {
        public Mods_Manager(System.IntPtr ptr) : base(ptr) { }
        public static Mods_Manager instance { get; private set; }

        GameObject character_autopotion_obj = null;
        GameObject character_potionreplenishment_obj = null;
        GameObject character_blessings_obj = null;
        GameObject character_godmode_obj = null;
        GameObject character_lowlife_obj = null;
        GameObject character_masteries_obj = null;
        GameObject character_bank_from_anywhere = null;
        GameObject character_permanentbuffs_obj = null;
        GameObject character_safetp_obj = null;
        GameObject items_nbsocket_obj = null;
        GameObject items_autosell_timer_obj = null;
        GameObject items_headhunter_obj = null;
        GameObject items_mjolner_obj = null;
        GameObject items_sandsofsilk_obj = null;
        GameObject items_essentiasanguis_obj = null;
        //GameObject items_temporalis_obj = null;
        //GameObject items_crafting_obj = null;
        GameObject minimap_icons_obj = null;
        GameObject monoliths_complete_objectives_obj = null;
        GameObject bank_quad_obj = null;
        GameObject craft_max_tier_obj = null;
        GameObject twohanded_with_shield_obj = null;

        bool initialized = false;
        bool enable = false;
        public bool change = false;

        void Awake()
        {
            instance = this;
            Main.logger_instance?.Msg("Mods Manager : Initialize");
            Il2CppSystem.Collections.Generic.List<GameObject> Mods_Objects = new Il2CppSystem.Collections.Generic.List<GameObject>();

            character_autopotion_obj = Object.Instantiate(new GameObject { name = "Mod_Character_AutoPotion" }, Vector3.zero, Quaternion.identity);
            character_autopotion_obj.active = false;
            character_autopotion_obj.AddComponent<Mods.Character.Character_AutoPotions>();
            Mods_Objects.Add(character_autopotion_obj);

            character_potionreplenishment_obj = Object.Instantiate(new GameObject { name = "Mod_Character_PotionReplenishment" }, Vector3.zero, Quaternion.identity);
            character_potionreplenishment_obj.AddComponent<Mods.Character.Character_PotionReplenishment>();
            Mods_Objects.Add(character_potionreplenishment_obj);

            character_blessings_obj = Object.Instantiate(new GameObject { name = "Mod_Character_Blessings" }, Vector3.zero, Quaternion.identity);
            character_blessings_obj.AddComponent<Mods.Character.Character_Blessings>();
            Mods_Objects.Add(character_blessings_obj);
            
            character_godmode_obj = Object.Instantiate(new GameObject { name = "Mod_Character_GodMode" }, Vector3.zero, Quaternion.identity);
            character_godmode_obj.active = false;
            character_godmode_obj.AddComponent<Mods.Character.Character_GodMode>();
            Mods_Objects.Add(character_godmode_obj);

            character_lowlife_obj = Object.Instantiate(new GameObject { name = "Mod_Character_LowLife" }, Vector3.zero, Quaternion.identity);
            character_lowlife_obj.active = false;
            character_lowlife_obj.AddComponent<Mods.Character.Character_LowLife>();
            Mods_Objects.Add(character_lowlife_obj);

            character_masteries_obj = Object.Instantiate(new GameObject { name = "Mod_Character_Masteries" }, Vector3.zero, Quaternion.identity);
            character_masteries_obj.AddComponent<Mods.Character.Character_Masteries>();
            Mods_Objects.Add(character_masteries_obj);

            character_permanentbuffs_obj = Object.Instantiate(new GameObject { name = "Mod_Character_Buffs" }, Vector3.zero, Quaternion.identity);
            character_permanentbuffs_obj.AddComponent<Mods.Character.Character_PermanentBuffs>();
            Mods_Objects.Add(character_permanentbuffs_obj);
                        
            character_safetp_obj = Object.Instantiate(new GameObject { name = "Mod_Character_SafeTp" }, Vector3.zero, Quaternion.identity);
            character_safetp_obj.AddComponent<Mods.Character.Character_TpSafe>();
            Mods_Objects.Add(character_safetp_obj);

            items_nbsocket_obj = Object.Instantiate(new GameObject { name = "Mod_Items_NbSocket" }, Vector3.zero, Quaternion.identity);
            items_nbsocket_obj.AddComponent<Mods.Items.Items_SocketsNb>();
            Mods_Objects.Add(items_nbsocket_obj);

            items_autosell_timer_obj = Object.Instantiate(new GameObject { name = "Mod_Items_AutoStore_All10Sec" }, Vector3.zero, Quaternion.identity);
            items_autosell_timer_obj.active = false;
            items_autosell_timer_obj.AddComponent<Mods.Items.Items_AutoStore_WithTimer>();
            Mods_Objects.Add(items_autosell_timer_obj);

            items_headhunter_obj = Object.Instantiate(new GameObject { name = "Mod_Items_Headhunter" }, Vector3.zero, Quaternion.identity);
            items_headhunter_obj.AddComponent<Mods.Items.Items_HeadHunter>();
            Mods_Objects.Add(items_headhunter_obj);

            items_mjolner_obj = Object.Instantiate(new GameObject { name = "Mod_Items_Mjolner" }, Vector3.zero, Quaternion.identity);
            items_mjolner_obj.AddComponent<Mods.Items.Items_Mjolner>();
            Mods_Objects.Add(items_mjolner_obj);

            items_sandsofsilk_obj = Object.Instantiate(new GameObject { name = "Mod_Items_SandsOfSilk" }, Vector3.zero, Quaternion.identity);
            items_sandsofsilk_obj.AddComponent<Mods.Items.Items_SandsOfSilk>();
            Mods_Objects.Add(items_sandsofsilk_obj);

            items_essentiasanguis_obj = Object.Instantiate(new GameObject { name = "Mod_Items_EssentiaSanguis" }, Vector3.zero, Quaternion.identity);
            items_essentiasanguis_obj.AddComponent<Mods.Items.Items_EssentiaSanguis>();
            Mods_Objects.Add(items_essentiasanguis_obj);

            twohanded_with_shield_obj = Object.Instantiate(new GameObject { name = "Mod_TwoHanded_with_Shield" }, Vector3.zero, Quaternion.identity);
            twohanded_with_shield_obj.AddComponent<Mods.Character.Character_TwoHandedShield>();
            Mods_Objects.Add(twohanded_with_shield_obj);

            //items_temporalis_obj = Object.Instantiate(new GameObject { name = "Mod_Items_Temporalis" }, Vector3.zero, Quaternion.identity);
            //items_temporalis_obj.AddComponent<Mods.Items.Items_Temporalis>();
            //Mods_Objects.Add(items_temporalis_obj);

            //items_crafting_obj = Object.Instantiate(new GameObject { name = "Mod_Items_Crafting" }, Vector3.zero, Quaternion.identity);
            //items_crafting_obj.AddComponent<Mods.Items.Items_Crafting>();
            //Mods_Objects.Add(items_crafting_obj);

            character_bank_from_anywhere = Object.Instantiate(new GameObject { name = "Mod_Bank_Anywhere" }, Vector3.zero, Quaternion.identity);
            character_bank_from_anywhere.AddComponent<Mods.Character.Character_Bank_Anywhere>();
            Mods_Objects.Add(character_bank_from_anywhere);

            minimap_icons_obj = Object.Instantiate(new GameObject { name = "Mod_Minimap_Icons" }, Vector3.zero, Quaternion.identity);
            minimap_icons_obj.AddComponent<Mods.Minimap.Minimap_Icons>();
            Mods_Objects.Add(minimap_icons_obj);

            monoliths_complete_objectives_obj = Object.Instantiate(new GameObject { name = "Mod_Monoliths_Complete_Objectives" }, Vector3.zero, Quaternion.identity);
            monoliths_complete_objectives_obj.AddComponent<Mods.Monoliths.Monoliths_CompleteObjective>();
            Mods_Objects.Add(monoliths_complete_objectives_obj);

            bank_quad_obj = Object.Instantiate(new GameObject { name = "mod_bank_quad" }, Vector3.zero, Quaternion.identity);
            bank_quad_obj.AddComponent<Mods.Bank.Bank_Quad>();
            Mods_Objects.Add(bank_quad_obj);

            craft_max_tier_obj = Object.Instantiate(new GameObject { name = "mod_craft_max_tier" }, Vector3.zero, Quaternion.identity);
            craft_max_tier_obj.AddComponent<Mods.Craft.Craft_MaxTier>();
            Mods_Objects.Add(craft_max_tier_obj);

            foreach (GameObject mod in Mods_Objects) { Object.DontDestroyOnLoad(mod); }
            Mods_Objects.Clear();

            initialized = true;
            Main.logger_instance?.Msg("Mods Manager : Mods initialized");
        }
        void Update() //This function will be removed soon (when i have time to do it ^^)
        {
            if (!Save_Manager.instance.IsNullOrDestroyed())
            {
                if (Save_Manager.instance.initialized)
                {
                    if ((!enable) || (change))
                    {
                        change = false;
                        Enable();
                    }
                }
            }
        }
        void Enable() //This function will be removed soon (when i have time to do it ^^)
        {
            if (initialized)
            {
                enable = true;
                character_godmode_obj.SetActive(Save_Manager.instance.data.Character.Cheats.Enable_GodMode);
                character_lowlife_obj.SetActive(Save_Manager.instance.data.Character.Cheats.Enable_LowLife);
                character_autopotion_obj.SetActive(Save_Manager.instance.data.Character.Cheats.Enable_AutoPot);
                items_autosell_timer_obj.SetActive(Save_Manager.instance.data.Items.Pickup.Enable_AutoStore_Timer);
                Mods.Items.Items_Update.Reqs(); //Used to update item req
            }
        }
    }
}
