using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StickyNoteManager : MonoBehaviour
{
    [SerializeField] private StickyNote stickyNotePrefab;
    [SerializeField] private int noteCount = 100;
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
        GameManager.Instance.uiManager.NextLevelPanel.SetActive(true);
    }
    public void NextLevel()
    {
        currentLevel += 1;
        StartLevel(currentLevel);
        nextLV?.Invoke();
    }
    public void SetupFromLevelData(StickyNoteLevelData levelData)
    {
        this.currentLevelData = levelData;
        noteCount = levelData.noteCount;
        notes.Clear();

        // Xóa note cũ nếu có
        foreach (Transform child in spawnParent)
            Destroy(child.gameObject);
    }

    public  void CreateStickyNotes()
    {
        for (int i = 0; i < noteCount; i++)
        {
            var pos = basePos + i * offset;
            var prefab = currentLevelData.possibleNotePrefabs[ UnityEngine.Random.Range(0, currentLevelData.possibleNotePrefabs.Count)];
            var note = Instantiate(prefab, pos, Quaternion.identity, spawnParent);
            note.Setup(i);
            notes.Push(note);
        }
    }
    public void PushNoteBack(StickyNote note)
    {
        notes.Push(note);
    }


}
