using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private float flashDuration = 0.1f;
        [SerializeField] private float lifeTime = 5.0f;
        [SerializeField] private float fadeInterval = 0.1f;
        [SerializeField] private float fadeStep = 0.1f;
        
        private Renderer _renderer;
        private Material _material;
        private Color _originalColor;
        private Coroutine _flashRoutine;
        private Coroutine _destroyRoutine;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            if (_renderer && _renderer.material)
            {
                _material = _renderer.material;
                _originalColor = _material.color;
            }
        }

        public void TakeDamage(float damage)
        {
            if (_material == null) return;
            if (_flashRoutine != null) StopCoroutine(_flashRoutine);
            _flashRoutine = StartCoroutine(FlashColor(Color.red));

            if (_destroyRoutine == null)
            {
                _destroyRoutine = StartCoroutine(DestroySequence());
            }
        }

        public bool CanAddHealth()
        {
            if (_material != null)
            {
                if (_flashRoutine != null) StopCoroutine(_flashRoutine);
                _flashRoutine = StartCoroutine(FlashColor(Color.green));
            }
            return true;
        }

        private IEnumerator FlashColor(Color color)
        {
            _material.color = color;
            yield return new WaitForSeconds(flashDuration);
            _material.color = _originalColor;
            _flashRoutine = null;
        }

        private IEnumerator DestroySequence()
        {
            if (lifeTime > 0f) yield return new WaitForSeconds(lifeTime);
            yield return StartCoroutine(FadeOut());
            Destroy(gameObject);
        }

        private IEnumerator FadeOut()
        {
            if (_material == null) yield break;
            Color color = _material.color;
            for (float a = 1.0f; a >= 0.0f; a -= Mathf.Abs(fadeStep))
            {
                color.a = a;
                _material.color = color;
                yield return new WaitForSeconds(Mathf.Max(0.01f, fadeInterval));
            }
        }
    }
}
