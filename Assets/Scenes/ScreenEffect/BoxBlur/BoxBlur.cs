﻿// ---------------------------【均值模糊】---------------------------
using UnityEngine;

[ExecuteInEditMode]
public class BoxBlur : PostEffectsBase
{
    static readonly string shaderName = "lcl/screenEffect/BoxBlur";

    //模糊半径  
    [Header("模糊半径")]
    [Range(0.2f, 10.0f)]
    public float BlurRadius = 1.0f;
    //降采样次数
    [Header("降采样次数")]
    [Range(1, 8)]
    public int downSample = 2;
    //迭代次数  
    [Header("迭代次数")]
    [Range(0, 4)]
    public int iteration = 1;

    void OnEnable()
    {
        shader = Shader.Find(shaderName);
    }
    //-------------------------------------【OnRenderImage函数】------------------------------------    
    // 说明：此函数在当完成所有渲染图片后被调用，用来渲染图片后期效果
    //--------------------------------------------------------------------------------------------------------  
    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (material)
        {
            //申请RenderTexture，RT的分辨率按照downSample降低
            RenderTexture rt = RenderTexture.GetTemporary(sourceTexture.width >> downSample, sourceTexture.height >> downSample, 0, sourceTexture.format);

            //直接将原图拷贝到降分辨率的RT上
            Graphics.Blit(sourceTexture, rt);

            //进行迭代
            for (int i = 0; i < iteration; i++)
            {
                material.SetFloat("_BlurRadius", BlurRadius);
                Graphics.Blit(rt, sourceTexture, material);
                Graphics.Blit(sourceTexture, rt, material);
            }
            //将结果输出  
            Graphics.Blit(rt, destTexture);

            //释放RenderBuffer
            RenderTexture.ReleaseTemporary(rt);
        }
    }
}