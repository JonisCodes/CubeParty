using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.AI
{
    [ExecuteInEditMode]
    public class Spline : MonoBehaviour
    {
        public List<Vector3> nodes = new List<Vector3>();

        private void Update()
        {
            if (nodes.Count <= 0) return;

            for (var i = 1; i < nodes.Count; i++)
                Debug.DrawLine(transform.TransformPoint(nodes[i]), transform.TransformPoint(nodes[i - 1]), Color.red);
        }

        public static Vector3 GetLocationAlongSpline(float t, Vector3 currentLocation, Vector3 targetLocation)
        {
            return Vector3.Lerp(currentLocation, targetLocation, t);
        }

        public Vector3 GetWorldPosition(int index) => transform.TransformPoint(nodes[index]);

        public Vector3 GetNextLocation(int currIndex)
        {
            return currIndex + 1 >= nodes.Count ? Vector3.zero : transform.TransformPoint(nodes[currIndex + 1]);
        }
    }
}