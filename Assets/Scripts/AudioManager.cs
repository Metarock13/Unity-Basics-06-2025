using UnityEngine;

namespace Scripts
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager I { get; private set; }

        [Header("Music")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioClip _musicLoop;
        [Range(0f,1f)] public float musicVolume = 0.4f;

        [Header("SFX")]
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioClip _sfxWall;
        [SerializeField] private AudioClip _sfxPaddle;
        [SerializeField] private AudioClip _sfxBrick;
        [SerializeField] private AudioClip _sfxGameOver;
        [Range(0f,1f)] public float sfxVolume = 0.8f;

        [SerializeField, Range(0f,0.2f)]
        private float _pitchJitter = 0.05f;

        private void Awake()
        {
            if (I && I != this) { Destroy(gameObject); return; }
            I = this;
            DontDestroyOnLoad(gameObject);

            if (!_musicSource) _musicSource = gameObject.AddComponent<AudioSource>();
            if (!_sfxSource)   _sfxSource   = gameObject.AddComponent<AudioSource>();

            _musicSource.playOnAwake = false;
            _musicSource.loop = true;
            _musicSource.spatialBlend = 0f;

            _sfxSource.playOnAwake = false;
            _sfxSource.spatialBlend = 0f;
        }

        public void PlayMusicLoop()
        {
            if (!_musicLoop) return;
            _musicSource.clip = _musicLoop;
            _musicSource.volume = musicVolume;
            if (!_musicSource.isPlaying) _musicSource.Play();
        }

        public void StopMusic() => _musicSource.Stop();

        private void PlayOneShotVar(AudioClip clip, float vol = 1f)
        {
            if (!clip) return;
            float oldPitch = _sfxSource.pitch;
            _sfxSource.pitch = 1f + Random.Range(-_pitchJitter, _pitchJitter);
            _sfxSource.PlayOneShot(clip, sfxVolume * vol);
            _sfxSource.pitch = oldPitch;
        }

        public void PlayWallHit()   => PlayOneShotVar(_sfxWall);
        public void PlayPaddleHit() => PlayOneShotVar(_sfxPaddle);
        public void PlayBrick()     => PlayOneShotVar(_sfxBrick);
        public void PlayGameOver()  => PlayOneShotVar(_sfxGameOver, 1.2f);
    }
}
