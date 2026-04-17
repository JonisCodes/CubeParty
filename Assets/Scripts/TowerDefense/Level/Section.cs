using TowerDefense.AI;
using UnityEngine;

namespace TowerDefense.Level
{
    public class Section : MonoBehaviour
    {
        [SerializeField] private Spline spline;
        [SerializeField] private Spline nextSpline;

        public Spline GetNextSpline()
        {
            return nextSpline;
        }

        public Spline GetSpline()
        {
            return spline;
        }

        public void SetNextSpline(Spline newSpline)
        {
            nextSpline = newSpline;
        }
    }
}