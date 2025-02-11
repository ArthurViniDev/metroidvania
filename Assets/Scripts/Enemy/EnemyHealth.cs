using UnityEngine;

public class EnemyHealth : BaseHealth
{
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] Enemy enemy;
    public override void TakeDamage(float damage)
    {
        particleSystem.Play();
        bool isIdleState = enemy.currentState is EnemyIdleState;
        bool isDeathState = enemy.currentState is EnemyDeathState;
        if(enemy && isIdleState || isDeathState) return;
        base.TakeDamage(damage);
    }
}
