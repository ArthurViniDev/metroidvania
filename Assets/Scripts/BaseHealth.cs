using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    [SerializeField] public Image healthBar;
    public float healthAmount;
    public float startLife;

    void Start()
    {
        startLife = healthAmount;
    }

    public virtual void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / startLife;
    }
}
