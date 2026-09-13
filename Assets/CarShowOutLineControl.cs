using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public class CarShowOutLineControl : MonoBehaviour
{
    [Header("动画过渡时长 s")]
    public float duration = 0.3f;
    [Header("玩家停留/离开多久 触发边框变化 s")]
    public float stayCheckTime = 0.5f;
    
    private bool realStay = false;
    
    // 材质
    private SpriteRenderer _spriteRenderer;
    private Material _material;
    private Tween _tween;
    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
        _material.SetFloat("_OutlineAlpha", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            realStay =  true;
            
            Sequence.Create()
                .ChainDelay(stayCheckTime)
                .ChainCallback(target:this, target =>
                {
                    if(!realStay) return;
                    if(target._tween.isAlive) target._tween.Stop();
                    target._tween = Tween.Custom(0, 1, target.duration, onValueChange: x => {target._material.SetFloat("_OutlineAlpha", x);});
                });
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            realStay = false;

            Sequence.Create()
                .ChainDelay(stayCheckTime)
                .ChainCallback(target:this, target =>
                {
                    if(realStay) return;
                    if(target._tween.isAlive) target._tween.Stop();
                    target._tween = Tween.Custom(1, 0, target.duration, onValueChange: x => {target._material.SetFloat("_OutlineAlpha", x);});
                });
        }
    }
}
