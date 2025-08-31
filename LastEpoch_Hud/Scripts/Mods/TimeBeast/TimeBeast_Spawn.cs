using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.TimeBeast
{
    [RegisterTypeInIl2Cpp]
    public class TimeBeast_Spawn : MonoBehaviour
    {
        public TimeBeast_Spawn(System.IntPtr ptr) : base(ptr) { }
        public static TimeBeast_Spawn instance { get; private set; }

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if ((Scenes.IsGameScene()) && (!Refs_Manager.player_actor.IsNullOrDestroyed()))
            {
                if (Input.GetKeyDown(KeyCode.F1)) //MysteriousRift
                {
                    RandomEncounterManager.DestroyAllTimeBeastRifts();
                    RandomEncounterManager.PlaceTimeBeastRift(Refs_Manager.player_actor.position(), Refs_Manager.player_actor);
                }
                if (Input.GetKeyDown(KeyCode.F2)) //RiftBeast
                {
                    RandomEncounterManager.DestroyAllTimeBeastEggs();
                    RandomEncounterManager.PlaceTimeBeastEggs(Refs_Manager.player_actor.position(), Refs_Manager.player_actor, null);
                }
            }
        }
    }
}
