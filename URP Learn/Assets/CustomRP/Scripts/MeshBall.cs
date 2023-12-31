using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class MeshBall : MonoBehaviour
{
    static int baseColorId = Shader.PropertyToID("_BaseColor");
    static int metallicId = Shader.PropertyToID("_Metallic");
    static int smoothnessId = Shader.PropertyToID("_Smoothness");

    [SerializeField]
    Mesh mesh = default;

    [SerializeField]
    Material material = default;

    [SerializeField]
    LightProbeProxyVolume lightProbeProxyVolume = null;

    Matrix4x4[] matrix4s = new Matrix4x4[1023];
    Vector4[] baseColors = new Vector4[1023];
    float[] metallic = new float[1023];
    float[] smoothness = new float[1023];

    MaterialPropertyBlock block;

    private void Awake()
    {
        for(int i = 0; i < matrix4s.Length; i++)
        {
            matrix4s[i] = Matrix4x4.TRS(
                    Random.insideUnitSphere * 10f, 
                    Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360), 
                    Vector3.one * Random.Range(0.5f, 1.5f)
                );
            baseColors[i] = new Vector4(Random.value, Random.value, Random.value, Random.Range(0.5f, 1f));
            metallic[i] = Random.value < 0.25f ? 1f : 0f;
            smoothness[i] = Random.Range(0.05f, 0.95f);
        }
    }

    private void Update()
    {
        if(block == null)
        {
            block = new MaterialPropertyBlock();
            block.SetVectorArray(baseColorId, baseColors);
            block.SetFloatArray(metallicId, metallic);
            block.SetFloatArray(smoothnessId, smoothness);

            if (!lightProbeProxyVolume)
            {
                Vector3[] positions = new Vector3[1023];
                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i] = matrix4s[i].GetColumn(3);
                }
                SphericalHarmonicsL2[] lightProbes = new SphericalHarmonicsL2[1023];
                Vector4[] occlusionProbes = new Vector4[1023]; 
                LightProbes.CalculateInterpolatedLightAndOcclusionProbes(
                        positions, lightProbes, occlusionProbes
                    );
                block.CopySHCoefficientArraysFrom(lightProbes);
                block.CopyProbeOcclusionArrayFrom(occlusionProbes);
            }
        }
        Graphics.DrawMeshInstanced(
            mesh, 0, material, matrix4s, 1023, block,
            ShadowCastingMode.On, true, 0, null, 
            lightProbeProxyVolume ? LightProbeUsage.UseProxyVolume : LightProbeUsage.CustomProvided,
            lightProbeProxyVolume);
    }
}
