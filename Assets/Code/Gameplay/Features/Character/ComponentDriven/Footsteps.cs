using Code.Gameplay.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Features.Character.ComponentDriven
{
    public class Footsteps : MonoBehaviour
    {
        [Header("Audio")] 
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip[] clips;
        [Range(0.05f, 1f)] [SerializeField] private float baseInterval = 0.5f;
        [Range(0f, 0.2f)] [SerializeField] private float pitchJitter = 0.04f;
        [Range(0f, 1f)] [SerializeField] private float volume = 1f;

        [Header("Motion detection")] 
        [SerializeField] private float speedThreshold = 0.1f;

        private CharacterController _cc;
        private Rigidbody _rb;
        private Vector3 _lastPos;
        private float _timer;

        private void Awake()
        {
            _cc = GetComponent<CharacterController>();
            _rb = GetComponent<Rigidbody>();
            if (!source) source = GetComponent<AudioSource>();
            _timer = baseInterval;
            _lastPos = transform.position;
        }

        private void Update()
        {
            if (!source || clips == null || clips.Length == 0) return;

            Vector3 v = Vector3.zero;
            if (_cc) v = _cc.velocity;
            else if (_rb) v = _rb.linearVelocity;
            else
            {
                Vector3 delta = (transform.position - _lastPos) / Mathf.Max(Time.deltaTime, 0.0001f);
                v = delta;
            }

            _lastPos = transform.position;

            float horizSpeed = new Vector2(v.x, v.z).magnitude;

            if (horizSpeed > speedThreshold)
            {
                float speedFactor = Mathf.Lerp(1f, 2.2f, Mathf.Clamp01(horizSpeed / 6f));
                _timer -= Time.deltaTime * speedFactor;

                if (_timer <= 0f)
                {
                    PlayStep();
                    _timer = baseInterval;
                }
            }
            else
            {
                _timer = Mathf.Min(_timer, baseInterval * 0.5f);
            }
        }

        private void PlayStep()
        {
            int i = Random.Range(0, clips.Length);
            float pitch = 1f + Random.Range(-pitchJitter, pitchJitter);
            SFXPlayer.Instance.PlayAt(clips[i], transform.position, volume, pitch,
                priority: 160, bus: SFXBus.Footstep);
        }
    }
}
