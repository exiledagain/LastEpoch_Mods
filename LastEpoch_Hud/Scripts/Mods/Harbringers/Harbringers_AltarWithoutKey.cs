using HarmonyLib;
using Il2Cpp;

namespace LastEpoch_Hud.Scripts.Mods.Harbringers
{
    public class Harbringers_AltarWithoutKey
    {
        public static bool CanRun()
        {
            if ((Scenes.IsGameScene()) && (!Save_Manager.instance.IsNullOrDestroyed()))
            {
                if (!Save_Manager.instance.data.IsNullOrDestroyed())
                {
                    return Save_Manager.instance.data.Scenes.harbringers.Enable_AltarWithoutKey;
                }
                else { return false; }
            }
            else { return false; }
        }

        [HarmonyPatch(typeof(PinnacleEnterPanelUI), "Open")]
        public class PinnacleEnterPanelUI_Open
        {
            [HarmonyPrefix]
            static void Prefix(ref PinnacleEnterPanelUI __instance)
            {
                if (CanRun())
                {
                    if (!__instance.keyContainer.IsNullOrDestroyed())
                    {
                        if (!__instance.keyContainer.Container.IsNullOrDestroyed())
                        {
                            OneSlotItemContainer one_slot_container = __instance.keyContainer.Container.TryCast<OneSlotItemContainer>();
                            if (!one_slot_container.IsNullOrDestroyed())
                            {                                
                                if (one_slot_container.content.IsNullOrDestroyed())
                                {
                                    ItemData item_data = new ItemData { itemType = 104, subType = 7, rarity = 0 };
                                    item_data.RefreshIDAndValues();
                                    one_slot_container.content = new ItemContainerEntry(item_data, new UnityEngine.Vector2Int(0, 0), new UnityEngine.Vector2Int(1, 1), 1);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
