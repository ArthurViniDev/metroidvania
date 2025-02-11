using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLevelConfig", menuName = "Configs/Enemy Level")]
public class EnemyLevelConfig : ScriptableObject
{
    [System.Serializable]
    public class LevelData
    {
        public int level;
        public float damage;
        public float moveSpeed;
        public float attackTimer;
        public int life;

        public float StartAttackTimer;
        public float StopAttackTimer;
    }

    public LevelData[] levels;
}
