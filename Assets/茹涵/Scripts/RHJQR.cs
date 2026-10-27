using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHJQR : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 2f;

    // 停止位置
    public Vector2 stopPosition = new Vector2(10f, 0f);
    public float stopDistance = 0.1f; // 到达停止位置的判定距离

    private Vector3 startPosition;
    private float timeOffset;
    private bool isMoving = true; // 是否还在移动晃动

    void Start()
    {
        startPosition = transform.position;
        timeOffset = Random.Range(0f, Mathf.PI * 2);
    }

    void Update()
    {
        if (!isMoving) return; // 已停止则不再执行

        // 检查是否到达目标位置
        float distance = Vector2.Distance(transform.position, stopPosition);
        if (distance <= stopDistance)
        {
            // 到达目标位置，精确对齐
            transform.position = new Vector3(stopPosition.x, stopPosition.y, transform.position.z);
            isMoving = false;
            return;
        }

        // 向前移动
        float moveX = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(moveX, 0, 0);

        // 上下晃动
        float floatY = Mathf.Sin((Time.time + timeOffset) * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(
            transform.position.x,
            startPosition.y + floatY,
            transform.position.z
        );
    }

    // 外部调用停止（如触发事件）
    public void StopMovement()
    {
        isMoving = false;
    }

    // 外部调用重新启动
    public void StartMovement()
    {
        isMoving = true;
    }
}