using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    public Image healthBarFill;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            UpdateHealthUI();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    public GameObject healthDropPrefab;

    void Die()
    {
        if (healthDropPrefab != null && Random.value <= 1f)
        {
            Instantiate(healthDropPrefab, transform.position, Quaternion.identity);
        }
        if(GameManager.Instance != null)
        {
            GameManager.Instance.OnEnemyKilled();
        }
        
        Destroy(gameObject);
    }
}