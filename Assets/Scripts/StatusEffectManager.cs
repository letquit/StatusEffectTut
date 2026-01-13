using System.Collections.Generic;
using System.Linq;
using GDX.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusEffectManager : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<StatusEffectType, StatusEffectSO> statusEffectToApplyDict = new();
    private SerializableDictionary<StatusEffectType, StatusEffectSO> enabledEffects = new();
    private Dictionary<StatusEffectType, StatusEffectSO> statusEffectCacheDict = new Dictionary<StatusEffectType, StatusEffectSO>();

    // vars for running every n sec
    [SerializeField, Tooltip("Run the updateCall in StatusEffectSO every what interval?")] private float interval = .1f;
    private float currentInterval = 0f;
    private float lastInterval = 0f;
    
    // Events
    public UnityAction<StatusEffectSO, float> ActivateStatus;
    public UnityAction<StatusEffectSO> DeactivateStatusEffect;
    public UnityAction<StatusEffectSO, float, float> UpdateStatusEffect;
    
    void Start()
    {
        
    }
    
    public void OnStatusTriggerBuildup(StatusEffectType effectType, float buildAmount)
    {
        // if effect has not started buildup and is neither enabled
        if (!enabledEffects.ContainsKey(effectType))
        {
            var effectToAdd = CreateEffectObject(effectType, statusEffectToApplyDict[effectType]);
            enabledEffects[effectType] = effectToAdd;

            // communicate to status UI to display the effect...
            ActivateStatus?.Invoke(effectToAdd, effectToAdd.GetCurrentDurationNormalized());
        }

        // status effect is not active but has some buildup already
        if (!enabledEffects[effectType].isEffectActive)
        {
            enabledEffects[effectType].AddBuildup(buildAmount, gameObject);
            // communicate to status ui to update effect
            UpdateStatusEffect?.Invoke(enabledEffects[effectType],
                enabledEffects[effectType].GetCurrentThresholdNormalized(),
                enabledEffects[effectType].GetCurrentDurationNormalized());
        }
        // status effect is already active
        else
        {
            // Do extra damage when status is active
            int tickDamageAmount = (int)Mathf.Ceil(buildAmount / 4);
            // call health damage function
        }
    }

    private StatusEffectSO CreateEffectObject(StatusEffectType statusEffectType, StatusEffectSO effectSO)
    {
        if (!statusEffectCacheDict.ContainsKey(statusEffectType))
        {
            statusEffectCacheDict[statusEffectType] = Instantiate(effectSO);
        }

        return statusEffectCacheDict[statusEffectType];
    }

    public void UpdateEffects(GameObject target)
    {
        foreach (var effect in enabledEffects.ToList())
        {
            effect.Value.UpdateCall(target, interval);
            
            // update status effect ui call
            UpdateStatusEffect?.Invoke(effect.Value,
                effect.Value.GetCurrentThresholdNormalized(),
                effect.Value.GetCurrentDurationNormalized());

            if (effect.Value.CanStatusVisualBeRemoved())
            {
                RemoveEffect(effect.Key);
            }
        }
    }

    public void RemoveEffect(StatusEffectType effectType)
    {
        if (enabledEffects.ContainsKey(effectType))
        {
            enabledEffects[effectType].RemoveEffect(gameObject);
            
            // remove status effect ui
            DeactivateStatusEffect?.Invoke(enabledEffects[effectType]);
            
            enabledEffects.Remove(effectType);
        }
    }
    
    private void Update()
    {
        // run UpdateEffects function every interval seconds
        // If want it based on number of turns - update currentInterval based on number of turns
        currentInterval += Time.deltaTime;
        if (currentInterval > lastInterval + interval)
        {
            UpdateEffects(gameObject);
            lastInterval = currentInterval;
        }
    }
}
