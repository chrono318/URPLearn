using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MeshBall : MonoBehaviour
{
    static int baseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField]
    Mesh mesh = default;

    [SerializeField]
    Material material = default;

    Matrix4x4[] matrix4s = new Matrix4x4[1023];
    Vector4[] baseColors = new Vector4[1023];

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
        }
    }

    private void Update()
    {
        if(block == null)
        {
            block = new MaterialPropertyBlock();
            block.SetVectorArray(baseColorId, baseColors);
        }
        Graphics.DrawMeshInstanced(mesh, 0, material, matrix4s, 1023, block);
    }
}
