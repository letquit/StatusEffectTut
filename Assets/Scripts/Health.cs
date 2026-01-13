using System;
using UnityEngine;

[Serializable]
public class DamageData
{
    public int damage;
    public StatusEffectType effectType;
    public float buildAmount;
    
    public DamageData(int damage, StatusEffectType effectType, float buildAmount)
    {
        this.damage = damage;
        this.effectType = effectType;
        this.buildAmount = buildAmount;
    }
}

public class Health : MonoBehaviour
{
    [SerializeField] private float MaxHealth;
    [SerializeField] private float CurrentHealth;
    // [SerializeField] private ParticleSystem dethVfx;
    // [SerializeField] private ParticleSystem hitVfx;
    
    // Health update events
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDied;
    
    private StatusEffectManager _statusEffectManager;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void ApplyDamage(DamageData damage, Vector3 attackerPos)
    {
        CurrentHealth -= damage.damage;
        
        _statusEffectManager.OnStatusTriggerBuildup(damage.effectType, damage.buildAmount);
        
        OnDamaged?.Invoke(this, EventArgs.Empty);
        CheckHealth();
    }
    
    private void CheckHealth()
    {
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
            
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Heal(int healAmount)
    {
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void SetInitialHealth(int maxHealth)
    {
        this.MaxHealth = maxHealth;
        this.CurrentHealth = maxHealth;
    }
    
    public float GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public float GetHealthAmount()
    {
        return CurrentHealth;
    }

    public float GetHealthAmountMax()
    {
        return MaxHealth;
    }

    public float GetHealthAmountNormalized()
    {
        return (float)CurrentHealth / MaxHealth;
    }

    public bool IsDead()
    {
        return CurrentHealth == 0;
    }

    public bool IsFullHealth()
    {
        return CurrentHealth == MaxHealth;
    }
}
