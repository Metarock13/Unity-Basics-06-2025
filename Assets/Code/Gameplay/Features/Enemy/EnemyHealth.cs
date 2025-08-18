using Code.Gameplay.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Features.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [Header("HP")]
        [SerializeField] private float maxHP = 50f;
        [SerializeField] private bool destroyOnDeath = true;
        [SerializeField] private float despawnDelay = 1.5f;

        [Header("Hurt SFX")]
        [SerializeField] private AudioClip[] hurtClips;
        [Range(0f, 1f)] [SerializeField] private float hurtVolume = 0.9f;
        [SerializeField] private Vector2 hurtPitch = new Vector2(0.98f, 1.02f);
        [SerializeField] private float minHurtInterval = 0.08f;

        [Header("Death SFX")]
        [SerializeField] private AudioClip[] deathClips;
        [Range(0f, 1f)] [SerializeField] private float deathVolume = 1f;
        [SerializeField] private Vector2 deathPitch = new Vector2(0.96f, 1.04f);

        [Header("Audio")]
        [SerializeField] private bool spatial3D = true;

        private float _hp;
        private float _lastHurtTime;
        private bool _dead;

        [SerializeField] private MonoBehaviour[] disableOnDeath;
        private Collider[] _colliders;

        private void Awake()
        {
            _hp = Mathf.Max(0, maxHP);
            _colliders = GetComponentsInChildren<Collider>();
        }

        public void TakeDamage(float damage, Vector3 hitPoint)
        {
            if (_dead || damage <= 0f) return;

            _hp = Mathf.Max(0f, _hp - damage);

            if (_hp > 0f)
            {
                PlayHurt(hitPoint);
                return;
            }

            Die(hitPoint);
        }

        public void TakeDamage(float damage) => TakeDamage(damage, transform.position);

        private void PlayHurt(Vector3 pos)
        {
            if (hurtClips == null || hurtClips.Length == 0) return;
            if (Time.time - _lastHurtTime < minHurtInterval) return;
            _lastHurtTime = Time.time;

            var clip = hurtClips[Random.Range(0, hurtClips.Length)];
            float pitch = Random.Range(hurtPitch.x, hurtPitch.y);

            PlayClip(clip, pos, hurtVolume, pitch, priority: 56, bus: SFXBus.Hurt);
        }

        private void Die(Vector3 pos)
        {
            if (_dead) return;
            _dead = true;

            if (disableOnDeath != null)
                foreach (var b in disableOnDeath) if (b) b.enabled = false;

            if (_colliders != null)
                foreach (var c in _colliders) if (c) c.enabled = false;

            if (deathClips != null && deathClips.Length > 0)
            {
                var clip = deathClips[Random.Range(0, deathClips.Length)];
                float pitch = Random.Range(deathPitch.x, deathPitch.y);

                PlayClip(clip, pos, deathVolume, pitch, priority: 48, bus: SFXBus.Death);
            }

            if (destroyOnDeath) Destroy(gameObject, despawnDelay);
        }

        private void PlayClip(AudioClip clip, Vector3 pos, float volume, float pitch, int priority,
                              SFXBus bus = SFXBus.Default)
        {
            if (!clip) return;

            if (SFXPlayer.Instance)
            {
                SFXPlayer.Instance.PlayAt(clip, spatial3D ? pos : transform.position,
                                          Mathf.Clamp01(volume), pitch, priority, bus);
                return;
            }

            var go = new GameObject("EnemySFX_Temp");
            go.transform.position = spatial3D ? pos : transform.position;
            var src = go.AddComponent<AudioSource>();
            src.spatialBlend = spatial3D ? 1f : 0f;
            src.dopplerLevel = 0f;
            src.loop = false;
            src.playOnAwake = false;
            src.priority = priority;
            src.pitch = pitch;
            src.volume = 1f;
            src.PlayOneShot(clip, Mathf.Clamp01(volume));
            Destroy(go, clip.length + 0.1f);
        }
    }
}
