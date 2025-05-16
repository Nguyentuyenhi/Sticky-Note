using DG.Tweening.Core.Easing;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class StickyNoteManager : MonoBehaviour
{
    [SerializeField] private StickyNote stickyNotePrefab;
    [SerializeField] public int noteCount;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private Transform statueSpawnerPoint;
    [SerializeField] private Transform moneySpawnerPoint;
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
    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle * 100;

        return moneySpawnerPoint.position + new Vector3(randomOffset.x, 0f, randomOffset.y);
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
        GameObject statue = Instantiate(currentLevelData.statuePrefab, statueSpawnerPoint);
        statue.transform.localPosition = Vector3.zero;
        statue.transform.localRotation = Quaternion.identity;
    }

    public void ShowNextLVPanel()
    {
        nextLV?.Invoke();
    //    GameManager.Instance.uiManager.NextLevelPanel.SetActive(true);
        GameManager.Instance.rewardButtonSpawner.SpawnStop();
    }
    public void NextLevel()
    {
        GameManager.Instance.rewardButtonSpawner.StartSpawn();
        //GameManager.Instance.rewardButtonSpawner.SpawnRoutine();
        currentLevel += 1;
        StartLevel(currentLevel);
    }

    public void MoneySpawner()
    {
        var money =GameManager.Instance.objectPooler.SpawnFromPool("MoneyFlying", GetRandomSpawnPosition(), Quaternion.identity);
        
        Debug.Log("hee");
        money.GetComponent<MoneyFlying>().Flying();

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
            StickyNote note = Instantiate(entry.prefab, pos, entry.prefab.transform.rotation, spawnParent);

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
