using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace Code.Gameplay.Infrastructure.Services
{
    public class SFXPlayer : MonoBehaviour
    {
        public static SFXPlayer Instance { get; private set; }

        [Header("Pool")] 
        [SerializeField] private int poolSize = 32;

        [Header("Mixer Groups (optional)")] 
        [SerializeField] private AudioMixerGroup sfxDefault;

        [SerializeField] private AudioMixerGroup sfxGunshots;
        [SerializeField] private AudioMixerGroup sfxFootsteps;
        [SerializeField] private AudioMixerGroup sfxHurt;
        [SerializeField] private AudioMixerGroup sfxDeath;

        [Header("3D Defaults")] 
        [SerializeField] private bool spatialByDefault = true;

        [SerializeField] private float minDistance = 2f;
        [SerializeField] private float maxDistance = 40f;

        private readonly List<AudioSource> _pool = new();
        private int _idx;

        void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < poolSize; i++)
            {
                var go = new GameObject($"SFX_{i}");
                go.transform.SetParent(transform);
                var src = go.AddComponent<AudioSource>();
                src.playOnAwake = false;
                src.loop = false;
                src.dopplerLevel = 0f;
                if (spatialByDefault)
                {
                    src.spatialBlend = 1f;
                    src.rolloffMode = AudioRolloffMode.Logarithmic;
                    src.minDistance = minDistance;
                    src.maxDistance = maxDistance;
                }
                else src.spatialBlend = 0f;

                _pool.Add(src);
            }
        }

        AudioSource Next()
        {
            for (int i = 0; i < _pool.Count; i++)
            {
                int idx = (_idx + i + 1) % _pool.Count;
                if (!_pool[idx].isPlaying)
                {
                    _idx = idx;
                    return _pool[idx];
                }
            }

            _idx = (_idx + 1) % _pool.Count;
            return _pool[_idx];
        }

        AudioMixerGroup ResolveGroup(SFXBus bus)
        {
            return bus switch
            {
                SFXBus.Gunshot => sfxGunshots ? sfxGunshots : sfxDefault,
                SFXBus.Footstep => sfxFootsteps ? sfxFootsteps : sfxDefault,
                SFXBus.Hurt => sfxHurt ? sfxHurt : sfxDefault,
                SFXBus.Death => sfxDeath ? sfxDeath : sfxDefault,
                _ => sfxDefault
            };
        }

        public void PlayAt(AudioClip clip, Vector3 pos, float volume = 1f, float pitch = 1f,
            int? priority = null, SFXBus bus = SFXBus.Default, bool force2D = false)
        {
            if (!clip) return;
            var src = Next();
            src.transform.position = pos;
            src.outputAudioMixerGroup = ResolveGroup(bus);
            src.priority = Mathf.Clamp(priority ?? 128, 0, 256);

            if (force2D) src.spatialBlend = 0f;
            else if (spatialByDefault) src.spatialBlend = 1f;

            src.pitch = pitch;
            src.volume = 1f;
            src.Stop();
            src.clip = clip;
            src.PlayOneShot(clip, Mathf.Clamp01(volume));
        }

        public void Play2D(AudioClip clip, float volume = 1f, float pitch = 1f,
            int? priority = null, SFXBus bus = SFXBus.Default)
            => PlayAt(clip, Vector3.zero, volume, pitch, priority, bus, force2D: true);
    }
}
