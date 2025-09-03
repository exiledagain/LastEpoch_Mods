using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Summon
{
    [RegisterTypeInIl2Cpp]
    public class Summon_GodMode : MonoBehaviour
    {
        public static Summon_GodMode instance { get; private set; }
        public Summon_GodMode(System.IntPtr ptr) : base(ptr) { }

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if ((CanRun()) && (!Refs_Manager.summon_tracker.IsNullOrDestroyed()))
            { 
                foreach (Summoned summoned in Refs_Manager.summon_tracker.summons)
                {
                    UnitHealth unit_health = summoned.gameObject.GetComponent<UnitHealth>();
                    if (!unit_health.IsNullOrDestroyed())
                    {
                        BaseHealth base_health = unit_health.TryCast<BaseHealth>();
                        if (!base_health.IsNullOrDestroyed())
                        {
                            if (base_health.damageable) { base_health.damageable = false; }
                        }
                    }
                }
            }
        }
        bool CanRun()
        {
            if ((Scenes.IsGameScene()) && (!Save_Manager.instance.IsNullOrDestroyed()))
            {
                if (!Save_Manager.instance.data.IsNullOrDestroyed())
                {
                    return Save_Manager.instance.data.Summon.Enable_GodMode;
                }
                else { return false; }
            }
            else { return false; }
        }
    }
}
