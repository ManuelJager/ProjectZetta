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
            if (PlayerPrefs.Instance.debug10)
                Debug.Log("Pure ship blueprint data is : " + output);
            StringUtil.Scramble(ref output);
            output.CopyToClipboard();
            if (PlayerPrefs.Instance.debug10)
                Debug.Log("Scrambled ship blueprint data is : " + output);
        }

        if (GUILayout.Button("Debug blueprint descrambling"))
        {
            grid.ClearGrid();
            var clipboard = StringUtil.ReadClipboard();
            if (PlayerPrefs.Instance.debug10)
                Debug.Log("clipboard data is : " + clipboard);
            StringUtil.ScrambleBack(ref clipboard);
            if (PlayerPrefs.Instance.debug10)
                Debug.Log("unscrambled clipboard data is : " + clipboard);
            var blueprint = new GridUtilities.Blueprint(clipboard);
            grid.LoadBlueprint(blueprint);
        }

        if (GUILayout.Button("Clear Clipboard"))
        {
            StringUtil.ClearClipboard();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            grid.ClearGrid();
        }
    }
}