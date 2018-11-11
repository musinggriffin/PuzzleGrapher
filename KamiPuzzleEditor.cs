//copyright Quinn Griffin 2017

using UnityEngine;
using UnityEditor;

namespace KamiExample
{
    /// <summary>
    /// This custom editor adds two buttons to the inspector. One to build the puzzle model, and a second to explore its state graph.
    /// </summary>
    [CustomEditor(typeof(KamiPuzzleBuilder))]
    public class KamiPuzzleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            KamiPuzzleBuilder myScript = (KamiPuzzleBuilder)target;

            if (GUILayout.Button("Build Puzzle"))
            {
                myScript.GeneratePuzzle();
            }

            if (GUILayout.Button("Build State Graph"))
            {
                myScript.GenerateStateGraph();
            }
        }
    }
}

