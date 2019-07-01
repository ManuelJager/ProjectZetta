using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Thruster))]
public class EThrusterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Thruster block = (Thruster)target;
        if (GUILayout.Button("Remove block"))
        {
            block.blockBaseClass.shipGrid.RemoveFromGrid(((MonoBehaviour)target).transform);
        }
    }
}