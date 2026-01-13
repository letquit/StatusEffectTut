using System;
using UnityEngine;

/// <summary>
/// 投射物类，用于处理追踪目标的投射物行为
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// 目标标签，默认为"Enemy"
    /// </summary>
    public string targetTag = "Enemy";
    
    /// <summary>
    /// 搜索范围，用于查找最近的目标
    /// </summary>
    public float searchRange = 10f;
    
    /// <summary>
    /// 当前追踪的目标
    /// </summary>
    public Transform target;
    
    /// <summary>
    /// 投射物移动速度
    /// </summary>
    public float speed = 5f;
    
    /// <summary>
    /// 旋转速度
    /// </summary>
    public float rotationSpeed = 12f;
    // private ParticleSystem ps;
    
    /// <summary>
    /// 伤害施加器组件
    /// </summary>
    private DamageCaster damageCaster;

    /// <summary>
    /// 自动销毁计时器
    /// </summary>
    private float autoDestructTimer = 1f;
    
    /// <summary>
    /// 自动销毁冷却时间
    /// </summary>
    private float autoDestructCD;

    /// <summary>
    /// 初始化方法，在对象唤醒时执行
    /// 获取DamageCaster组件并初始化粒子系统（已注释）
    /// </summary>
    private void Awake()
    {
        damageCaster = GetComponent<DamageCaster>();
        // ps = GetComponentInChildren<ParticleSystem>();
    }

    /// <summary>
    /// 开始方法，在对象启动时执行
    /// 如果没有设置目标，则查找最近的目标
    /// </summary>
    private void Start()
    {
        if (target == null)
            FindClosestTarget();
    }
    
    /// <summary>
    /// 更新方法，每帧执行
    /// 处理投射物的移动、目标追踪和自动销毁逻辑
    /// </summary>
    private void Update()
    {
        if (target == null)
        {
            autoDestructCD += Time.deltaTime;
    
            if (autoDestructCD > autoDestructTimer)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Calculate the direction to the target
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            
            // Move the projectile towards the target
            transform.Translate(direction * speed * Time.deltaTime);
            
            Quaternion targetRotation = Quaternion.LookRotation(-direction, Vector3.up);
    
            // ps.transform.rotation = targetRotation;
        }
    }
    
    /// <summary>
    /// 查找最近的目标
    /// 遍历指定标签的所有游戏对象，找到距离最近且在搜索范围内的目标
    /// </summary>
    private void FindClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance < searchRange)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy.transform;
            damageCaster.EnableDamageCaster();
        }
    }
}
