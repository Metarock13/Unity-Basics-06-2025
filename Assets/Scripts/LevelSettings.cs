using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Create LevelSettings", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        public List<RowData> LevelData;
    }
}
