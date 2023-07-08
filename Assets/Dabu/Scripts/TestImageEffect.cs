using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//编辑模式可见
[ExecuteInEditMode]
public class TestImageEffect : MonoBehaviour
{
    //该脚本必须挂载在有相机组件的游戏对象上，在该相机渲染完成后调用OnRenderImage()函数；
    //无论是否play，OnRenderImage都会执行
    public Material EffectMaterial;
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, EffectMaterial);
    }
}