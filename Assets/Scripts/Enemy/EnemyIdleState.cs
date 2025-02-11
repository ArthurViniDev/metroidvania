using UnityEngine;

public class EnemyIdleState : MonoBehaviour, IStateEnemy
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform enemy;
    public void UpdateState()
    {
        transform.position = Vector2.Lerp(transform.position, enemy.position, moveSpeed * Time.deltaTime);
    }
    public void EnterState()
    {
        // logica
    }
    public void ExitState()
    {
        // logica
    }
}

