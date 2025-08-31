using HarmonyLib;
using Il2Cpp;

namespace LastEpoch_Hud.Scripts.Mods.Craft
{
    public class Craft_Locales
    {
        //Craft to T8
        public const string affix_is_maxed_key = "Crafting_ForgeButton_Title_AffixMaxed_2"; //LastEpoch v1.3.1.1
        public static string affix_is_maxed = "affix_maxed";

        [HarmonyPatch(typeof(Localization), "TryGetText")]
        public class Localization_TryGetText
        {
            [HarmonyPrefix]
            static bool Prefix(ref bool __result, string __0)
            {
                bool result = true;
                if (__0 == affix_is_maxed_key)
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
                switch (__0)
                {
                    case affix_is_maxed_key: { __result = affix_is_maxed; result = false; break; }
                }

                return result;
            }
        }
    }
}
