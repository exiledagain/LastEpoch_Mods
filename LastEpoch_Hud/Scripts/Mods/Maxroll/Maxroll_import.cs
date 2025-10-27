//Mod From https://github.com/exiledagain

using Il2Cpp;
using Il2CppLE.Data;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Maxroll
{
    public class Maxroll_import
    {
        public class Root
        {
            [JsonProperty("items")]
            public Items Items;

            [JsonProperty("idols")]
            public System.Collections.Generic.List<Item> Idols;

            [JsonProperty("blessings")]
            public System.Collections.Generic.List<Blessing> Blessings;

            // items placed inside the weaver tree
            [JsonProperty("weaverItems")]
            public System.Collections.Generic.List<Item> WeaverItems;

            [JsonProperty("weaver")]
            public WeaverTree WeaverTree;

            [JsonProperty("skillTrees")]
            public System.Collections.Generic.Dictionary<string, TreeHistory> Skills;

            [JsonProperty("passives")]
            public TreeHistory PassiveNodes;

            [JsonProperty("class")]
            public int? Class;

            [JsonProperty("mastery")]
            public byte? Mastery;
        }

        public class Items
        {
            [JsonProperty("body")]
            public Item Body;

            [JsonProperty("offhand")]
            public Item Offhand;

            [JsonProperty("waist")]
            public Item Waist;

            [JsonProperty("feet")]
            public Item Feet;

            [JsonProperty("finger1")]
            public Item Finger1;

            [JsonProperty("finger2")]
            public Item Finger2;

            [JsonProperty("neck")]
            public Item Neck;

            [JsonProperty("relic")]
            public Item Relic;

            [JsonProperty("hands")]
            public Item Hands;

            [JsonProperty("head")]
            public Item Head;

            [JsonProperty("weapon")]
            public Item Weapon;
        }

        public class Item
        {
            [JsonProperty("itemType")]
            public byte ItemType;

            [JsonProperty("subType")]
            public ushort SubType;

            [JsonProperty("uniqueID")]
            public ushort? UniqueID;

            [JsonProperty("uniqueRolls")]
            public System.Collections.Generic.List<double?> UniqueRolls;

            [JsonProperty("affixes")]
            public System.Collections.Generic.List<Affix> Affixes;

            [JsonProperty("sealedAffix")]
            public Affix SealedAffix;

            [JsonProperty("primordialAffix")]
            public Affix PrimordialAffix;

            [JsonProperty("implicits")]
            public System.Collections.Generic.List<double?> Implicits;
        }

        public class Blessing
        {
            [JsonProperty("itemType")]
            public int ItemType;

            [JsonProperty("subType")]
            public ushort SubType;

            [JsonProperty("implicits")]
            public System.Collections.Generic.List<double?> Implicits;
        }

        public class Affix
        {
            [JsonProperty("id")]
            public int Id;

            [JsonProperty("tier")]
            public int Tier;

            [JsonProperty("roll")]
            public double? Roll;
        }

        public class TreeHistory
        {
            [JsonProperty("history")]
            public System.Collections.Generic.List<byte> History;

            [JsonProperty("position")]
            public int Position;
        }

        public class WeaverTree
        {
            [JsonProperty("weaver")]
            public TreeHistory WeaverNodes;
        }

        public static void ImportJson()
        {
            if (Refs_Manager.ground_item_manager.IsNullOrDestroyed() || Refs_Manager.player_actor.IsNullOrDestroyed())
            {
                return;
            }

            try
            {
                string text = GUIUtility.systemCopyBuffer;
                Root root = JsonConvert.DeserializeObject<Root>(text);

                CreateItems(root);
                LearnPassives(root);
                LearnSkillTrees(root);
                LearnBlessings(root);
            }
            catch (Exception ex)
            {
                Main.logger_instance.Error(ex);
            }
        }

        private static void LearnBlessing(int slotId, Blessing blessing)
        {
            if (blessing.ItemType != (int)EquipmentType.BLESSING)
            {
                Main.logger_instance.Error("Learn blessing failed to learn non-blessing.");
                return;
            }

            var containedId = (ContainerID)Enum.Parse(typeof(ContainerID), "BLESSING_" + slotId);

            var item = new ItemDataUnpacked()
            {
                itemType = (int)EquipmentType.BLESSING,
                subType = blessing.SubType,
            };
            item.SetAllImplicitRolls(255);

            if (blessing.Implicits != null)
            {
                for (int i = 0; i < item.implicitRolls.Count && i < blessing.Implicits.Count; i++)
                {
                    item.implicitRolls[i] = EpochExtensions.clampToByte((int)(255 * (blessing.Implicits[i] ?? 0.0)));
                }
            }
            item.RefreshIDAndValues();

            Refs_Manager.item_containers_manager.GetContainer(containedId).Clear();
            Refs_Manager.item_containers_manager.GetContainer(containedId).TryAddItem(item, 1, Context.SILENT);
        }

        private static void LearnBlessings(Root root)
        {
            if (root.Blessings != null)
            {
                for(int i = 0; i < root.Blessings.Count; ++i)
                {
                    if (root.Blessings[i] != null)
                    {
                        LearnBlessing(i, root.Blessings[i]);
                    }
                }
            }
        }

        private static void ApplySkillHistory(Ability ability, TreeHistory treeHistory)
        {
            var player_actor = Refs_Manager.player_actor;

            foreach (var id in treeHistory.History)
            {
                if (!player_actor.localTreeData.receiveSpendSkillPointCommand(ability, id))
                {
                    break;
                }
            }
        }

        private static bool CanLearnAbility(string abilityId)
        {
            var player_actor = Refs_Manager.player_actor;
            var player_data = Refs_Manager.player_data;

            var namePred = (Ability e) =>
            {
                return e.playerAbilityID == abilityId;
            };

            return player_data.GetCharacterClass().getUnlockedAbilities(player_data.Level, player_actor.localTreeData.masteryLevels, player_data.ChosenMastery).FindIndex(namePred) >= 0;
        }

        private static void LearnSkillTree(string abilityId, TreeHistory treeHistory)
        {
            var player_actor = Refs_Manager.player_actor;

            if (!CanLearnAbility(abilityId))
            {
                return;
            }

            foreach (var ability in player_actor.localTreeData.getSpecialisedAbilities())
            {
                if (ability.playerAbilityID == abilityId)
                {
                    player_actor.localTreeData.receiveDespecialiseCommand(ability);
                    player_actor.localTreeData.receiveSpecialiseCommand(ability, false, 0);
                    player_actor.localTreeData.ApplyAbilityXp(100_000_000, true);
                    ApplySkillHistory(ability, treeHistory);
                    return;
                }
            }

            // delete all if no slots available
            if (player_actor.localTreeData.getFreeSlots().Count < 1)
            {
                foreach (var ability in player_actor.localTreeData.getSpecialisedAbilities())
                {
                    player_actor.localTreeData.receiveDespecialiseCommand(ability);
                }
            }

            // add the new skill
            foreach (var ability in Refs_Manager.ability_manager.abilities)
            {
                if (ability && ability.playerAbilityID == abilityId)
                {
                    player_actor.localTreeData.receiveSpecialiseCommand(ability, false, 0);
                    player_actor.localTreeData.ApplyAbilityXp(100000000, true);
                    ApplySkillHistory(ability, treeHistory);
                    return;
                }
            }
        }

        private static void LearnSkillTrees(Root root)
        {
            if (root.Skills == null)
            {
                return;
            }

            foreach (var pair in root.Skills)
            {
                LearnSkillTree(pair.Key, pair.Value);
            }
        }

        private static void LearnPassives(Root root)
        {
            if (!(root.Mastery.HasValue && root.Class.HasValue && root.PassiveNodes != null))
            {
                if (root.Mastery.HasValue)
                {
                    Main.logger_instance.Error("Learning passives failed but mastery was found.");
                }
                if (root.Class.HasValue)
                {
                    Main.logger_instance.Error("Learning passives failed but class was found.");
                }
                if (root.PassiveNodes != null)
                {
                    Main.logger_instance.Error("Learning passives failed but passives were found.");
                }
                return;
            }

            if (Refs_Manager.player_data.CharacterClass != root.Class.Value)
            {
                Main.logger_instance.Error("Learning passives failed because the class is wrong.");
                return;
            }

            var player_actor = Refs_Manager.player_actor;
            var player_data = Refs_Manager.player_data;
            player_actor.localTreeData.ReceiveRespecAllCommand(player_data.GetCharacterClass(), 0);
            player_actor.localTreeData.resetChosenMastery();
            player_actor.localTreeData.receiveChooseMasteriesCommand(root.Mastery.Value);
            foreach (var e in root.PassiveNodes.History)
            {
                if (player_actor.localTreeData.getPassiveTreeData().getUnspentPoints() > 0)
                {
                    if (!player_actor.localTreeData.receiveSpendPassivePointCommand(player_data.GetCharacterClass(), e))
                    {
                        break;
                    }
                }
            }
        }

        private static void CreateItems(Root root)
        {
            System.Collections.Generic.List<Item> itemsToDrop = new();

            if (root.Items != null)
            {
                System.Collections.Generic.List<Item> equippableItems = new()
                    {
                        root.Items.Head,
                        root.Items.Neck,
                        root.Items.Weapon,
                        root.Items.Body,
                        root.Items.Offhand,
                        root.Items.Finger1,
                        root.Items.Waist,
                        root.Items.Finger2,
                        root.Items.Hands,
                        root.Items.Feet,
                        root.Items.Relic
                    };

                itemsToDrop.AddRange(equippableItems);
            }
            itemsToDrop.AddRange(root.Idols ?? new());

            foreach (Item item in itemsToDrop)
            {
                if (item != null)
                {
                    CreateItem(item);
                }
            }
        }

        private static void CreateItem(Item spec)
        {
            var item = new ItemDataUnpacked()
            {
                itemType = spec.ItemType,
                subType = spec.SubType
            };

            item.SetAllImplicitRolls(255);
            item.RefreshIDAndValues();

            if (spec.Implicits != null)
            {
                for (int i = 0; i < item.implicitRolls.Count && i < spec.Implicits.Count; ++i)
                {
                    item.implicitRolls[i] = EpochExtensions.clampToByte((int)(255 * (spec.Implicits[i] ?? 0.0)));
                }
            }
            item.RefreshIDAndValues();

            // order matters here: unique, affixes, primordial, sealed
            Il2CppSystem.Collections.Generic.List<Stats.Stat> changes = new();
            if (spec.UniqueID != null)
            {
                item.uniqueID = spec.UniqueID.Value;
                item.rarity = (byte)(UniqueList.getUnique(item.uniqueID).isSetItem ? 8 : 7);
                if (spec.UniqueRolls != null)
                {
                    for (int i = 0; i < item.uniqueRolls.Count && i < spec.UniqueRolls.Count; i++)
                    {
                        item.uniqueRolls[i] = EpochExtensions.clampToByte((int)(255 * (spec.UniqueRolls[i] ?? 0.0)));
                    }
                }
                item.RefreshIDAndValues();
            }
            if (spec.Affixes != null)
            {
                if (item.isUnique() && spec.Affixes != null && spec.Affixes.Count > 0)
                {
                    item.rarity = 9;
                }
                if (spec.Affixes != null)
                {
                    for (int i = 0; i < 4 && i < spec.Affixes.Count; ++i)
                    {
                        Il2CppSystem.Nullable<byte> roll = new(EpochExtensions.clampToByte((int)(255 * (spec.Affixes[i].Roll ?? 0.0))));
                        item.AddAffixNoCostOrChecks(spec.Affixes[i].Id, false, Math.Clamp(spec.Affixes[i].Tier - 1, 0, 6), ref changes, roll);
                    }
                }
                if (!item.isUniqueSetOrLegendary())
                {
                    item.setRarityFromAffixesForNormalMagicOrRareItem();
                    item.forgingPotential = 40;
                }
            }
            if (spec.PrimordialAffix != null)
            {
                var affix = spec.PrimordialAffix;
                Il2CppSystem.Nullable<byte> roll = new(EpochExtensions.clampToByte((int)(255 * (affix.Roll ?? 0.0))));
                item.AddAffixNoCostOrChecks(affix.Id, true, Math.Clamp(affix.Tier - 1, 0, 7), ref changes, roll);
                foreach (var itemAffix in item.affixes)
                {
                    if (itemAffix.affixId == affix.Id)
                    {
                        // the function that makes an affix primordial doesn't seem to work but this does
                        itemAffix.affixTier = 7;
                        itemAffix.sealedAffixType = SealedAffixType.Primordial;
                        item.hasSealedPrimordialAffix = true;
                        break;
                    }
                }
            }
            if (spec.SealedAffix != null)
            {
                var affix = spec.SealedAffix;
                Il2CppSystem.Nullable<byte> roll = new(EpochExtensions.clampToByte((int)(255 * (affix.Roll ?? 0.0))));
                item.AddAffixNoCostOrChecks(affix.Id, true, Math.Clamp(affix.Tier - 1, 0, 6), ref changes, roll);
                foreach (var itemAffix in item.affixes)
                {
                    if (itemAffix.affixId == affix.Id)
                    {
                        item.SealAffix(itemAffix);
                        item.hasSealedRegularAffix = true;
                        break;
                    }
                }
            }

            item.RefreshIDAndValues();
            Refs_Manager.ground_item_manager.dropItemForPlayer(Refs_Manager.player_actor, item, Refs_Manager.player_actor.position(), false);
        }
    }
}
