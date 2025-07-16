using UnityEngine;

public class PlayerAmmo : MonoBehaviour
{
    [SerializeField] private float adrenalineLevel = 0f;
    [SerializeField] private float maxAdrenaline = 100f;
    [SerializeField] private float recoveryRate = 1f;
    [SerializeField] private float selfHarmCost = 10f;

    [SerializeField] private float lowAdrenalineThreshold = 20f;
    [SerializeField] private float highAdrenalineThreshold = 70f;
    [SerializeField] private float overdoseThreshold = 100f;

    [SerializeField] private float movementSpeedBoostFactor = 1.5f;
    [SerializeField] private float damageBoostFactor = 1.5f;
    [SerializeField] private float healthLossPerHit = 5f;

    private PlayerHealth playerHealth;

    public float AdrenalineLevel => adrenalineLevel;
    public float MaxAdrenaline => maxAdrenaline;
    public float RecoveryRate => recoveryRate;
    public float LowAdrenalineThreshold => lowAdrenalineThreshold;
    public float HighAdrenalineThreshold => highAdrenalineThreshold;
    public float OverdoseThreshold => overdoseThreshold;
    public float MovementSpeedBoostFactor => movementSpeedBoostFactor;
    public float DamageBoostFactor => damageBoostFactor;
    public float HealthLossPerHit => healthLossPerHit;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
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
        adrenalineLevel = Mathf.Clamp(adrenalineLevel, 0f, maxAdrenaline);
    }

    public bool ConsumeAdrenaline(float amount)
    {
        if (adrenalineLevel >= amount)
        {
            adrenalineLevel -= amount;
            adrenalineLevel = Mathf.Clamp(adrenalineLevel, 0f, maxAdrenaline);
            return true;
        }
        return false;
    }

    private void RecoverAdrenaline()
    {
        adrenalineLevel -= recoveryRate * Time.deltaTime;
        adrenalineLevel = Mathf.Max(adrenalineLevel, 0f);
    }

    private bool IsInAction()
    {
        return Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
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
        // Example penalty logic (customize as needed)
        // E.g., could set a flag for PlayerController to reduce speed
    }

    private void DamageReduction()
    {
        // Example penalty logic (customize as needed)
    }

    private void BoostMovementAndDamage()
    {
        // Example boost logic (customize as needed)
    }

    private void EnableOverdoseEffect()
    {
        if (playerHealth != null)
            playerHealth.TakeDamage((int)healthLossPerHit);
    }

    public void SelfHarmForAdrenaline()
    {
        if (playerHealth != null)
            playerHealth.TakeDamage((int)selfHarmCost);
        GainAdrenaline(selfHarmCost * 2);
    }
}