using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RHItem : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;
    private bool isDragging = false;
    private GameObject selectedObject;

    public float rate;
    public float nextTime;

    public TextMeshProUGUI timeUi;
    public int t;

    void Update()
    {
        if (Time.time > nextTime)
        {
            nextTime = Time.time + rate;
            if (t>0) {
                t--;

                timeUi.text = t.ToString();
            }
            

        }

        if (Input.GetMouseButtonDown(0))
        {
            // 鼠标按下时检测点击的物体
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero
            );

            if (hit.collider != null)
            {
                selectedObject = hit.collider.gameObject;
                zCoord = Camera.main.WorldToScreenPoint(selectedObject.transform.position).z;
                offset = selectedObject.transform.position - GetMouseWorldPos();
                isDragging = true;
            }
        }

        if (Input.GetMouseButton(0) && isDragging && selectedObject != null)
        {
            // 拖拽物体
            selectedObject.transform.position = GetMouseWorldPos() + offset;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            selectedObject = null;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}