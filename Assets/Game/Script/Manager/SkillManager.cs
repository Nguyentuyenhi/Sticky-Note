using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public SkillData incomeSkill;
    public SkillData speedSkill;
    public SkillData handsSkill;
    public TMP_Text incomeSkillLv;
    public TMP_Text speedSkillLv;
    public TMP_Text handSkillLv;
    public TMP_Text incomeSkillLvPrice;
    public TMP_Text speedSkillLvPrice;
    public TMP_Text handSkillLvPrice;
    [SerializeField] private Button incomeButton;
    [SerializeField] private Button speedButton;
    [SerializeField] private Button handsButton;
    [SerializeField] private ColorBlock normalColor;
    [SerializeField] private Color affordableColor;

    private void Start()
    {
        StartReset();
        normalColor = incomeButton.colors;
        affordableColor =new Color(0.6f, 0.8f, 1f);

        UpdateText();
        incomeButton.onClick.AddListener(() => UpgradeSkill(incomeSkill));
        speedButton.onClick.AddListener(() => UpgradeSkill(speedSkill));
        handsButton.onClick.AddListener(() => UpgradeSkill(handsSkill));

    }
    private void Update()
    {
        UpdateButtonColor(incomeButton, incomeSkill);
        UpdateButtonColor(speedButton, speedSkill);
        UpdateButtonColor(handsButton, handsSkill);
    }

    private void UpdateButtonColor(Button button, SkillData skill)
    {
        bool canAfford = GameManager.Instance.uiManager.coin >= skill.GetUpgradeCost(skill.skillType, skill.level);
        ColorBlock cb = button.colors;
        cb.normalColor = canAfford ? affordableColor : normalColor.normalColor;
        button.colors = cb;
    }
    public void StartReset()
    {
        GameManager.Instance.stickyNoteManager.nextLV += SkillReset;
    }
    public void SkillReset()
    {
        incomeSkill.level = 0;
        handsSkill.level = 0;
        speedSkill.level = 0;
        UpdateText();

    }
    public void UpgradeSkill(SkillData skill)
    {
        float cost = skill.GetUpgradeCost(skill.skillType, skill.level);
        if (GameManager.Instance.uiManager.coin >= cost)
        {
            GameManager.Instance.uiManager.SpendCoin(cost);
            skill.LevelUp();
            ApplySkillEffect(skill);
        }
    }

    private void ApplySkillEffect(SkillData skill)
    {
        if (skill == incomeSkill)
        {
            GameManager.Instance.incomePerNote =incomeSkill.level + incomeSkill.temporaryLevel;
        }
        else if (skill == speedSkill)
        {
            GameManager.Instance.armController.UpdateSpeedMultiplier(speedSkill.level + speedSkill.temporaryLevel);
        }
        else if (skill == handsSkill)
        {
            GameManager.Instance.armController.SetHandCount(1 + handsSkill.level + handsSkill.temporaryLevel);
        }
        UpdateText();
    }
    public void UpdateText()
    {
        incomeSkillLv.text = $"Level: {incomeSkill.level}";
        speedSkillLv.text = $"Level: {speedSkill.level}";
        handSkillLv.text = $"Level: {handsSkill.level}";

        incomeSkillLvPrice.text = $"${incomeSkill.GetUpgradeCost(incomeSkill.skillType,incomeSkill.level)}";
        speedSkillLvPrice.text = $"${speedSkill.GetUpgradeCost( speedSkill.skillType,speedSkill.level)}";
        handSkillLvPrice.text = $"${handsSkill.GetUpgradeCost(handsSkill.skillType, handsSkill.level)}";
    }
}
