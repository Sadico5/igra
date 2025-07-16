using UnityEngine;

public class AdrenalineSystem : MonoBehaviour
{
    public float adrenalineLevel = 0f;      // Уровень адреналина
    public float maxAdrenaline = 100f;      // Максимально возможный уровень адреналина
    public float recoveryRate = 1f;         // Скорость снижения адреналина во время покоя
    public float selfHarmCost = 10f;        // Стоимость потери ХП ради увеличения адреналина

    public float lowAdrenalineThreshold = 20f;     // Граница низкого уровня адреналина
    public float highAdrenalineThreshold = 70f;    // Граница высокого уровня адреналина
    public float overdoseThreshold = 100f;         // Граница чрезмерно высокого уровня адреналина

    public float movementSpeedBoostFactor = 1.5f;  // Коэффициент ускорения при высоком уровне адреналина
    public float damageBoostFactor = 1.5f;         // Коэффициент усиления урона при высоком уровне адреналина
    public float healthLossPerHit = 5f;            // Сколько HP теряется при атаке с высоким уровнем адреналина

    private PlayerHealth PlayerHealth;             // Скрипт системы здоровья персонажа

    void Start()
    {
        PlayerHealth = GetComponent<PlayerHealth>(); // Связываем с системой здоровья
    }

    void Update()
    {
        if (adrenalineLevel > 0 && !IsInAction())
            RecoverAdrenaline();

        ApplyAdrenalineEffects();
    }

    public void GainAdrenaline(float amount)
    {
        adrenalineLevel += amount;
        adrenalineLevel = Mathf.Min(adrenalineLevel, maxAdrenaline);
    }

    private void RecoverAdrenaline()
    {
        adrenalineLevel -= recoveryRate * Time.deltaTime;
        adrenalineLevel = Mathf.Max(adrenalineLevel, 0f);
    }

    private bool IsInAction()
    {
        return Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0); // Проверка действий: атака или прыжок
    }

    private void ApplyAdrenalineEffects()
    {
        if (adrenalineLevel < lowAdrenalineThreshold)
        {
            MovementSpeedPenalty();
            DamageReduction();
        }
        else if (adrenalineLevel >= highAdrenalineThreshold)
        {
            BoostMovementAndDamage();
            if (adrenalineLevel >= overdoseThreshold)
                EnableOverdoseEffect();
        }
    }

    private void MovementSpeedPenalty()
    {
        GetComponent<Rigidbody2D>().gravityScale *= 0.8f; // Замедляем движение
    }

    private void DamageReduction()
    {
        GetComponent<MeleeAttack>().damage *= 0.8f; // Ослабляем атаку
    }

    private void BoostMovementAndDamage()
    {
        GetComponent<Rigidbody2D>().gravityScale /= movementSpeedBoostFactor; // Ускоряем движение
        GetComponent<MeleeAttack>().damage *= damageBoostFactor;              // Усиливаем атаку
    }

    private void EnableOverdoseEffect()
    {
        PlayerHealth.TakeDamage(healthLossPerHit);                            // Понемногу уменьшаем здоровье
    }

    public void SelfHarmForAdrenaline()
    {
        PlayerHealth.TakeDamage(selfHarmCost);
        GainAdrenaline(selfHarmCost * 2);                                    // Немного повышаем адреналин
    }
}