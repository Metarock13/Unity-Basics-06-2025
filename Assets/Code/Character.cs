using UnityEngine;

namespace Code
{
    public class Character : MonoBehaviour
    {
        public enum CharacterType
        {
            Player,
            Ally,
            Enemy
        }
        
        public CharacterType characterType;
        
        private float hp = 100.0f;
        public bool isDeath = false;
        private float baseDamage = 10.0f;
        private float damageMultiplier = 1.5f;
        
        [SerializeField]
        private MeshRenderer meshRenderer;
        private Color greyColor = Color.grey;
        private Color redColor = Color.red;
        private Color blueColor = Color.blue;
        private Color defaultColor = Color.white;
        
        void Start()
        {
            SetColorByType(); 
        }

        private float CalcDamage(float damage, float multiplier)
        {
            float result = baseDamage * damageMultiplier;
            Debug.Log($"Расчет урона {gameObject.name}: {baseDamage} + {damageMultiplier} = {result}");
            return result;
        }

        private void SetColor(Color color)
        {
            meshRenderer.material.color = color;
        }

        private void SetColorByType()
        {
            switch (characterType)
            {
                case CharacterType.Player:
                    SetColor(greyColor);
                    break;
                case CharacterType.Ally:
                    SetColor(blueColor);
                    break;
                case CharacterType.Enemy:
                    SetColor(redColor);
                    break;
                default:
                    SetColor(defaultColor);
                    break;
            }
        }

        private void TakeDamage(float damage, Character attacker)
        {
            if (isDeath) return;
            
            hp -= damage;
            if (hp <= 0)
            {
                hp = 0;
                isDeath = true;
                Debug.Log($"{gameObject.name} погиб!");
                return;
            }
            Debug.Log($"{gameObject.name} получил урон {damage} от {attacker.gameObject.name}, осталось HP: {hp}");

            if (!isDeath && attacker != null && !attacker.isDeath)
            {
                Attack(attacker);
            }
        }
        
        private void TakeDamage(int damage, Character attacker)
        {
            if (isDeath) return;
            
            hp -= damage;
            if (hp <= 0)
            {
                hp = 0;
                isDeath = true;
                Debug.Log($"{gameObject.name} погиб!");
                return;
            }
            Debug.Log($"{gameObject.name} получил урон {damage} от {attacker.gameObject.name}, осталось HP: {hp}");

            if (!isDeath && attacker != null && !attacker.isDeath)
            {
                Attack(attacker);
            }
        }

        public void Attack(Character target)
        {
            if (isDeath || target == null || target.isDeath) return;
            float damage = CalcDamage(baseDamage, damageMultiplier);
            Debug.Log($"{gameObject.name} атакует {target.gameObject.name} на {damage} урона!");
            target.TakeDamage(damage, this);
        }
    }
}