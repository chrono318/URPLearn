using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRender
{
    ScriptableRenderContext context;

    Camera camera;
    const string bufferName = "Render Camera";
    CommandBuffer commandBuffer = new CommandBuffer { name = bufferName };
    
    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        SetUp();
        DrawVisibleGeometry();

        Submit();
    }

    void DrawVisibleGeometry()
    {
        context.DrawSkybox(camera);
    }

    void SetUp()
    {
        context.SetupCameraProperties(camera);
        commandBuffer.ClearRenderTarget(true, true, Color.clear);
        commandBuffer.BeginSample(bufferName);
        ExcuteBuffer();
    }

    void Submit()
    {
        commandBuffer.EndSample(bufferName);
        ExcuteBuffer();
        context.Submit();
    }

    void ExcuteBuffer()
    {
        context.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();
    }
}
