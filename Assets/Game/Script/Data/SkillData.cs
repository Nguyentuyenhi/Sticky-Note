using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "StickyNote/Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public int level = 1;
    public SkillType skillType;
    public int temporaryLevel = 0;
    public int baseCost = 10;
    public float costMultiplier = 1.5f;
    public float CountdownTimer;

    public enum SkillType
    {
        Income,
        Speed,
        Hands
    }

    public float GetUpgradeCost(SkillType skillType, int level)
    {
        switch (skillType)
        {
            case SkillType.Income:
                return 10f * Mathf.Pow((Mathf.Log(4f) / Mathf.Log(2.7f)), level);

            case SkillType.Speed:
                return 15f * Mathf.Pow( (Mathf.Log(4f) / Mathf.Log(2.7f)),  level);

            case SkillType.Hands:
                return Mathf.Pow(100f, level + 1);

            default:
                Debug.LogWarning("Unknown skill type!");
                return 0f;
        }

    }
    public float GetRoundedUpgradeCost(SkillType skillType, int level)
    {
        float cost = GetUpgradeCost(skillType, level);
        float roundedCost = Mathf.Round(cost * 10f) / 10f;
        return roundedCost;
    }



    public void LevelUp()
    {
        level++;
    }
    public void LevelDown()
    {
        level--;
    }
    public void LevelReset()
    {
        level = 0;
    }
}
