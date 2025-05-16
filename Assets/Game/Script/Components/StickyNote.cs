using UnityEngine;
using DG.Tweening;

public class StickyNote : MonoBehaviour
{
    public int id;

    public void Setup(int index)
    {
        id = index;
        // Setup dữ liệu nếu cần
    }

    public void Tear()
    {
        GameManager.Instance.stickyNoteManager.MoneySpawner();
        GameManager.Instance.skillManager.UpDateImageBtn();
        Vector3 targetPosition = transform.position + new Vector3(
           Random.Range(-2f,2f),   
            0f,   
            -5f   
        );


        transform.DOMove(targetPosition, 2f);
        Destroy(gameObject, 0.7f);
    }


}
