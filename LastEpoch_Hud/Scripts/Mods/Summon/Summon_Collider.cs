using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Summon
{
    [RegisterTypeInIl2Cpp]
    public class Summon_Collider : MonoBehaviour
    {
        public static Summon_Collider instance { get; private set; }
        public Summon_Collider(System.IntPtr ptr) : base(ptr) { }

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
                    UnityEngine.AI.NavMeshAgent nav_mesh_agent = summoned.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();                    
                    if (!nav_mesh_agent.IsNullOrDestroyed())
                    {
                        if (nav_mesh_agent.radius > 0) { nav_mesh_agent.radius = 0; }
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
                    return Save_Manager.instance.data.Summon.Enable_DontCollide;
                }
                else { return false; }
            }
            else { return false; }
        }
    }
}
