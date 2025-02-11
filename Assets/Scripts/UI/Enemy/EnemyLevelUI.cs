using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLevelUI : MonoBehaviour
{
    [SerializeField] RawImage[] levels;
    [SerializeField] Enemy enemy;
    [SerializeField] Color32 color;
    int enemyLevel;
    // Start is called before the first frame update
    void Start()
    {
        enemyLevel = enemy.level;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyLevel != enemy.level)
        {
            for (int i = 0; i <= enemyLevel; i++)
            {
                levels[i].color = color;
            }
            enemyLevel = enemy.level;
        }
    }
}
