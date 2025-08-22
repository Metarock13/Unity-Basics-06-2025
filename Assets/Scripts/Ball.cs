using UnityEngine;
using System;

namespace Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed = 8f;
        [SerializeField] private Vector3 _spawnPoint;

        private Vector2 _direction;
        public event Action OnDeadZone;

        private void Awake()
        {
            if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        private void Start() => Reset();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _direction == Vector2.zero)
                Launch();
        }

        public void Reset()
        {
            transform.position = _spawnPoint;
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            SetVelocity(Vector2.zero);
        }

        private void SetVelocity(Vector2 dir)
        {
            _direction = dir;
            _rigidbody.linearVelocity = new Vector3(_direction.x, _direction.y, 0f) * _speed;
        }

        private void Launch()
        {
            int sign = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
            SetVelocity(new Vector2(sign, 1f).normalized);
        }

        private bool IsDeadZone(Transform t)
        {
            return t.GetComponent<DeadZone>() || t.GetComponentInParent<DeadZone>() != null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var t = collision.transform;

            if (t.TryGetComponent(out Paddle paddle))
            {
                float dx = transform.position.x - paddle.transform.position.x;
                float ratio = Mathf.Clamp(dx / collision.collider.bounds.size.x, -1f, 1f);
                SetVelocity(new Vector2(ratio, 1f).normalized);
                AudioManager.I?.PlayPaddleHit();
                return;
            }

            if (t.TryGetComponent(out Brick brick))
            {
                Vector3 n = collision.GetContact(0).normal;
                Vector2 newDir = Vector2.Reflect(_direction, new Vector2(n.x, n.y)).normalized;
                SetVelocity(newDir);
                bool destroyed = brick.TakeHit();
                if (destroyed) AudioManager.I?.PlayBrick();
                return;
            }

            if (t.TryGetComponent(out Wall _))
            {
                AudioManager.I?.PlayWallHit();
                return;
            }

            if (IsDeadZone(t))
            {
                OnDeadZone?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsDeadZone(other.transform))
                OnDeadZone?.Invoke();
        }
    }
}
