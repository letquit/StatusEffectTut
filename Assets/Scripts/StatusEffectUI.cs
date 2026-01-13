using System;
using System.Collections.Generic;
using GDX.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectIconCache
{
    public GameObject statusIconContainer;
    public Image statusBuildupFill;
    public Image statusActiveTimerFill;
    public Image statusIcon;

    public StatusEffectIconCache(GameObject statusIconContainer, Image statusBuildupFill, Image statusActiveTimerFill,
        Image statusIcon)
    {
        this.statusIconContainer = statusIconContainer;
        this.statusBuildupFill = statusBuildupFill;
        this.statusActiveTimerFill = statusActiveTimerFill;
        this.statusIcon = statusIcon;
    }
}

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private GameObject statusEffectIconTemplate;
    [SerializeField] private SerializableDictionary<StatusEffectType, Sprite> statusEffectSpriteDict;

    private Dictionary<StatusEffectSO, StatusEffectIconCache> statusEffectToIconDict;
    
    private Camera mainCamera;
    private StatusEffectManager statusEffectmanagerRef;

    private void Start()
    {
        mainCamera = Camera.main;
        statusEffectmanagerRef = GetComponentInParent<StatusEffectManager>();
        statusEffectToIconDict = new Dictionary<StatusEffectSO, StatusEffectIconCache>();
        
        statusEffectmanagerRef.ActivateStatus += OnActivateStatus;
        statusEffectmanagerRef.UpdateStatusEffect += OnUpdateStatusEffect;
        statusEffectmanagerRef.DeactivateStatusEffect += OnDeactiveStatusEffect;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.parent.position - mainCamera.transform.forward);
    }
    
    private StatusEffectIconCache CreateStatusIcon(StatusEffectSO statusEffect)
    {
        if (statusEffectToIconDict.ContainsKey(statusEffect))
        {
            statusEffectToIconDict[statusEffect].statusIconContainer.SetActive(true);
            return statusEffectToIconDict[statusEffect];
        }
        GameObject createdStatusIcon = Instantiate(statusEffectIconTemplate, transform);
        GameObject statusActiveTimer = createdStatusIcon.transform.Find("StatusActiveTimer").gameObject;
        Image statusBuildupRadialFill = createdStatusIcon.GetComponent<Image>();
        statusBuildupRadialFill.fillAmount = 0;

        Image statusActiveTimerRadialFill = statusActiveTimer.GetComponent<Image>();
        statusActiveTimerRadialFill.fillAmount = 0;

        Image statusIcon = createdStatusIcon.transform.Find("Icon").GetComponent<Image>();
        statusIcon.sprite = statusEffectSpriteDict[statusEffect.statusEffectType];
        
        createdStatusIcon.SetActive(true);
        return new StatusEffectIconCache(createdStatusIcon, statusBuildupRadialFill, statusActiveTimerRadialFill,
            statusIcon);
    }


    private void OnActivateStatus(StatusEffectSO statusEffect, float buildAmount)
    {
        StatusEffectIconCache statusEffectIconCache = CreateStatusIcon(statusEffect);

        statusEffectToIconDict[statusEffect] = statusEffectIconCache;
        
        OnUpdateStatusEffect(statusEffect, buildAmount, 0);
    }

    private void OnUpdateStatusEffect(StatusEffectSO statusEffect, float buildAmount, float duration)
    {
        statusEffectToIconDict[statusEffect].statusBuildupFill.fillAmount = buildAmount;
        statusEffectToIconDict[statusEffect].statusActiveTimerFill.fillAmount = duration;
    }

    private void OnDeactiveStatusEffect(StatusEffectSO statusEffect)
    {
        statusEffectToIconDict[statusEffect].statusIconContainer.SetActive(false);
    }
}
