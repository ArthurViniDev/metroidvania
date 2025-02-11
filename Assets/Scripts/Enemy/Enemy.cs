using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public IStateEnemy currentState;

    public List<IStateEnemy> states = new List<IStateEnemy>();

    [SerializeField] new ParticleSystem particleSystem;
    //[SerializeField] float setStateTimer;
    [SerializeField] EnemyHealth enemyHealth;

    [SerializeField] EnemyLevelConfig enemyLevelConfig;

    [Header("Normal Difficulty Variables")]
    [HideInInspector] public int level = 1;
    [HideInInspector] public float levelDamage;
    [HideInInspector] public float levelMoveSpeed;
    [HideInInspector] public float levelAttackTimer;
    [HideInInspector] public int levelLife;

    // Attack State
    [Header("Attack State")]
    public float StartAttackTimer;
    public float StopAttackTimer;

    void Start()
    {
        states.Add(GetComponent<EnemyAttackState>());
        states.Add(GetComponent<EnemyIdleState>());
        states.Add(GetComponent<EnemyFollowState>());
        states.Add(GetComponent<EnemyDeathState>());

        if (states.Count > 0)
        {
            SetLevelAttributes();
            StartCoroutine(ToggleState());
            currentState = states[2];  // Inicializa com o estado de seguir o jogador
        }
        currentState?.EnterState();
    }

    void Update()
    {
        //Debug.Log(currentState.ToString());
        if (enemyHealth.healthAmount <= 0 && currentState != states[3])
        {
            currentState = states[3];
            StartCoroutine(ReviveEnemy());
            return;
        }
        currentState?.UpdateState();
    }

    void ChangeState(IStateEnemy newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }

    void SetLevelAttributes()
    {
        bool levelFound = false;

        foreach (var enemyLevel in enemyLevelConfig.levels)
        {
            if (enemyLevel.level == level)
            {
                levelDamage = enemyLevel.damage;
                levelMoveSpeed = enemyLevel.moveSpeed;
                levelAttackTimer = enemyLevel.attackTimer;
                levelLife = enemyLevel.life;
                levelFound = true;

                StartAttackTimer = enemyLevel.StartAttackTimer;
                StopAttackTimer = enemyLevel.StopAttackTimer;
                break;
            }
        }

        if (!levelFound)
        {
            //Debug.LogWarning($"[Enemy] Nenhuma configura��o encontrada para o level {level}. Mantendo os atributos do n�vel anterior.");
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator ReviveEnemy()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        //setStateTimer = 4f;

        // Reinicia os estados ap�s reviver
        foreach (var state in states)
        {
            if (state is EnemyAttackState attackState)
            {
                attackState.EnterState();  // Reinicia o ataque com os novos valores
            }
        }

        yield return new WaitForSeconds(2);
        level++;
        SetLevelAttributes();

        enemyHealth.healthAmount = levelLife;
        enemyHealth.startLife = levelLife;
        enemyHealth.healthBar.fillAmount = 1;
    }

    IEnumerator ToggleState()
    {
        while (true)
        {
            yield return new WaitForSeconds(levelAttackTimer);
            if (enemyHealth.healthAmount > 0)  // Garante que o inimigo esteja vivo
            {
                ChangeState(GetRandomState());
            }
        }
    }

    IStateEnemy GetRandomState()
    {
        if (states.Count == 0) return null;
        List<IStateEnemy> validStates = new List<IStateEnemy>(states);
        validStates.Remove(states[3]);  // Remove o estado de morte
        if (level >= 2) validStates.Remove(GetComponent<EnemyIdleState>());
        return validStates[Random.Range(0, validStates.Count)];
    }
}
