using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmWorker : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    public Vector3 startPos;
    public Transform target;
    private StickyNoteManager noteManager;
    public bool isBoosting =false;
    public float baseSpeed;
    private float boostTimer = 0f;
    private float boostDuration = 1f;
    public StickyNote stickyNote;
    private Coroutine workRoutine;
    public float distance;
    private float boostMultiplier = 2f;
    private float boostTimerActive = 0f;
    private bool isSpeedBoostActive = false;
    private void Start()
    {
        baseSpeed = speed;
        startPos = transform.position;
    }
    public void Init(Transform target, StickyNoteManager noteManager)
    {
        this.target = target;
        this.noteManager = noteManager;
    }
    public void StartAutoBoost(float duration)
    {
        RequestBoost(duration);
    }

    public void RequestBoost(float duration)
    {
        // Nếu boost đang hoạt động và còn thời gian dài hơn, không thay đổi
        if ((isBoosting && boostTimer > duration) ) return;

        isBoosting = true;
        boostTimer = duration;
        speed *= 2;

        Debug.Log($"Boost requested for {duration} seconds");
    }
    public void ActivateSpeedBoost( float duration)
    {
        if (!isSpeedBoostActive)
        {
            baseSpeed = speed;
            speed  *=2;
            boostTimerActive = duration;
            isSpeedBoostActive = true;
        }
        else
        {
            // Nếu đang trong boost, reset lại thời gian
            boostTimerActive = duration;
        }
    }

    private void ResetSpeed()
    {
        speed /= 2f;
        isSpeedBoostActive = false;
    }

    private void EndBoost()
    {
        speed = baseSpeed;
        isBoosting = false;
        

        Debug.Log("Auto Boost Ended!");
    }

    void Update()
    {
        if (isSpeedBoostActive)
        {
            boostTimerActive -= Time.deltaTime;
            if (boostTimerActive <= 0f)
            {
                ResetSpeed();
            }
        }
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RequestBoost(boostDuration); // boostDuration = 1f
        }

        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;

            if (boostTimer <= 0f)
            {
                isBoosting = false;
                speed = baseSpeed;
                Debug.Log("Boost Ended");
            }
        }
    }

    public void StopWork()
    {
        if (workRoutine != null)
        {
            StopCoroutine(workRoutine);
            workRoutine = null;

            if (stickyNote != null )
            {
                GameManager.Instance.stickyNoteManager.PushNoteBack(stickyNote);
            }

            stickyNote = null;
        }
    }


    public void StartWorkLoop(Action onDone)
    {
        workRoutine = StartCoroutine(WorkCoroutine(onDone));
    }

    public void CalculateSpeedFor1NotePerSecond()
    {
         distance = Vector3.Distance(startPos, target.position + new Vector3(0, 0.6f, 0));
        baseSpeed = distance / 0.45f;
        speed = baseSpeed;
    }

    public void SetSpeedByLevel(int level)
    {
        float targetTime = Mathf.Max(0.1f, 1f - (0.1f * level));
        float moveTime = (targetTime - 0.1f) / 2f;

        baseSpeed = distance / moveTime;

        speed = isBoosting ? baseSpeed * boostMultiplier : baseSpeed;
      //  baseSpeed = speed;  
        //speed = isSpeedBoostActive ? baseSpeed * boostMultiplier : baseSpeed;
    }



    private IEnumerator WorkCoroutine(Action onDone)
    {
        while (noteManager.HasNotes())
        {
            // Di chuyển tới vị trí xé
            yield return MoveTo(target.position + new Vector3(0, 0.6f, 0));

            // Xé giấy ngay lập tức
            var note = GameManager.Instance.stickyNoteManager.PopNote();
            if (note != null)
            {
                stickyNote = note;
                note.Tear();
                GameManager.Instance.uiManager.UpdateCoinValue();
            }

            // Di chuyển về vị trí ban đầu
            yield return MoveTo(startPos);

            // Nghỉ 0.1s để tổng mỗi vòng đúng 1 giây
            yield return new WaitForSeconds(0.1f);
        }

        GameManager.Instance.stickyNoteManager.ShowNextLVPanel();
        onDone?.Invoke();
    }

    private IEnumerator MoveTo(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }
    }

    
}
