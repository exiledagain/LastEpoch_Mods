using Il2CppRewired;
using MelonLoader;
using UnityEngine;

namespace LastEpoch_Hud.Scripts.Mods.Character
{
    [RegisterTypeInIl2Cpp]
    public class Character_TwoHandedShield : MonoBehaviour
    {
        public Character_TwoHandedShield(System.IntPtr ptr) : base(ptr) { }
        public static Character_TwoHandedShield instance { get; private set; }

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if ((!Refs_Manager.character_mutator.IsNullOrDestroyed()) && (!Save_Manager.instance.IsNullOrDestroyed()))
            {
                if (Save_Manager.instance.data.Character.Cheats.Enable_TwoHandedWithShield)
                {
                    Refs_Manager.character_mutator.twohandersAllowedWithShieldBaseTypes = Il2Cpp.Item.TwoHandersAllowedWithShieldSetID.ForgeGuard;
                }
            }
        }
    }
}
