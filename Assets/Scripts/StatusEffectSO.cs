using UnityEngine;

public enum StatusEffectType { Fire, Ice }
public abstract class StatusEffectSO : ScriptableObject
{
    public StatusEffectType statusEffectType;

    public float activationThreshold;
    public float thresholdReductionMultiplier = 1f;
    public float thresholdReductionEverySecond = 1f;

    public float activeDuration;

    public GameObject visualEffectPrefab;

    private float currentThreshold;
    private float remainingDuration;
    private GameObject vfxPlaying;
    
    [HideInInspector] public bool isBuildUpOnlyShow;
    [HideInInspector] public bool isEffectActive;

    public float tickInterval = .5f; // Every 1/2 sec updateEffect will run IF .1s is the UpdateCall
    private float tickIntervalCD;

    protected Health health;

    public virtual void AddBuildup(float buildUpAmount, GameObject target)
    {
        isBuildUpOnlyShow = true;
        currentThreshold += buildUpAmount;

        if (currentThreshold >= activationThreshold)
        {
            ApplyEffect(target);
        }
    }

    public virtual void ApplyEffect(GameObject target)
    {
        isEffectActive = true;
        remainingDuration = activeDuration;

        SetTargetData(target);
        if (visualEffectPrefab != null)
        {
            vfxPlaying = Instantiate(visualEffectPrefab, target.transform.position, Quaternion.identity, target.transform);
        }
    }

    private void SetTargetData(GameObject target)
    {
        health = target.GetComponent<Health>();
    }

    public void UpdateCall(GameObject target, float tickAmount)
    {
        if (isEffectActive)
        {
            isBuildUpOnlyShow = false;
            remainingDuration -= tickAmount;
            if (remainingDuration <= 0)
            {
                isEffectActive = false;
            }
        }
        else
        {
            currentThreshold -= (tickAmount * thresholdReductionEverySecond) * thresholdReductionMultiplier; // Ex 1s * 2 threshold every sec ---> In 1 sec threshold will reduce by 2
            if (currentThreshold <= 0)
            {
                isBuildUpOnlyShow = false;
            }
        }
        
        // Trigger statusEffect update every nth interval
        
        tickIntervalCD += tickAmount;
        if (tickIntervalCD >= tickInterval)
        {
            UpdateEffect(target);
            tickIntervalCD = 0;
        }
    }

    // This method can be hooked into derived class to run every
    public virtual void UpdateEffect(GameObject target) { }

    public virtual void RemoveEffect(GameObject target)
    {
        isEffectActive = false;
        currentThreshold = 0;
        remainingDuration = 0;

        if (vfxPlaying != null)
        {
            Destroy(vfxPlaying);
        }

        // other actions to perform when SE is deactivated
    }
    
    public virtual bool CanStatusVisualBeRemoved()
    {
        return !(isEffectActive || isBuildUpOnlyShow);
    }

    public float GetCurrentThresholdNormalized()
    {
        return currentThreshold / activationThreshold;
    }

    public float GetCurrentDurationNormalized()
    {
        return remainingDuration / activeDuration;
    }
}
