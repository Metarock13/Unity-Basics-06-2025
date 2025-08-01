using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
         
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                weapon.Fire();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                weapon.Recharge();
            }
        }
    }
}
