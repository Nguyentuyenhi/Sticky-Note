using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

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

    void Start()
    {
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

        rewardNameText.text = reward.rewardName;
        // icon.sprite = reward.icon;
        claim.onClick.AddListener(OnClaim);

        timerCoroutine = StartCoroutine(TimerCountdown(duration));
    }

    IEnumerator TimerCountdown(float time)
    {
        float timeLeft = time;
        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            if (timerImage) timerImage.fillAmount = timeLeft / time;
            yield return null;
        }

        Destroy(gameObject);
    }

    void OnClaim()
    {
        if (isClaimed) return;
        isClaimed = true;

        reward.ApplyReward();
        timerImage.color = Color.red;
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(TimerCountdown(reward.countDownTimer));
    }
}
