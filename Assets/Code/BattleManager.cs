using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class BattleManager : MonoBehaviour
    {
        public Character characterPrefab;
        public int alliesCount = 4;
        public int enemiesCount = 5;

        private List<Character> allies = new List<Character>();
        private List<Character> enemies = new List<Character>();

        private void Start()
        {
            SpawnCharacters();
            InvokeRepeating(nameof(EnemiesAttack), 1f, 1.5f);
        }

        private void SpawnCharacters()
        {
            for (int i = 0; i < alliesCount; i++)
            {
                var pos = new Vector3(-5 + i * 2, 0, 0);
                var ally = Instantiate(characterPrefab, pos, Quaternion.identity);
                ally.characterType = CharacterType.Ally;
                ally.gameObject.name = "Ally_" + i;
                allies.Add(ally);
            }
            allies.Add(characterPrefab);

            for (int i = 0; i < enemiesCount; i++)
            {
                var pos = new Vector3(5 + i * 2, 0, 0);
                var enemy = Instantiate(characterPrefab, pos, Quaternion.identity);
                enemy.characterType = CharacterType.Enemy;
                enemy.gameObject.name = "Enemy_" + i;
                enemies.Add(enemy);
            }
        }

        private void EnemiesAttack()
        {
            foreach (var enemy in enemies)
            {
                if (enemy.isDeath) continue;
                var target = allies.Find(x => !x.isDeath);
                if (target != null)
                {
                    enemy.Attack(target);
                }
            }
        }
    }
}