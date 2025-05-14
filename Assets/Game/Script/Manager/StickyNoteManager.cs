using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StickyNoteManager : MonoBehaviour
{
    [SerializeField] private StickyNote stickyNotePrefab;
    [SerializeField] public int noteCount;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private List<StickyNoteLevelData> levels;
    [SerializeField] public Vector3 basePos, offset;
    public int currentLevel = 0;
    public Stack<StickyNote> notes = new Stack<StickyNote>();
    private StickyNoteLevelData currentLevelData;
    public Action nextLV;
    /* public void CreateStickyNotes()
     {
         for (int i = 0; i < noteCount; i++)
         {
             var pos = basePos + i * offset;
             var note = Instantiate(stickyNotePrefab, pos, Quaternion.identity, spawnParent);
             note.Setup(i); // có thể thêm thông tin cho từng note
             notes.Push(note);
         }
     }*/

    public StickyNote PopNote()
    {
        return notes.Count > 0 ? notes.Pop() : null;
    }

    public void CutNotes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (!HasNotes()) break;

            StickyNote note = notes.Pop();
            note.Tear();
        }

        if (!HasNotes())
        {
            Debug.Log("Đã xé hết giấy!");
            ShowNextLVPanel();
        }
    }
    /*public Transform GetPlayerWithHighestID()
    {
        Player[] players = FindObjectsOfType<Player>();
        Player highestIDPlayer = players[0];

        foreach (Player player in players)
        {
            if (player.ID > highestIDPlayer.ID)
            {
                highestIDPlayer = player;
            }
        }

        return highestIDPlayer.transform;
    }*/


    public bool HasNotes() => notes.Count > 0;

    public void StartLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError("Invalid level index");
            return;
        }

        currentLevel = levelIndex;
        var levelData = levels[currentLevel];
        SetupFromLevelData(levelData);
       CreateStickyNotes();
    }

    public void ShowNextLVPanel()
    {
        nextLV?.Invoke();
        GameManager.Instance.uiManager.NextLevelPanel.SetActive(true);
    }
    public void NextLevel()
    {
        currentLevel += 1;
        StartLevel(currentLevel);
    }
    public void SetupFromLevelData(StickyNoteLevelData levelData)
    {
        this.currentLevelData = levelData;
        noteCount = levelData.TotalNoteCount;
        notes.Clear();

        // Xóa note cũ nếu có
        foreach (Transform child in spawnParent)
            Destroy(child.gameObject);
    }

    public void CreateStickyNotes()
    {
        
        int noteIndex = 0;
        int entryIndex = 0;
        int currentCount = 0;

        for (int i = 0; i < noteCount; i++)
        {
            StickyNoteEntry entry = currentLevelData.noteEntries[entryIndex];

            Vector3 pos = basePos + i * offset * 2;

            // Tạo giấy
            StickyNote note = Instantiate(entry.prefab, pos, Quaternion.identity, spawnParent);
            note.Setup(noteIndex);
            notes.Push(note);
            noteIndex++;

            currentCount++;

            if (currentCount >= entry.count)
            {
                entryIndex++;
                currentCount = 0;

                if (entryIndex >= currentLevelData.noteEntries.Count)
                    break;
            }
        }
    }

    public void PushNoteBack(StickyNote note)
    {
        notes.Push(note);
    }


}
