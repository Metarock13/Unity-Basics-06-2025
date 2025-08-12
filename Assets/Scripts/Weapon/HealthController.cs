using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private float flashDuration = 0.1f;
        [SerializeField] private float lifeTime = 5.0f;
        [SerializeField] private float fadeInterval = 0.1f;
        [SerializeField] private float fadeStep = 0.1f;
        
        private Renderer[] _renderers;
        private Material[] _materials;
        private Color[] _originalColors;
        private Coroutine _flashRoutine;
        private Coroutine _destroyRoutine;

        private void Start()
        {
            _renderers = GetComponentsInChildren<Renderer>(true);
            var mats = new List<Material>(8);
            var colors = new List<Color>(8);
            foreach (var r in _renderers)
            {
                if (!r) continue;
                var rms = r.materials;
                for (int i = 0; i < rms.Length; i++)
                {
                    var m = rms[i];
                    if (!m) continue;
                    mats.Add(m);
                    colors.Add(m.color);
                }
            }
            _materials = mats.ToArray();
            _originalColors = colors.ToArray();
        }

        public void TakeDamage(float damage)
        {
            if (_materials == null || _materials.Length == 0) return;
            if (_flashRoutine != null) StopCoroutine(_flashRoutine);
            _flashRoutine = StartCoroutine(FlashColor(Color.red));

            if (_destroyRoutine == null)
            {
                _destroyRoutine = StartCoroutine(DestroySequence());
            }
        }

        public bool CanAddHealth()
        {
            if (_materials != null && _materials.Length > 0)
            {
                if (_flashRoutine != null) StopCoroutine(_flashRoutine);
                _flashRoutine = StartCoroutine(FlashColor(Color.green));
            }
            return true;
        }

        private IEnumerator FlashColor(Color color)
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i].color = color;
            }
            yield return new WaitForSeconds(flashDuration);
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i].color = _originalColors[i];
            }
            _flashRoutine = null;
        }

        private IEnumerator DestroySequence()
        {
            if (lifeTime > 0f) yield return new WaitForSeconds(lifeTime);
            if (_flashRoutine != null)
            {
                StopCoroutine(_flashRoutine);
                _flashRoutine = null;
                for (int i = 0; i < _materials.Length; i++)
                {
                    _materials[i].color = _originalColors[i];
                }
            }
            yield return StartCoroutine(FadeOut());
            Destroy(gameObject);
        }

        private IEnumerator FadeOut()
        {
            if (_materials == null || _materials.Length == 0) yield break;
            int steps = Mathf.Max(1, Mathf.CeilToInt(1f / Mathf.Max(0.01f, Mathf.Abs(fadeStep))));
            float interval = Mathf.Max(0.01f, fadeInterval);
            for (int s = steps; s >= 0; s--)
            {
                float a = s / (float)steps;
                for (int i = 0; i < _materials.Length; i++)
                {
                    var c = _materials[i].color;
                    c.a = a;
                    _materials[i].color = c;
                }
                yield return new WaitForSeconds(interval);
            }
        }
    }
}