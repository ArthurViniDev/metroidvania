using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackCooldown = .3f;
    [SerializeField] public bool canAttack;
    [SerializeField] private bool canHit;
    [SerializeField] private bool hasHit;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject playerGameObject;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerAttackTrigger playerAttackTrigger;
    [SerializeField] EnemyHealth enemyHealth;

    public enum AttackDirection { Up, Down, Forward, Backward }

    public AttackDirection attackDirection;

    void SetAttackDirection()
    {
        if (!canAttack) return;
        if (Input.GetKey(KeyCode.W))
        {
            attackDirection = AttackDirection.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            attackDirection = AttackDirection.Down;
        }
        else
        {
            SetPlayerFaceDirection();
        }
    }

    void SetPlayerFaceDirection()
    {
        attackDirection = playerMovement.isFacingRight ? AttackDirection.Forward : AttackDirection.Backward;
    }

    private void Awake()
    {
        attackDirection = AttackDirection.Forward;
    }

    public void PlayerHit()
    {
        if (hasHit) return;
        enemyHealth.TakeDamage(20);
        if (attackDirection == AttackDirection.Down) playerAttackTrigger.ApplyKnockback();
        hasHit = true;
    }

    private void Update()
    {
        Attack();
        SetAttackDirection();
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            bool horizontalAttack = attackDirection == AttackDirection.Forward || attackDirection == AttackDirection.Backward;
            if (horizontalAttack)
            {
                playerGameObject.GetComponent<Animator>().SetTrigger("playerAttack");
            }
            canAttack = false;
            playerAnimator.SetTrigger("Attack");
            StartCoroutine(EnableAttack());
        }
        else if (!canAttack)
        {
            if (playerAttackTrigger.IsHittingEnemy() && canHit && !hasHit)
            {
                PlayerHit();
                FindObjectOfType<AudioManager>().AudioPlay("PlayerHit");
            }
        }
        RotateAttackPoint();
    }

    public void PlayerCanHit()
    {
        canHit = true;
    }

    public void PlayerCantHit()
    {
        canHit = false;
    }

    IEnumerator EnableAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        playerGameObject.GetComponent<Animator>().ResetTrigger("playerAttack");
        playerAnimator.SetTrigger("Attack");
        hasHit = false;
    }

    public int RotateAttackPoint()
    {
        switch (attackDirection)
        {
            case AttackDirection.Up:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                return 90;
            case AttackDirection.Down:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                return -90;
            case AttackDirection.Forward:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                return 0;
            case AttackDirection.Backward:
                transform.rotation = Quaternion.Euler(0, -180, 0);
                return 180;
            default: return 0;
        }
    }
}
