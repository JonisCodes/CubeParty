using UnityEditor;
using UnityEngine;

namespace TowerDefense.AI
{
    [CustomEditor(typeof(Spline))]
    public class SplineEditor : Editor
    {
        private void OnSceneGUI()
        {
            var spline = (Spline)target;

            for (var i = 0; i < spline.nodes.Count; i++)
            {
                EditorGUI.BeginChangeCheck();

                var newPos = Handles.PositionHandle(spline.nodes[i], Quaternion.identity);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(spline, "Move Spline Node");
                    spline.nodes[i] = newPos;
                }

                Handles.Label(spline.nodes[i] + Vector3.up * 0.3f, $"Node {i}");
            }

            // Draw lines between nodes in the scene view
            Handles.color = Color.red;
            for (var i = 1; i < spline.nodes.Count; i++)
            {
                Handles.DrawLine(spline.nodes[i - 1], spline.nodes[i]);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var spline = (Spline)target;

            EditorGUILayout.Space();

            if (GUILayout.Button("Add Node"))
            {
                Undo.RecordObject(spline, "Add Spline Node");
                var lastPos = spline.nodes.Count > 0
                    ? spline.nodes[^1] + Vector3.right * 2f
                    : spline.transform.position;
                spline.nodes.Add(lastPos);
            }

            if (spline.nodes.Count > 0 && GUILayout.Button("Remove Last Node"))
            {
                Undo.RecordObject(spline, "Remove Spline Node");
                spline.nodes.RemoveAt(spline.nodes.Count - 1);
            }
        }
    }
}
