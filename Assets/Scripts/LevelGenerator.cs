using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Brick _brickPrefab;

        [Header("Layout")]
        [SerializeField] private float leftRightPadding = 0.3f;
        [SerializeField] private float columnGap = 0.1f;
        [SerializeField] private float rowGap = 0.1f;
        [SerializeField] private bool coverEdges = false;

        private readonly List<Brick> _bricks = new();
        private int _aliveCount;

        public event Action OnLevelCleared;

        public void GenerateLevel(LevelSettings levelSettings)
        {
            ClearLevel();

            Camera cam = Camera.main;
            if (!cam || !_brickPrefab || levelSettings == null || levelSettings.LevelData == null)
                return;

            Vector2 brickSize = GetBrickWorldSize();
            float stepX = brickSize.x + columnGap;
            float stepY = brickSize.y + rowGap;

            float viewWidth   = GetCameraWidthAtRowY(cam, _spawnPoint.position);
            float usableWidth = Mathf.Max(0f, viewWidth - 2f * leftRightPadding);

            int columns = coverEdges
                ? Mathf.Max(1, Mathf.CeilToInt((usableWidth + columnGap) / stepX))
                : Mathf.Max(1, Mathf.FloorToInt((usableWidth + columnGap) / stepX));

            float startX = _spawnPoint.position.x - 0.5f * (columns - 1) * stepX;

            _aliveCount = 0;

            for (int j = 0; j < levelSettings.LevelData.Count; j++)
            {
                var row = levelSettings.LevelData[j];
                float y = _spawnPoint.position.y - stepY * j;

                for (int i = 0; i < columns; i++)
                {
                    float x = startX + i * stepX;
                    var pos = new Vector3(x, y, 0f);

                    var brick = Instantiate(_brickPrefab, pos, Quaternion.identity, transform);
                    brick.Init(row.HitPoints, row.Color);
                    brick.OnDestroyed += HandleBrickDestroyed;

                    _bricks.Add(brick);
                    _aliveCount++;
                }
            }
        }

        private void HandleBrickDestroyed(Brick brick)
        {
            if (brick) brick.OnDestroyed -= HandleBrickDestroyed;
            _bricks.Remove(brick);
            _aliveCount = Mathf.Max(0, _aliveCount - 1);

            if (_aliveCount == 0)
                OnLevelCleared?.Invoke();
        }

        private void ClearLevel()
        {
            foreach (var b in _bricks)
            {
                if (b)
                {
                    b.OnDestroyed -= HandleBrickDestroyed;
                    Destroy(b.gameObject);
                }
            }
            _bricks.Clear();
            _aliveCount = 0;
        }

        private void OnDisable()
        {
            foreach (var b in _bricks)
                if (b) b.OnDestroyed -= HandleBrickDestroyed;
        }

        private float GetCameraWidthAtRowY(Camera cam, Vector3 rowWorldPoint)
        {
            var vp = cam.WorldToViewportPoint(rowWorldPoint);
            var left  = cam.ViewportToWorldPoint(new Vector3(0f, vp.y, vp.z));
            var right = cam.ViewportToWorldPoint(new Vector3(1f, vp.y, vp.z));
            return Vector3.Distance(left, right);
        }

        private Vector2 GetBrickWorldSize()
        {
            var r = _brickPrefab.GetComponentInChildren<Renderer>();
            if (r)
            {
                var s = r.bounds.size;
                if (s.x > 0.0001f && s.y > 0.0001f) return new Vector2(s.x, s.y);
            }
            return _brickPrefab.BrickSize;
        }
    }
}
