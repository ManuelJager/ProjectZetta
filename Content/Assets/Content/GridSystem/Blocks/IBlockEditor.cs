using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Thruster))]
public class IBlockEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Thruster block = (Thruster)target;
        if (GUILayout.Button("Build Object"))
        {
            block.blockBaseClass.shipGrid.RemoveFromGrid(((MonoBehaviour)target).transform);
        }
    }
}