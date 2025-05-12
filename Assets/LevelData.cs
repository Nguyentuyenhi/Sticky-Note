using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StickyNoteLevelData", menuName = "StickyNote/Level Data", order = 1)]
public class StickyNoteLevelData : ScriptableObject
{
    public int noteCount;
    public List<StickyNote> possibleNotePrefabs;
}
