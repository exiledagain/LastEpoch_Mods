using HarmonyLib;
using Il2CppLE.Services.Visuals;
using Il2CppLE.Services.Visuals.Portals.Data;

namespace LastEpoch_Hud.Scripts.Mods.Cosmetics
{
    public class Cosmetics_Portal
    {
        /*public static void GetAllPortals()
        {
            foreach (Il2CppLE.Services.Cosmetics.Cosmetic cosmetic in Object.FindObjectsOfType<Il2CppLE.Services.Cosmetics.Cosmetic>())
            {
                if (cosmetic.Type == CosmeticType.PORTAL)
                {

                }
            }
        }*/

        [HarmonyPatch(typeof(ClientVisualsService), "GetPortalVisual")]
        public class ClientVisualsService_GetPortalVisual
        {
            [HarmonyPrefix]
            static void Prefix(ClientVisualsService __instance, ref PortalVisualKey __0)
            {
                //Main.logger_instance.Msg("ClientVisualsService.GetPortalVisual();");
                Refs_Manager.client_visual_service = __instance;

                if (!Save_Manager.instance.IsNullOrDestroyed())
                {
                    if (!Save_Manager.instance.data.IsNullOrDestroyed())
                    {
                        ushort result = Save_Manager.instance.data.Cosmetics.Portal;
                        if (result < 39) { __0.Variation = result;  }
                    }
                }
            }
        }
    }
}
