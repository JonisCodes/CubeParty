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
            {
                Debug.DrawLine(nodes[i], nodes[i - 1], Color.red);
            }
        }

        public static Vector3 GetLocationAlongSpline(float t, Vector3 currentLocation, Vector3 targetLocation)
        {
            var newLocation = Vector3.Lerp(currentLocation, targetLocation, t);
            return newLocation;
        }

        public Vector3 GetNextLocation(int currIndex)
        {
            return currIndex + 1 >= nodes.Count ? Vector3.zero : nodes[currIndex + 1];
        }
    }
}