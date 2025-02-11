using System.Collections;
using UnityEngine;

public class EnemyAttackState : MonoBehaviour, IStateEnemy
{
    [SerializeField] bool canAttack;

    float startAttackTimer;
    float stopAttackTimer;
    [SerializeField] float moveSpeed;

    [SerializeField] EnemyFollowPlayer enemyFollowPlayer;
    [SerializeField] Enemy enemy;

    Coroutine attackCoroutine;

    public void EnterState()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        canAttack = false;
        startAttackTimer = enemy.StartAttackTimer;
        stopAttackTimer = enemy.StopAttackTimer;
        attackCoroutine = StartCoroutine(ResetTimer());
    }

    public void ExitState()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    public void UpdateState()
    {
        if (canAttack)
        {
            enemyFollowPlayer.FollowPlayer(moveSpeed);
        }
        else
        {
            CantAttack();
        }
    }

    IEnumerator ResetTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(startAttackTimer);
            canAttack = true;
            yield return new WaitForSeconds(stopAttackTimer);
            canAttack = false;
        }
    }

    void CantAttack()
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, 1.5f), moveSpeed * Time.deltaTime);
    }
}
