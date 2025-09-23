using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastEpoch_Hud.Scripts.Mods.Items
{
    [RegisterTypeInIl2Cpp]
    public class Items_Heralds : MonoBehaviour
    {
        public static Items_Heralds instance { get; private set; }
        public Items_Heralds(System.IntPtr ptr) : base(ptr) { }
        
        bool InGame = false;

        public static string ice_ability_name = "Runemaster 15.2 Explosion";
        public static string fire_ability_name = "FireballExplosion";
        public static string lightning_ability_name = "Runemaster 05c3.1 Runebolt Lightning Explosion";
        public static string poison_ability_name = "NemesisSoldierPoison 03.1 PoisonExplosion";

        //use to set collider size (i don't recommend)
        public static bool increased_ice_collider_radius = false;
        public static bool increased_fire_collider_radius = false;
        public static bool increased_lightning_collider_radius = false;
        public static bool increased_poison_collider_radius = false;
        public static float SphereColliderRadius = 1f; 

        //Use this to increase radius of vfx (i don't recommend)
        public static bool increased_ice_vfx_radius = false;
        public static bool increased_fire_vfx_radius = false;
        public static bool increased_lightning_vfx_radius = false;
        public static bool increased_poison_vfx_radius = false;
        public static float CreateVfxOnDeathIncreaseRadius = 1.75f; //increase Radius   

        void Awake()
        {
            instance = this;
            SceneManager.add_sceneLoaded(new System.Action<Scene, LoadSceneMode>(OnSceneLoaded));
        }
        void Update()
        {
            if ((Uniques.Ice.Icon.IsNullOrDestroyed()) ||
                (Uniques.Fire.Icon.IsNullOrDestroyed()) ||
                (Uniques.Lightning.Icon.IsNullOrDestroyed()) ||
                (Uniques.Poison.Icon.IsNullOrDestroyed()))
            { Assets.Loaded = false; }
            if (!Assets.Loaded) { Assets.Load(); }
            if (Locales.current != Locales.Selected.Unknow)
            {
                if (!Basic.AddedToBasicList) { Basic.AddToBasicList(); }
                if (!Uniques.AddedToUniqueList) { Uniques.AddToUniqueList(); }
                if ((Uniques.AddedToUniqueList) && (!Uniques.AddedToDictionary)) { Uniques.AddToDictionary(); }
            }
            if (!Events.OnKillEvent_Initialized) { Events.Init_OnKillEvent(); }
            if (!Events.OnMinionKillEvent_Initialized) { Events.Init_OnMinionKillEvent(); }
            if (Scenes.IsGameScene())
            {
                if (Uniques.Ice.prefab_obj.IsNullOrDestroyed()) { Uniques.Ice.GetPrefab(); }
                if (Uniques.Ice.ability.IsNullOrDestroyed()) { Uniques.Ice.GetAbility(); }
                if (Uniques.Fire.prefab_obj.IsNullOrDestroyed()) { Uniques.Fire.GetPrefab(); }
                if (Uniques.Fire.ability.IsNullOrDestroyed()) { Uniques.Fire.GetAbility(); }
                if (Uniques.Lightning.prefab_obj.IsNullOrDestroyed()) { Uniques.Lightning.GetPrefab(); }
                if (Uniques.Lightning.ability.IsNullOrDestroyed()) { Uniques.Lightning.GetAbility(); }
                if (Uniques.Poison.prefab_obj.IsNullOrDestroyed()) { Uniques.Poison.GetPrefab(); }
                if (Uniques.Poison.ability.IsNullOrDestroyed()) { Uniques.Poison.GetAbility(); }

                if (Input.GetKeyDown(KeyCode.F9)) { Uniques.Ice.Launch(Refs_Manager.player_actor.gameObject, Refs_Manager.player_actor.position()); }
                if (Input.GetKeyDown(KeyCode.F10)) { Uniques.Fire.Launch(Refs_Manager.player_actor.gameObject, Refs_Manager.player_actor.position()); }
                if (Input.GetKeyDown(KeyCode.F11)) { Uniques.Lightning.Launch(Refs_Manager.player_actor.gameObject, Refs_Manager.player_actor.position()); }
                if (Input.GetKeyDown(KeyCode.F12)) { Uniques.Poison.Launch(Refs_Manager.player_actor.gameObject, Refs_Manager.player_actor.position()); }
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
                                if ((Functions.Check_Texture(name)) && (name.Contains("icon")) && (Uniques.Ice.Icon.IsNullOrDestroyed()))
                                {
                                    Texture2D texture = Hud_Manager.asset_bundle.LoadAsset(name).TryCast<Texture2D>();
                                    Uniques.Ice.Icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                                }
                            }
                            if (name.Contains("/heraldofash/"))
                            {
                                if ((Functions.Check_Texture(name)) && (name.Contains("icon")) && (Uniques.Fire.Icon.IsNullOrDestroyed()))
                                {
                                    Texture2D texture = Hud_Manager.asset_bundle.LoadAsset(name).TryCast<Texture2D>();
                                    Uniques.Fire.Icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                                }
                            }
                            if (name.Contains("/heraldofthunder/"))
                            {
                                if ((Functions.Check_Texture(name)) && (name.Contains("icon")) && (Uniques.Lightning.Icon.IsNullOrDestroyed()))
                                {
                                    Texture2D texture = Hud_Manager.asset_bundle.LoadAsset(name).TryCast<Texture2D>();
                                    Uniques.Lightning.Icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                                }
                            }
                            if (name.Contains("/heraldofagony/"))
                            {
                                if ((Functions.Check_Texture(name)) && (name.Contains("icon")) && (Uniques.Poison.Icon.IsNullOrDestroyed()))
                                {
                                    Texture2D texture = Hud_Manager.asset_bundle.LoadAsset(name).TryCast<Texture2D>();
                                    Uniques.Poison.Icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                                }
                            }
                        }
                        if ((!Uniques.Ice.Icon.IsNullOrDestroyed()) &&
                            (!Uniques.Fire.Icon.IsNullOrDestroyed()) &&
                            (!Uniques.Lightning.Icon.IsNullOrDestroyed()) &&
                            (!Uniques.Poison.Icon.IsNullOrDestroyed()))
                        {
                            Loaded = true;
                        }
                        else { Loaded = false; }
                    }
                    catch { Main.logger_instance?.Error("Heralds Asset Error"); }
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
                    name = Languagues.Get_Subtype_Name(),
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
            public static Il2CppSystem.Collections.Generic.List<byte> SubType()
            {
                Il2CppSystem.Collections.Generic.List<byte> result = new Il2CppSystem.Collections.Generic.List<byte>();
                byte r = (byte)Basic.base_id;
                result.Add(r);

                return result;
            }

            private static Il2CppSystem.Collections.Generic.List<ItemList.EquipmentImplicit> implicits()
            {
                Il2CppSystem.Collections.Generic.List<ItemList.EquipmentImplicit> implicits = new Il2CppSystem.Collections.Generic.List<ItemList.EquipmentImplicit>();

                return implicits;
            }
        }
        public class Uniques
        {
            public static bool AddedToUniqueList = false;
            public static bool AddedToDictionary = false;
                        
            public static void AddToUniqueList()
            {
                if ((!AddedToUniqueList) && (!Refs_Manager.unique_list.IsNullOrDestroyed()))
                {
                    try
                    {
                        UniqueList.getUnique(0);
                        Refs_Manager.unique_list.uniques.Add(Ice.Item());
                        Refs_Manager.unique_list.uniques.Add(Fire.Item());
                        Refs_Manager.unique_list.uniques.Add(Lightning.Item());
                        Refs_Manager.unique_list.uniques.Add(Poison.Item());
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
                        UniqueList.Entry ice_item = null;
                        UniqueList.Entry fire_item = null;
                        UniqueList.Entry ligtning_item = null;
                        UniqueList.Entry poison_item = null;
                        if (Refs_Manager.unique_list.uniques.Count > 1)
                        {
                            foreach (UniqueList.Entry unique in Refs_Manager.unique_list.uniques)
                            {
                                if ((unique.uniqueID == Ice.unique_id) && (unique.name == Ice.Get_Unique_Name())) { ice_item = unique; }
                                else if ((unique.uniqueID == Fire.unique_id) && (unique.name == Fire.Get_Unique_Name())) { fire_item = unique; }
                                else if ((unique.uniqueID == Lightning.unique_id) && (unique.name == Lightning.Get_Unique_Name())) { ligtning_item = unique; }
                                else if ((unique.uniqueID == Poison.unique_id) && (unique.name == Poison.Get_Unique_Name())) { poison_item = unique; }
                            }
                        }
                        if ((!ice_item.IsNullOrDestroyed()) &&
                            (!fire_item.IsNullOrDestroyed()) &&
                            (!ligtning_item.IsNullOrDestroyed()) &&
                            (!poison_item.IsNullOrDestroyed()))
                        {
                            Refs_Manager.unique_list.entryDictionary.Add(Ice.unique_id, ice_item);
                            Refs_Manager.unique_list.entryDictionary.Add(Fire.unique_id, fire_item);
                            Refs_Manager.unique_list.entryDictionary.Add(Lightning.unique_id, ligtning_item);
                            Refs_Manager.unique_list.entryDictionary.Add(Poison.unique_id, poison_item);
                            AddedToDictionary = true;
                        }
                    }
                    catch { Main.logger_instance?.Error("Herald of Ice Unique Dictionary Error"); }
                }
            }

            public class Ice
            {
                //HorridSlimeWater 04 DeathExplosionIce             TooBig
                //Runemaster 05c2.1 Runebolt Cold Explosion
                //IceMortalExplosion
                //FrostWallDetonation
                //IceShard
                //Twin Dragon (Ice) 08.2 Sheer Cold Explosion
                //NemesisSoldierUltimate 04 SlowingSnapFreeze
                //IceTitan 02.1 Glacier
                //Runemaster 15.2 Explosion
                //Runemaster Base 12 Large Explosion Cold
                //IceNova                                           TooBig
                //Runemaster 22.2 Ice Zipper Spike L
                //IceTitan 03.1 IceSpikeDamage
                //NemesisSoldierCold 04 SlowingSnapFreeze
                //FrozenDryad 02.2 BulbExplosion
                //Runemaster 18.1 Ice Shield Impact
                //Twin Dragon (Ice) 05.2 Icicle Crash Explosion
                //Runemaster 22.1 Ice Zipper Spike R
                //ChonkerDrake 07.5 Icebomb Explosion
                //Chilling Wave
                //Runemaster Base 11 Small Explosion Cold
                //Crystal Mod 01.1 Crystallized Heart Explosion
                //Heorot Boss Ice Circle Small
                //ImprisonedMage 19.1 Ice Nova
                //Bone Golem 10 Freezing Grasp
                //Runemaster 23 Small Cold Explosion
                //SmallIceCircle
                //GraelIceNova
                //Aterroth 14c3.1 Meteor Explosion
                //Frost Claw Explosion
                //Skeletal Mages Cold Projectile
                //OasisSalamander 02.2 WaterSplash
                public static bool Initialize_ability = false;
                public static Ability ability = null;
                public static bool Initialize_prefab = false;
                public static GameObject prefab_obj = null;
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
                        levelRequirement = 15,
                        legendaryType = UniqueList.LegendaryType.LegendaryPotential,
                        overrideEffectiveLevelForLegendaryPotential = true,
                        effectiveLevelForLegendaryPotential = 0,
                        canDropRandomly = true,
                        rerollChance = 1,
                        itemModelType = UniqueList.ItemModelType.Unique,
                        subTypeForIM = 0,
                        baseType = Basic.base_type,
                        subTypes = Basic.SubType(),
                        mods = Mods(),
                        tooltipDescriptions = TooltipDescription(),
                        loreText = Get_Unique_Lore(),
                        tooltipEntries = TooltipEntries(),
                        oldSubTypeID = 0,
                        oldUniqueID = 0
                    };

                    return item;
                }

                public static void GetAbility()
                {
                    if (!Initialize_ability)
                    {
                        Initialize_ability = true;
                        if (!Refs_Manager.player_actor.IsNullOrDestroyed())
                        {
                            foreach (Ability ab in Resources.FindObjectsOfTypeAll<Ability>())
                            {
                                if (ab.name == ice_ability_name)
                                {
                                    ability = new Ability
                                    {
                                        name = "Herald of Ice",
                                        abilityName = "Herald of Ice",
                                        abilityObjectRotation = Ability.AbilityObjectRotation.FacingTarget,
                                        abilityObjectType = Ability.AbilityObjectType.Default,
                                        animation = AbilityAnimation.Cast,
                                        attachCastingVFXToCaster = true,
                                        attributeScaling = new Il2CppSystem.Collections.Generic.List<Ability.AttributeScaling>(),
                                        baseMovementAnimationLength = 1f,
                                        castingVFXPositioning = CastingVFXPositioning.Default,
                                        description = "",
                                        manaCost = 0f,
                                        moveOrAttackFallback = Ability.MoveOrAttackFallback.Wait,
                                        speedMultiplier = 1f,
                                        speedScaler = SP.CastSpeed,
                                        tags = ab.tags,
                                        useDelay = 0.4f,
                                        useDuration = 0.75f
                                    };
                                    break;
                                }
                            }
                        }
                        Initialize_ability = false;
                    }
                }
                public static void GetPrefab()
                {
                    if (!Initialize_prefab)
                    {
                        Initialize_prefab = true;
                        foreach (Ability ab in Resources.FindObjectsOfTypeAll<Ability>())
                        {
                            if (ab.name == ice_ability_name)
                            {
                                prefab_obj = Object.Instantiate(ab.abilityPrefab, Vector3.zero, Quaternion.identity);
                                prefab_obj.active = false;
                                prefab_obj.name = "Herald of Ice prefab";
                                SphereCollider collider = prefab_obj.GetComponent<UnityEngine.SphereCollider>();
                                if ((!collider.IsNullOrDestroyed()) && (increased_ice_collider_radius))
                                { collider.radius = SphereColliderRadius; }
                                CreateVfxOnDeath vfx_on_death = prefab_obj.GetComponent<CreateVfxOnDeath>();
                                if ((!vfx_on_death.IsNullOrDestroyed()) && (increased_ice_vfx_radius))
                                { vfx_on_death.increasedRadius = CreateVfxOnDeathIncreaseRadius; }
                                break;
                            }
                        }
                        Initialize_prefab = false;
                    }
                }
                public static string Get_Unique_Name()
                {
                    string result = "";
                    switch (Locales.current)
                    {
                        case Locales.Selected.English: { result = Languagues.Ice.UniqueName.en; break; }
                        case Locales.Selected.French: { result = Languagues.Ice.UniqueName.en; break; }
                        case Locales.Selected.German: { result = Languagues.Ice.UniqueName.en; break; }
                        case Locales.Selected.Russian: { result = Languagues.Ice.UniqueName.en; break; }
                        case Locales.Selected.Portuguese: { result = Languagues.Ice.UniqueName.en; break; }
                        case Locales.Selected.Korean: { result = Languagues.Ice.UniqueName.en; break; }
                        case Locales.Selected.Polish: { result = Languagues.Ice.UniqueName.en; break; }
                        case Locales.Selected.Chinese: { result = Languagues.Ice.UniqueName.en; break; }
                        case Locales.Selected.Spanish: { result = Languagues.Ice.UniqueName.en; break; }
                    }

                    return result;
                }
                public static string Get_Unique_Description()
                {
                    string result = "";
                    switch (Locales.current)
                    {
                        case Locales.Selected.English: { result = Languagues.Ice.UniqueDescription.en; break; }
                        case Locales.Selected.French: { result = Languagues.Ice.UniqueDescription.en; break; }
                        case Locales.Selected.Korean: { result = Languagues.Ice.UniqueDescription.en; break; }
                        case Locales.Selected.German: { result = Languagues.Ice.UniqueDescription.en; break; }
                        case Locales.Selected.Russian: { result = Languagues.Ice.UniqueDescription.en; break; }
                        case Locales.Selected.Polish: { result = Languagues.Ice.UniqueDescription.en; break; }
                        case Locales.Selected.Portuguese: { result = Languagues.Ice.UniqueDescription.en; break; }
                        case Locales.Selected.Chinese: { result = Languagues.Ice.UniqueDescription.en; break; }
                        case Locales.Selected.Spanish: { result = Languagues.Ice.UniqueDescription.en; break; }
                    }

                    return result;
                }
                public static string Get_Unique_Lore()
                {
                    string result = "";
                    switch (Locales.current)
                    {
                        case Locales.Selected.English: { result = Languagues.Ice.Lore.en; break; }
                        case Locales.Selected.French: { result = Languagues.Ice.Lore.en; break; }
                        case Locales.Selected.German: { result = Languagues.Ice.Lore.en; break; }
                        case Locales.Selected.Korean: { result = Languagues.Ice.Lore.en; break; }
                        case Locales.Selected.Russian: { result = Languagues.Ice.Lore.en; break; }
                        case Locales.Selected.Polish: { result = Languagues.Ice.Lore.en; break; }
                        case Locales.Selected.Portuguese: { result = Languagues.Ice.Lore.en; break; }
                        case Locales.Selected.Chinese: { result = Languagues.Ice.Lore.en; break; }
                        case Locales.Selected.Spanish: { result = Languagues.Ice.Lore.en; break; }
                    }

                    return result;
                }
                public static bool Equipped()
                {
                    bool r = false;
                    foreach (ItemContainerEntry entry in Refs_Manager.player_actor.itemContainersManager.idols.content)
                    {
                        if (entry.data.uniqueID == unique_id) { r = true; break; }
                    }

                    return r;
                }                
                public static void Launch(GameObject actor, Vector3 target_position)
                {
                    if ((!ability.IsNullOrDestroyed()) && (!prefab_obj.IsNullOrDestroyed()))
                    {
                        if (ability.abilityPrefab.IsNullOrDestroyed()) { ability.abilityPrefab = Object.Instantiate(prefab_obj, Vector3.zero, Quaternion.identity); }
                        if (!ability.abilityPrefab.IsNullOrDestroyed())
                        {
                            //Vector3 position = new Vector3(target_position.x, target_position.y, (target_position.z + 10));
                            ability.abilityPrefab.active = true;
                            ability.CastAfterDelay(actor.GetComponent<AbilityObjectConstructor>(), target_position, target_position, 0f);
                        }
                    }
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
            }
            public class Fire
            {                
                public static bool Initialize_ability = false;
                public static Ability ability = null;
                public static bool Initialize_prefab = false;
                public static GameObject prefab_obj = null;
                public static Sprite Icon = null;
                public static readonly ushort unique_id = 505;
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
                        levelRequirement = 15,
                        legendaryType = UniqueList.LegendaryType.LegendaryPotential,
                        overrideEffectiveLevelForLegendaryPotential = true,
                        effectiveLevelForLegendaryPotential = 0,
                        canDropRandomly = true,
                        rerollChance = 1,
                        itemModelType = UniqueList.ItemModelType.Unique,
                        subTypeForIM = 0,
                        baseType = Basic.base_type,
                        subTypes = Basic.SubType(),
                        mods = Mods(),
                        tooltipDescriptions = TooltipDescription(),
                        loreText = Get_Unique_Lore(),
                        tooltipEntries = TooltipEntries(),
                        oldSubTypeID = 0,
                        oldUniqueID = 0
                    };

                    return item;
                }

                public static void GetAbility()
                {
                    if (!Initialize_ability)
                    {
                        Initialize_ability = true;
                        if (!Refs_Manager.player_actor.IsNullOrDestroyed())
                        {
                            foreach (Ability ab in Resources.FindObjectsOfTypeAll<Ability>())
                            {
                                if (ab.name == fire_ability_name)
                                {
                                    ability = new Ability
                                    {
                                        name = "Herald of Ash",
                                        abilityName = "Herald of Ash",
                                        abilityObjectRotation = Ability.AbilityObjectRotation.FacingTarget,
                                        abilityObjectType = Ability.AbilityObjectType.Default,
                                        animation = AbilityAnimation.Cast,
                                        attachCastingVFXToCaster = true,
                                        attributeScaling = new Il2CppSystem.Collections.Generic.List<Ability.AttributeScaling>(),
                                        baseMovementAnimationLength = 1f,
                                        castingVFXPositioning = CastingVFXPositioning.Default,
                                        description = "",
                                        manaCost = 0f,
                                        moveOrAttackFallback = Ability.MoveOrAttackFallback.Wait,
                                        speedMultiplier = 1f,
                                        speedScaler = SP.CastSpeed,
                                        tags = ab.tags,
                                        useDelay = 0.4f,
                                        useDuration = 0.75f
                                    };
                                    break;
                                }
                            }
                        }
                        Initialize_ability = false;
                    }
                }
                public static void GetPrefab()
                {
                    if (!Initialize_prefab)
                    {
                        Initialize_prefab = true;
                        foreach (Ability ab in Resources.FindObjectsOfTypeAll<Ability>())
                        {
                            if (ab.name == fire_ability_name)
                            {
                                prefab_obj = Object.Instantiate(ab.abilityPrefab, Vector3.zero, Quaternion.identity);
                                prefab_obj.active = false;
                                prefab_obj.name = "Herald of Ash prefab";
                                SphereCollider collider = prefab_obj.GetComponent<UnityEngine.SphereCollider>();
                                if ((!collider.IsNullOrDestroyed()) && (increased_fire_collider_radius))
                                { collider.radius = SphereColliderRadius; }
                                CreateVfxOnDeath vfx_on_death = prefab_obj.GetComponent<CreateVfxOnDeath>();
                                if ((!vfx_on_death.IsNullOrDestroyed()) && (increased_fire_vfx_radius))
                                { vfx_on_death.increasedRadius = CreateVfxOnDeathIncreaseRadius; }
                                break;
                            }
                        }
                        Initialize_prefab = false;
                    }
                }
                public static string Get_Unique_Name()
                {
                    string result = "";
                    switch (Locales.current)
                    {
                        case Locales.Selected.English: { result = Languagues.Fire.UniqueName.en; break; }
                        case Locales.Selected.French: { result = Languagues.Fire.UniqueName.en; break; }
                        case Locales.Selected.German: { result = Languagues.Fire.UniqueName.en; break; }
                        case Locales.Selected.Russian: { result = Languagues.Fire.UniqueName.en; break; }
                        case Locales.Selected.Portuguese: { result = Languagues.Fire.UniqueName.en; break; }
                        case Locales.Selected.Korean: { result = Languagues.Fire.UniqueName.en; break; }
                        case Locales.Selected.Polish: { result = Languagues.Fire.UniqueName.en; break; }
                        case Locales.Selected.Chinese: { result = Languagues.Fire.UniqueName.en; break; }
                        case Locales.Selected.Spanish: { result = Languagues.Fire.UniqueName.en; break; }
                    }

                    return result;
                }
                public static string Get_Unique_Description()
                {
                    string result = "";
                    switch (Locales.current)
                    {
                        case Locales.Selected.English: { result = Languagues.Fire.UniqueDescription.en; break; }
                        case Locales.Selected.French: { result = Languagues.Fire.UniqueDescription.en; break; }
                        case Locales.Selected.Korean: { result = Languagues.Fire.UniqueDescription.en; break; }
                        case Locales.Selected.German: { result = Languagues.Fire.UniqueDescription.en; break; }
                        case Locales.Selected.Russian: { result = Languagues.Fire.UniqueDescription.en; break; }
                        case Locales.Selected.Polish: { result = Languagues.Fire.UniqueDescription.en; break; }
                        case Locales.Selected.Portuguese: { result = Languagues.Fire.UniqueDescription.en; break; }
                        case Locales.Selected.Chinese: { result = Languagues.Fire.UniqueDescription.en; break; }
                        case Locales.Selected.Spanish: { result = Languagues.Fire.UniqueDescription.en; break; }
                    }

                    return result;
                }
                public static string Get_Unique_Lore()
                {
                    string result = "";
                    switch (Locales.current)
                    {
                        case Locales.Selected.English: { result = Languagues.Fire.Lore.en; break; }
                        case Locales.Selected.French: { result = Languagues.Fire.Lore.en; break; }
                        case Locales.Selected.German: { result = Languagues.Fire.Lore.en; break; }
                        case Locales.Selected.Korean: { result = Languagues.Fire.Lore.en; break; }
                        case Locales.Selected.Russian: { result = Languagues.Fire.Lore.en; break; }
                        case Locales.Selected.Polish: { result = Languagues.Fire.Lore.en; break; }
                        case Locales.Selected.Portuguese: { result = Languagues.Fire.Lore.en; break; }
                        case Locales.Selected.Chinese: { result = Languagues.Fire.Lore.en; break; }
                        case Locales.Selected.Spanish: { result = Languagues.Fire.Lore.en; break; }
                    }

                    return result;
                }
                public static bool Equipped()
                {
                    bool r = false;
                    foreach (ItemContainerEntry entry in Refs_Manager.player_actor.itemContainersManager.idols.content)
                    {
                        if (entry.data.uniqueID == unique_id) { r = true; break; }
                    }

                    return r;
                }
                public static void Launch(GameObject actor, Vector3 target_position)
                {
                    if ((!ability.IsNullOrDestroyed()) && (!prefab_obj.IsNullOrDestroyed()))
                    {
                        if (ability.abilityPrefab.IsNullOrDestroyed()) { ability.abilityPrefab = Object.Instantiate(prefab_obj, Vector3.zero, Quaternion.identity); }
                        if (!ability.abilityPrefab.IsNullOrDestroyed())
                        {
                            //Vector3 position = new Vector3(target_position.x, target_position.y, (target_position.z + 10));
                            ability.abilityPrefab.active = true;
                            ability.CastAfterDelay(actor.GetComponent<AbilityObjectConstructor>(), target_position, target_position, 0f);
                        }
                    }
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
            }
            public class Lightning
            {
                //NemesisSoldierLightning 04 LightningExplosion                 TooBig
                //LightningLich 02 LightningBolt
                //Runemaster Base 21 Small Explosion Lightning No Target Set
                //ValeStorm
                //Runemaster 05c3.1 Runebolt Lightning Explosion
                //Runemaster 43.1 Static Field Damage
                //NemesisSoldierLightning 04 LightningExplosion
                //ImprisonedMage 18.1 Lightning Bolt
                //Event Actor Mod 04.1 Lightning Bolt Damage
                //Gaspar Boss 07.2 Mine Lightning Explosion
                //Apophis 053.1 LightningCrystalExplosion
                //PrimalLightning
                //Runemaster 02.1 Glyph of Dominion Damage
                //Void Harton Void Stab AoE Damage
                //AdmiralHarton 01.2 StormLightningBolt
                //GiantStatueRuneExplosion Endgame
                //ImperialEraEnemy03 Miniboss Glyph of Dominion Damage
                //Rune Warden 01.2 Thunder Clap Large Explosion
                //Runemaster Base 21 Small Explosion Lightning
                //armored phoenix 02.1 feathers damages
                //Liath Lightning Mine Explosion
                //GiantStatueRuneExplosion
                //Runemaster 23 Small Lightning Explosion
                //MummifiedNagasa Champion 02.2 SandProjectileDamage
                //Lightning Dragon Random Echo Shake Explosion
                //AdmiralHarton 02.1 LightningBlast
                //Miniboss Fire Golem 02.2 Flame Feather Explosion
                //Spire 03.2 Lagon Beam Damage
                //Runemaster Base 22 Large Explosion Lightning
                //Apophis 053.3 LightningCrystalDOTDamages
                //GoldthreadArachne 03.1 SmallExplosion
                //Crystal Dragon Champion 05 Lightning Explosion
                //AdmiralHarton 05.1 LightningBlastStab
                //ImprisonedMage 10.1 Arc Volley Impact
                //TempleBeast 03.1 Lightning Strike Damage
                //Runemaster 46 Discharging snap
                //TempleGuardian-Staff 02 Heavenly Tempest Strikes
                //LightningNova                                                 TooBig
                
                public static bool Initialize_ability = false;
                public static Ability ability = null;
                public static bool Initialize_prefab = false;
                public static GameObject prefab_obj = null;
                public static Sprite Icon = null;
                public static readonly ushort unique_id = 506;
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
                        levelRequirement = 15,
                        legendaryType = UniqueList.LegendaryType.LegendaryPotential,
                        overrideEffectiveLevelForLegendaryPotential = true,
                        effectiveLevelForLegendaryPotential = 0,
                        canDropRandomly = true,
                        rerollChance = 1,
                        itemModelType = UniqueList.ItemModelType.Unique,
                        subTypeForIM = 0,
                        baseType = Basic.base_type,
                        subTypes = Basic.SubType(),
                        mods = Mods(),
                        tooltipDescriptions = TooltipDescription(),
                        loreText = Get_Unique_Lore(),
                        tooltipEntries = TooltipEntries(),
                        oldSubTypeID = 0,
                        oldUniqueID = 0
                    };

                    return item;
                }

                public static void GetAbility()
                {
                    if (!Initialize_ability)
                    {
                        Initialize_ability = true;
                        if (!Refs_Manager.player_actor.IsNullOrDestroyed())
                        {
                            foreach (Ability ab in Resources.FindObjectsOfTypeAll<Ability>())
                            {
                                if (ab.name == lightning_ability_name)
                                {
                                    ability = new Ability
                                    {
                                        name = "Herald of Thunder",
                                        abilityName = "Herald of Thunder",
                                        abilityObjectRotation = Ability.AbilityObjectRotation.FacingTarget,
                                        abilityObjectType = Ability.AbilityObjectType.Default,
                                        animation = AbilityAnimation.Cast,
                                        attachCastingVFXToCaster = true,
                                        attributeScaling = new Il2CppSystem.Collections.Generic.List<Ability.AttributeScaling>(),
                                        baseMovementAnimationLength = 1f,
                                        castingVFXPositioning = CastingVFXPositioning.Default,
                                        description = "",
                                        manaCost = 0f,
                                        moveOrAttackFallback = Ability.MoveOrAttackFallback.Wait,
                                        speedMultiplier = 1f,
                                        speedScaler = SP.CastSpeed,
                                        tags = ab.tags,
                                        useDelay = 0.4f,
                                        useDuration = 0.75f
                                    };
                                    break;
                                }
                            }
                        }
                        Initialize_ability = false;
                    }
                }
                public static void GetPrefab()
                {
                    if (!Initialize_prefab)
                    {
                        Initialize_prefab = true;
                        foreach (Ability ab in Resources.FindObjectsOfTypeAll<Ability>())
                        {
                            if (ab.name == lightning_ability_name)
                            {
                                prefab_obj = Object.Instantiate(ab.abilityPrefab, Vector3.zero, Quaternion.identity);
                                prefab_obj.active = false;
                                prefab_obj.name = "Herald of Thunder prefab";
                                SphereCollider collider = prefab_obj.GetComponent<UnityEngine.SphereCollider>();
                                if ((!collider.IsNullOrDestroyed()) && (increased_lightning_collider_radius))
                                { collider.radius = SphereColliderRadius; }
                                CreateVfxOnDeath vfx_on_death = prefab_obj.GetComponent<CreateVfxOnDeath>();
                                if((!vfx_on_death.IsNullOrDestroyed()) && (increased_lightning_vfx_radius))
                                { vfx_on_death.increasedRadius = CreateVfxOnDeathIncreaseRadius; }
                                break;
                            }
                        }
                        Initialize_prefab = false;
                    }
                }
                public static string Get_Unique_Name()
                {
                    string result = "";
                    switch (Locales.current)
                    {
                        case Locales.Selected.English: { result = Languagues.Lightning.UniqueName.en; break; }
                        case Locales.Selected.French: { result = Languagues.Lightning.UniqueName.en; break; }
                        case Locales.Selected.German: { result = Languagues.Lightning.UniqueName.en; break; }
                        case Locales.Selected.Russian: { result = Languagues.Lightning.UniqueName.en; break; }
                        case Locales.Selected.Portuguese: { result = Languagues.Lightning.UniqueName.en; break; }
                        case Locales.Selected.Korean: { result = Languagues.Lightning.UniqueName.en; break; }
                        case Locales.Selected.Polish: { result = Languagues.Lightning.UniqueName.en; break; }
                        case Locales.Selected.Chinese: { result = Languagues.Lightning.UniqueName.en; break; }
                        case Locales.Selected.Spanish: { result = Languagues.Lightning.UniqueName.en; break; }
                    }

                    return result;
                }
                public static string Get_Unique_Description()
                {
                    string result = "";
                    switch (Locales.current)
                    {
                        case Locales.Selected.English: { result = Languagues.Lightning.UniqueDescription.en; break; }
                        case Locales.Selected.French: { result = Languagues.Lightning.UniqueDescription.en; break; }
                        case Locales.Selected.Korean: { result = Languagues.Lightning.UniqueDescription.en; break; }
                        case Locales.Selected.German: { result = Languagues.Lightning.UniqueDescription.en; break; }
                        case Locales.Selected.Russian: { result = Languagues.Lightning.UniqueDescription.en; break; }
                        case Locales.Selected.Polish: { result = Languagues.Lightning.UniqueDescription.en; break; }
                        case Locales.Selected.Portuguese: { result = Languagues.Lightning.UniqueDescription.en; break; }
                        case Locales.Selected.Chinese: { result = Languagues.Lightning.UniqueDescription.en; break; }
                        case Locales.Selected.Spanish: { result = Languagues.Lightning.UniqueDescription.en; break; }
                    }

                    return result;
                }
                public static string Get_Unique_Lore()
                {
                    string result = "";
                    switch (Locales.current)
                    {
                        case Locales.Selected.English: { result = Languagues.Lightning.Lore.en; break; }
                        case Locales.Selected.French: { result = Languagues.Lightning.Lore.en; break; }
                        case Locales.Selected.German: { result = Languagues.Lightning.Lore.en; break; }
                        case Locales.Selected.Korean: { result = Languagues.Lightning.Lore.en; break; }
                        case Locales.Selected.Russian: { result = Languagues.Lightning.Lore.en; break; }
                        case Locales.Selected.Polish: { result = Languagues.Lightning.Lore.en; break; }
                        case Locales.Selected.Portuguese: { result = Languagues.Lightning.Lore.en; break; }
                        case Locales.Selected.Chinese: { result = Languagues.Lightning.Lore.en; break; }
                        case Locales.Selected.Spanish: { result = Languagues.Lightning.Lore.en; break; }
                    }

                    return result;
                }
                public static bool Equipped()
                {
                    bool r = false;
                    foreach (ItemContainerEntry entry in Refs_Manager.player_actor.itemContainersManager.idols.content)
                    {
                        if (entry.data.uniqueID == unique_id) { r = true; break; }
                    }

                    return r;
                }
                public static void Launch(GameObject actor, Vector3 target_position)
                {
                    if ((!ability.IsNullOrDestroyed()) && (!prefab_obj.IsNullOrDestroyed()))
                    {
                        if (ability.abilityPrefab.IsNullOrDestroyed()) { ability.abilityPrefab = Object.Instantiate(prefab_obj, Vector3.zero, Quaternion.identity); }
                        if (!ability.abilityPrefab.IsNullOrDestroyed())
                        {
                            //Vector3 position = new Vector3(target_position.x, target_position.y, (target_position.z + 10));
                            ability.abilityPrefab.active = true;
                            ability.CastAfterDelay(actor.GetComponent<AbilityObjectConstructor>(), target_position, target_position, 0f);
                        }
                    }
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
            }
            public class Poison
            {                
                public static bool Initialize_ability = false;
                public static Ability ability = null;
                public static bool Initialize_prefab = false;
                public static GameObject prefab_obj = null;
                public static Sprite Icon = null;
                public static readonly ushort unique_id = 507;
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
                        levelRequirement = 15,
                        legendaryType = UniqueList.LegendaryType.LegendaryPotential,
                        overrideEffectiveLevelForLegendaryPotential = true,
                        effectiveLevelForLegendaryPotential = 0,
                        canDropRandomly = true,
                        rerollChance = 1,
                        itemModelType = UniqueList.ItemModelType.Unique,
                        subTypeForIM = 0,
                        baseType = Basic.base_type,
                        subTypes = Basic.SubType(),
                        mods = Mods(),
                        tooltipDescriptions = TooltipDescription(),
                        loreText = Get_Unique_Lore(),
                        tooltipEntries = TooltipEntries(),
                        oldSubTypeID = 0,
                        oldUniqueID = 0
                    };

                    return item;
                }

                public static void GetAbility()
                {
                    if (!Initialize_ability)
                    {
                        Initialize_ability = true;
                        if (!Refs_Manager.player_actor.IsNullOrDestroyed())
                        {
                            foreach (Ability ab in Resources.FindObjectsOfTypeAll<Ability>())
                            {
                                if (ab.name == poison_ability_name)
                                {
                                    ability = new Ability
                                    {
                                        name = "Herald of Agony",
                                        abilityName = "Herald of Agony",
                                        abilityObjectRotation = Ability.AbilityObjectRotation.FacingTarget,
                                        abilityObjectType = Ability.AbilityObjectType.Default,
                                        animation = AbilityAnimation.Cast,
                                        attachCastingVFXToCaster = true,
                                        attributeScaling = new Il2CppSystem.Collections.Generic.List<Ability.AttributeScaling>(),
                                        baseMovementAnimationLength = 1f,
                                        castingVFXPositioning = CastingVFXPositioning.Default,
                                        description = "",
                                        manaCost = 0f,
                                        moveOrAttackFallback = Ability.MoveOrAttackFallback.Wait,
                                        speedMultiplier = 1f,
                                        speedScaler = SP.CastSpeed,
                                        tags = ab.tags,
                                        useDelay = 0.4f,
                                        useDuration = 0.75f
                                    };
                                    break;
                                }
                            }
                        }
                        Initialize_ability = false;
                    }
                }
                public static void GetPrefab()
                {
                    if (!Initialize_prefab)
                    {
                        Initialize_prefab = true;
                        foreach (Ability ab in Resources.FindObjectsOfTypeAll<Ability>())
                        {
                            if (ab.name == poison_ability_name)
                            {
                                prefab_obj = Object.Instantiate(ab.abilityPrefab, Vector3.zero, Quaternion.identity);
                                prefab_obj.active = false;
                                prefab_obj.name = "Herald of Agony prefab";
                                SphereCollider collider = prefab_obj.GetComponent<UnityEngine.SphereCollider>();
                                if ((!collider.IsNullOrDestroyed()) && (increased_poison_collider_radius))
                                { collider.radius = SphereColliderRadius; }
                                CreateVfxOnDeath vfx_on_death = prefab_obj.GetComponent<CreateVfxOnDeath>();
                                if ((!vfx_on_death.IsNullOrDestroyed()) && (increased_poison_vfx_radius))
                                { vfx_on_death.increasedRadius = CreateVfxOnDeathIncreaseRadius; }
                                break;
                            }
                        }
                        Initialize_prefab = false;
                    }
                }
                public static string Get_Unique_Name()
                {
                    string result = "";
                    switch (Locales.current)
                    {
                        case Locales.Selected.English: { result = Languagues.Poison.UniqueName.en; break; }
                        case Locales.Selected.French: { result = Languagues.Poison.UniqueName.en; break; }
                        case Locales.Selected.German: { result = Languagues.Poison.UniqueName.en; break; }
                        case Locales.Selected.Russian: { result = Languagues.Poison.UniqueName.en; break; }
                        case Locales.Selected.Portuguese: { result = Languagues.Poison.UniqueName.en; break; }
                        case Locales.Selected.Korean: { result = Languagues.Poison.UniqueName.en; break; }
                        case Locales.Selected.Polish: { result = Languagues.Poison.UniqueName.en; break; }
                        case Locales.Selected.Chinese: { result = Languagues.Poison.UniqueName.en; break; }
                        case Locales.Selected.Spanish: { result = Languagues.Poison.UniqueName.en; break; }
                    }

                    return result;
                }
                public static string Get_Unique_Description()
                {
                    string result = "";
                    switch (Locales.current)
                    {
                        case Locales.Selected.English: { result = Languagues.Poison.UniqueDescription.en; break; }
                        case Locales.Selected.French: { result = Languagues.Poison.UniqueDescription.en; break; }
                        case Locales.Selected.Korean: { result = Languagues.Poison.UniqueDescription.en; break; }
                        case Locales.Selected.German: { result = Languagues.Poison.UniqueDescription.en; break; }
                        case Locales.Selected.Russian: { result = Languagues.Poison.UniqueDescription.en; break; }
                        case Locales.Selected.Polish: { result = Languagues.Poison.UniqueDescription.en; break; }
                        case Locales.Selected.Portuguese: { result = Languagues.Poison.UniqueDescription.en; break; }
                        case Locales.Selected.Chinese: { result = Languagues.Poison.UniqueDescription.en; break; }
                        case Locales.Selected.Spanish: { result = Languagues.Poison.UniqueDescription.en; break; }
                    }

                    return result;
                }
                public static string Get_Unique_Lore()
                {
                    string result = "";
                    switch (Locales.current)
                    {
                        case Locales.Selected.English: { result = Languagues.Poison.Lore.en; break; }
                        case Locales.Selected.French: { result = Languagues.Poison.Lore.en; break; }
                        case Locales.Selected.German: { result = Languagues.Poison.Lore.en; break; }
                        case Locales.Selected.Korean: { result = Languagues.Poison.Lore.en; break; }
                        case Locales.Selected.Russian: { result = Languagues.Poison.Lore.en; break; }
                        case Locales.Selected.Polish: { result = Languagues.Poison.Lore.en; break; }
                        case Locales.Selected.Portuguese: { result = Languagues.Poison.Lore.en; break; }
                        case Locales.Selected.Chinese: { result = Languagues.Poison.Lore.en; break; }
                        case Locales.Selected.Spanish: { result = Languagues.Poison.Lore.en; break; }
                    }

                    return result;
                }
                public static bool Equipped()
                {
                    bool r = false;
                    foreach (ItemContainerEntry entry in Refs_Manager.player_actor.itemContainersManager.idols.content)
                    {
                        if (entry.data.uniqueID == unique_id) { r = true; break; }
                    }

                    return r;
                }
                public static void Launch(GameObject actor, Vector3 target_position)
                {
                    if ((!ability.IsNullOrDestroyed()) && (!prefab_obj.IsNullOrDestroyed()))
                    {
                        if (ability.abilityPrefab.IsNullOrDestroyed()) { ability.abilityPrefab = Object.Instantiate(prefab_obj, Vector3.zero, Quaternion.identity); }
                        if (!ability.abilityPrefab.IsNullOrDestroyed())
                        {
                            //Vector3 position = new Vector3(target_position.x, target_position.y, (target_position.z + 10));
                            ability.abilityPrefab.active = true;
                            ability.CastAfterDelay(actor.GetComponent<AbilityObjectConstructor>(), target_position, target_position, 0f);
                        }
                    }
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
            }

            [HarmonyPatch(typeof(InventoryItemUI), "SetImageSpritesAndColours")]
            public class InventoryItemUI_SetImageSpritesAndColours
            {
                [HarmonyPostfix]
                static void Postfix(ref Il2Cpp.InventoryItemUI __instance)
                {
                    if ((__instance.EntryRef.data.getAsUnpacked().FullName == Ice.Get_Unique_Name()) && (!Ice.Icon.IsNullOrDestroyed()))
                    {
                        __instance.contentImage.sprite = Ice.Icon;
                    }
                    else if ((__instance.EntryRef.data.getAsUnpacked().FullName == Fire.Get_Unique_Name()) && (!Fire.Icon.IsNullOrDestroyed()))
                    {
                        __instance.contentImage.sprite = Fire.Icon;
                    }
                    else if ((__instance.EntryRef.data.getAsUnpacked().FullName == Lightning.Get_Unique_Name()) && (!Lightning.Icon.IsNullOrDestroyed()))
                    {
                        __instance.contentImage.sprite = Lightning.Icon;
                    }
                    else if ((__instance.EntryRef.data.getAsUnpacked().FullName == Poison.Get_Unique_Name()) && (!Poison.Icon.IsNullOrDestroyed()))
                    {
                        __instance.contentImage.sprite = Poison.Icon;
                    }
                }
            }

            [HarmonyPatch(typeof(UITooltipItem), "GetItemSprite")]
            public class UITooltipItem_GetItemSprite
            {
                [HarmonyPostfix]
                static void Postfix(ref UnityEngine.Sprite __result, ItemData __0)
                {
                    if ((__0.getAsUnpacked().FullName == Ice.Get_Unique_Name()) && (!Ice.Icon.IsNullOrDestroyed()))
                    {
                        __result = Ice.Icon;
                    }
                    else if ((__0.getAsUnpacked().FullName == Fire.Get_Unique_Name()) && (!Fire.Icon.IsNullOrDestroyed()))
                    {
                        __result = Fire.Icon;
                    }
                    else if ((__0.getAsUnpacked().FullName == Lightning.Get_Unique_Name()) && (!Lightning.Icon.IsNullOrDestroyed()))
                    {
                        __result = Lightning.Icon;
                    }
                    else if ((__0.getAsUnpacked().FullName == Poison.Get_Unique_Name()) && (!Poison.Icon.IsNullOrDestroyed()))
                    {
                        __result = Poison.Icon;
                    }
                }
            }
        }
        public class Languagues
        {
            public static string basic_subtype_name_key = "Item_SubType_Name_" + Basic.base_type + "_" + Basic.base_id;
            public static string Get_Subtype_Name()
            {
                string result = "";
                switch (Locales.current)
                {
                    case Locales.Selected.English: { result = SubType.en; break; }
                    case Locales.Selected.French: { result = SubType.en; break; }
                    case Locales.Selected.German: { result = SubType.en; break; }
                    case Locales.Selected.Russian: { result = SubType.en; break; }
                    case Locales.Selected.Portuguese: { result = SubType.en; break; }
                    case Locales.Selected.Korean: { result = SubType.en; break; }
                    case Locales.Selected.Polish: { result = SubType.en; break; }
                    case Locales.Selected.Chinese: { result = SubType.en; break; }
                    case Locales.Selected.Spanish: { result = SubType.en; break; }
                }

                return result;
            }
            public class SubType
            {
                public static string en = "Herald Idol";
                //Add all languages here
            }
            public class Ice
            {
                public static string unique_name_key = "Unique_Name_" + Uniques.Ice.unique_id;
                public static string unique_description_key = "Unique_Tooltip_0_" + Uniques.Ice.unique_id;
                public static string unique_lore_key = "Unique_Lore_" + Uniques.Ice.unique_id;

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
            }
            public class Fire
            {
                public static string unique_name_key = "Unique_Name_" + Uniques.Fire.unique_id;
                public static string unique_description_key = "Unique_Tooltip_0_" + Uniques.Fire.unique_id;
                public static string unique_lore_key = "Unique_Lore_" + Uniques.Fire.unique_id;

                public class UniqueName
                {
                    public static string en = "Herald of Ash";
                    //Add all languages here
                }
                public class UniqueDescription
                {
                    public static string en = "Grants a buff, when you or your minions Kill a monster, this item will cause them to explode and deal AoE fire damage to enemies near them";
                    //Add all languages here
                }
                public class Lore
                {
                    public static readonly string en = "";
                    //Add all languages here
                }
            }
            public class Lightning
            {
                public static string unique_name_key = "Unique_Name_" + Uniques.Lightning.unique_id;
                public static string unique_description_key = "Unique_Tooltip_0_" + Uniques.Lightning.unique_id;
                public static string unique_lore_key = "Unique_Lore_" + Uniques.Lightning.unique_id;

                public class UniqueName
                {
                    public static string en = "Herald of Thunder";
                    //Add all languages here
                }
                public class UniqueDescription
                {
                    public static string en = "Grants a buff, when you or your minions Kill a monster, this item will cause them to explode and deal AoE lightning damage to enemies near them";
                    //Add all languages here
                }
                public class Lore
                {
                    public static readonly string en = "";
                    //Add all languages here
                }
            }
            public class Poison
            {
                public static string unique_name_key = "Unique_Name_" + Uniques.Poison.unique_id;
                public static string unique_description_key = "Unique_Tooltip_0_" + Uniques.Poison.unique_id;
                public static string unique_lore_key = "Unique_Lore_" + Uniques.Poison.unique_id;

                public class UniqueName
                {
                    public static string en = "Herald of Agony";
                    //Add all languages here
                }
                public class UniqueDescription
                {
                    public static string en = "Grants a buff, when you or your minions Kill a monster, this item will cause them to explode and deal AoE poison damage to enemies near them";
                    //Add all languages here
                }
                public class Lore
                {
                    public static readonly string en = "";
                    //Add all languages here
                }
            }

            [HarmonyPatch(typeof(Localization), "TryGetText")]
            public class Localization_TryGetText
            {
                [HarmonyPrefix]
                static bool Prefix(ref bool __result, string __0) //, Il2CppSystem.String __1)
                {
                    bool result = true;
                    if ((__0 == basic_subtype_name_key) ||
                        (__0 == Ice.unique_name_key) || (__0 == Ice.unique_description_key) || (__0 == Ice.unique_lore_key) ||
                        (__0 == Fire.unique_name_key) || (__0 == Fire.unique_description_key) || (__0 == Fire.unique_lore_key) ||
                        (__0 == Lightning.unique_name_key) || (__0 == Lightning.unique_description_key) || (__0 == Lightning.unique_lore_key) ||
                        (__0 == Poison.unique_name_key) || (__0 == Poison.unique_description_key) || (__0 == Poison.unique_lore_key))
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
                        __result = Languagues.Get_Subtype_Name();
                        result = false;
                    }
                    //Ice
                    else if (__0 == Ice.unique_name_key)
                    {
                        __result = Uniques.Ice.Get_Unique_Name();
                        result = false;
                    }
                    else if (__0 == Ice.unique_description_key)
                    {
                        string description = Uniques.Ice.Get_Unique_Description();
                        if (description != "")
                        {
                            __result = description;
                            result = false;
                        }
                    }
                    else if (__0 == Ice.unique_lore_key)
                    {
                        string lore = Uniques.Ice.Get_Unique_Lore();
                        if (lore != "")
                        {
                            __result = lore;
                            result = false;
                        }
                    }
                    //Fire
                    else if (__0 == Fire.unique_name_key)
                    {
                        __result = Uniques.Fire.Get_Unique_Name();
                        result = false;
                    }
                    else if (__0 == Fire.unique_description_key)
                    {
                        string description = Uniques.Fire.Get_Unique_Description();
                        if (description != "")
                        {
                            __result = description;
                            result = false;
                        }
                    }
                    else if (__0 == Fire.unique_lore_key)
                    {
                        string lore = Uniques.Fire.Get_Unique_Lore();
                        if (lore != "")
                        {
                            __result = lore;
                            result = false;
                        }
                    }
                    //Lightning
                    else if (__0 == Lightning.unique_name_key)
                    {
                        __result = Uniques.Lightning.Get_Unique_Name();
                        result = false;
                    }
                    else if (__0 == Lightning.unique_description_key)
                    {
                        string description = Uniques.Lightning.Get_Unique_Description();
                        if (description != "")
                        {
                            __result = description;
                            result = false;
                        }
                    }
                    else if (__0 == Fire.unique_lore_key)
                    {
                        string lore = Uniques.Lightning.Get_Unique_Lore();
                        if (lore != "")
                        {
                            __result = lore;
                            result = false;
                        }
                    }
                    //Poison
                    else if (__0 == Poison.unique_name_key)
                    {
                        __result = Uniques.Poison.Get_Unique_Name();
                        result = false;
                    }
                    else if (__0 == Poison.unique_description_key)
                    {
                        string description = Uniques.Poison.Get_Unique_Description();
                        if (description != "")
                        {
                            __result = description;
                            result = false;
                        }
                    }
                    else if (__0 == Poison.unique_lore_key)
                    {
                        string lore = Uniques.Poison.Get_Unique_Lore();
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
                if (!Refs_Manager.player_actor.IsNullOrDestroyed())
                {
                    if ((Uniques.Ice.Equipped()) && (!Uniques.Ice.ability.IsNullOrDestroyed()))
                    {
                        Uniques.Ice.Launch(Refs_Manager.player_actor.gameObject, killedActor.position());
                    }
                    if ((Uniques.Fire.Equipped()) && (!Uniques.Fire.ability.IsNullOrDestroyed()))
                    {
                        Uniques.Fire.Launch(Refs_Manager.player_actor.gameObject, killedActor.position());
                    }
                    if ((Uniques.Lightning.Equipped()) && (!Uniques.Lightning.ability.IsNullOrDestroyed()))
                    {
                        Uniques.Lightning.Launch(Refs_Manager.player_actor.gameObject, killedActor.position());
                    }
                    if ((Uniques.Poison.Equipped()) && (!Uniques.Poison.ability.IsNullOrDestroyed()))
                    {
                        Uniques.Poison.Launch(Refs_Manager.player_actor.gameObject, killedActor.position());
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
                if (!Refs_Manager.player_actor.IsNullOrDestroyed())
                {
                    if ((Uniques.Ice.Equipped()) && (!Uniques.Ice.ability.IsNullOrDestroyed()))
                    {
                        Uniques.Ice.Launch(summon.gameObject, killedActor.position());
                    }
                    if ((Uniques.Fire.Equipped()) && (!Uniques.Fire.ability.IsNullOrDestroyed()))
                    {
                        Uniques.Fire.Launch(summon.gameObject, killedActor.position());
                    }
                    if ((Uniques.Lightning.Equipped()) && (!Uniques.Lightning.ability.IsNullOrDestroyed()))
                    {
                        Uniques.Lightning.Launch(summon.gameObject, killedActor.position());
                    }
                    if ((Uniques.Poison.Equipped()) && (!Uniques.Poison.ability.IsNullOrDestroyed()))
                    {
                        Uniques.Poison.Launch(summon.gameObject, killedActor.position());
                    }
                }
            }
        }        
    }
}
