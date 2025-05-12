using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "StickyNote/Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public int level = 0;
    public int temporaryLevel = 0;
    public int baseCost = 10;
    public float costMultiplier = 1.5f;
    public float CountdownTimer;

    public int GetCost(int level)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level));
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
