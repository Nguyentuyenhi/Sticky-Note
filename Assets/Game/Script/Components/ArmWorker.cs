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
    public StickyNote stickyNote;
    private Coroutine workRoutine;

    public float baseSpeed;
    public float distance;

    private float boostMultiplier = 2f;

    // Buff trạng thái
    private bool isBoosting = false;
    private float boostTimer = 0f;

    private bool isSpeedBoostActive = false;
    private float boostTimerActive = 0f;

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
        // Nếu boost đang hoạt động và thời gian còn lại dài hơn thì bỏ qua
        if (isBoosting && boostTimer > duration) return;

        isBoosting = true;
        boostTimer = duration;

        UpdateSpeed();
        Debug.Log($"[ArmWorker] Auto Boost requested for {duration} seconds");
    }

    public void ActivateSpeedBoost(float duration)
    {
        boostTimerActive = duration;
        isSpeedBoostActive = true;

        UpdateSpeed();
    }

    private void ResetSpeedBoost()
    {
        isSpeedBoostActive = false;
        UpdateSpeed();
    }

    private void EndAutoBoost()
    {
        isBoosting = false;
        UpdateSpeed();
        Debug.Log("[ArmWorker] Auto Boost Ended");
    }

    private void UpdateSpeed()
    {
        float finalMultiplier = 1f;

        if (isBoosting) finalMultiplier *= boostMultiplier;
        if (isSpeedBoostActive) finalMultiplier *= boostMultiplier;

        speed = baseSpeed * finalMultiplier;
    }

    void Update()
    {
        // Kiểm tra hết thời gian các buff
        if (isSpeedBoostActive)
        {
            boostTimerActive -= Time.deltaTime;
            if (boostTimerActive <= 0f)
            {
                ResetSpeedBoost();
            }
        }

        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
            {
                EndAutoBoost();
            }
        }

        // Test click chuột để boost
        if (Input.GetMouseButtonDown(0))
        {
            RequestBoost(1f); // boost 1 giây
        }
    }

    public void StopWork()
    {
        if (workRoutine != null)
        {
            StopCoroutine(workRoutine);
            workRoutine = null;

            if (stickyNote != null)
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

        // Tính lại speed theo các buff đang có
        UpdateSpeed();
    }

    private IEnumerator WorkCoroutine(Action onDone)
    {
        while (noteManager.HasNotes())
        {
            // Di chuyển tới vị trí xé
            yield return MoveTo(target.position + new Vector3(0, 0.6f, 0));

            // Xé giấy
                var note = GameManager.Instance.stickyNoteManager.PopNote();
                if (note != null)
                {
                    stickyNote = note;
                    GameManager.Instance.uiManager.UpdateCoinValue();
                    note.Tear();
                }

            // Quay lại vị trí ban đầu
            yield return MoveTo(startPos);

            // Delay để đảm bảo mỗi vòng mất 1 giây
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
