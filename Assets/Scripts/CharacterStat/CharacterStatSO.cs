using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "NewCharacterDataSO", menuName = "ScriptableObjects/CharacterDataSO")]
    public class CharacterStatSO : ScriptableObject
    {
        public float HP;
        public float Def;
        public float Attack;
    }
}

