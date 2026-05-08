using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "TileTrip/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public List<TileLayoutData> tileLayouts;
    public int rackSlots = 7;
    public int targetTriples = 4;
    public List<int> iconIndices;
}

[System.Serializable]
public struct TileLayoutData
{
    public Vector3 position;
    public int layer;
}
