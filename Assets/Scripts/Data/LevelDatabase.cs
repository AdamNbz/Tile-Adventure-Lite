using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "TileTrip/LevelDatabase")]
public class LevelDatabase : ScriptableObject
{
    public List<LevelData> levels;

    public LevelData GetLevel(int levelNumber)
    {
        return levels.Find(l => l.levelNumber == levelNumber);
    }
}
