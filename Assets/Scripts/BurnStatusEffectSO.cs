using UnityEngine;

[CreateAssetMenu(menuName = "Custom/StatusEffects/BurnStatusEffects", fileName = "BurnStatusEffect")]
public class BurnStatusEffectSO : StatusEffectSO
{
    public int tickDamage;

    public override void UpdateEffect(GameObject target)
    {
        base.UpdateEffect(target);

        if (isEffectActive)
        {
            health.ApplyDamage(new DamageData(tickDamage, statusEffectType, 0), Vector3.zero);
        }
    }
}

