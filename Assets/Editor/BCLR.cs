using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BAOC_Color))]
public class BCLR : Editor
{
    public override void OnInspectorGUI()
    {
        BAOC_Color tgt = new BAOC_Color(0, 0, 0);

        SerializedObject so = new SerializedObject(target);
        BAOC.that = this;
        BAOC.Log(so.FindProperty("R").floatValue);
    }
}