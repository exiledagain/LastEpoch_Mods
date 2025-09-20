using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastEpoch_Hud.Scripts.Mods.Items
{
    [RegisterTypeInIl2Cpp]
    public class Items_Herald_Ice : MonoBehaviour
    {
        public static Items_Herald_Ice instance { get; private set; }
        public Items_Herald_Ice(System.IntPtr ptr) : base(ptr) { }
        public static Ability ability = null;
        bool InGame = false;

        void Awake()
        {
            instance = this;
            SceneManager.add_sceneLoaded(new System.Action<Scene, LoadSceneMode>(OnSceneLoaded));
        }
        void Update()
        {
            if (Unique.Icon.IsNullOrDestroyed()) { Assets.Loaded = false; }
            if (!Assets.Loaded) { Assets.Load(); }
            if ((Locales.current != Locales.Selected.Unknow) && (!Basic.AddedToBasicList)) { Basic.AddToBasicList(); }
            if ((Locales.current != Locales.Selected.Unknow) && (!Unique.AddedToUniqueList)) { Unique.AddToUniqueList(); }
            if ((Locales.current != Locales.Selected.Unknow) && (Unique.AddedToUniqueList) && (!Unique.AddedToDictionary)) { Unique.AddToDictionary(); }
            if (!Events.OnKillEvent_Initialized) { Events.Init_OnKillEvent(); }
            if (!Events.OnMinionKillEvent_Initialized) { Events.Init_OnMinionKillEvent(); }
            if (Scenes.IsGameScene())
            {
                if (ability.IsNullOrDestroyed()) { AOE.GetAbility(); }
            }
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (Scenes.IsGameScene())
            {
                if (!InGame)
                {
                    Events.OnKillEvent_Initialized = false;
                    Events.OnMinionKillEvent_Initialized = false;
                    ability = null;
                }
                InGame = true;
            }
            else if (InGame) { InGame = false; }
        }

        public class Assets
        {
            public static bool Loaded = false;
            public static bool loading = false;
            public static void Load()
            {
                if ((!Loaded) && (!Hud_Manager.asset_bundle.IsNullOrDestroyed()) && (!loading))
                {
                    loading = true;
                    try
                    {
                        foreach (string name in Hud_Manager.asset_bundle.GetAllAssetNames())
                        {
                            if (name.Contains("/heraldofice/"))
                            {
                                if ((Functions.Check_Texture(name)) && (name.Contains("icon")) && (Unique.Icon.IsNullOrDestroyed()))
                                {
                                    Texture2D texture = Hud_Manager.asset_bundle.LoadAsset(name).TryCast<Texture2D>();
                                    Unique.Icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                                }
                            }
                        }
                        if (!Unique.Icon.IsNullOrDestroyed()) { Loaded = true; }
                        else { Loaded = false; }
                    }
                    catch { Main.logger_instance?.Error("Herald of Ice Asset Error"); }
                    loading = false;
                }
            }
        }
        public class Basic
        {
            public static bool AddedToBasicList = false;
            public static readonly byte base_type = 25; //Small Idol
            public static readonly int base_id = 3;
            public static ItemList.EquipmentItem Item()
            {
                ItemList.EquipmentItem item = new ItemList.EquipmentItem
                {
                    classRequirement = ItemList.ClassRequirement.None,
                    implicits = implicits(),
                    subClassRequirement = ItemList.SubClassRequirement.None,
                    cannotDrop = true,
                    itemTags = ItemLocationTag.None,
                    levelRequirement = 15,
                    name = Get_Subtype_Name(),
                    subTypeID = base_id
                };

                return item;
            }

            public static void AddToBasicList()
            {
                if ((!AddedToBasicList) && (!Refs_Manager.item_list.IsNullOrDestroyed()))
                {
                    Refs_Manager.item_list.EquippableItems[base_type].subItems.Add(Item());
                    AddedToBasicList = true;
                }
            }
            public static string Get_Subtype_Name()
            {
                string result = "";
                switch (Locales.current)
                {
                    case Locales.Selected.English: { result = HeraldOfIceLocales.SubType.en; break; }
                    case Locales.Selected.French: { result = HeraldOfIceLocales.SubType.en; break; }
                    case Locales.Selected.German: { result = HeraldOfIceLocales.SubType.en; break; }
                    case Locales.Selected.Russian: { result = HeraldOfIceLocales.SubType.en; break; }
                    case Locales.Selected.Portuguese: { result = HeraldOfIceLocales.SubType.en; break; }
                    case Locales.Selected.Korean: { result = HeraldOfIceLocales.SubType.en; break; }
                    case Locales.Selected.Polish: { result = HeraldOfIceLocales.SubType.en; break; }
                    case Locales.Selected.Chinese: { result = HeraldOfIceLocales.SubType.en; break; }
                    case Locales.Selected.Spanish: { result = HeraldOfIceLocales.SubType.en; break; }
                }

                return result;
            }

            private static Il2CppSystem.Collections.Generic.List<ItemList.EquipmentImplicit> implicits()
            {
                Il2CppSystem.Collections.Generic.List<ItemList.EquipmentImplicit> implicits = new Il2CppSystem.Collections.Generic.List<ItemList.EquipmentImplicit>();
                
                return implicits;
            }
        }
        public class Unique
        {
            public static bool AddedToUniqueList = false;
            public static bool AddedToDictionary = false;
            public static Sprite Icon = null;
            public static readonly ushort unique_id = 504;
            public static UniqueList.Entry Item()
            {
                UniqueList.Entry item = new UniqueList.Entry
                {
                    name = Get_Unique_Name(),
                    displayName = Get_Unique_Name(),
                    uniqueID = unique_id,
                    isSetItem = false,
                    setID = 0,
                    overrideLevelRequirement = true,
                    levelRequirement = 40,
                    legendaryType = UniqueList.LegendaryType.LegendaryPotential,
                    overrideEffectiveLevelForLegendaryPotential = true,
                    effectiveLevelForLegendaryPotential = 0,
                    canDropRandomly = true,
                    rerollChance = 1,
                    itemModelType = UniqueList.ItemModelType.Unique,
                    subTypeForIM = 0,
                    baseType = Basic.base_type,
                    subTypes = SubType(),
                    mods = Mods(),
                    tooltipDescriptions = TooltipDescription(),
                    loreText = Get_Unique_Lore(),
                    tooltipEntries = TooltipEntries(),
                    oldSubTypeID = 0,
                    oldUniqueID = 0
                };

                return item;
            }

            public static void AddToUniqueList()
            {
                if ((!AddedToUniqueList) && (!Refs_Manager.unique_list.IsNullOrDestroyed()))
                {
                    try
                    {
                        UniqueList.getUnique(0);
                        Refs_Manager.unique_list.uniques.Add(Item());
                        AddedToUniqueList = true;
                    }
                    catch { Main.logger_instance?.Error("Herald of Ice Unique List Error"); }
                }
            }
            public static void AddToDictionary()
            {
                if ((AddedToUniqueList) && (!AddedToDictionary) && (!Refs_Manager.unique_list.IsNullOrDestroyed()))
                {
                    try
                    {
                        UniqueList.Entry item = null;
                        if (Refs_Manager.unique_list.uniques.Count > 1)
                        {
                            foreach (UniqueList.Entry unique in Refs_Manager.unique_list.uniques)
                            {
                                if ((unique.uniqueID == unique_id) && (unique.name == Get_Unique_Name()))
                                {
                                    item = unique;
                                    break;
                                }
                            }
                        }
                        if (!item.IsNullOrDestroyed())
                        {
                            Refs_Manager.unique_list.entryDictionary.Add(unique_id, item);
                            AddedToDictionary = true;
                        }
                    }
                    catch { Main.logger_instance?.Error("Herald of Ice Unique Dictionary Error"); }
                }
            }
            public static string Get_Unique_Name()
            {
                string result = "";
                switch (Locales.current)
                {
                    case Locales.Selected.English: { result = HeraldOfIceLocales.UniqueName.en; break; }
                    case Locales.Selected.French: { result = HeraldOfIceLocales.UniqueName.en; break; }
                    case Locales.Selected.German: { result = HeraldOfIceLocales.UniqueName.en; break; }
                    case Locales.Selected.Russian: { result = HeraldOfIceLocales.UniqueName.en; break; }
                    case Locales.Selected.Portuguese: { result = HeraldOfIceLocales.UniqueName.en; break; }
                    case Locales.Selected.Korean: { result = HeraldOfIceLocales.UniqueName.en; break; }
                    case Locales.Selected.Polish: { result = HeraldOfIceLocales.UniqueName.en; break; }
                    case Locales.Selected.Chinese: { result = HeraldOfIceLocales.UniqueName.en; break; }
                    case Locales.Selected.Spanish: { result = HeraldOfIceLocales.UniqueName.en; break; }
                }

                return result;
            }
            public static string Get_Unique_Description()
            {
                string result = "";
                switch (Locales.current)
                {
                    case Locales.Selected.English: { result = HeraldOfIceLocales.UniqueDescription.en; break; }
                    case Locales.Selected.French: { result = HeraldOfIceLocales.UniqueDescription.en; break; }
                    case Locales.Selected.Korean: { result = HeraldOfIceLocales.UniqueDescription.en; break; }
                    case Locales.Selected.German: { result = HeraldOfIceLocales.UniqueDescription.en; break; }
                    case Locales.Selected.Russian: { result = HeraldOfIceLocales.UniqueDescription.en; break; }
                    case Locales.Selected.Polish: { result = HeraldOfIceLocales.UniqueDescription.en; break; }
                    case Locales.Selected.Portuguese: { result = HeraldOfIceLocales.UniqueDescription.en; break; }
                    case Locales.Selected.Chinese: { result = HeraldOfIceLocales.UniqueDescription.en; break; }
                    case Locales.Selected.Spanish: { result = HeraldOfIceLocales.UniqueDescription.en; break; }
                }

                return result;
            }
            public static string Get_Unique_Lore()
            {
                string result = "";
                switch (Locales.current)
                {
                    case Locales.Selected.English: { result = HeraldOfIceLocales.Lore.en; break; }
                    case Locales.Selected.French: { result = HeraldOfIceLocales.Lore.en; break; }
                    case Locales.Selected.German: { result = HeraldOfIceLocales.Lore.en; break; }
                    case Locales.Selected.Korean: { result = HeraldOfIceLocales.Lore.en; break; }
                    case Locales.Selected.Russian: { result = HeraldOfIceLocales.Lore.en; break; }
                    case Locales.Selected.Polish: { result = HeraldOfIceLocales.Lore.en; break; }
                    case Locales.Selected.Portuguese: { result = HeraldOfIceLocales.Lore.en; break; }
                    case Locales.Selected.Chinese: { result = HeraldOfIceLocales.Lore.en; break; }
                    case Locales.Selected.Spanish: { result = HeraldOfIceLocales.Lore.en; break; }
                }

                return result;
            }
            public static bool Equipped()
            {
                bool r = false;
                foreach (ItemContainerEntry entry in Refs_Manager.player_actor.itemContainersManager.idols.content)
                {
                    if (entry.data.uniqueID == Unique.unique_id) { r = true; break; }
                }

                return r;
            }

            private static Il2CppSystem.Collections.Generic.List<byte> SubType()
            {
                Il2CppSystem.Collections.Generic.List<byte> result = new Il2CppSystem.Collections.Generic.List<byte>();
                byte r = (byte)Basic.base_id;
                result.Add(r);

                return result;
            }
            private static Il2CppSystem.Collections.Generic.List<UniqueItemMod> Mods()
            {
                Il2CppSystem.Collections.Generic.List<UniqueItemMod> result = new Il2CppSystem.Collections.Generic.List<UniqueItemMod>();
                
                return result;
            }
            private static Il2CppSystem.Collections.Generic.List<UniqueModDisplayListEntry> TooltipEntries()
            {
                Il2CppSystem.Collections.Generic.List<UniqueModDisplayListEntry> result = new Il2CppSystem.Collections.Generic.List<UniqueModDisplayListEntry>();                
                result.Add(new UniqueModDisplayListEntry(128));

                return result;
            }
            private static Il2CppSystem.Collections.Generic.List<ItemTooltipDescription> TooltipDescription()
            {
                Il2CppSystem.Collections.Generic.List<ItemTooltipDescription> result = new Il2CppSystem.Collections.Generic.List<ItemTooltipDescription>();
                result.Add(new ItemTooltipDescription { description = Get_Unique_Description() });

                return result;
            }

            [HarmonyPatch(typeof(InventoryItemUI), "SetImageSpritesAndColours")]
            public class InventoryItemUI_SetImageSpritesAndColours
            {
                [HarmonyPostfix]
                static void Postfix(ref Il2Cpp.InventoryItemUI __instance)
                {
                    if ((__instance.EntryRef.data.getAsUnpacked().FullName == Get_Unique_Name()) && (!Icon.IsNullOrDestroyed()))
                    {
                        __instance.contentImage.sprite = Icon;
                    }
                }
            }

            [HarmonyPatch(typeof(UITooltipItem), "GetItemSprite")]
            public class UITooltipItem_GetItemSprite
            {
                [HarmonyPostfix]
                static void Postfix(ref UnityEngine.Sprite __result, ItemData __0)
                {
                    if ((__0.getAsUnpacked().FullName == Get_Unique_Name()) && (!Icon.IsNullOrDestroyed()))
                    {
                        __result = Icon;
                    }
                }
            }
        }
        public class HeraldOfIceLocales
        {
            private static string basic_subtype_name_key = "Item_SubType_Name_" + Basic.base_type + "_" + Basic.base_id;
            private static string unique_name_key = "Unique_Name_" + Unique.unique_id;
            private static string unique_description_key = "Unique_Tooltip_0_" + Unique.unique_id;
            private static string unique_lore_key = "Unique_Lore_" + Unique.unique_id;

            public class SubType
            {
                public static string en = "Herald of Ice Idol";
                //Add all languages here
            }
            public class UniqueName
            {
                public static string en = "Herald of Ice";
                //Add all languages here
            }
            public class UniqueDescription
            {
                public static string en = "Grants a buff, when you or your minions Kill a monster, this item will cause them to explode and deal AoE cold damage to enemies near them";
                //Add all languages here
            }
            public class Lore
            {
                public static readonly string en = "";
                //Add all languages here
            }

            [HarmonyPatch(typeof(Localization), "TryGetText")]
            public class Localization_TryGetText
            {
                [HarmonyPrefix]
                static bool Prefix(ref bool __result, string __0) //, Il2CppSystem.String __1)
                {
                    bool result = true;
                    if ((__0 == basic_subtype_name_key) || (__0 == unique_name_key) ||
                        (__0 == unique_description_key) || (__0 == unique_lore_key))
                    {
                        __result = true;
                        result = false;
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
                    bool result = true;
                    if (__0 == basic_subtype_name_key)
                    {
                        __result = Basic.Get_Subtype_Name();
                        result = false;
                    }
                    else if (__0 == unique_name_key)
                    {
                        __result = Unique.Get_Unique_Name();
                        result = false;
                    }
                    else if (__0 == unique_description_key)
                    {
                        string description = Unique.Get_Unique_Description();
                        if (description != "")
                        {
                            __result = description;
                            result = false;
                        }
                    }
                    else if (__0 == unique_lore_key)
                    {
                        string lore = Unique.Get_Unique_Lore();
                        if (lore != "")
                        {
                            __result = lore;
                            result = false;
                        }
                    }

                    return result;
                }
            }
        }
        public class AOE
        {
            public static bool Initialize_ability = false;
            public static void GetAbility()
            {
                if (!Initialize_ability)
                {
                    Initialize_ability = true;
                    if (!Refs_Manager.player_actor.IsNullOrDestroyed())
                    {
                        foreach (Ability ab in Resources.FindObjectsOfTypeAll<Ability>())
                        {
                            if (ab.abilityName == "Maggot Explosion")
                            {
                                //Get only vfx then make a new Ability Here

                                /*ability = new Ability();
                                ability.abilityName = "Herald of Ice";
                                ability.tags = AT.Cold;
                                ability.playerCastVfx = PlayerCastVfxID.Cold;*/

                                /*ability = Object.Instantiate(ab, Vector3.zero, Quaternion.identity);
                                ability.abilityName = "Herald of Ice";                                
                                ability.tags = AT.Cold;
                                ability.playerCastVfx = PlayerCastVfxID.Cold;*/
                                ability = ab;
                                break;
                            }
                        }
                        if (!ability.IsNullOrDestroyed())
                        {
                            if (!Refs_Manager.player_actor.abilityList.abilities.Contains(ability))
                            {
                                Refs_Manager.player_actor.abilityList.abilities.Add(ability);
                            }
                            GameObject prefab = ability.abilityPrefab;
                            if (!prefab.IsNullOrDestroyed())
                            {
                                SphereCollider collider = prefab.GetComponent<UnityEngine.SphereCollider>();
                                if (!collider.IsNullOrDestroyed())
                                {
                                    collider.radius = 255;
                                }
                            }

                        }
                    }
                    Initialize_ability = false;
                }
            }
        }
        public class Events
        {
            public static bool OnKillEvent_Initialized = false;
            public static void Init_OnKillEvent()
            {
                if (!Refs_Manager.player_actor.IsNullOrDestroyed())
                {
                    if (!Refs_Manager.player_actor.gameObject.IsNullOrDestroyed())
                    {
                        AbilityEventListener listener = Refs_Manager.player_actor.gameObject.GetComponent<AbilityEventListener>();
                        if (!listener.IsNullOrDestroyed())
                        {
                            listener.add_onKillEvent(OnKillAction);
                            OnKillEvent_Initialized = true;
                        }
                    }
                }
            }
            private static readonly System.Action<Ability, Actor> OnKillAction = new System.Action<Ability, Actor>(OnKill);
            private static void OnKill(Ability ability, Actor killedActor)
            {
                if ((!Refs_Manager.player_actor.IsNullOrDestroyed()) && (!ability.IsNullOrDestroyed())) //&& (!ability_constructor.IsNullOrDestroyed()))
                {
                    if (Unique.Equipped())
                    {
                        ability.CastAfterDelay(Refs_Manager.player_actor.gameObject.GetComponent<AbilityObjectConstructor>(), Refs_Manager.player_actor.position(), killedActor.position(),0f);
                    }
                }
            }

            public static bool OnMinionKillEvent_Initialized = false;
            public static void Init_OnMinionKillEvent()
            {
                if (!Refs_Manager.player_actor.IsNullOrDestroyed())
                {
                    if (!Refs_Manager.player_actor.gameObject.IsNullOrDestroyed())
                    {
                        SummonTracker listener = Refs_Manager.player_actor.gameObject.GetComponent<SummonTracker>();
                        if (!listener.IsNullOrDestroyed())
                        {
                            listener.add_minionKillEvent(OnMinionKillAction);
                            OnMinionKillEvent_Initialized = true;
                        }
                    }
                }
            }
            private static readonly System.Action<Summoned, Ability, Actor> OnMinionKillAction = new System.Action<Summoned, Ability, Actor>(OnMinionKill);
            private static void OnMinionKill(Summoned summon, Ability ability, Actor killedActor)
            {
                if ((!Refs_Manager.player_actor.IsNullOrDestroyed()) && (!ability.IsNullOrDestroyed())) //&& (!ability_constructor.IsNullOrDestroyed()))
                {
                    if (Unique.Equipped())
                    {
                        ability.CastAfterDelay(summon.gameObject.GetComponent<AbilityObjectConstructor>(), Refs_Manager.player_actor.position(), killedActor.position(), 0f);
                    }
                }
            }
        }
    }
}
