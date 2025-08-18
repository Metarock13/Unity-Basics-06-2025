using System;
using System.Collections;
using UnityEngine;

namespace Code.Gameplay.Features.Weapons
{
   public class Bullet : MonoBehaviour
   {
      [SerializeField] private Rigidbody _rigidbody;
      [SerializeField] private float _timeToDisable = 3f;
      private bool isTriggered;

      public event Action<Bullet> OnDisable;

      private void OnEnable()
      {
         StartCoroutine(TimerForDisable());
      }

      public void SetDirection(Vector3 direction, float speed)
      {
         _rigidbody.linearVelocity = Vector3.zero;
         transform.forward = direction;

         _rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);
      }

      private void OnCollisionEnter(Collision other)
      {
         OnDisable?.Invoke(this);
         _rigidbody.linearVelocity = Vector3.zero;
         
         Destroy(gameObject);
      }

      private void OnTriggerEnter(Collider other)
      {
         if (isTriggered) return;
         
         OnDisable?.Invoke(this);
         
         isTriggered = true;
         
         Destroy(gameObject);
      }

      private IEnumerator TimerForDisable()
      {
         yield return new WaitForSeconds(_timeToDisable);
         OnDisable?.Invoke(this);
         Destroy(gameObject);
      }
   }
}