using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;        // 要跟随的目标（玩家）
    public float smoothSpeed = 0.125f; // 跟随平滑速度
    public Vector3 offset;          // 相机与目标的偏移量

    // 限制范围（在Inspector中设置）
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    void LateUpdate()
    {
        // 计算目标位置（目标位置 + 偏移量）
        Vector3 desiredPosition = target.position + offset;

        // 对X和Y进行限制（Z保持偏移量中的值不变）
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        // 使用Lerp函数进行平滑插值移动
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 更新相机位置
        transform.position = smoothedPosition;
    }
}