using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform playerTarget;

    public void FollowPlayer( float moveSpeed)
    {
        if(playerTarget) transform.position = Vector3.Lerp(transform.position, playerTarget.position, moveSpeed * Time.deltaTime);
    }
}
