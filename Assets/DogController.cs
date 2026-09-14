using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class DogController : MonoBehaviour
{
    [Header("移动与跳跃设置")]
    [Tooltip("向右移动的速度")]
    public float moveSpeed = 5f;
    [Tooltip("跳跃的力度（决定跳跃高度）")]
    public float jumpForce = 8f;

    [Header("射线检测设置")]
    [Tooltip("射线的长度（检测距离）")]
    public float rayDistance = 3f;
    [Tooltip("三条射线的上下间距")]
    public float raySpacing = 0.5f;
    [Tooltip("需要触发跳跃的碰撞体Tag")]
    public string targetTag = "Obstacle"; 
    [Tooltip("射线发射点的偏移量（根据狗的中心点微调射线位置）")]
    public Vector2 rayOffset = Vector2.zero;

    private Rigidbody2D rb;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 初始状态确保不受其他力影响而移动
        rb.velocity = Vector2.zero;
    }

    void Update()
    {
        // 如果没有被激活移动，则不进行射线检测
        if (!isMoving) return;

        CheckObstacleAndJump();
    }

    void FixedUpdate()
    {
        // 物理移动逻辑放在 FixedUpdate 中更平滑
        if (isMoving)
        {
            // 保持向右移动，Y轴保留原有速度（以保持重力下落和跳跃的物理效果）
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }

    /// <summary>
    /// 公开方法：外部调用此方法后，狗才会开始向右移动
    /// </summary>
    public void StartMoving()
    {
        isMoving = true;
    }

    /// <summary>
    /// 停止移动的公开方法（按需使用）
    /// </summary>
    public void StopMoving()
    {
        isMoving = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void CheckObstacleAndJump()
    {
        // 简单的接地检测：防止狗在空中连续起跳（如果Y轴速度不接近0，说明在空中或在下落）
        // 如果你的游戏允许空中连跳，可以把这一行注释掉
        if (Mathf.Abs(rb.velocity.y) > 0.05f) return;

        // 计算射线的三个发射点：中间、偏上、偏下
        Vector2 centerOrigin = (Vector2)transform.position + rayOffset;
        Vector2 topOrigin = centerOrigin + Vector2.up * raySpacing;
        Vector2 bottomOrigin = centerOrigin + Vector2.down * raySpacing;

        // 检测三条射线中是否有一条碰到了目标
        bool shouldJump = CastRayAndCheck(centerOrigin) || 
                          CastRayAndCheck(topOrigin) || 
                          CastRayAndCheck(bottomOrigin);

        if (shouldJump)
        {
            Jump();
        }
    }

    private bool CastRayAndCheck(Vector2 origin)
    {
        // 在 Unity 的 Scene 窗口画出红色的射线，方便你直观地调整射线长度和位置
        Debug.DrawRay(origin, Vector2.right * rayDistance, Color.red);

        // 向右发射 2D 射线
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, rayDistance);
        // 如果射线碰到了碰撞体，并且该碰撞体的 Tag 与设置的匹配
        if (hit.collider && hit.collider.CompareTag(targetTag))
        {
            return true;
        }

        return false;
    }

    private void Jump()
    {
        // 给予刚体一个向上的速度，实现跳跃
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 重新加载当前活动的场景
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}