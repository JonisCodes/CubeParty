using TowerDefense.Managers;
using UnityEngine;

namespace TowerDefense.AI
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;

        private float _timeAlongSpline;
        private Vector3 _startLocation;
        private Vector3 _targetLocation;
        private bool _moving;
        private int _currentNodeIndex;

        private Spline _currentSpline;

        public void Initialize(Spline spline)
        {
            _currentSpline = spline;
            _startLocation = _currentSpline.nodes[0];
            _targetLocation = _currentSpline.GetNextLocation(_currentNodeIndex);
            transform.position = _startLocation;
        }

        private void Update()
        {
            _timeAlongSpline += speed * Time.deltaTime;

            if (_timeAlongSpline >= 1f)
            {
                _timeAlongSpline = 0f;
                _currentNodeIndex++;
                _startLocation = _targetLocation;
                _targetLocation = _currentSpline.GetNextLocation(_currentNodeIndex);
            }

            transform.position = Spline.GetLocationAlongSpline(_timeAlongSpline, _startLocation, _targetLocation);
            transform.LookAt(_targetLocation);

            if (_targetLocation != _startLocation) return;
            
            WaveManager.Instance.OnEnemyRemoved(gameObject.GetComponent<Enemy>());
            transform.gameObject.SetActive(false);
        }

        public void SetSpeed(float inSpeed)
        {
            speed *= inSpeed;
        }
    }
}