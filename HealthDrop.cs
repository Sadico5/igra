using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public int healAmount = 20;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.AddHealth(healAmount);
                Debug.Log("Подобран дроп! Здоровье увеличено на " + healAmount);
            }
            Destroy(gameObject);
        }
    }
}