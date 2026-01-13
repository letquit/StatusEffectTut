using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理伤害投射器组件，用于检测碰撞并应用伤害到目标对象
/// </summary>
public class DamageCaster : MonoBehaviour
{
    private Collider _damageCasterCollider;
    public string targetTag;
    public List<Collider> _damagedTargetList;

    [SerializeField] private DamageData damageData;
    // [SerializeField] private GameObject impactVfx;
    [SerializeField] private Vector3 impactSpawnOffset;
    [SerializeField] private bool enableByDefault;
    [SerializeField] public List<GameObject> toAvoidList;
    
    public bool IsDamaged { get; private set; } = false;


    /// <summary>
    /// 初始化伤害投射器组件，在Awake阶段设置初始状态
    /// </summary>
    private void Awake()
    {
        _damageCasterCollider = GetComponent<Collider>();
        _damagedTargetList = new List<Collider>();

        if (!enableByDefault)
        {
            DisableDamageCaster();
        }
    }
    
    /// <summary>
    /// 当触发器进入碰撞时处理伤害逻辑
    /// </summary>
    /// <param name="other">发生碰撞的其他碰撞体</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag && !toAvoidList.Contains(other.gameObject) && !_damagedTargetList.Contains(other))
        {
            Health targetHealth = other.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.ApplyDamage(damageData, transform.position);
                
                // // 立即销毁子弹并播放特效
                // if (impactVfx != null)
                // {
                //     Instantiate(impactVfx, transform.position + impactSpawnOffset, Quaternion.identity);
                // }
                Destroy(gameObject); // 确保这里执行销毁
            }
            _damagedTargetList.Add(other);
        }
    }


    /// <summary>
    /// 启用伤害投射器功能，清空已伤害目标列表并启用碰撞体
    /// </summary>
    public void EnableDamageCaster()
    {
        _damagedTargetList.Clear();
        _damageCasterCollider.enabled = true;
    }

    /// <summary>
    /// 禁用伤害投射器功能，清空已伤害目标列表并禁用碰撞体
    /// </summary>
    public void DisableDamageCaster()
    {
        _damagedTargetList.Clear();
        _damageCasterCollider.enabled = false;
    }
}
