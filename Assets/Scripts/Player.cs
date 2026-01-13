using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制类，负责处理玩家的移动和攻击逻辑
/// </summary>
public class Player : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController controller;

    [SerializeField] private List<GameObject> projectileToShootList;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // Calculate the movement direction based on input
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementDirection);
            controller.Move(movementDirection * speed * Time.deltaTime);
        }
        
        // 检查攻击输入并执行相应攻击
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Attack(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Attack(1);
        }
    }

    /// <summary>
    /// 执行攻击动作，根据按键索引发射对应的弹射物
    /// </summary>
    /// <param name="key">弹射物列表中的索引</param>
    private void Attack(int key)
    {
        // spawn projectile
        Instantiate(projectileToShootList[key], transform.position, Quaternion.identity);
    }
}
