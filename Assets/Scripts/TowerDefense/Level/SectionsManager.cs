using System.Collections.Generic;
using TowerDefense.AI;
using UnityEngine;

namespace TowerDefense.Level
{
    public class SectionsManager : MonoBehaviour
    {
        private readonly Dictionary<Spline, int> _splineIndices = new();
        public static SectionsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            var children = GetComponentsInChildren<Section>();
            print(children.Length);
            for (var i = 0; i < children.Length; i++)
            {
                _splineIndices[children[i].GetSpline()] = i;

                if (i >= children.Length - 1)
                {
                    print("End of map sections");
                    return;
                }

                children[i].SetNextSpline(children[i + 1].GetSpline());
            }
        }

        public int GetSectionIndex(Spline spline) =>
            _splineIndices.TryGetValue(spline, out var index) ? index : 0;
    }
}