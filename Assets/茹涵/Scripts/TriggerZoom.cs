using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoom : MonoBehaviour
{
    public float targetSize = 5f;   // 目标正交大小（拉近设小，拉远设大）
    public float zoomSpeed = 2f;    // 变焦速度

    private Camera mainCamera;
    private float originalSize;

    void Start()
    {
        mainCamera = Camera.main;
        originalSize = mainCamera.orthographicSize;
    }

    // 当有其他碰撞器进入触发器时调用
    void OnTriggerEnter2D(Collider2D other)
    {
        // 可以加个判断：如果进入的是玩家，则开始变焦
        if (other.CompareTag("Player"))
        {
            
            // 这里演示了直接设置大小，如果你需要平滑过渡，可以用协程或Update里做插值
            // 简单方法：直接设置
            // mainCamera.orthographicSize = targetSize;

            // 高级方法：使用协程进行平滑过渡
            StartCoroutine(SmoothZoom(targetSize));
        }
    }

    // 当有其他碰撞器离开触发器时调用（可选，用于恢复视角）
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SmoothZoom(originalSize));
        }
    }

    // 一个简单的平滑变焦协程
    System.Collections.IEnumerator SmoothZoom(float target)
    {
        float startSize = mainCamera.orthographicSize;
        float elapsedTime = 0f;
        float duration = 1f / zoomSpeed; // 持续时间

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            // 使用Mathf.Lerp进行插值
            mainCamera.orthographicSize = Mathf.Lerp(startSize, target, t);
            yield return null; // 等待下一帧
        }
        mainCamera.orthographicSize = target; // 确保最终值准确
    }
}