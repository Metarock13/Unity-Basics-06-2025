using UnityEngine;

namespace Code
{
    public class Player : MonoBehaviour
    {
        private int playerLevel = 1;
        private float health = 100.0f;
        private bool isAlive = true;
        private float baseDamage = 10.0f;
        private float damageMultiplier = 1.5f;
        
        void Start()
        {
            Debug.Log("Статистика игрока:\n" +
                      $"Int (yровень игрока): {playerLevel}\n" +
                      $"Float (здоровье игрока): {health}\n" +
                      $"Float (базовый урон игрока): {baseDamage}\n" +
                      $"Float (множитель урона игрока): {damageMultiplier}\n" +
                      $"Bool (живой ли игрок): {isAlive}");

            GameObject enemyObject = new GameObject("Enemy");
            Enemy enemy = enemyObject.AddComponent<Enemy>();
            
            enemy.SetHealth(50f);
            float finalDamage = CalcDamage(baseDamage, damageMultiplier);
            enemy.TakeDamage(finalDamage);
        }

        private float CalcDamage(float damage, float multiplier)
        {
            float result = baseDamage * damageMultiplier;
            Debug.Log($"Расчет урона игрока: {baseDamage} + {damageMultiplier} = {result}");
            return result;
        }
    }
}