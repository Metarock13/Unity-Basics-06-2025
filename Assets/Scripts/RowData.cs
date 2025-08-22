using UnityEngine;

namespace Scripts
{
    [System.Serializable]
    public class RowData
    {
        public Color Color = Color.white;
        [Min(1)] public int HitPoints = 3;
    }
}