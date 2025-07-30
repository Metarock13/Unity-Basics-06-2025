using UnityEngine;

namespace Scripts
{
    public class Player : MonoBehaviour
    {
        private const float MoveSpeed = 5f;
        private const float RotateSpeed = 200f;

        private const float MaxHealth = 100f;
        private float _currentHealth = 50f;

        [SerializeField] private bool usePhysicsMovement = true;
    
        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }
    
        private void FixedUpdate()
        {
            if (!usePhysicsMovement)
            {
                return;
            }
        
            var move = Input.GetAxis("Vertical") * MoveSpeed;
            var rotate = Input.GetAxis("Horizontal") * RotateSpeed * Time.fixedDeltaTime;
        
            var moveDirection = transform.forward * (move * Time.fixedDeltaTime);
            _rb.MovePosition(_rb.position + moveDirection);
        
            var rotateDirection = Quaternion.Euler(0f, rotate, 0f);
            _rb.MoveRotation(_rb.rotation * rotateDirection);
        }
    
        private void Update()
        {
            if (usePhysicsMovement)
            {
                return;
            }
        
            var move = Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime;
            var rotate = Input.GetAxis("Horizontal") * RotateSpeed * Time.deltaTime;
            
            transform.Translate(Vector3.forward * move);
            transform.Rotate(Vector3.up * rotate);
        }

        internal void Heal(float amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, MaxHealth);
            Debug.Log("Healed: " + amount + ", Health: " + _currentHealth);
        }
    }
}