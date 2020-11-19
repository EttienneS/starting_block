using UnityEditor;
using UnityEngine;

namespace Assets
{
    [CustomEditor(typeof(SimpleMapGen))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var myTarget = (SimpleMapGen)target;

            GUILayout.Space(10);
            if (GUILayout.Button("Regenerate World"))
            {
                myTarget.RegenerateMap();
            }
        }
    }
}