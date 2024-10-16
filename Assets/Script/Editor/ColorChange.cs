using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorAdd))]
public class ColorChange : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ColorAdd colorAdd = (ColorAdd)target;

        if (GUILayout.Button("Color"))
        {
            colorAdd.SetColor();
        }
    }
}
