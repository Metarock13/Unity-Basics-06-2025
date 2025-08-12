using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private List<WeaponBase> weapons = new();
        [SerializeField] private int startIndex = 0;

        private int _current;
        private static readonly KeyCode[] NumberKeys = System.Array.Empty<KeyCode>();

        private void Start()
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i]) weapons[i].gameObject.SetActive(false);
            }

            if (weapons.Count > 0)
            {
                _current = Mathf.Clamp(startIndex, 0, weapons.Count - 1);
                weapons[_current].OnSelect();
            }
        }

        private void Update()
        {
            if (weapons.Count == 0) return;

            if (Input.GetMouseButton(0))
            {
                weapons[_current].Fire();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                weapons[_current].Recharge();
            }

            // Mouse wheel switching only
            var delta = Input.mouseScrollDelta.y;
            int steps = Mathf.RoundToInt(delta);
            if (steps > 0)
            {
                for (int i = 0; i < steps; i++) Next();
            }
            else if (steps < 0)
            {
                for (int i = 0; i < -steps; i++) Prev();
            }
        }

        private void Next() => SwitchTo((_current + 1) % weapons.Count);
        private void Prev() => SwitchTo((_current - 1 + weapons.Count) % weapons.Count);

        private void SwitchTo(int index)
        {
            if (index < 0 || index >= weapons.Count || index == _current) return;

            weapons[_current].OnDeselect();
            _current = index;
            weapons[_current].OnSelect();
        }
    }
}
