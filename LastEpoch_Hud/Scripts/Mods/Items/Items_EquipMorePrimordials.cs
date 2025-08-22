using HarmonyLib;
using Il2Cpp;

namespace LastEpoch_Hud.Scripts.Mods.Items
{
    public class Items_EquipMorePrimordials
    {
        [HarmonyPatch(typeof(CharacterMutator), "GetMaxPrimordialItems")]
        public class CharacterMutator_GetMaxPrimordialItems
        {
            [HarmonyPrefix]
            static bool Prefix(/*ref CharacterMutator __instance,*/ ref int __result)
            {
                __result = 99;
                return false;
            }
        }
    }
}
