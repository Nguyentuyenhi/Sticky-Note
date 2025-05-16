using UnityEngine;
using DG.Tweening;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class MoneyFlying : MonoBehaviour
{
    public float moveUpDistance = 100f; 
    private float moveDuration = 0.8f;  
    public float destroyDelay = 0.8f;
    public TMP_Text value;

    void Start()
    {
        
    }
    public void Flying()
    {
        value.text =  GameManager.Instance.incomePerNote.ToString();
        // Bay lên
        transform.DOMoveY(transform.position.y + moveUpDistance, moveDuration)
            .SetEase(Ease.OutCubic);

        // Biến mất (sau delay)
        DOVirtual.DelayedCall(destroyDelay, () =>
        {
            GameManager.Instance.objectPooler.ReturnToPool(gameObject);
        });
    }
}
