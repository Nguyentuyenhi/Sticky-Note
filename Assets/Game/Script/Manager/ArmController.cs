﻿using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ArmController : MonoBehaviour
{
    [SerializeField] private ArmWorker[] arms;
    [SerializeField] private Transform target;
    [SerializeField] private StickyNoteManager noteManager;

    private int activeHandCount = 1;
    private int speedMultiplier = 0;
    public void StartTearingSequence(Action onComplete)
    {
        int finishedCount = 0;

        for (int i = 0; i < activeHandCount; i++)
        {
            arms[i].gameObject.SetActive(true);
            Vector3 spawnPos = target.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, 0f);

            arms[i].Init(target, noteManager);
            arms[i].CalculateSpeedFor1NotePerSecond();
            arms[i].StartWorkLoop(() =>
            {
                finishedCount++;
                if (finishedCount == activeHandCount)
                    onComplete?.Invoke();
            });
        }

    }

    public void UpdateSpeedMultiplier(int multiplier)
    {
        speedMultiplier = multiplier;
        for(int i = 0;i < arms.Length; i++)
        {
            arms[i].SetSpeedByLevel(speedMultiplier);
        }

    }

    public void UpdateSpeedAutoClick(float duration)
    {
        for (int i = 0; i < arms.Length; i++)
        {
            arms[i].StartAutoBoost(duration);
        }

    }
    public void UpdateSpeed(float duration)
    {
        for (int i = 0; i < arms.Length; i++)
        {
            arms[i].ActivateSpeedBoost(duration);
        }

    }

    public void SetHandCount(int count)
    {
        int previousHandCount = activeHandCount;
        activeHandCount = Mathf.Clamp(count, 1, arms.Length);

        if (activeHandCount > previousHandCount)
        {
            for (int i = previousHandCount; i < activeHandCount; i++)
            {
                arms[i].gameObject.SetActive(true);
                arms[i].Init(target, noteManager);
                arms[i].CalculateSpeedFor1NotePerSecond();
                arms[i].StartWorkLoop(null);
            }
        }

        for (int i = activeHandCount; i < arms.Length; i++)
        {
            arms[i].gameObject.SetActive(false);
            arms[i].StopWork();
            arms[i].StopAllCoroutines();
        }
    }


}
