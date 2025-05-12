using UnityEngine;
using System.Collections;

public class RewardManager : MonoBehaviour
{
    public void ApplyReward(RewardData reward)
    {
        switch (reward.rewardName)
        {
            case "+3 Income":
                Debug.Log("Tăng income x3");
                GameManager.Instance.skillManager.incomeSkill.temporaryLevel += 3;
                GameManager.Instance.incomePerNote = 1 + GameManager.Instance.skillManager.incomeSkill.level + GameManager.Instance.skillManager.incomeSkill.temporaryLevel;

                StartCoroutine(ReverseIncomeBuffAfterDelay(3, reward.countDownTimer));
                break;

            case "+5 Hands":
                GameManager.Instance.skillManager.handsSkill.temporaryLevel += 5;

                GameManager.Instance.armController.SetHandCount(1 + GameManager.Instance.skillManager.handsSkill.level + GameManager.Instance.skillManager.handsSkill.temporaryLevel);

                StartCoroutine(ReverseHandsBuffAfterDelay(5, reward.countDownTimer));
                break;

            case "+2 Speed":
                Debug.Log("Tăng tốc x2");
                GameManager.Instance.skillManager.speedSkill.temporaryLevel += 2;
                GameManager.Instance.armController.UpdateSpeedMultiplier(1f + 0.2f * (GameManager.Instance.skillManager.speedSkill.level + GameManager.Instance.skillManager.speedSkill.temporaryLevel));

                StartCoroutine(ReverseSpeedBuffAfterDelay(2, reward.countDownTimer));
                break;

            case "+1 Paper Cutter":
                Debug.Log("Thêm dao cắt giấy");
                GameManager.Instance.paperCutter.StartCut();
                break;

            default:
                Debug.LogWarning("Reward không xác định");
                break;
        }
    }

    private IEnumerator ReverseIncomeBuffAfterDelay(int amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.skillManager.incomeSkill.temporaryLevel = 0;
        GameManager.Instance.incomePerNote = 1 + GameManager.Instance.skillManager.incomeSkill.level;
        Debug.Log("Income trở lại bình thường");
    }

    private IEnumerator ReverseHandsBuffAfterDelay(int amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.skillManager.handsSkill.temporaryLevel = 0;

        GameManager.Instance.armController.SetHandCount(1 + GameManager.Instance.skillManager.handsSkill.level);
        Debug.Log("Số tay trở lại bình thường");
    }

    private IEnumerator ReverseSpeedBuffAfterDelay(int amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.skillManager.speedSkill.temporaryLevel = 0;

        GameManager.Instance.armController.UpdateSpeedMultiplier(1f + 0.2f * GameManager.Instance.skillManager.speedSkill.level);
        Debug.Log("Tốc độ trở lại bình thường");
    }
}
