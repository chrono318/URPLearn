using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperty : MonoBehaviour
{
    static int baseColorId = Shader.PropertyToID("_BaseColor");
    static int cutoffId = Shader.PropertyToID("_Cutoff");
    static int metallicId = Shader.PropertyToID("_Metallic");
    static int smoothnessId = Shader.PropertyToID("_Smoothness");
    static int emissionColorId = Shader.PropertyToID("_EmissionColor");

    //[SerializeField]
    public Color baseColor = Color.white;

    [Range(0f, 1f)]
    public float cutoff = 0.0f, metallic = 0f, smoothness = 0.5f;

    [ColorUsage(false, true)]
    public Color emissionColor = Color.black;

    static MaterialPropertyBlock block;

    public void SetColor()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
        }
        block.SetColor(baseColorId, baseColor);
        block.SetFloat(cutoffId, cutoff);
        block.SetFloat(metallicId, metallic);
        block.SetFloat(smoothnessId, smoothness);
        block.SetColor(emissionColorId, emissionColor);
        GetComponent<Renderer>().SetPropertyBlock(block);
    }
    private void OnValidate()
    {
        SetColor();
    }
}
