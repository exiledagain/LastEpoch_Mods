using HarmonyLib;
using Il2Cpp;
using Il2CppLE.Factions;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Factions.TheWoven
{
    public class Faction_Woven_NodeChanges
    {
        //Use this file to set more points in weaver tree

        /*public static bool loaded = false;
        public static Il2CppSystem.Collections.Generic.List<byte> node_ids = null;
        public static Il2CppSystem.Collections.Generic.List<byte> node_points = null;
        public static SkillTreeNode Skill_Tree_Node = null;
        public static WeaverTree Weaver_Tree = null;
        public static int Max_Points = 50;
        public static bool click = false; //Fix Spam Click

        [HarmonyPatch(typeof(LocalTreeData), "LoadWeaverTree")]
        public class LocalTreeData_LoadWeaverTree
        {
            [HarmonyPostfix]
            static void Postfix(LocalTreeData __instance, ushort __0, Il2CppSystem.Collections.Generic.List<byte> __1, Il2CppSystem.Collections.Generic.List<byte> __2)
            {
                loaded = true;
                node_ids = __1;
                node_points = __2;
            }
        }

        [HarmonyPatch(typeof(SkillTreeNode), "Awake")]
        public class SkillTreeNode_Awake
        {
            [HarmonyPrefix]
            static void Prefix(ref SkillTreeNode __instance)
            {
                if (__instance.id == 51) { Skill_Tree_Node = __instance; }
            }
        }

        [HarmonyPatch(typeof(FactionRankPanelWeaver), "OpenWeaverTree")]
        public class FactionRankPanelWeaver_OpenWeaverTree
        {
            [HarmonyPrefix]
            static void Prefix(FactionRankPanelWeaver __instance)
            {
                if ((loaded) && (!Skill_Tree_Node.IsNullOrDestroyed()) && (!Refs_Manager.player_treedata.IsNullOrDestroyed()))
                {
                    loaded = false;
                    int index = 0;
                    foreach (byte node_id in node_ids)
                    {
                        if (node_id == Skill_Tree_Node.id)
                        {
                            if (index < node_points.Count)
                            {
                                foreach (LocalTreeData.NodeData node_data in Refs_Manager.player_treedata.weaverTree.nodes)
                                {
                                    if (node_data.id == Skill_Tree_Node.id)
                                    {
                                        node_data.pointsAllocated = node_points[index];
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                        index++;
                    }
                }
            }
            [HarmonyPostfix]
            static void Postfix(FactionRankPanelWeaver __instance)
            {
                if ((!Refs_Manager.player_treedata.IsNullOrDestroyed()) && (!Skill_Tree_Node.IsNullOrDestroyed()))
                {
                    Skill_Tree_Node.maxPoints = (byte)Max_Points;
                    int index = 0;
                    foreach (LocalTreeData.NodeData node_data in Refs_Manager.player_treedata.weaverTree.nodes)
                    {
                        if (node_data.id == Skill_Tree_Node.id)
                        {
                            Skill_Tree_Node.pointsAllocated = node_data.pointsAllocated;
                            break;
                        }
                        index++;
                    }
                    Skill_Tree_Node.updateText();
                }
            }
        }
        
        [HarmonyPatch(typeof(SkillTreeNode), "Clicked", new System.Type[] { })]
        public class SkillTreeNode_Clicked
        {
            [HarmonyPrefix]
            static bool Prefix(ref SkillTreeNode __instance)
            {
                bool r = true;
                if (click) { r = false; }
                else
                {
                    if (!Skill_Tree_Node.IsNullOrDestroyed())
                    {
                        click = true;
                        if (__instance.id == Skill_Tree_Node.id)
                        {
                            if ((__instance.pointsAllocated >= 3) && (__instance.pointsAllocated < (Max_Points)))
                            {
                                __instance.pointsAllocated++;
                                __instance.updateText();
                                if (Weaver_Tree.IsNullOrDestroyed())
                                {
                                    foreach (WeaverTree weaver_tree in Object.FindObjectsOfType<WeaverTree>())
                                    {
                                        if (weaver_tree._hasFirstTicked)
                                        {
                                            Weaver_Tree = weaver_tree;
                                            break;
                                        }
                                    }
                                }
                                if (!Weaver_Tree.IsNullOrDestroyed()) { Weaver_Tree.SaveTreeData(false); }
                                else { Main.logger_instance.Error("Weaver Tree Not Found"); }

                                if (!Refs_Manager.player_treedata.IsNullOrDestroyed())
                                {
                                    foreach (LocalTreeData.NodeData node_data in Refs_Manager.player_treedata.weaverTree.nodes)
                                    {
                                        if (node_data.id == Skill_Tree_Node.id)
                                        {
                                            node_data.pointsAllocated = (byte)__instance.pointsAllocated;
                                            break;
                                        }
                                    }
                                    Refs_Manager.player_treedata.SaveWeaverTreeData();
                                }
                                if (!Refs_Manager.player_data.IsNullOrDestroyed()) { Refs_Manager.player_data.SaveData(); }
                                r = false;
                            }
                            else if (__instance.pointsAllocated > Max_Points) { r = false; }
                        }
                        click = false;
                    }
                }                
                
                return r;
            }
        }

        [HarmonyPatch(typeof(SkillTreeNode), "enoughPointsAvailable")]
        public class SkillTreeNode_enoughPointsAvailable
        {
            [HarmonyPrefix]
            static bool Prefix(SkillTreeNode __instance, ref bool __result)
            {
                bool r = true;
                if (!Skill_Tree_Node.IsNullOrDestroyed())
                {
                    if (__instance.id == Skill_Tree_Node.id)
                    {
                        __result = true;
                        r = false;
                    }
                }

                return r;
            }
        }*/

        //Free points
        /*[HarmonyPatch(typeof(LocalTreeData), "TryToSpendWeaverPoint")]
        public class LocalTreeData_TryToSpendWeaverPoint
        {
            [HarmonyPrefix]
            static bool Prefix(ref LocalTreeData __instance, ref bool __result, byte __0)
            {
                bool r = true;
                if ((!__result) && (__0 == 51))
                {
                    __result = true;
                    r = false;
                }

                return r;
            }
        }*/
    }
}
