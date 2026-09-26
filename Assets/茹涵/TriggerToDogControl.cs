using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerToDogControl : MonoBehaviour
{
    [Header("玩家进入触发器 执行")]
    public UnityEvent OnTriggerEnter;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnTriggerEnter?.Invoke();
            // 取消触发器
            gameObject.SetActive(false);
        }
    }
}
