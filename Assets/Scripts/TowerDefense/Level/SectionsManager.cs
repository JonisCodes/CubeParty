using UnityEngine;

namespace TowerDefense.Level
{
    public class SectionsManager : MonoBehaviour
    {
        private void Awake()
        {
            var children = GetComponentsInChildren<Section>();
            print(children.Length);
            for (var i = 0; i < children.Length; i++)
            {
                if (i >= children.Length - 1)
                {
                    print("End of map sections");
                    return;
                }

                var nextChild = children[i + 1];

                children[i].SetNextSpline(nextChild.GetSpline());
            }
        }
    }
}