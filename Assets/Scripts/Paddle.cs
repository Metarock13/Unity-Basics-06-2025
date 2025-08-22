using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Paddle : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed = 12f;
        [SerializeField] private Vector3 _spawnPoint;
        [SerializeField] private float leftRightPadding = 0.3f;

        private float _axis;
        private Camera _cam;
        private float _minX, _maxX, _halfWidth;

        private void Awake()
        {
            if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();

            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY |
                                     RigidbodyConstraints.FreezePositionZ |
                                     RigidbodyConstraints.FreezeRotation;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

            _cam = Camera.main;

            var r = GetComponentInChildren<Renderer>();
            _halfWidth = r ? r.bounds.extents.x : 0.5f;

            RecalcBounds();
        }

        private void RecalcBounds()
        {
            var vp = _cam.WorldToViewportPoint(transform.position);
            var leftWorld  = _cam.ViewportToWorldPoint(new Vector3(0f, vp.y, vp.z));
            var rightWorld = _cam.ViewportToWorldPoint(new Vector3(1f, vp.y, vp.z));

            _minX = leftWorld.x  + leftRightPadding + _halfWidth;
            _maxX = rightWorld.x - leftRightPadding - _halfWidth;
        }

        private void Update()
        {
            _axis = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {
            Vector3 pos = _rigidbody.position;
            float newX = Mathf.Clamp(pos.x + _axis * _speed * Time.fixedDeltaTime, _minX, _maxX);
            _rigidbody.MovePosition(new Vector3(newX, pos.y, pos.z));
        }

        public void Reset()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.position = _spawnPoint;
            RecalcBounds();
        }
    }
}
