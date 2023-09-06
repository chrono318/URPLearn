using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRender
{
    ScriptableRenderContext context;

    Camera camera;
    const string bufferName = "Render Camera";
    CommandBuffer commandBuffer = new CommandBuffer { name = bufferName };

    CullingResults cullingResults;
    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
    
    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        PrepareBuffer();
        PrepareForSceneWindow();
        if (!Cull()) return;

        SetUp();
        DrawVisibleGeometry();
        DrawUnsupportedShaders();
        DrawGizmos();

        Submit();
    }

    void DrawVisibleGeometry()
    {
        SortingSettings sortingSettings = new SortingSettings(camera) { criteria = SortingCriteria.CommonOpaque };
        DrawingSettings drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
        FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        context.DrawRenderers(cullingResults,ref drawingSettings,ref filteringSettings);

        context.DrawSkybox(camera);

        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }

    void SetUp()
    {
        context.SetupCameraProperties(camera);
        CameraClearFlags clearFlags = camera.clearFlags;
        commandBuffer.ClearRenderTarget(
            clearFlags <= CameraClearFlags.Depth,
            clearFlags == CameraClearFlags.Color,
            clearFlags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear);
        commandBuffer.BeginSample(SampleName);
        ExcuteBuffer();
    }

    void Submit()
    {
        commandBuffer.EndSample(SampleName);
        ExcuteBuffer();
        context.Submit();
    }

    void ExcuteBuffer()
    {
        context.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();
    }

    bool Cull()
    {
        ScriptableCullingParameters P;
        if(camera.TryGetCullingParameters(out P))
        {
            cullingResults = context.Cull(ref P);
            return true;
        }
        return false;
    }
}
