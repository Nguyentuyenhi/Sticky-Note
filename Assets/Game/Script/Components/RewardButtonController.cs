using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RewardButtonController : MonoBehaviour
{
    public Image icon;
    public TMP_Text rewardNameText;
    public Image timerImage;
    public Button claim;
    private RewardData reward;
    private float duration;
    private Coroutine timerCoroutine;
    public Canvas canvas;

    private bool isClaimed = false;
    private bool isActive = true;

    void Start()
    {
        StartReset();
        if (canvas == null)
            canvas = gameObject.transform.GetComponentInChildren<Canvas>();

        if (canvas != null)
        {
            canvas.worldCamera = Camera.main;
        }
    }

    public void Setup(RewardData rewardDef, float lifetime)
    {
        reward = rewardDef;
        duration = lifetime;
        isClaimed = false;
        isActive = true;

        rewardNameText.text = reward.rewardName;
        // icon.sprite = reward.icon;
        claim.onClick.RemoveAllListeners(); // Xóa các listener cũ nếu có
        claim.onClick.AddListener(OnClaim);

        if (timerCoroutine != null )
            StopCoroutine(timerCoroutine);
            timerCoroutine = StartCoroutine(TimerCountdown(duration));
    }

    IEnumerator TimerCountdown(float time)
    {
        float timeLeft = time;
        while (timeLeft > 0f && isActive)
        {
            timeLeft -= Time.deltaTime;
            if (timerImage) timerImage.fillAmount = timeLeft / time;
            yield return null;
        }

        if (isActive)
        {
            GameManager.Instance.objectPooler.ReturnToPool(gameObject);
        }
    }

    public void StartReset()
    {
        GameManager.Instance.stickyNoteManager.nextLV += ResetReward;
    }

    void OnClaim()
    {
        if (isClaimed || !isActive) return;
        isClaimed = true;

        reward.ApplyReward();
        timerImage.color = Color.red;

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(TimerCountdown(reward.countDownTimer));
    }

    public void ResetReward()
    {
        isActive = false;

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = null;
        claim.onClick.RemoveAllListeners();
        GameManager.Instance.objectPooler.ReturnToPool(gameObject);

    }
}
