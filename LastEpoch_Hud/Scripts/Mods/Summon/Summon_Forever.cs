using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Summon
{
    [RegisterTypeInIl2Cpp]
    public class Summon_Forever : MonoBehaviour
    {
        public static Summon_Forever instance { get; private set; }
        public Summon_Forever(System.IntPtr ptr) : base(ptr) { }

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
                    UnsummonAfterDelay unsummon = summoned.gameObject.GetComponent<UnsummonAfterDelay>();
                    if (!unsummon.IsNullOrDestroyed()) { Destroy(unsummon); }
                }
            }
        }
        bool CanRun()
        {
            if ((Scenes.IsGameScene()) && (!Save_Manager.instance.IsNullOrDestroyed()))
            {
                if (!Save_Manager.instance.data.IsNullOrDestroyed())
                {
                    return Save_Manager.instance.data.Summon.Enable_Forever;
                }
                else { return false; }
            }
            else { return false; }
        }
    }
}
