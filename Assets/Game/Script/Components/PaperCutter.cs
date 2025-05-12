using UnityEngine;
using DG.Tweening;

public class PaperCutter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Transform defaultPosition;

    [Header("Settings")]
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float rotateDuration = 0.2f;
    [SerializeField] private float pauseDuration = 0.2f;
    [SerializeField] private float cutAngle = 180f; 

    private bool isCutting = false;


    public void StartCut()
    {
        targetPosition.position = GameManager.Instance.stickyNoteManager.basePos;
        defaultPosition = gameObject.transform;
        if (isCutting) return;

        isCutting = true;

        Sequence seq = DOTween.Sequence();

        Vector3 targetPos = new Vector3(targetPosition.position.x, targetPosition.position.y, transform.position.z) + new Vector3(0,0.75f, 0);
        seq.Append(transform.DOMove(targetPos, moveDuration).SetEase(Ease.InOutSine));
        // Tạm dừng
        seq.AppendInterval(pauseDuration);
        seq.Append(transform.DORotate(
            new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, cutAngle),
            rotateDuration,
            RotateMode.Fast
        ));
        seq.AppendCallback(() =>
        {
            GameManager.Instance.stickyNoteManager.CutNotes(20);
        });
        seq.AppendInterval(pauseDuration);
        seq.Append(transform.DOMove(defaultPosition.position, moveDuration).SetEase(Ease.InOutSine));
        seq.AppendInterval(pauseDuration);
        seq.Append(transform.DORotate(
            new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f),
            rotateDuration,
            RotateMode.Fast
        ));
        seq.OnComplete(() =>
        {
            isCutting = false;
        });
    }
}
