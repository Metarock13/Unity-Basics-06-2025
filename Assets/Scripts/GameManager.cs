using UnityEngine;

namespace Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private LevelSettings _levelSettings;

        [Header("Round refs")]
        [SerializeField] private Ball _ball;
        [SerializeField] private Paddle _paddle;

        [Header("Player")]
        [SerializeField, Min(1)] private int _startLives = 5;
        private int _lives;

        private bool _isQuitting;

        private void Awake()
        {
            if (!_levelGenerator) _levelGenerator = FindObjectOfType<LevelGenerator>();
            if (!_ball)           _ball           = FindObjectOfType<Ball>();
            if (!_paddle)         _paddle         = FindObjectOfType<Paddle>();
        }

        private void OnEnable()
        {
            if (_levelGenerator) _levelGenerator.OnLevelCleared += OnLevelCleared;
            if (_ball)           _ball.OnDeadZone += OnBallDead;
        }

        private void Start()
        {
            _lives = _startLives;

            if (_levelGenerator && _levelSettings)
                _levelGenerator.GenerateLevel(_levelSettings);

            ResetRound();
            Debug.Log($"Lives: {_lives}");
        }

        private void OnDisable()
        {
            if (_levelGenerator) _levelGenerator.OnLevelCleared -= OnLevelCleared;
            if (_ball)           _ball.OnDeadZone -= OnBallDead;
        }

        private void OnApplicationQuit() => _isQuitting = true;

        private void OnBallDead()
        {
            if (_isQuitting) return;

            _lives--;
            Debug.Log($"Life lost. Lives left: {_lives}");

            if (_lives > 0)
            {
                ResetRound();
            }
            else
            {
                Debug.Log("Game Over — restarting level");
                AudioManager.I?.PlayGameOver();
                _levelGenerator.GenerateLevel(_levelSettings);
                _lives = _startLives;
                ResetRound();
                Debug.Log($"Lives reset: {_lives}");
            }
        }

        private void OnLevelCleared()
        {
            if (_isQuitting) return;

            Debug.Log("Level cleared — restarting same level");
            _levelGenerator.GenerateLevel(_levelSettings);
            ResetRound();
        }

        private void ResetRound()
        {
            if (_isQuitting) return;
            _ball?.Reset();
            _paddle?.Reset();
        }
    }
}
