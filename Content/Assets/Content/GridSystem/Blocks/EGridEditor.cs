using UnityEngine;
using System.Collections;
using UnityEditor;
using GridUtilities.GridReader;
using GridUtilities.GridWriter;

[CustomEditor(typeof(ShipGrid))]
public class EGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ShipGrid grid = (ShipGrid)target;
        if (GUILayout.Button("Copy blueprint to clipboard"))
        {
            grid.blockGrid.ReadGrid().CopyToClipboard();
        }

        if (GUILayout.Button("Set blueprint from clipboard"))
        {

        }

        if (GUILayout.Button("Debug blueprint scrabling"))
        {
            var output = grid.blockGrid.ReadGrid();
            Debug.Log("Pure ship blueprint data is : " + output);
            StringUtil.Scramble(ref output);
            output.CopyToClipboard();
            Debug.Log("Scrambled ship blueprint data is : " + output);
        }

        if (GUILayout.Button("Debug blueprint descrambling"))
        {
            var clipboard = StringUtil.ReadClipboard();
            Debug.Log("clipboard data is : " + clipboard);
            StringUtil.ScrambleBack(ref clipboard);
            Debug.Log("unscrambled clipboard data is : " + clipboard);

        }

        if (GUILayout.Button("Clear Clipboard"))
        {
            StringUtil.ClearClipboard();
        }
    }
}