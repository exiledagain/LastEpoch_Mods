using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Spawn
{
    [RegisterTypeInIl2Cpp]
    public class RunePrison : MonoBehaviour
    {
        public RunePrison(System.IntPtr ptr) : base(ptr) { }
        public static RunePrison instance { get; private set; }
        
        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            /*if ((Scenes.IsGameScene()) && (!Refs_Manager.player_actor.IsNullOrDestroyed()))
            {
                if (Input.GetKeyDown(KeyCode.F5))
                {
                    RandomEncounterManager.DestroyAllRunePrisons();
                    RandomEncounterManager.PlaceRunePrison(Refs_Manager.player_actor.position(), Refs_Manager.player_actor);
                }
            }*/
        }
    }
}
