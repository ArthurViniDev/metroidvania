using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Vector2 boxSize;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] int angle;
    [SerializeField] float attackRange;
    [SerializeField] float knockbackForce;

    [SerializeField] PlayerAttack playerAttack;

    void Update()
    {
        angle = playerAttack.RotateAttackPoint();
    }

    public void ApplyKnockback()
    {
        rb.velocity = Vector2.up * knockbackForce;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
        Gizmos.matrix = oldMatrix;
    }
    public bool IsHittingEnemy()
    {
        return Physics2D.BoxCast(transform.position, boxSize, angle, Vector2.up, attackRange, enemyLayer);
    }

}