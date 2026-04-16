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

                var worldPos = spline.transform.TransformPoint(spline.nodes[i]);
                var newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(spline, "Move Spline Node");
                    spline.nodes[i] = spline.transform.InverseTransformPoint(newWorldPos);
                }

                Handles.Label(worldPos + Vector3.up * 0.3f, $"Node {i}");
            }

            Handles.color = Color.red;
            for (var i = 1; i < spline.nodes.Count; i++)
                Handles.DrawLine(
                    spline.transform.TransformPoint(spline.nodes[i - 1]),
                    spline.transform.TransformPoint(spline.nodes[i]));
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
                    : Vector3.zero;
                spline.nodes.Add(lastPos);
            }

            if (spline.nodes.Count > 0 && GUILayout.Button("Remove Last Node"))
            {
                Undo.RecordObject(spline, "Remove Spline Node");
                spline.nodes.RemoveAt(spline.nodes.Count - 1);
            }

            if (spline.nodes.Count > 0 && GUILayout.Button("Update All Node Positions"))
            {
                Undo.RecordObject(spline, "Convert Nodes to Local Space");
                for (var i = 0; i < spline.nodes.Count; i++)
                    spline.nodes[i] = spline.transform.InverseTransformPoint(spline.nodes[i]);
            }
        }
    }
}
