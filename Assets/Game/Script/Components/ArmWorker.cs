using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmWorker : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    private Vector3 startPos;
    private Transform target;
    private StickyNoteManager noteManager;
    public bool isBoosting =false;
    public float baseSpeed;
    private float boostTimer = 0f;
    private float boostDuration = 1f;
    public StickyNote stickyNote;
    private Coroutine workRoutine;

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

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (!isBoosting)
            {
                baseSpeed = speed;
                speed *= 4f;
                isBoosting = true;
            }

            boostTimer = boostDuration;
        }

        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;

            if (boostTimer <= 0f)
            {
                speed = baseSpeed;
                isBoosting = false;
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

    private IEnumerator WorkCoroutine(Action onDone)
    {
        while (noteManager.HasNotes())
        {
            yield return MoveTo(target.position + new Vector3 (0, 0.6f,0));

            yield return new WaitForSeconds(0.2f);
            var note = GameManager.Instance.stickyNoteManager.PopNote();

            if (note != null)
            {
                stickyNote = note;
                note.Tear();
                GameManager.Instance.uiManager.AddCoin(GameManager.Instance.incomePerNote);
            }

            yield return MoveTo(startPos);
            yield return new WaitForSeconds(0.1f);
        }
        GameManager.Instance.stickyNoteManager.ShowNextLVPanel();
        onDone?.Invoke();
        GameManager.Instance.stickyNoteManager.ShowNextLVPanel();
    }

    private IEnumerator MoveTo(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speed = 1f * multiplier;
    }
}
