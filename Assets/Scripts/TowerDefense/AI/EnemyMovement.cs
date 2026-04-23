using TowerDefense.Level;
using TowerDefense.Managers;
using UnityEngine;

namespace TowerDefense.AI
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        private int _currentNodeIndex;

        private Spline _currentSpline;
        private Enemy _enemy;
        private Vector3 _startLocation;
        private Vector3 _targetLocation;

        private float _timeAlongSpline;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
        }

        private void Update()
        {
            var segmentLength = Vector3.Distance(_startLocation, _targetLocation);
            _timeAlongSpline += segmentLength > 0f ? speed / segmentLength * Time.deltaTime : 0f;

            if (_timeAlongSpline >= 1f)
            {
                _timeAlongSpline = 0f;
                _currentNodeIndex++;
                _startLocation = _targetLocation;

                if (_currentNodeIndex >= _currentSpline.nodes.Count - 1)
                {
                    GetNextSpline();
                    return;
                }

                _targetLocation = _currentSpline.GetNextLocation(_currentNodeIndex);
            }

            transform.position = Spline.GetLocationAlongSpline(_timeAlongSpline, _startLocation, _targetLocation);
            transform.LookAt(_targetLocation);

            UpdatePathProgress();
        }

        private void UpdatePathProgress()
        {
            var nodeCount = _currentSpline.nodes.Count;
            var localProgress = nodeCount > 1
                ? (_currentNodeIndex + _timeAlongSpline) / (nodeCount - 1)
                : 0f;
            var sectionIndex = SectionsManager.Instance.GetSectionIndex(_currentSpline);
            _enemy.PathProgress = sectionIndex + localProgress;
        }

        public void Initialize(Spline spline)
        {
            _currentSpline = spline;
            _currentNodeIndex = 0;
            _timeAlongSpline = 0f;
            _startLocation = _currentSpline.GetWorldPosition(0);
            _targetLocation = _currentSpline.GetNextLocation(_currentNodeIndex);
            transform.position = _startLocation;
        }

        public void SetSpeed(float inSpeed)
        {
            speed *= inSpeed;
        }

        private void GetNextSpline()
        {
            var section = _currentSpline.GetComponentInParent(typeof(Section)) as Section;

            var nextSpline = section?.GetNextSpline();

            if (nextSpline is null)
            {
                WaveManager.Instance.OnEnemyRemoved(_enemy);
                transform.gameObject.SetActive(false);
                return;
            }

            _currentSpline = nextSpline;
            Initialize(nextSpline);
        }
    }
}