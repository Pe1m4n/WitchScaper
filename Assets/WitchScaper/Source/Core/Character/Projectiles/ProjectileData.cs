using UnityEngine;
using WitchScaper.Core.Character;

namespace WitchScaper.Core
{
    [CreateAssetMenu(menuName = "WitchScaper/ProjectileData")]
    public class ProjectileData : ScriptableObject
    {
        public enum Type
        {
            Hex,
            Damage
        }
        
        public GameObject Prefab;
        public float Speed;
        public Type ProjectileType;
        public ColorType ProjectileColor;
    }
}