using UnityEngine;

namespace _Project.Scripts.PlayerWeapons.Configs
{
    [CreateAssetMenu(fileName = "ShipLaserConfig", menuName = "Scriptable Objects/ShipLaserConfig")]
    public class ShipLaserConfig : ScriptableObject
    {
        public LayerMask LayersToDestroy;
    }
}