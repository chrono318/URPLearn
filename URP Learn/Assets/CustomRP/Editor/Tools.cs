using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tools
{
    [MenuItem("Tools/Éú³É24¸öunlitÇò")]
    static void GenerateBalls()
    {
        //color
        List<Color> colors = new List<Color>();
        colors.Add(Color.white);
        colors.Add(Color.yellow);
        colors.Add(Color.green);
        colors.Add(Color.blue);
        colors.Add(Color.red);
        colors.Add(Color.gray);
        colors.Add(Color.cyan);
        colors.Add(Color.grey);
        //Generate
        GameObject select = Selection.activeGameObject;
        Transform root = select.transform;
        for(int i=0; i < 24; i++)
        {
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            child.transform.parent = root;
            child.transform.localPosition = new Vector3 (i%8, i/8, 0f);
            Material material = new Material(Shader.Find("Custom RP/Unlit"));
            child.GetComponent<MeshRenderer>().material = material;
            PerObjectMaterialProperty comp = child.AddComponent<PerObjectMaterialProperty>();
            
            comp.baseColor = colors[Random.Range(0, colors.Count)];
            comp.SetColor();
        }
    }
}
