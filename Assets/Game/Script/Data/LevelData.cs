using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StickyNoteLevelData", menuName = "StickyNote/Level Data", order = 1)]
public class StickyNoteLevelData : ScriptableObject
{
    public List<StickyNoteEntry> noteEntries;
    public GameObject statuePrefab;
    public int realNote;
    public int index => realNote / 100;

    public int TotalNoteCount
    {
        get
        {
            int total = 0;
            foreach (var entry in noteEntries)
            {
                total += entry.count;
            }
            return total;
        }
    }
}

[System.Serializable]
public class StickyNoteEntry
{
    public StickyNote prefab;
    public int count; 
}

