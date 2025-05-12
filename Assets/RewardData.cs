using UnityEngine;

[CreateAssetMenu(menuName = "StickyNote/Reward Definition", fileName = "Reward_")]
public class RewardData: ScriptableObject
{
    public string rewardName;
    public Sprite icon;
    [TextArea] public string description;
    public float countDownTimer;
    public float count;

    public void ApplyReward()
    {
        Debug.Log("Apply reward: " + rewardName);
        GameManager.Instance.rewardManager.ApplyReward(this);
    }
}
