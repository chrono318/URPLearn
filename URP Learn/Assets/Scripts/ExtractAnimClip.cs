using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

public class ExtractAnimClip : MonoBehaviour
{
    public AnimationClip clip;
    [Range(0, 60)]
    public int index;

    [Serializable]
    public struct MyAnimKeyInfo
    {
        public Object animObject;
        public string propertyName;
        public float value;
        public float oriValue;
    }

    public List<MyAnimKeyInfo> animKeyInfos;
    [ContextMenu("Load Anim")]
    void LoadAnim()
    {
        animKeyInfos = new List<MyAnimKeyInfo>();
        StringBuilder builder = new StringBuilder();
        foreach(var binding in UnityEditor.AnimationUtility.GetCurveBindings(clip))
        {
            MyAnimKeyInfo keyInfo = new MyAnimKeyInfo();
            keyInfo.animObject = transform.Find(binding.path).GetComponent(binding.type);
            builder.Append(binding.path);
            builder.Append(' ');
            builder.Append(binding.type);
            builder.Append(' ');
            builder.Append(binding.propertyName);
            builder.Append(' ');

            AnimationCurve curve = UnityEditor.AnimationUtility.GetEditorCurve(clip, binding);
            keyInfo.propertyName = binding.propertyName;
            keyInfo.value = curve[1].value;
            keyInfo.oriValue = curve[0].value;
            animKeyInfos.Add(keyInfo);
            builder.AppendLine();
        }
        Debug.Log(builder.ToString());
    }
    [ContextMenu("Update Anim", false, 0)]
    void UpdateAnim()
    {
        for(int i = 0; i < animKeyInfos.Count; i++)
        {
            MyAnimKeyInfo keyInfo = animKeyInfos[i];
            Type animType = keyInfo.animObject.GetType();
            string propertyName = keyInfo.propertyName.Split('.')[0];
            var property = animType.GetProperty(propertyName);
            if (property != null)
                property.SetValue(keyInfo.animObject, Mathf.Lerp(keyInfo.oriValue, keyInfo.value, index / 60f));
            else
                Debug.LogError("property is null");
        }Animation animation = GetComponent<Animation>();
    }
}
