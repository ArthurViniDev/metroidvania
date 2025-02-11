using UnityEngine;

public class EnemyEyes : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Enemy enemy;

    void Update()
    {
        if (enemy.currentState == (IStateEnemy)enemy.GetComponent<EnemyAttackState>())
        {
            spriteRenderer.color = Color.red;
        }
        else if (enemy.currentState == (IStateEnemy)enemy.GetComponent<EnemyIdleState>())
        {
            spriteRenderer.color = Color.white;
        }
        else if (enemy.currentState == (IStateEnemy)enemy.GetComponent<EnemyFollowState>())
        {
            spriteRenderer.color = Color.cyan;
        }
        else if (enemy.currentState == (IStateEnemy)enemy.GetComponent<EnemyDeathState>())
        {
            spriteRenderer.color = Color.black;
        }
    }

    public void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
