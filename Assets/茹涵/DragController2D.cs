using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DragController2D : MonoBehaviour
{
    public enum DragAxis
    {
        Free,       
        Horizontal, 
        Vertical    
    }

    [Header("拖拽设置")]
    public DragAxis dragAxis = DragAxis.Horizontal;
    
    [Tooltip("拖拽时追踪鼠标的灵敏度/速度")]
    public float dragSpeed = 20f; 
    [Header("材质纹理")]
    public Texture2D[] textures;
    
    private Rigidbody2D rb;
    private Camera mainCamera;
    // 材质
    private SpriteRenderer _spriteRenderer;
    private Material _material;
    
    private bool isDragging = false;
    private Vector2 clickOffset;    
    private Vector2 targetPosition; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(_spriteRenderer) _material = _spriteRenderer.material;
        if(_material) _material.SetFloat("_OutlineAlpha", 0);
        mainCamera = Camera.main;
        if (rb) AutoSetConstraints();
    }
    
    // 自动设置物理约束
    private void AutoSetConstraints()
    {
        // Freeze Rotation Z
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 根据枚举选项追加（|=）位置锁定
        switch (dragAxis)
        {
            case DragAxis.Free:
                // 自由模式：不增加任何位置限制
                if(_material && textures.Length > 0) _material.SetTexture("_OutlineTex", textures[0]);
                if(_material) _material.SetFloat("_OutlineTexXSpeed", 10);
                if(_material) _material.SetFloat("_OutlineTexYSpeed", 10);
                break;
                
            case DragAxis.Horizontal:
                // 水平模式：冻结 Y 轴，确保物理引擎层面绝对不会上下移动
                rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
                if(_material && textures.Length > 1) _material.SetTexture("_OutlineTex", textures[1]);
                if(_material) _material.SetFloat("_OutlineTexXSpeed", 10);
                if(_material) _material.SetFloat("_OutlineTexYSpeed", 0);
                break;
                
            case DragAxis.Vertical:
                // 竖直模式：冻结 X 轴，确保物理引擎层面绝对不会左右移动
                rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
                if(_material && textures.Length > 2) _material.SetTexture("_OutlineTex", textures[2]);
                if(_material) _material.SetFloat("_OutlineTexXSpeed", 0);
                if(_material) _material.SetFloat("_OutlineTexYSpeed", 10);
                break;
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        clickOffset = rb.position - GetMouseWorldPosition();
        if(_material) _material.SetFloat("_OutlineAlpha", 1);
    }

    void OnMouseUp()
    {
        isDragging = false;
        // 松开鼠标时，清空当前方向的速度，防止物体因为惯性继续滑动
        rb.velocity = Vector2.zero; 
        if(_material) _material.SetFloat("_OutlineAlpha", 0);
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            targetPosition = GetMouseWorldPosition() + clickOffset;
        }
    }

    void FixedUpdate()
    {
        if (isDragging)
        {
            // 计算物体当前位置到鼠标目标位置的向量
            Vector2 moveDirection = targetPosition - rb.position;

            switch (dragAxis)
            {
                case DragAxis.Free:
                    // 自由移动：赋予指向鼠标的速度
                    rb.velocity = moveDirection * dragSpeed;
                    break;
                    
                case DragAxis.Horizontal:
                    // 仅限水平：计算X轴的追踪速度，保留Y轴原有的速度（由物理引擎接管）
                    rb.velocity = new Vector2(moveDirection.x * dragSpeed, rb.velocity.y);
                    break;
                    
                case DragAxis.Vertical:
                    // 仅限竖直：计算Y轴的追踪速度，保留X轴原有的速度
                    rb.velocity = new Vector2(rb.velocity.x, moveDirection.y * dragSpeed);
                    break;
            }
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        return new Vector2(mouseWorldPos.x, mouseWorldPos.y);
    }

    public void ChangeDragAxis(DragAxis dragAxis)
    {
        this.dragAxis = dragAxis;
        
        if (rb) AutoSetConstraints();
    }
}