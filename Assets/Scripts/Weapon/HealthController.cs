using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private float health = 3.0f;
        [SerializeField] private float lifeTime = 5.0f;

        private float _maxHp;
        private bool _isAlive = true;
        
        private void Start()
        {
            _maxHp = health;
        }

        public bool TakeDamage(float damage)
        {
            if (!_isAlive) return false;

            health -= damage;

            if (health <= 0)
            {
                StartCoroutine(Die());
                _isAlive = false;
                return false;
            }
            
            return true;
        }

        public bool CanAddHealth()
        {
            if (_isAlive == false)
            {
                return false;
            }

            if (health >= _maxHp)
            {
                return false;
            }

            var hp = health + _maxHp * 0.25f;

            health = Mathf.Min(hp, _maxHp);
            
            return true;
        }

        private IEnumerator Die()
        {
            var component = GetComponent<Renderer>();
            
            component.material.color = Color.red;
            yield return new WaitForSeconds(1.0f);
            component.material.color = Color.green;
            yield return new WaitForSeconds(1.0f);
            component.material.color = Color.red;
            yield return new WaitForSeconds(1.0f);
            component.material.color = Color.magenta;


            yield return new WaitForSeconds(lifeTime);

            StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            if (TryGetComponent(out Renderer component))
            {
                Color color = component.material.color;
                for (float alpha = 1.0f; alpha >= 0.0f; alpha -= 0.1f)
                {
                    color.a = alpha;
                    component.material.color = color;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            
            Destroy(gameObject);
        }
    }
}
