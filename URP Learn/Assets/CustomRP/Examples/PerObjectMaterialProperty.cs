using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperty : MonoBehaviour
{
    static int baseColorId = Shader.PropertyToID("_BaseColor");

    //[SerializeField]
    public Color baseColor = Color.white;

    static MaterialPropertyBlock block;

    private void OnValidate()
    {
        if(block == null)
        {
            block = new MaterialPropertyBlock();
        }
        block.SetColor(baseColorId, baseColor);
        GetComponent<Renderer>().SetPropertyBlock(block);
    }
}
