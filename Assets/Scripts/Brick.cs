using UnityEngine;
using System;

namespace Scripts
{
    public class Brick : MonoBehaviour
    {
        public Vector2 BrickSize => new Vector2(3.2f, 1.2f);

        [SerializeField] private MeshRenderer _meshRenderer;

        public event Action<Brick> OnDestroyed;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId     = Shader.PropertyToID("_Color");

        private MaterialPropertyBlock _mpb;
        private int _hp;
        private int _maxHp;
        private Color _baseColor;

        private void Awake()
        {
            _mpb = new MaterialPropertyBlock();
        }

        public void Init(int hp, Color color)
        {
            _maxHp = Mathf.Max(1, hp);
            _hp = _maxHp;
            _baseColor = color;
            ApplyColor(1f);
        }

        public bool TakeHit()
        {
            if (_hp <= 0) return false;
            _hp--;
            if (_hp <= 0)
            {
                Destroy(gameObject);
                return true;
            }
            else
            {
                float t = Mathf.Clamp01((float)_hp / _maxHp);
                ApplyColor(t);
                return false;
            }
        }

        private void ApplyColor(float t)
        {
            Color c = Color.Lerp(Color.black, _baseColor, Mathf.Lerp(0.55f, 1f, t));

            _meshRenderer.GetPropertyBlock(_mpb);
            var mat = _meshRenderer.sharedMaterial;
            if (mat != null && mat.HasProperty(BaseColorId))
                _mpb.SetColor(BaseColorId, c);
            else
                _mpb.SetColor(ColorId, c);    
            _meshRenderer.SetPropertyBlock(_mpb);
        }

        private void OnDestroy()
        {
            if (!Application.isPlaying) return;
            OnDestroyed?.Invoke(this);
        }
    }
}
