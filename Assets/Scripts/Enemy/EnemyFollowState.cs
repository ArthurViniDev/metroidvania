using UnityEngine;

public class EnemyFollowState : MonoBehaviour, IStateEnemy
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float moveSpeed;

    [SerializeField] Enemy enemy;
    [SerializeField] Transform enemyRotation;
    [SerializeField] EnemyFollowPlayer enemyFollowPlayer;
    public void EnterState()
    {

    }

    public void UpdateState()
    {
        enemyRotation.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.World);
        enemyFollowPlayer.FollowPlayer(enemy.levelMoveSpeed);
    }

    public void ExitState()
    {
        enemyRotation.rotation = new Quaternion(0, 0, 0, 0);       
    }
}
