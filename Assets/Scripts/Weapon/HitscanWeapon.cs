using System.Collections.Generic;
using Configs;
using UnityEngine;

namespace Scripts
{
    public class HitscanWeapon : WeaponBase
    {
        [Header("Bullet Hole")]
        [SerializeField] protected GameObject bulletHolePrefab;
        [SerializeField] protected float holeSize = 0.12f;
        [SerializeField] protected float normalOffset = 0.002f;
        [SerializeField] protected int maxHoles = 60;
        [SerializeField] protected float holeLifetime = 20f;

        private readonly Queue<GameObject> _activeHoles = new();
        private readonly Queue<GameObject> _pooledHoles = new();
        private readonly List<GameObject> _allHoles = new();

        protected override void Awake()
        {
            base.Awake();
            if (config is HitscanWeaponConfig hc)
            {
                bulletHolePrefab = hc.BulletHolePrefab;
                holeSize = hc.HoleSize;
                normalOffset = hc.NormalOffset;
                maxHoles = hc.MaxHoles;
                holeLifetime = hc.HoleLifetime;
            }
        }

        protected override void OnFire()
        {
            if (!barrel) {Debug.LogWarning("No barrel!");
                return; 
            }

            if (Physics.Raycast(barrel.position, barrel.forward, out var hit, range, ~0,
                    QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.TryGetComponent(out HealthController h))
                {
                    h.TakeDamage(damage);
                }

                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForce(barrel.forward * force, ForceMode.Impulse);
                }

                SpawnBulletHole(hit);
            }
        }

        protected void SpawnBulletHole(RaycastHit hit)
        {
            if (!bulletHolePrefab) return;
            
            var pos = hit.point + hit.normal * normalOffset;
            var rot = Quaternion.LookRotation(-hit.normal, Vector3.up);
            rot *= Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            GameObject hole = GetPooledHole();
            if (!hole)
            {
                hole = Instantiate(bulletHolePrefab);
                _allHoles.Add(hole);
            }

            hole.transform.position = pos;
            hole.transform.rotation = rot;
            float s = holeSize * Random.Range(0.95f, 1.05f);
            hole.transform.localScale = new Vector3(s, s, s);
            hole.transform.SetParent(hit.collider.transform, true);
            hole.SetActive(true);
            
            _activeHoles.Enqueue(hole);
            
            if (_activeHoles.Count > maxHoles)
            {
                var oldHole = _activeHoles.Dequeue();
                if (oldHole) ReturnHoleToPool(oldHole);
            }
            
            if (holeLifetime > 0f)
            {
                StartCoroutine(ReturnHoleAfterDelay(hole, holeLifetime));
            }
        }

        private GameObject GetPooledHole()
        {
            if (_pooledHoles.Count > 0)
            {
                return _pooledHoles.Dequeue();
            }
            return null;
        }

        private void ReturnHoleToPool(GameObject hole)
        {
            if (!hole) return;
            hole.SetActive(false);
            hole.transform.SetParent(transform, false);
            _pooledHoles.Enqueue(hole);
        }

        private System.Collections.IEnumerator ReturnHoleAfterDelay(GameObject hole, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (hole && hole.activeInHierarchy)
            {
                var tempList = new List<GameObject>(_activeHoles.Count);
                while (_activeHoles.Count > 0)
                {
                    var h = _activeHoles.Dequeue();
                    if (h != hole && h) tempList.Add(h);
                }
                foreach (var h in tempList) _activeHoles.Enqueue(h);
                
                ReturnHoleToPool(hole);
            }
        }

        private void OnDestroy()
        {
            foreach (var hole in _allHoles)
            {
                if (hole) Destroy(hole);
            }
        }
    }
}