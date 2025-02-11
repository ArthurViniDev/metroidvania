using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : BaseHealth
{
    bool canTakeDamage;
    bool isInDamageCooldown;

    [SerializeField] float damageCooldown;
    [SerializeField] float effectCooldown;
    [SerializeField] Enemy enemy;

    PlayerMovement playerMovement;

    [Header("Sprite Effect")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color damageColor;

    [Header("Screen Flash")]
    [SerializeField] Image damageFlash;
    [SerializeField] float flashDuration = .2f;
    [SerializeField] float flashFadeSpeed = 3f;

    private void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        spriteRenderer = playerMovement.spriteRenderer;
    }

    private void Update()
    {
        if (healthAmount <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            canTakeDamage = true;
            if (!isInDamageCooldown && !(enemy.currentState is EnemyDeathState))
            {
                StartCoroutine(DamageCooldown());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            canTakeDamage = false;
        }
    }

    IEnumerator FlashScreen()
    {
        float alpha = .05f;
        damageFlash.color = new Color(0.4716981f, 0.2247241f, 0.2247241f, alpha);
        yield return new WaitForSeconds(flashDuration);

        while (alpha > 0f) 
        { 
            alpha -= Time.deltaTime * flashFadeSpeed;
            damageFlash.color = new Color(0.4716981f, 0.2247241f, 0.2247241f, alpha);
            yield return null;
        }
    }

    IEnumerator DamageCooldown()
    {
        while (canTakeDamage)
        {
            TakeDamage(enemy.levelDamage);
            isInDamageCooldown = true;

            spriteRenderer.color = damageColor;
            yield return StartCoroutine(FlashScreen());
            yield return new WaitForSeconds(effectCooldown);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(damageCooldown);
            isInDamageCooldown = false;
        }
    }
}
