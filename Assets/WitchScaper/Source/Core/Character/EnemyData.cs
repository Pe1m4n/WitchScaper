using UnityEngine;

namespace WitchScaper.Core.Character
{
    [CreateAssetMenu(menuName = "WitchScaper/Enemy Data", fileName = "New Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public GameObject Prefab;
        public ColorType Color;
        public float TurnedTimeSeconds;
        public float AggroRadius = 10f;
    }
}