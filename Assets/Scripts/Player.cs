using UnityEngine;

public class Player : MonoBehaviour
{
    private const float MoveSpeed = 5f;
    private const float RotateSpeed = 200f;

    private const float MaxHealth = 100f;
    private float _currentHealth = 50f;
        
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        var move = Input.GetAxis("Vertical") * MoveSpeed;
        var rotate = Input.GetAxis("Horizontal") * RotateSpeed * Time.fixedDeltaTime;
        
        var moveDirection = transform.forward * (move * Time.fixedDeltaTime);
        _rb.MovePosition(_rb.position + moveDirection);
        
        var rotateDirection = Quaternion.Euler(0f, rotate, 0f);
        _rb.MoveRotation(_rb.rotation * rotateDirection);
    }
    
    private void Update()
    {
        var move = Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime;
        var rotate = Input.GetAxis("Horizontal") * RotateSpeed * Time.deltaTime;
            
        transform.Translate(Vector3.forward * move);
        transform.Rotate(Vector3.up * rotate);
    }

    private void OnTriggerEnter(Collider other)
    {
        var medkit = other.gameObject.GetComponent<Medkit>();
        if (medkit)
        {
           Heal(Medkit.HealAmount);
           Destroy(medkit.gameObject);
        }
    }

    private void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, MaxHealth);
        Debug.Log("Healed: " + amount + ", Health: " + _currentHealth);
    }
}